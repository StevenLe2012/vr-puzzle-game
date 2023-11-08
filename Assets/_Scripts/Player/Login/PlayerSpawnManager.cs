using System;
using System.Collections.Generic;
using Foundry;
using Foundry.Networking;
using UnityEngine;

public class PlayerSpawnManager : NetworkComponent
{
    [SerializeField] private GameObject p1Avatar;
    [SerializeField] private GameObject p2Avatar;
    [SerializeField] private Vector3 scaleP1 = new Vector3(10, 10, 10);

    private NetworkProperty<int> _avatarSpawned = new (0);
    private NetworkProperty<int> _layerIndex = new(0);
    // private NetworkProperty<Vector3> _playerSize = new (new Vector3(1, 1, 1));
    
    public int AvatarSpawned
    {
        get => _avatarSpawned.Value;
        set => _avatarSpawned.Value = value;
    }
    
    public int LayerIndex
    {
        get => _layerIndex.Value;
        set => _layerIndex.Value = value;
    }
    
    // public Vector3 PlayerSize
    // {
    //     get => _playerSize.Value;
    //     set => _playerSize.Value = value;
    // }
    
    
    [SerializeField] private Transform avatarHolder;
    
    private PlayerSpawnPointsManager _playerSpawnPointsManager;
    // Start is called before the first frame update
    void Start()
    {
        _playerSpawnPointsManager = PlayerSpawnPointsManager.Instance;
        if (PlayerPrefs.GetInt("PlayerRole") == 1)
        {
            SpawnP1();
        }
        if (PlayerPrefs.GetInt("PlayerRole") == 2)
        {
            SpawnP2();
        }
    }

    // Spawn overlord
    // TODO: I MOST LIKELY DON'T NEED AN AVATAR (JUST HANDS) FOR THE OVERLORD
    private void SpawnP1()
    {
        transform.position = _playerSpawnPointsManager.SpawnPointP1.position;
        // SetGameLayerRecursive(gameObject, LayerMask.NameToLayer("FoundryPlayer1"));
        LayerIndex = LayerMask.NameToLayer("FoundryPlayer1");
        Invoke(nameof(MakeOverLord), 1f);
        AvatarSpawned = 1;
    }

    // Spawn top-down character
    private void SpawnP2()
    {
        transform.position = _playerSpawnPointsManager.SpawnPointP2.position;
        // SetGameLayerRecursive(gameObject, LayerMask.NameToLayer("FoundryPlayer2"));
        LayerIndex = LayerMask.NameToLayer("FoundryPlayer2");
        Invoke(nameof(MakeTopDownCharacter), 1f);
        AvatarSpawned = 2;
    }
    
    public override void RegisterProperties(List<INetworkProperty> properties)
    {
        _layerIndex.OnValueChanged += layerIndex =>
        {
            print($"RECURSIVE CALL of {gameObject.name} was changed to: {layerIndex}");
            SetGameLayerRecursive(gameObject, layerIndex);
        };
        properties.Add(_layerIndex);
        
        _avatarSpawned.OnChanged += SpawnSelectedAvatar;
        properties.Add(_avatarSpawned);
        // properties.Add(_playerSize);
    }

    private void SpawnSelectedAvatar()
    {
        if (AvatarSpawned == 1)
        {
            // Instantiate(p1Avatar, avatarHolder);
        }

        if (AvatarSpawned == 2)
        {
            // Instantiate(p2Avatar, avatarHolder);
        }
    }

    private void MakeOverLord()
    {
        // Set starting scale
        // var controlRig = GetComponentInChildren<IPlayerControlRig>();
        // if (controlRig == null) throw new Exception("Could not find control rig");
        // controlRig.transform.localScale = scaleP1;
        
        // enable scaling
        var playerScale = GetComponent<PlayerScale>();
        if (playerScale == null) throw new Exception("Could not find player scale");
        playerScale.enabled = true;
    }
    
    private void MakeTopDownCharacter()
    {
        // enable jumping
        var playerJump = GetComponent<PlayerJump>();
        if (playerJump == null) throw new Exception("Could not find player jump");
        playerJump.enabled = true;
    }
    
    
    private void SetGameLayerRecursive(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;
 
            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetGameLayerRecursive(child.gameObject, _layer);
        }
    }
}
