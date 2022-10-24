using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUi : MonoBehaviour
{
    [SerializeField] private Button _connectBtn;
    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _serverBtn;
    
    private void Awake()
    {
        _connectBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        _hostBtn.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        _serverBtn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
    }
}
