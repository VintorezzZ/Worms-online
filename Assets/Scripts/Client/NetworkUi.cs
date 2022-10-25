using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUi : MonoBehaviour
{
    [SerializeField] private Button _connectBtn;
    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _serverBtn;
    [SerializeField] private Button _disconnectBtn;
    
    private void Awake()
    {
        _connectBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        _hostBtn.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        _serverBtn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        _disconnectBtn.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.Shutdown();
            }
            else
            {
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<ServerPlayer>().DisconnectServerRpc();
            }
        });
    }
}
