using UnityEngine;

namespace Client
{
    public class ClientRaycastWeapon : ClientWeapon
    {
        public override void Process()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseButtonDown();
            }
            
            if (Input.GetMouseButton(0))
            {
                OnMouseButton();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnMouseButtonUp();
            }
            
            base.Process();
        }

        protected override void OnMouseButtonUp()
        {
            serverWeapon.AttackWithRaycastServerRpc();
            base.OnMouseButtonUp();
        }
    }
}