using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    [SerializeField] private Button _buttonBack;
    [SerializeField] private Button _buttonSignIn;
    [SerializeField] private InputField _inputFieldUsername;
    [SerializeField] private InputField _inputFieldPassword;
    [SerializeField] private GameObject _objStart;
    [SerializeField] private GameObject _objLoadingIcon;

    private string _username;
    private string _userPass;

    private void Start()
    {
        _buttonBack.onClick.AddListener(ButtonBack);
        _buttonSignIn.onClick.AddListener(ButonSignIn);
        _inputFieldUsername.onEndEdit.AddListener(UpdateUsername);
        _inputFieldPassword.onEndEdit.AddListener(UpdatePassword);
    }

    public void UpdateUsername(string username) => _username = username;
    public void UpdatePassword(string pass) => _userPass = pass;

    private void ButonSignIn()
    {
        _objLoadingIcon.SetActive(true);
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _userPass
        }, result =>
        {
            _objLoadingIcon.SetActive(false);
            Debug.Log($"Success: {_username}");
        }, error =>
        {
            _objLoadingIcon.SetActive(false);
            Debug.LogError($"Fail: {error.ErrorMessage}");
        });
    }

    private void ButtonBack()
    {
        gameObject.SetActive(false);
        _objStart.SetActive(true);
    }
}
