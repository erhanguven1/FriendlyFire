using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Customer : SceneObject, IPunObservable
{
    public OrderType myOrder;
    public TextMesh orderText;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlaceCustomer();
        }
    }

    public void CompareOrder(OrderType input)
    {
        if (true)
        {

        }
    }

    void PlaceCustomer()
    {
        var tableToSit = TableInitializer.Instance.unusedTableList.RandomItem();
        TableInitializer.Instance.unusedTableList.Remove(tableToSit);

        transform.position = tableToSit.transform.position + Vector3.up * 2;
        Invoke("Order", 2f);
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
