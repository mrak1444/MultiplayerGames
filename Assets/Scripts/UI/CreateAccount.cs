using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccount : MonoBehaviour
{
    [SerializeField] private Button _buttonBack;
    [SerializeField] private Button _buttonSignIn;
    [SerializeField] private InputField _inputFieldUsername;
    [SerializeField] private InputField _inputFieldPassword;
    [SerializeField] private InputField _inputFieldEmail;
    [SerializeField] private GameObject _objStart;
    [SerializeField] private GameObject _objLoadingIcon;

    private string _username;
    private string _userMail;
    private string _userPass;


    private void Start()
    {
        _buttonBack.onClick.AddListener(ButtonBack);
        _buttonSignIn.onClick.AddListener(ButonSignIn);

        _inputFieldUsername.onEndEdit.AddListener(UpdateUsername);
        _inputFieldPassword.onEndEdit.AddListener(UpdatePassword);
        _inputFieldEmail.onEndEdit.AddListener(UpdateEmail);
    }

    private void ButonSignIn()
    {
        _objLoadingIcon.SetActive(true);
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _userMail,
            Password = _userPass,
            RequireBothUsernameAndEmail = true
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

    public void UpdateUsername(string username) => _username = username;
    public void UpdateEmail(string mail) => _userMail = mail;
    public void UpdatePassword(string pass) => _userPass = pass;

    private void ButtonBack()
    {
        gameObject.SetActive(false);
        _objStart.SetActive(true);
    }
}
