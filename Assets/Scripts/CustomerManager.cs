using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;
    public Transform spawnPoint;
    void Awake()
    {
        Instance = this;
    }


    public int startCustomerCount;
    public IEnumerator InitializeCustomers()
    {
        for (int i = 0; i < startCustomerCount; i++)
        {
            var customer = PhotonNetwork.Instantiate("Customer", spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }

}
