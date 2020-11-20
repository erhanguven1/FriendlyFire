using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    public GameObject UI;
    public Dropdown teamSelector;
    public Text nameSelector;

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
        UI.SetActive(true);
    }

    public void SpawnCharacter()
    {
        var player = PhotonNetwork.Instantiate("Player", Vector3.right * Random.Range(-9, 9) + Vector3.up, Quaternion.identity).GetComponent<PlayerMovement>();
        player.myTeam = teamSelector.value == 0 ? Team.Team1 : Team.Team2;
        TableInitializer.Instance.InitializeTables();
        UI.SetActive(false);
    }
}
