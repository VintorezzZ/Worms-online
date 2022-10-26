using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

namespace Server
{
    public class ServerThrowing : ServerWeapon
    {
        [SerializeField] private float _speedMultiplier = 0.03f;
        [SerializeField] private ServerGrenade _explodePrefab;
        
        [ServerRpc]
        public override void AttackServerRpc(Vector3 delta)
        {
            Vector3 velocity = delta * _speedMultiplier;
            
            ServerGrenade grenade = Instantiate(_explodePrefab, transform.position, Quaternion.identity);
            grenade.owner = owner;
            grenade.SetVelocity(velocity);
            grenade.GetComponent<NetworkObject>().Spawn();
            base.Attack(delta);
        }
    }
}