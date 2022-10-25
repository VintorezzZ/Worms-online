using System;
using DefaultNamespace;
using Unity.Collections;
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
        public DamageReceiver damageReceiver;
        
        private NetworkVariable<FixedString32Bytes> _playerName = new NetworkVariable<FixedString32Bytes>("Player");
        
        public FixedString32Bytes Name => _playerName.Value;
        public NetworkLifeState NetLifeState { get; private set; }
        public NetworkHealthState NetHealthState { get; private set; }

        
        private void Awake()
        {
            _clientPlayer = GetComponent<ClientPlayer>();
            damageReceiver = GetComponent<DamageReceiver>();
            NetLifeState = GetComponent<NetworkLifeState>();
            NetHealthState = GetComponent<NetworkHealthState>();
        }

        public override void OnNetworkSpawn()
        {
            enabled = IsServer;
            if (!enabled)
                return;

            NetHealthState.HitPoints.Value = 100;
            NetLifeState.LifeState.OnValueChanged += OnLifeStateChanged;
            damageReceiver.DamageReceived += ReceiveHP;
            damageReceiver.CollisionEntered += CollisionEntered;
            
            _weapon = Instantiate(_weaponPrefab, _weaponRoot.position, _weaponRoot.rotation) as ServerWeapon;
            _weapon.owner = this;
            var networkWeapon = _weapon.GetComponent<NetworkObject>();
            networkWeapon.SpawnWithOwnership(OwnerClientId);
            networkWeapon.transform.parent = this.transform;
            _clientPlayer.OnChangeWeaponClientRpc();
            
            base.OnNetworkSpawn();
        }
        
        public override void OnNetworkDespawn()
        {
            NetLifeState.LifeState.OnValueChanged -= OnLifeStateChanged;

            if (damageReceiver)
            {
                damageReceiver.DamageReceived -= ReceiveHP;
                damageReceiver.CollisionEntered -= CollisionEntered;
            }
        }

        private void CollisionEntered(Collision obj)
        {
            
        }

        /// <summary>
        /// Receive an HP change from somewhere. Could be healing or damage.
        /// </summary>
        /// <param name="inflicter">Person dishing out this damage/healing. Can be null. </param>
        /// <param name="HP">The HP to receive. Positive value is healing. Negative is damage.  </param>
        private void ReceiveHP(ServerPlayer inflicter, int HP)
        {
            //to our own effects, and modify the damage or healing as appropriate. But in this game, we just take it straight.
            if (HP > 0)
            {
                
            }
            else
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                // Don't apply damage if god mode is on
                if (NetLifeState.IsGodMode.Value)
                {
                    return;
                }
#endif
            }

            NetHealthState.HitPoints.Value = Mathf.Clamp(NetHealthState.HitPoints.Value + HP, 0, 100);

            //we can't currently heal a dead character back to Alive state.
            //that's handled by a separate function.
            if (NetHealthState.HitPoints.Value <= 0)
            {
                NetLifeState.LifeState.Value = LifeState.Dead;
            }
        }

        private void OnLifeStateChanged(LifeState prevLifeState, LifeState lifeState)
        {
            if (lifeState != LifeState.Alive)
            {
                
            }
        }
        
        [ServerRpc]
        public void DisconnectServerRpc()
        {
            NetworkManager.Singleton.DisconnectClient(OwnerClientId);
        }
    }
}