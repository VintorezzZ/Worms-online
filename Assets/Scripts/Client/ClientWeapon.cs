using DefaultNamespace;
using Server;
using Unity.Netcode;

namespace Client
{
    public class ClientWeapon : BaseWeapon
    {
        public ServerPlayer serverOwner;
        protected override void Attack()
        {
            base.Attack();
        }
    }
}