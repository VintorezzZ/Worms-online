using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class ServerPlayerMove : NetworkBehaviour
    {
        private ClientPlayerMove _clientPlayerMove;
        private ClientInputSender _clientInputSender;
        private Rigidbody2D _rigidbody;

        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _jumpSpeed = 5f;
        
        public NetworkVariable<bool> IsGrounded = new NetworkVariable<bool>();
        public NetworkVariable<int> LastMoveDirection = new NetworkVariable<int>();

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }
            
            base.OnNetworkSpawn();
        }
        private void Awake()
        {
            _clientPlayerMove = GetComponent<ClientPlayerMove>();
            _clientInputSender = GetComponent<ClientInputSender>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            LastMoveDirection.Value = _clientInputSender.HorizontalInput switch
            {
                > 0.01f => 1,
                < -0.01f => -1,
                _ => LastMoveDirection.Value
            };
            
            if (_clientInputSender.Inputs.ToList().Contains(new Input() {KeyCode = KeyCode.Space}))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            if (_clientInputSender.HorizontalInput != 0)
            {
                Move();
            }
        }

        private void Jump()
        {
            if (!IsGrounded.Value)
                return;
            
            _rigidbody.velocity += new Vector2(0, _jumpSpeed);
            _clientPlayerMove.JumpClientRpc();
        }

        private void Move()
        {
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = _clientInputSender.HorizontalInput * _speed;
            _rigidbody.velocity = velocity;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            IsGrounded.Value = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            IsGrounded.Value = false;
        }
    }
}