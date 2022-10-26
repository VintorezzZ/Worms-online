using Server;
using Unity.Netcode;
using UnityEngine;

namespace Client
{
    public class ClientThrowing : ClientWeapon
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _sencetivity = 0.01f;
        [SerializeField] private Transform _pointerLine;
        
        private void Start()
        {
            _renderer.enabled = false;
        }

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

        protected override void OnMouseButtonDown()
        {
            _renderer.enabled = true;
            base.OnMouseButtonDown();
        }

        protected override void OnMouseButton()
        {
            Vector3 delta = mouseStart - Input.mousePosition;
            _pointerLine.localScale = new Vector3(delta.magnitude * _sencetivity, 1, 1);
            base.OnMouseButton();
        }

        protected override void OnMouseButtonUp()
        {
            _renderer.enabled = false;
            Vector3 delta = mouseStart - Input.mousePosition;
            serverWeapon.AttackServerRpc(delta);
        }
    }
}