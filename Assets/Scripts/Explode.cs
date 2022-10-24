using System;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class Explode : NetworkBehaviour
    {
        protected Cutter cutter;
        
        private void Start()
        {
            cutter = FindObjectOfType<Cutter>();
        }
        
        protected void DoCut()
        {
            cutter.DoCut();
            Destroy(gameObject);
        }
    }
}