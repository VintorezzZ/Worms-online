using System;
using Client;
using DefaultNamespace;
using Server;
using Unity.Netcode;
using UnityEngine;

public class ClientPlayer : NetworkBehaviour
{
    private ServerPlayer _serverPlayer;
    [SerializeField] private BaseWeapon _weaponPrefab;
    [SerializeField] private Transform _weaponRoot;
    private ClientWeapon _weapon;

    private void Awake()
    {
        _serverPlayer = GetComponent<ServerPlayer>();
    }

    public override void OnNetworkSpawn()
    {
        enabled = IsClient && IsLocalPlayer;
        if (!enabled)
            return;

        if (IsLocalPlayer)
        {
            CameraFollow360.player = transform;
        }
        
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        _weapon?.Process();
    }

    [ClientRpc]
    public void OnChangeWeaponClientRpc()
    {
        _weapon = GetComponentInChildren<ClientWeapon>();
        _weapon.serverOwner = _serverPlayer;
    }
}
