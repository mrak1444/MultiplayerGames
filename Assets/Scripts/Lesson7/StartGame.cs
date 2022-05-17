using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class StartGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        PhotonNetwork.Instantiate(_player.name, new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5)), Quaternion.identity);
    }
}
