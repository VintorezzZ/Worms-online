using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class ServerGrenade : Explode
    {
        private ClientGrenade _clientGrenade;
        private Rigidbody2D _rigidbody;
        private bool dead;

        private void Awake()
        {
            _rigidbody = GetComponentInChildren<Rigidbody2D>();
            _clientGrenade = GetComponent<ClientGrenade>();
        }

        public void SetVelocity(Vector2 value)
        {
            _rigidbody.velocity = value;
            _rigidbody.AddTorque(Random.Range(-8f,8f));
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (dead) 
                return;
        
            cutter.transform.position = transform.position;
            _clientGrenade.OnCollisionEnterClientRpc(transform.position);
            Invoke(nameof(DoCut), 0.01f);
            StartCoroutine(DespawnRoutine());
            
            dead = true;
        }

        private IEnumerator DespawnRoutine()
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}