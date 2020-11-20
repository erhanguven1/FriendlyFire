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

public class GameManager : MonoBehaviourPun, IPunObservable
{
    public static GameManager Instance;
    public Plate[] hitPlates;
    public List<PlayerMovement> players = new List<PlayerMovement>();

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.IsWriting)
        {
            foreach (var item in players)
            {
                stream.SendNext(item);
            }
        }
        else
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                players.Add((PlayerMovement)stream.ReceiveNext());
            }

        }*/
    }

    private void Awake()
    {
        Instance = this; 
    }
}
