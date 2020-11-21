using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectStats : MonoBehaviourPun
{

    public float objectMass;
    public Rigidbody myRigidbody;

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.GetComponent<PhotonView>())
        {
            photonView.TransferOwnership(col.collider.GetComponent<PhotonView>().Owner.ActorNumber);
        }
    }

    [PunRPC]
    void AlignToPlayersHand(int id)
    {
        myRigidbody = GetComponent<Rigidbody>();
        PlayerMovement player = GameManager.Instance.players[id];

        transform.SetParent(player.handObject.transform);
        transform.position = player.handObject.transform.position;
        GetComponent<Collider>().enabled = false;

        myRigidbody.isKinematic = true;
        myRigidbody.useGravity = false;
    }

    [PunRPC]
    void DropMe()
    {
        myRigidbody.isKinematic = false;
        myRigidbody.useGravity = true;

        GetComponent<Collider>().enabled = true;
        transform.SetParent(null);
    }

    [PunRPC]
    void ThrowMe(Vector3 force)
    {
        myRigidbody.isKinematic = false;
        myRigidbody.useGravity = true;
        myRigidbody.AddForce(force / objectMass, ForceMode.Force);

        GetComponent<Collider>().enabled = true;
        transform.SetParent(null);
    }
}
