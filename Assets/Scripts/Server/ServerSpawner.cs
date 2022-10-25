using Unity.Netcode;
using UnityEngine;

public class ServerSpawner : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsClient)
                return;
            
            var client = NetworkManager.Singleton.ConnectedClients[id];
            client.PlayerObject.transform.position = new Vector3(0, 10, 0);
        };
    }
}
