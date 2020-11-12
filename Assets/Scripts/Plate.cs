using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private PlayerMovement player;
    public TextMesh plateText;

    public List<Ingredients> ingredientStack;
    public OrderType type;

    int ingredientCount;

    public void OnAddIngredient(Ingredients ingredient)
    {
        ingredientCount++;
        bool succeed = false;

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
        player.photonView.RPC("DisplayType", Photon.Pun.RpcTarget.AllBuffered, (int)type);
    }

    [Photon.Pun.PunRPC]

    void DisplayType(int t)
    {
        plateText.text = ((OrderType)t).ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
