using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Team { Team1, Team2 }
public enum NPCType { Customer, Kitchen }
public enum OrderType 
{
    Cola,
    Wine,
    Beer,
    Fries,
    Burger,
    Pizza,
    Hotdog,
    Empty
}

public enum Ingredients
{
    Cola,
    Wine,
    Beer,
    Bread,
    Meatball,
    Lettuce,
    Dough,
    Sausage,
    Cheese,
    Sos,
    Potato,
    Wienie,
    Empty
}

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;
    public Plate[] hitPlates;
    public List<PlayerMovement> players = new List<PlayerMovement>();


    private void Awake()
    {
        Instance = this; 
    }
}
