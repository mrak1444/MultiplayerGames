using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using PlayFab;
using PlayFab.ClientModels;

public class StartLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _buttonPanelCreateAccount;

    [SerializeField] private Button _buttonConnect;
    [SerializeField] private InputField _inputFieldNickName;
    [SerializeField] private InputField _inputFieldPasword;

    [SerializeField] private Button _buttonCreateAccount;
    [SerializeField] private Button _buttonCreateAccountBack;
    [SerializeField] private InputField _inputFieldNickNameCreateAccount;
    [SerializeField] private InputField _inputFieldEmailCreateAccount;
    [SerializeField] private InputField _inputFieldPaswordCreateAccount;

    [SerializeField] private GameObject _panelSignIn;
    [SerializeField] private GameObject _panelCreateAccount;

    [SerializeField] private byte maxPlayers = 4;

    private RoomOptions _roomOptions = new RoomOptions();
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private string _roomName = "room1";

    private void Start()
    {
        _buttonConnect.onClick.AddListener(ConnectToServer);
        _buttonPanelCreateAccount.onClick.AddListener(PanelAccountPlayFab);
        _buttonCreateAccount.onClick.AddListener(CreateAccountPlayFab);
        _buttonCreateAccountBack.onClick.AddListener(PanelCreateAccountBack);

        PhotonNetwork.NickName = _inputFieldNickName.text;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
    }

    private void PanelCreateAccountBack()
    {
        _panelSignIn.SetActive(true);
        _panelCreateAccount.SetActive(false);
        _buttonPanelCreateAccount.gameObject.SetActive(true);
    }

    private void CreateAccountPlayFab()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _inputFieldNickNameCreateAccount.text,
            Email = _inputFieldEmailCreateAccount.text,
            Password = _inputFieldPaswordCreateAccount.text,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            _panelSignIn.SetActive(true);
            _panelCreateAccount.SetActive(false);
            _buttonPanelCreateAccount.gameObject.SetActive(true);
            Debug.Log($"Success: {_inputFieldNickNameCreateAccount.text}");
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
    }

    private void PanelAccountPlayFab()
    {
        _panelSignIn.SetActive(false);
        _panelCreateAccount.SetActive(true);
        _buttonPanelCreateAccount.gameObject.SetActive(false);
    }

    private void ConnectToServer()
    {
        

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _inputFieldNickName.text,
            Password = _inputFieldPasword.text
        }, result =>
        {
            Debug.Log($"Success: {_inputFieldNickName.text}");
            PhotonNetwork.ConnectUsingSettings();
        }, error =>
        {
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
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
