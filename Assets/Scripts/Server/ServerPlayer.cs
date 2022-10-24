using System;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

namespace Server
{
    public class ServerPlayer : NetworkBehaviour
    {
        private ClientPlayer _clientPlayer;
        [SerializeField] private BaseWeapon _weaponPrefab;
        [SerializeField] private Transform _weaponRoot;
        public ServerWeapon _weapon;

        private void Awake()
        {
            _clientPlayer = GetComponent<ClientPlayer>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient && !IsHost)
            {
                enabled = false;
                return;
            }
            
            _weapon = Instantiate(_weaponPrefab, _weaponRoot.position, _weaponRoot.rotation) as ServerWeapon;
            var networkWeapon = _weapon.GetComponent<NetworkObject>();
            networkWeapon.Spawn();
            networkWeapon.transform.parent = this.transform;
            _clientPlayer.OnChangeWeaponClientRpc();
            base.OnNetworkSpawn();
        }
        
        [ServerRpc]
        public void ThrowServerRpc(Vector3 delta)
        {
            _weapon.Attack(delta);
        }
    }
}