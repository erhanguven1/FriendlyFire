using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Plate[] hitPlates;
    public List<PlayerMovement> players = new List<PlayerMovement>();

    public void OnJoined(PlayerMovement playerToAdd)
    {
        foreach (var item in FindObjectsOfType<PlayerMovement>())
        {
            players.Add(item);
        }
    }

    private void Awake()
    {
        Instance = this; 
    }
}
