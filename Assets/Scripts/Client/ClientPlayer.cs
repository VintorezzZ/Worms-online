using System;
using Client;
using DefaultNamespace;
using Server;
using Unity.Netcode;
using UnityEngine;

public class ClientPlayer : NetworkBehaviour
{
    private ServerPlayer _serverPlayer;    
    [SerializeField] Texture m_Box;
    [SerializeField] Vector2 m_NameLabelOffset;
    [SerializeField] Vector2 m_ResourceBarsOffset;
    [SerializeField] private BaseWeapon _weaponPrefab;
    [SerializeField] private Transform _weaponRoot;
    private ClientWeapon _weapon;

    private void Awake()
    {
        _serverPlayer = GetComponent<ServerPlayer>();
    }

    public override void OnNetworkSpawn()
    {
        enabled = IsClient;
        if (!enabled)
            return;

        if (IsLocalPlayer)
        {
            CameraFollow360.player = transform;
        }
        
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (!IsLocalPlayer)
            return;
        
        _weapon?.Process();
    }

    [ClientRpc]
    public void OnChangeWeaponClientRpc()
    {
        _weapon = GetComponentInChildren<ClientWeapon>();
        _weapon.serverOwner = _serverPlayer;
    }
    
    void OnGUI()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        // draw the name with a shadow (colored for buf)	
        GUI.color = Color.black;
        GUI.Label(new Rect((pos.x + m_NameLabelOffset.x) - 20, Screen.height - (pos.y + m_NameLabelOffset.y) - 30, 400, 30), _serverPlayer.Name.Value);
        
        GUI.color = Color.white;
        
        GUI.Label(new Rect((pos.x + m_NameLabelOffset.x) - 21, Screen.height - (pos.y + m_NameLabelOffset.y) - 31, 400, 30), _serverPlayer.Name.Value);

        // draw health bar background
        GUI.color = Color.grey;
        GUI.DrawTexture(new Rect((pos.x + m_ResourceBarsOffset.x) - 26, Screen.height - (pos.y + m_ResourceBarsOffset.y) + 20, 52, 7), m_Box);

        // draw health bar amount
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect((pos.x + m_ResourceBarsOffset.x) - 25, Screen.height - (pos.y + m_ResourceBarsOffset.y) + 21, _serverPlayer.NetHealthState.HitPoints.Value / 2, 5), m_Box);
    }
}
