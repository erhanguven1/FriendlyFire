using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviourPun
{
    public Team myTeam;
    
    [PunRPC]
    public void SetTeam(int team)
    {
        myTeam = (Team)team;
    }

}
