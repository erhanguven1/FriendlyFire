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
    }

    
}
