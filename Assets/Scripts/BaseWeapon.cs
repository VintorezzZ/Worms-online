using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class BaseWeapon : NetworkBehaviour
    {
        protected virtual void Attack()
        {
            
        }
        
        public virtual void Attack(Vector3 delta)
        {
            
        }
        
        public virtual void Process()
        {
            
        }
    }
}