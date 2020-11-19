using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;
    public Transform Sp;
    void Awake()
    {
        Instance = this;
    }


    public int startCustomerCount = 5;
    public void InitializeCustomers()
    {
        for (int i = 0; i < startCustomerCount; i++)
        {
            var customer = PhotonNetwork.InstantiateRoomObject("Customer", Sp.transform.position, Quaternion.identity);

        }
    }

}
