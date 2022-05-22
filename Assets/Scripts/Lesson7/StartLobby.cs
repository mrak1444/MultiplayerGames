using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class StartLobby : MonoBehaviourPunCallbacks
{
    [Header("Account")]
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
    [SerializeField] private GameObject _panelAccount;

    [Header("Character")]
    [SerializeField] private Button[] _buttonsAddCharacter;
    [SerializeField] private Button[] _buttonsSelectCharacter;
    [SerializeField] private GameObject _panelCharacter;
    [SerializeField] private GameObject _pamelCreateCharacter;
    [SerializeField] private InputField _inputFieldNameCreateCharacter;
    [SerializeField] private Button _buttonCreateCharacter;
    [SerializeField] private Button _buttonBack;

    [Header("Other")]
    [SerializeField] private GameObject _loaderIcon;
    [SerializeField] private byte maxPlayers = 4;

    private RoomOptions _roomOptions = new RoomOptions();
    private TypedLobby _customLobby = new TypedLobby("customLobby", LobbyType.Default);
    private string _roomName = "room1";
    private List<CharacterResult> _characters = new List<CharacterResult>();
    private string _itemId = "character_token";

    private void Start()
    {
        _buttonConnect.onClick.AddListener(ConnectToServer);
        _buttonPanelCreateAccount.onClick.AddListener(PanelAccountPlayFab);
        _buttonCreateAccount.onClick.AddListener(CreateAccountPlayFab);
        _buttonCreateAccountBack.onClick.AddListener(PanelCreateAccountBack);

        for (int i = 0; i < _buttonsAddCharacter.Length; i++)
        {
            _buttonsAddCharacter[i].onClick.AddListener(() => { _pamelCreateCharacter.SetActive(true); });
        }

        _buttonBack.onClick.AddListener(() => { _pamelCreateCharacter.SetActive(false); });
        _buttonCreateCharacter.onClick.AddListener(CreateCharacter);

        //
    }

    private void CreateCharacter()
    {
        _loaderIcon.SetActive(true);

        Debug.Log("CreateCharacter");

        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "1",
            ItemId = _itemId,
            Price = 1,
            VirtualCurrency = "GD"
        }, Debug.Log, Debug.LogError);

        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _inputFieldNameCreateCharacter.text,
            ItemId = _itemId
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
        }, Debug.LogError);

        _pamelCreateCharacter.SetActive(false);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        Debug.Log("UpdateCharacterStatistics");
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"XP", 0},
                {"Gold", 0}
            }
        }, result =>
        {
            Debug.Log($"Initial stats set, telling client to update character list");
            //CloseCreateNewCharacterPrompt();
            GetCharacters();
        },
        Debug.LogError);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
        res =>
        {
            Debug.Log($"Characters owned: + {res.Characters.Count}");
            if (res.Characters.Count > 0)
            {
                if (_characters.Count > 0)
                {
                    _characters.Clear();
                }

                foreach (var character in res.Characters)
                {
                    _characters.Add(character);
                }

                var num = _characters.Count < 2 ? _characters.Count : 2;

                for (int i = 0; i < num; i++)
                {
                    var name = _characters[i].CharacterName;

                    _buttonsSelectCharacter[i].onClick.AddListener(delegate { StartGame(name); });

                    _buttonsSelectCharacter[i].gameObject.SetActive(true);
                    _buttonsAddCharacter[i].gameObject.SetActive(false);
                    _buttonsSelectCharacter[i].transform.GetChild(0).GetComponent<Text>().text = name;
                    
                    UpdeteCharacterView(_characters[i].CharacterId, i);
                }
            }
            else
            {
                _loaderIcon.SetActive(false);
            }

        }, Debug.LogError);
    }

    private void StartGame(string nameCharacter)
    {
        PhotonNetwork.NickName = nameCharacter;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    private void UpdeteCharacterView(string characterId, int i)
    {
        PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
        {
            CharacterId = characterId
        }, result =>
        {
            _buttonsSelectCharacter[i].transform.GetChild(1).GetComponent<Text>().text = result.CharacterStatistics["Level"].ToString();
            _buttonsSelectCharacter[i].transform.GetChild(2).GetComponent<Text>().text = result.CharacterStatistics["XP"].ToString();
            _buttonsSelectCharacter[i].transform.GetChild(3).GetComponent<Text>().text = result.CharacterStatistics["Gold"].ToString();

            _loaderIcon.SetActive(false);
        }, Debug.LogError);
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
        _loaderIcon.SetActive(true);
        _panelAccount.SetActive(false);
        _panelCharacter.SetActive(true);
        
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _inputFieldNickName.text,
            Password = _inputFieldPasword.text
        }, result =>
        {
            Debug.Log($"Success: {_inputFieldNickName.text}");
            GetCharacters();
            //PhotonNetwork.ConnectUsingSettings();
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
