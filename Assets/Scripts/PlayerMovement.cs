using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public Team myTeam;
    public float speed;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myTeam);
        }
        else
        {
            myTeam = (Team)stream.ReceiveNext();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "FallObject" && col.GetComponent<SceneObject>().myTeam != myTeam)
        {
            StartCoroutine(Fall());
        }
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

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime*speed*(Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical"));

        if (Input.GetMouseButtonDown(1))
        {
            DropFallObject();
        }
    }

    
}
