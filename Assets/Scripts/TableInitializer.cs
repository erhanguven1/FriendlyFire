using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TableInitializer : MonoBehaviour
{
    
    public static TableInitializer Instance;
    void Awake()
    {
        Instance = this;
    }

    public Vector2Int area;
    public GameObject tablePrefab;

    int TableNumber;
    bool CanWalk;

    public List<GameObject> tableList, unusedTableList;

    // Start is called before the first frame update

    void Start()
    {
        
    }
    public void InitializeTables()
    {

        for (int i = 0; i < area.x; i+=5)
        {
            for (int j = 0; j < area.y; j+=5)
            {
                var t = Instantiate(tablePrefab, transform);
                tableList.Add(t);
                unusedTableList.Add(t);
                t.transform.localPosition = new Vector3(i, 0, j);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            CustomerManager.Instance.InitializeCustomers();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
