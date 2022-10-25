using System;
using Unity.Netcode;
using UnityEngine;

namespace Server
{
    public class DamageReceiver : NetworkBehaviour, IDamageable
    {
        public event Action<ServerPlayer, int> DamageReceived;
        public event Action<Collision> CollisionEntered;

        [SerializeField] private NetworkLifeState _networkLifeState;

        private void Awake()
        {
            _networkLifeState = GetComponent<NetworkLifeState>();
        }

        public void ReceiveHP(ServerPlayer inflicter, int HP)
        {
            if (IsDamageable())
            {
                DamageReceived?.Invoke(inflicter, HP);
            }
        }

        public IDamageable.SpecialDamageFlags GetSpecialDamageFlags()
        {
            return IDamageable.SpecialDamageFlags.None;
        }

        public bool IsDamageable()
        {
            return _networkLifeState.LifeState.Value == LifeState.Alive;
        }

        void OnCollisionEnter(Collision other)
        {
            CollisionEntered?.Invoke(other);
        }
    }
}