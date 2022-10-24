using System;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientPlayerMove : NetworkBehaviour
    {
        private ServerPlayerMove _serverPlayerMove;
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private int _lastMoveDirection;
        
        [SerializeField] SpriteRenderer _wormSprite;
        
        private static readonly int grounded = Animator.StringToHash("Grounded");
        private static readonly int walk = Animator.StringToHash("Walk");

        private void Awake()
        {
            _serverPlayerMove = GetComponent<ServerPlayerMove>();
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _animator.SetBool(grounded, _serverPlayerMove.IsGrounded.Value);
            _wormSprite.flipX = _serverPlayerMove.LastMoveDirection.Value > 0;
            
            if (_rigidbody.velocity.x != 0)
            {
                _animator.SetBool(walk, true);
            }
            else 
            {
                _animator.SetBool(walk, false);
            }
        }
        

        [ClientRpc]
        public void JumpClientRpc()
        {
            _animator.SetBool(grounded, false);
        }
    }
}