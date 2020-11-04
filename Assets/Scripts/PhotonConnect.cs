using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonConnect : MonoBehaviourPunCallbacks
{

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "0.1";
    }

    public override void OnConnectedToMaster()
    {
        print("Connected");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(string.Empty);
    }

    public override void OnJoinedRoom()
    {
        print("Joined room");
        SpawnCharacter();
    }

    void SpawnCharacter()
    {
        var player = PhotonNetwork.Instantiate("Player", Vector3.right * Random.Range(-9, 9), Quaternion.identity);

    }
}
