using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientInputSender : NetworkBehaviour
    {
        public Input[] Inputs = new Input[5];
        public float HorizontalInput;

        public override void OnNetworkSpawn()
        {
            if (IsServer && !IsHost || !IsOwner)
            {
                enabled = false;
                return;
            }
            
            base.OnNetworkSpawn();
        }

        private void Update()
        {
            List<Input> inputsList = new List<Input>(5);

            if (!IsOwner || !IsClient)
                return;
            
            HorizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
            
            if (UnityEngine.Input.GetKey(KeyCode.A))
            {
                RegisterInput(inputsList, KeyCode.A);
            }
            if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                RegisterInput(inputsList, KeyCode.D);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                RegisterInput(inputsList, KeyCode.Space);
            }

            SendInputServerRpc(inputsList.ToArray(), HorizontalInput);
        }

        [ServerRpc]
        private void SendInputServerRpc(Input[] inputsArray, float horizontalInput)
        {
            Inputs = inputsArray;
            HorizontalInput = horizontalInput;
        }

        private void RegisterInput(List<Input> inputs, KeyCode keyCode)
        {
            Input input = new Input() {KeyCode = keyCode};
            
            if (!inputs.Contains(input))
            {
                inputs.Add(input);
            }
        }
    }

    public struct Input : INetworkSerializable
    {
        public KeyCode KeyCode;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsWriter)
            {
                serializer.GetFastBufferWriter().WriteValueSafe(KeyCode);
            }
            else
            {
                serializer.GetFastBufferReader().ReadValueSafe(out KeyCode);
            }
        }
    }
}