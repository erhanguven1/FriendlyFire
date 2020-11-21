using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;


public class Customer : SceneObject, IPunObservable
{

    public OrderType myOrder;
    public TextMesh orderText;
    NavMeshAgent agent;
    bool isOrdered;

    GameObject chosenTable;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        if (PhotonNetwork.IsMasterClient)
        {
            chosenTable = TableInitializer.Instance.unusedTableList.RandomItem();
            TableInitializer.Instance.unusedTableList.Remove(chosenTable);

            photonView.RPC("FindAndGo", RpcTarget.AllBuffered, chosenTable.transform.position);
        }
        else
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if (agent.remainingDistance<2f && !isOrdered)
        {
            isOrdered = true;
            orderText.transform.rotation = Quaternion.identity;
            Invoke("Order", 2f);
        }
    }

    public void CompareOrder(OrderType input)
    {
        if (true)
        {

        }
    }

    [PunRPC]
    void FindAndGo(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }

    void Order()
    {
        int r = Random.Range(0, Enum.GetNames(typeof(OrderType)).Length - 1);
        myOrder = (OrderType)r;
        orderText.text = myOrder.ToString();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)myOrder);
            stream.SendNext(orderText.text);
        }
        else
        {
            myOrder = (OrderType)((int)stream.ReceiveNext());
            orderText.text = (string)stream.ReceiveNext();
        }
    }
}
