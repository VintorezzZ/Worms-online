using System;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class ClientGrenade : Explode
{
    [SerializeField] private GameObject _explosionPrefab;
    
    public override void OnNetworkSpawn()
    {
        enabled = IsClient;
        if (!enabled)
            return;
        
        base.OnNetworkSpawn();
    }
    
    [ClientRpc]
    public void OnCollisionEnterClientRpc(Vector3 position)
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        if (IsServer)
            return;
        cutter.transform.position = position;
        Invoke(nameof(DoCut), 0.01f);
    }
}
