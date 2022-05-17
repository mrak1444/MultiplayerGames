using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Photon.Pun.Demo.PunBasics;
using PlayFab;
using PlayFab.ClientModels;

public class StartGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _player;

    private PlayerManager _playerManager;

    private void Start()
    {
        var player = PhotonNetwork.Instantiate(_player.name, new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5)), Quaternion.identity);
        _playerManager = player.GetComponent<PlayerManager>();
        
        GetUserData("B37A5");
    }

    private void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = null,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");

            if (result.Data.Count == 0)
            {
                SetUserData();
            }
            else
            {
                var value = result.Data["Health"].Value;
                Debug.Log("Health: " + value);
                _playerManager.Health = float.Parse(value);
            }

        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"Health", _playerManager.Health.ToString()},
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
