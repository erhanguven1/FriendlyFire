using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public int myId;

    public List<GameObject> ingredientList;
    public HandPlate handPlate;
    public Ingredients handObjectType;
    public GameObject handObject, throwableObject;
    public Rigidbody throwableObjectRigidBody;
    bool isHandEmpty = true, isSame = false, isThrowable = false;

    private Vector3 hitPoint;

    public float throwForce;

    Plate hitPlate;

    public Team myTeam;
    public float speed;
    int Score=0;

    void Start()
    {
        hitPlate = GameManager.Instance.hitPlates[(int)myTeam];
        myId = GameManager.Instance.players.Count;

        if (!photonView.IsMine)
        {
            this.enabled = false;
        }

        GameManager.Instance.players.Add(this);

    }

    void OnDestroy()
    {
        GameManager.Instance.photonView.RPC("OnPlayerDisconnected", RpcTarget.AllBuffered, myId);
        GameManager.Instance.players.Remove(this);
    }

    void AddMeToPlayerList()
    {
        GameManager.Instance.players.Add(this);
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + Time.deltaTime * speed * (Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical")));
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            DropFallObject();
        }

        Raycast();

        if (isThrowable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                throwableObject.GetPhotonView().RPC("ThrowMe", RpcTarget.AllBuffered, transform.forward*throwForce);
                isHandEmpty = true;
                isThrowable = false;
                throwableObject = null;
            }

            if (Input.GetMouseButtonDown(1))
            {
                throwableObject.GetPhotonView().RPC("DropMe", RpcTarget.AllBuffered);

                isHandEmpty = true;
                isThrowable = false;
                throwableObject = null;
            }
        }

        if (!isHandEmpty && Input.GetKeyDown(KeyCode.X))
        {
            photonView.RPC("DropKitchenElement", RpcTarget.AllBuffered);
            handObjectType = Ingredients.Empty;
        }

    }
    private void Raycast()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            hitPoint = hit.point;

            var direction = (hitPoint - transform.position).normalized;
            direction.y = 0;

            transform.LookAt(transform.position + direction);

            if (isHandEmpty && Input.GetMouseButtonDown(0) && hit.collider.GetComponent<KitchenElement>())
            {

                photonView.RPC("GrabKitchenElement", RpcTarget.AllBuffered, (int)hit.collider.GetComponent<KitchenElement>().type);
            }


            if (hit.collider.gameObject.GetComponent<Plate>() && hit.collider.gameObject.GetComponent<Plate>().PlateTeam == myTeam)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    if (isHandEmpty && hitPlate.type != OrderType.Empty)
                    {
                        this.photonView.RPC("TakePlate", RpcTarget.AllBuffered, (int)hitPlate.type);
                        isHandEmpty = false;
                    }

                    if (!isHandEmpty)
                    {
                        ControlPlateStackList();
                        if (hitPlate.type == OrderType.Empty && !isSame)
                        {
                            hit.collider.GetComponent<PhotonView>().RPC("PutPlate", RpcTarget.AllBuffered, (int)handObjectType);
                            photonView.RPC("CloseHandObj", RpcTarget.AllBuffered);
                            isHandEmpty = true;
                        }
                    }
                }

                if (Input.GetMouseButtonDown(1) && hitPlate.ingredientCount>0 && hitPlate.type == OrderType.Empty)
                {
                    photonView.RPC("DropPlateIngredients", RpcTarget.AllBuffered);
                }
            }

            if (hit.collider.tag=="Throwable")
            {
                if (Input.GetKeyDown(KeyCode.E) && isHandEmpty)
                {
                    isHandEmpty = false;
                    isThrowable = true;

                    throwableObject = hit.collider.gameObject;
                    throwableObject.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);


                    throwableObject.GetComponent<ObjectStats>().photonView.RPC("AlignToPlayersHand", RpcTarget.AllBuffered, myId);
                }
            }

        }
    }

    [PunRPC]
    void TeamScore()
    {
        Score++;   
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "FallObject" && col.GetComponent<SceneObject>().myTeam != myTeam)
        {
            StartCoroutine(Fall());
        }

        if (col.tag == "Customer" && col.GetComponent<Customer>().myOrder == handPlate.GetComponent<HandPlate>().inPlateType)
        {
            photonView.RPC("TeamScore", RpcTarget.AllBuffered);

            handPlate.inPlateType = OrderType.Empty;
            handPlate.inPlate.text = string.Empty;
            handPlate.gameObject.SetActive(false);

            isHandEmpty = true;
        }
    }

    [PunRPC]
    void GrabKitchenElement(int type)
    {
        handObjectType = (Ingredients)type;
        ingredientList[(int)handObjectType].SetActive(true);
        isHandEmpty = false;
    }

    [PunRPC]
    void DropKitchenElement()
    {
        ingredientList[(int)handObjectType].SetActive(false);

        handPlate.inPlateType = OrderType.Empty;
        handPlate.inPlate.text = string.Empty;
        handPlate.gameObject.SetActive(false);

        isHandEmpty = true;
    }

    #region Fall and Slowdown
    IEnumerator Fall()
    {
        var tempSpeed = speed;
        speed = 0;
        yield return new WaitForSeconds(1.5f);
        speed = tempSpeed;
    }

    IEnumerator SlowDown()
    {
        var tempSpeed = speed;
        speed /= 2;
        yield return new WaitForSeconds(1.5f);
        speed = tempSpeed;
    }
    #endregion

    void DropFallObject()
    {
        var obj = PhotonNetwork.Instantiate("Banana", new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity).GetComponent<SceneObject>();
        obj.photonView.RPC("SetTeam", RpcTarget.AllBuffered, (int)myTeam);
    }

    [PunRPC]
    void CloseHandObj()
    {
        for (int i = 0; i < ingredientList.Count; i++)
        {
            ingredientList[i].SetActive(false);
        }
    }

    void ControlPlateStackList()
    {
        if (hitPlate.ingredientStack.Count==0)
        {
            isSame = false;
        }
        else
        {
            for (int i = 0; i < hitPlate.ingredientStack.Count; i++)
            {
                isSame = hitPlate.ingredientStack[i] == handObjectType;
            }
        }
    }

    [PunRPC]
    void TakePlate(int inPlate)
    {
        handPlate.gameObject.SetActive(true);
        handPlate.inPlateType = (OrderType)inPlate;
        handPlate.inPlate.text = ((OrderType)inPlate).ToString();

        EmptyPlate();
    }

    void EmptyPlate()
    {
        hitPlate.ingredientStack.Clear();
        hitPlate.plateText.text = string.Empty;
        hitPlate.ingredientCount = 0;
    }
    [PunRPC]
    void DropPlateIngredients()
    {
        hitPlate.ingredientStack.RemoveAt(hitPlate.ingredientStack.Count - 1);
        hitPlate.ingredientCount--;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myTeam);
            GetComponent<Renderer>().material.color = myTeam == Team.Team1 ? Color.blue : Color.red;
            stream.SendNext(myId);
        }
        else
        {
            myTeam = (Team)stream.ReceiveNext();
            GetComponent<Renderer>().material.color = myTeam == Team.Team1 ? Color.blue : Color.red;
            myId=(int)stream.ReceiveNext();
        }
    }


}