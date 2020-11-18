using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Plate : MonoBehaviour
{
    private PlayerMovement player;
    public TextMesh plateText;

    public List<Ingredients> ingredientStack;
    public OrderType type;

    public Team PlateTeam;

    public int ingredientCount;


    public static Plate Instance;


    void Awake()
    {
        Instance = this;   
    }

    public void OnAddIngredient(Ingredients ingredient)
    {
        ingredientCount++;
        

        if (ingredientCount==1)
        {
            if ((int)ingredient<3)
            {
                type = (OrderType)((int)ingredient);
                TypeCalculated();
                return;
            }
        }

        if (ingredientCount == 2)
        {
            if (ingredientStack[0]==Ingredients.Potato && ingredientStack[1] == Ingredients.Sos)
            {
                type = OrderType.Fries;

                TypeCalculated();
                return;
            }
        }

        if (ingredientCount == 3)
        {
            if (ingredientStack[0] == Ingredients.Bread && ingredientStack[1] == Ingredients.Meatball && ingredientStack[2] == Ingredients.Lettuce)
            {
                type = OrderType.Burger;

                TypeCalculated();
                return;
            }

            if (ingredientStack[0] == Ingredients.Bread && ingredientStack[1] == Ingredients.Wienie && ingredientStack[2] == Ingredients.Sos)
            {
                type = OrderType.Hotdog;

                TypeCalculated();
                return;
            }
        }

        if (ingredientCount == 4)
        {
            if (ingredientStack[0] == Ingredients.Dough && ingredientStack[1] == Ingredients.Sausage && ingredientStack[2] == Ingredients.Cheese
                && ingredientStack[3] == Ingredients.Sos)
            {
                type = OrderType.Pizza;

                TypeCalculated();
                return;
            }
        }
    }

    void TypeCalculated()
    {
        
        this.GetComponent<PhotonView>().RPC("DisplayType", RpcTarget.AllBuffered, (int)type);
        
    }

    [PunRPC]

    void DisplayType(int t)
    {
        plateText.text = ((OrderType)t).ToString();
    }



    

    [Photon.Pun.PunRPC]
    void PutPlate(int HandElement)
    {
        ingredientStack.Add((Ingredients)HandElement);
        OnAddIngredient((Ingredients)HandElement);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        type = OrderType.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (ingredientCount==0)
        {
            type = OrderType.Empty;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            print(ingredientStack.Count);
            for (int i = 0; i < ingredientStack.Count; i++)
            {
                print(ingredientStack[i].ToString()+"\n");
            }
        }

        if (ingredientCount<0)
        {
            ingredientCount = 0;
        }
    }

    
}
