using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public List<GameObject> ingredientList;
    public GameObject HandPlate;
    public Ingredients handObjectType;
    public GameObject handObject;
    bool isHandEmpty = true, isSame=false;

    public Team myTeam;
    public float speed;
    int Score=0;



    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myTeam);
            GetComponent<Renderer>().material.color = myTeam == Team.Team1 ? Color.blue : Color.red;
        }
        else
        {
            myTeam = (Team)stream.ReceiveNext();
            GetComponent<Renderer>().material.color = myTeam == Team.Team1 ? Color.blue : Color.red;
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

        if (col.tag == "Customer" && col.GetComponent<Customer>().myOrder == HandPlate.GetComponent<HandPlate>().inPlateType)
        {

            photonView.RPC("TeamScore", RpcTarget.AllBuffered);
            HandPlate.GetComponent<HandPlate>().inPlateType = OrderType.Empty;
            HandPlate.GetComponent<HandPlate>().inPlate.text = string.Empty;
            HandPlate.SetActive(false);
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
        HandPlate.GetComponent<HandPlate>().inPlateType = OrderType.Empty;
        HandPlate.GetComponent<HandPlate>().inPlate.text = string.Empty;
        HandPlate.SetActive(false);
        isHandEmpty = true;
    }

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

    void DropFallObject()
    {
        var obj = PhotonNetwork.Instantiate("Banana", new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity).GetComponent<SceneObject>();
        obj.photonView.RPC("SetTeam", RpcTarget.AllBuffered, (int)myTeam);
    }

    void Awake()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
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
        if (Plate.Instance.ingredientStack.Count==0)
        {
            isSame = false;
        }
        else
        {
            for (int i = 0; i < Plate.Instance.ingredientStack.Count; i++)
            {
                if (Plate.Instance.ingredientStack[i] == handObjectType)
                {
                    isSame = true;
                }
                else
                {
                    isSame = false;
                }
            }
        }
    }


    [PunRPC]
    void TakePlate(int inPlate)
    {
        HandPlate.SetActive(true);
        HandPlate.GetComponent<HandPlate>().inPlateType = (OrderType)inPlate;
        HandPlate.GetComponent<HandPlate>().inPlate.text = ((OrderType)inPlate).ToString();
        var TP = Plate.Instance;
        TP.ingredientStack.Clear();
        TP.plateText.text = string.Empty;
        TP.ingredientCount = 0;
        
    }

    [PunRPC]
    void DropPlateIngredients()
    {
        var TP = Plate.Instance;
        TP.ingredientStack.RemoveRange(TP.ingredientStack.Count - 1, 1);
        TP.ingredientCount--;

    }

    // Update is called once per frame
    void Update()
    {

        transform.position += Time.deltaTime*speed*(Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical"));

        if (Input.GetMouseButtonDown(2))
        {
            DropFallObject();
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit))
        {
            if (isHandEmpty && Input.GetMouseButtonDown(0) && hit.collider.GetComponent<KitchenElement>())
            {
                
                photonView.RPC("GrabKitchenElement", RpcTarget.AllBuffered, (int)hit.collider.GetComponent<KitchenElement>().type);
            }

            if (isHandEmpty && hit.collider.gameObject.GetComponent<Plate>() && Plate.Instance.type!=OrderType.Empty && Plate.Instance.PlateTeam==myTeam && Input.GetMouseButtonDown(0))
            {
                this.photonView.RPC("TakePlate", RpcTarget.AllBuffered, (int)Plate.Instance.type);
                isHandEmpty = false;
            }

            if (!isHandEmpty && hit.collider.GetComponent<Plate>() && Plate.Instance.PlateTeam==myTeam && Input.GetMouseButtonDown(0))
            {
                ControlPlateStackList();
                if (Plate.Instance.type==OrderType.Empty && !isSame)
                {
                    hit.collider.GetComponent<PhotonView>().RPC("PutPlate", RpcTarget.AllBuffered, (int)handObjectType);
                    photonView.RPC("CloseHandObj", RpcTarget.AllBuffered);
                    isHandEmpty = true;
                }
            }

            if (hit.collider.GetComponent<Plate>() && Input.GetMouseButtonDown(1))
            {
                photonView.RPC("DropPlateIngredients", RpcTarget.AllBuffered);
            }
           
        }


        if (!isHandEmpty && Input.GetKeyDown(KeyCode.X))
        {
            photonView.RPC("DropKitchenElement", RpcTarget.AllBuffered);

        }

        
    }

}
