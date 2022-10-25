using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

namespace Server
{
    public class ServerWeapon : BaseWeapon
    {
        public ServerPlayer owner;

        protected override void Attack()
        {
            base.Attack();
        }
        
    }
}