using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectStats : MonoBehaviourPun
{

    public float objectMass;
    public Rigidbody myRigidbody;

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
}
