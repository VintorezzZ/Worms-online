using System;
using DefaultNamespace;
using Server;
using Unity.Netcode;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Client
{
    public class ClientWeapon : BaseWeapon
    {
        public ServerPlayer serverOwner;
        protected ServerWeapon serverWeapon;
        protected Vector3 mouseStart;

        private void Awake()
        {
            serverWeapon = GetComponent<ServerWeapon>();
        }

        public override void OnNetworkSpawn()
        {
            enabled = IsClient;
            if (!enabled)
                return;
        
            base.OnNetworkSpawn();
        }

        public override void Process()
        {
            transform.right = serverWeapon.rotation.Value;
        }

        protected virtual void OnMouseButtonDown()
        {
            mouseStart = Input.mousePosition;
        }

        protected virtual void OnMouseButton()
        {
            Vector3 delta = mouseStart - Input.mousePosition;
            //transform.right = delta;
            serverWeapon.UpdateRotationServerRpc(delta);
        }

        protected virtual void OnMouseButtonUp()
        {
            
        }
    }
}