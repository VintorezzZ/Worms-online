using Unity.Netcode;
using UnityEngine;

public class ServerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private int _lastPoint = -1;
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost)
                return;
            
            var client = NetworkManager.Singleton.ConnectedClients[id];
            _lastPoint++;
                
            if (_lastPoint >= spawnPoints.Length)
                _lastPoint = 0;

            client.PlayerObject.transform.position = spawnPoints[_lastPoint].position;
        };
    }
}
