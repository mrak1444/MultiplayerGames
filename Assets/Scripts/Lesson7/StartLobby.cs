using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class StartLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _buttonConnect;
    [SerializeField] private InputField _inputFieldNickName;
    [SerializeField] private byte maxPlayers = 4;

    private RoomOptions _roomOptions = new RoomOptions();
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private string _roomName = "room1";

    private void Start()
    {
        _buttonConnect.onClick.AddListener(ConnectToServer);
        PhotonNetwork.NickName = _inputFieldNickName.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
    }

    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        _roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.JoinOrCreateRoom(_roomName, _roomOptions, _customLobby);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        if(!PhotonNetwork.InRoom) PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        PhotonNetwork.LoadLevel("Game");
    }
}
