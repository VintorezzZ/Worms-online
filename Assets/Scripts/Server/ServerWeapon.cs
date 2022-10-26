using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

namespace Server
{
    public class ServerWeapon : BaseWeapon
    {
        public ServerPlayer owner;
        
        public NetworkVariable<Vector3> rotation = new NetworkVariable<Vector3>();

        [SerializeField] private int _dmg;
        
        
        public override void OnNetworkSpawn()
        {
            enabled = IsServer;
            if (!enabled)
                return;
            
            base.OnNetworkSpawn();
        }

        [ServerRpc]
        public void UpdateRotationServerRpc(Vector3 delta)
        {
            rotation.Value = delta;
            transform.right = delta;
        }
        //_whatCanBeDetected = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Heavy Enemy");
        [ServerRpc]
        public virtual void AttackWithRaycastServerRpc()
        {
            Ray ray = new Ray(transform.position, transform.right);
            Debug.DrawRay(transform.position, transform.right * 10, Color.red, 3);

            RaycastHit2D[] hits = new RaycastHit2D[2];
            Physics2D.RaycastNonAlloc(transform.position, transform.right, hits, 20);
            
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out ServerPlayer serverPlayer))
                    {
                        if (serverPlayer == owner)
                            continue;
                        
                        serverPlayer.damageReceiver.ReceiveHP(owner, -_dmg);
                        var direction = serverPlayer.transform.position - owner.transform.position;
                        serverPlayer.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 200f);
                    }
                    else
                    {
                        //todo play particles on client side
                    }
                }
            }
        }
        
        [ServerRpc]
        public virtual void AttackServerRpc(Vector3 delta)
        {
            
        }
    }
}