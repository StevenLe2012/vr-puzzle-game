using System;
using UnityEngine;

public class PlayerSpawnPointsManager : MonoBehaviour
{
    public static PlayerSpawnPointsManager Instance;
    public Transform SpawnPointP1;
    public Transform SpawnPointP2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
