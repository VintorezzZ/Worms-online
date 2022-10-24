using System;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class ClientGrenade : Explode
{
    [SerializeField] private GameObject _explosionPrefab;
    
    [ClientRpc]
    public void OnCollisionEnterClientRpc(Vector3 position)
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        cutter.transform.position = position;
        Invoke(nameof(DoCut), 0.001f);
    }
}
