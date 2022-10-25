using Server;
using Unity.Netcode;
using UnityEngine;

namespace Client
{
    public class ClientThrowing : ClientWeapon
    {
        private ServerThrowing _serverThrowing;

        private Vector3 mouseStart;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _sencetivity = 0.01f;
        [SerializeField] private Transform _pointerLine;

        private void Awake()
        {
            _serverThrowing = GetComponent<ServerThrowing>();
        }
        
        private void Start()
        {
            _renderer.enabled = false;
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
            if (Input.GetMouseButtonDown(0))
            {
                _renderer.enabled = true;
                mouseStart = Input.mousePosition;
            }
        
            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - mouseStart;
                //transform.right = delta;
                _pointerLine.localScale = new Vector3(delta.magnitude * _sencetivity, 1, 1);
                
                _serverThrowing.UpdateRotationServerRpc(delta);
            }

            if (Input.GetMouseButtonUp(0))
            {
                _renderer.enabled = false;
                Vector3 delta = Input.mousePosition - mouseStart;
                
                _serverThrowing.ThrowServerRpc(delta);
            }

            transform.right = _serverThrowing.rotation.Value;
        }
    }
}