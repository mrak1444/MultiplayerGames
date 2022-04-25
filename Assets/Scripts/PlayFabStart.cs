using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabStart : MonoBehaviour
{
    [SerializeField] private Button _connectButton;
    [SerializeField] private Text _text;

    void Start()
    {
        _connectButton.onClick.AddListener(ConnectPlayFab);
    }

    private void ConnectPlayFab()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "B37A5";
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "MrAK",
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made successful API call!");
        _text.text = "Congratulations, you made successful API call!";
        _text.color = Color.green;
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        _text.text = $"Something went wrong: {errorMessage}";
        _text.color = Color.red;
    }
}
