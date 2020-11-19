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
    int TableNumber;
    bool CanWalk;
    NavMeshAgent Agent;
    bool isFound = false, CanOrder=false, isOrdered;
    void Start()
    {
        Agent = this.GetComponent<NavMeshAgent>();

        if (PhotonNetwork.IsMasterClient)
        {
            PlaceCustomer();
        }

        if (!isFound)
        {
            FindAndGo();
        }
    }

     void Update()
    {
        if (Agent.remainingDistance<2f && !isOrdered)
        {
            CanOrder = true;
            if (CanOrder)
            {
                Invoke("Order", 2f);
                CanOrder = false;
                isOrdered = true;
            }
            
        }
    }

    public void CompareOrder(OrderType input)
    {
        if (true)
        {

        }
    }

    void CompareTables()
    {
        
        for (int i = 0; i < TableInitializer.Instance.unusedTableList.Count; i++)
        {
            if (TableInitializer.Instance.tableList[TableNumber] == TableInitializer.Instance.unusedTableList[i])
            {
                TableInitializer.Instance.unusedTableList.RemoveRange(i, 0);
                Agent.SetDestination(TableInitializer.Instance.tableList[TableNumber].transform.position);
                isFound = true;
                break;
            }

           if (i == TableInitializer.Instance.unusedTableList.Count-1 && !isFound)
            {
                isFound = false;
                FindAndGo();
                break;
            }
        }
    }

    void FindAndGo()
    {
        TableNumber = Random.RandomRange(0, TableInitializer.Instance.tableList.Count - 1);
        CompareTables();
    }

    void PlaceCustomer()
    {
        var tableToSit = TableInitializer.Instance.unusedTableList.RandomItem();
        TableInitializer.Instance.unusedTableList.Remove(tableToSit);
    }

    void Order()
    {
        int r = Random.Range(0,6);
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
