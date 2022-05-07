using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotonStart1 : MonoBehaviour, ILobbyCallbacks, IConnectionCallbacks, IMatchmakingCallbacks
{
    [SerializeField] private AppSettings _appSettings = new AppSettings();
    [SerializeField] private byte maxPlayers = 4;
    [SerializeField] private GameObject _loadIcon;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<Text> _buttonsText;
    [SerializeField] private Button _createRoom;
    [SerializeField] private InputField _roomNameForCreate;
    [SerializeField] private Text _textInfo;
    [SerializeField] private Button _closeRoom;
    [SerializeField] private Button _invisibleRoom;
    [SerializeField] private InputField _roomNameforJoin;
    [SerializeField] private Button _joinRoom;

    private LoadBalancingClient _lbc;
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();
    private bool _flag = false;

    private void Start()
    {
        _createRoom.onClick.AddListener(CreateRoom);
        _createRoom.interactable = false;

        _closeRoom.onClick.AddListener(CloseRoom);
        _closeRoom.interactable = false;

        _joinRoom.onClick.AddListener(JoinRoom);

        _invisibleRoom.onClick.AddListener(InvasibleRoom);
        _invisibleRoom.interactable = false;

        _loadIcon.SetActive(true);

        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);

        if (!_lbc.ConnectUsingSettings(_appSettings)) Debug.LogError("Error while connecting");
    }

    public void Update()
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        var state = _lbc.State.ToString();

        if (_flag && !_lbc.CurrentRoom.IsOpen) _textInfo.text = "Room is close";
        if (_flag && !_lbc.CurrentRoom.IsVisible) _textInfo.text = "Room is invisible";
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                _cachedRoomList.Remove(info.Name);
            }
            else
            {
                _cachedRoomList[info.Name] = info;
            }
        }
    }

    private void ShowListRooms()
    {
        int num = _cachedRoomList.Count < 11 ? _cachedRoomList.Count : 11;

        foreach(var room in _cachedRoomList)
        {
            int i = 0;
            if(i < num)
            {
                _buttonsText[i].text = room.Key;

                _buttons[i].interactable = true;
                _buttons[i].onClick.AddListener(delegate { ClickButtons((RoomInfo)room.Value); });

                i++;
            }
        }
    }

    private void ClickButtons(RoomInfo rm)
    {
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = rm.Name;
        _lbc.OpJoinRoom(enterRoomParams);
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.Lobby = _customLobby;
        enterRoomParams.RoomName = _roomNameForCreate.text;
        enterRoomParams.RoomOptions = roomOptions;
        _lbc.OpCreateRoom(enterRoomParams);
    }

    private void CloseRoom()
    {
        _lbc.CurrentRoom.IsOpen = false;
        
    }

    private void JoinRoom()
    {
        EnterRoomParams enterRoomParams = new EnterRoomParams();
        enterRoomParams.RoomName = _roomNameforJoin.text;
        _lbc.OpJoinRoom(enterRoomParams);
    }

    private void InvasibleRoom()
    {
        _lbc.CurrentRoom.IsVisible = false;
    }

    public void OnConnected()
    {
        Debug.Log("1");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("2");
        _lbc.OpJoinLobby(_customLobby);
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("3");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("4");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("5");
        _cachedRoomList.Clear();
    }

    public void OnJoinedLobby()
    {
        Debug.Log("6");
        _cachedRoomList.Clear();
    }

    public void OnLeftLobby()
    {
        Debug.Log("7");
        _cachedRoomList.Clear();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("8");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("9");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)   //10
    {
        Debug.Log("10");
        _loadIcon.SetActive(false);
        _createRoom.interactable = true;
        UpdateCachedRoomList(roomList);
        ShowListRooms();
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("11");
    }

    public void OnCreatedRoom()
    {
        Debug.Log("12");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("13");
    }

    public void OnJoinedRoom()
    {
        _textInfo.text = "JoinedRoom!";
        _closeRoom.interactable = true;
        _invisibleRoom.interactable = true;
        _createRoom.interactable = false;
        _flag = true;
        Debug.Log("14");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        _textInfo.text = "Join Room Failed";
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("16");
    }

    public void OnLeftRoom()
    {
        Debug.Log("17");
    }
}
