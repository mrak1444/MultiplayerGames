using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Realtime.Demo
{
    public class Test : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
    {
        [SerializeField] private AppSettings _appSettings = new AppSettings();
        [SerializeField] private byte maxPlayers = 4;
        [SerializeField] private Button _createRoom;
        [SerializeField] private Text StateUiText;

        private LoadBalancingClient _lbc;

        public const string MAP_PROP_KEY = "map";
        public const string GAME_MODE_PROP_KEY = "gm";
        public const string AI_PROP_KEY = "ai";

        //public Text StateUiText;

        public void Start()
        {
            _createRoom.enabled = false;

            _lbc = new LoadBalancingClient();
            _lbc.AddCallbackTarget(this);

            _createRoom.onClick.AddListener(CreateRoom);

            if (!_lbc.ConnectUsingSettings(_appSettings)) Debug.LogError("Error while connecting");
        }

        public void Update()
        {
            if (_lbc == null)
                return;

            _lbc.Service();

            var state = _lbc.State.ToString();

            if (StateUiText != null && !StateUiText.text.Equals(state)) StateUiText.text = "State: " + state + _lbc.UserId;
        }

        private void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.CustomRoomPropertiesForLobby = new string[3] { MAP_PROP_KEY, GAME_MODE_PROP_KEY, AI_PROP_KEY };
            roomOptions.CustomRoomProperties = new Hashtable { { MAP_PROP_KEY, 1 }, {GAME_MODE_PROP_KEY, 0 } };
            EnterRoomParams enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomOptions = roomOptions;
            _lbc.OpCreateRoom(enterRoomParams);
        }


        public void OnConnected()
        {
            Debug.Log("1");  //4
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");  //5
            //_lbc.OpJoinRandomRoom();    // joins any open room (no filter)
            _createRoom.enabled = true;
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.Log("2");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.Log("3");
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");  //1
            regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("4");
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            Debug.Log("5");
        }

        public void OnJoinedLobby()
        {
            Debug.Log("6");
        }

        public void OnLeftLobby()
        {
            Debug.Log("7");
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            Debug.Log("8");
        }

        public void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");  //6
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("10");
        }

        public void OnJoinedRoom()
        {
            Debug.Log($"OnJoinedRoom - {_lbc.CurrentRoom.PlayerCount}");  //7
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("11");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            _lbc.OpCreateRoom(new EnterRoomParams());
        }

        public void OnLeftRoom()
        {
            Debug.Log("12");
        }


        /// <summary>A callback of the RegionHandler, provided in OnRegionListReceived.</summary>
        /// <param name="regionHandler">The regionHandler wraps up best region and other region relevant info.</param>
        private void OnRegionPingCompleted(RegionHandler regionHandler)  
        {
            Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion); //2
            Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache); //3
            _lbc.ConnectToRegionMaster(regionHandler.BestRegion.Code);
        }
    }
}