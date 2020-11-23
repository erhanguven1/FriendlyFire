using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectStats : MonoBehaviourPun
{

    public float objectMass;
    public Rigidbody myRigidbody;

    PlayerMovement player;

    int objectId;

    public static ObjectStats instance;

    void Awake()
    {
        instance = this;
    }

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
        player = GameManager.Instance.players[id];
        objectId = id;

        transform.SetParent(player.handObject.transform);
        transform.position = player.handObject.transform.position;
        GetComponent<Collider>().enabled = false;

        GetComponent<PhotonRigidbodyView>().enabled = false;

        myRigidbody.isKinematic = true;
        myRigidbody.useGravity = false;
    }

    [PunRPC]
    void DropMe()
    {
        GetComponent<PhotonRigidbodyView>().enabled = true;

        myRigidbody.isKinematic = false;
        myRigidbody.useGravity = true;

        GetComponent<Collider>().enabled = true;
        transform.SetParent(null);
    }

    [PunRPC]
    void ThrowMe(Vector3 force)
    {
        GetComponent<PhotonRigidbodyView>().enabled = true;

        myRigidbody.isKinematic = false;
        myRigidbody.useGravity = true;
        myRigidbody.AddForce(force / objectMass, ForceMode.Force);

        GetComponent<Collider>().enabled = true;
        transform.SetParent(null);
    }

    [PunRPC]
    void OnDisconnectedPlayer()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            if (GameManager.Instance.players[objectId] == GameManager.Instance.players[i])
            {
                print("oyunda");
                break;
            }
            else if(i == GameManager.Instance.players.Count-1)
            {
                GetComponent<PhotonRigidbodyView>().enabled = true;

                myRigidbody.isKinematic = false;
                myRigidbody.useGravity = true;

                GetComponent<Collider>().enabled = true;
                transform.SetParent(null);
            }
        }
    }
}
