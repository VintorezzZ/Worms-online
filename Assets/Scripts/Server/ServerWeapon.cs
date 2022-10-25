using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

namespace Server
{
    public class ServerWeapon : BaseWeapon
    {
        public ServerPlayer owner;

        public NetworkVariable<Vector3> rotation = new NetworkVariable<Vector3>();

        protected override void Attack()
        {
            base.Attack();
        }
        
    }
}