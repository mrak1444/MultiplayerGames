using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Button _buttonSignin;
    [SerializeField] private Button _buttonReristration;
    [SerializeField] private GameObject _objSignin;
    [SerializeField] private GameObject _objReristration;

    private void Start()
    {
        _buttonSignin.onClick.AddListener(Signin);
        _buttonReristration.onClick.AddListener(Reristration);
    }

    private void Reristration()
    {
        gameObject.SetActive(false);
        _objReristration.SetActive(true);
    }

    private void Signin()
    {
        gameObject.SetActive(false);
        _objSignin.SetActive(true);
    }
}
