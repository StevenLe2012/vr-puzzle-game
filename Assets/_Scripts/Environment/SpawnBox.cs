using System.Collections;
using System.Collections.Generic;
using Foundry;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    [SerializeField] private GameObject boxToBeSpawned;
    [SerializeField] private Transform boxSpawnPoint;
    
    
    private GameObject _boxSpawned;
    
    public void SpawnBoxAtPosition(NetEventSource netEventSource, bool isTrue)
    {
        if (isTrue)
        {
            if (_boxSpawned != null)
            {
                Destroy(_boxSpawned);
            }
            _boxSpawned = Instantiate(boxToBeSpawned, boxSpawnPoint.position, Quaternion.identity);
        }
        
    }
    
    public void DespawnBox(NetEventSource netEventSource, bool isTrue)
    {
        if (isTrue)
        {
            Destroy(_boxSpawned);
        }
    }
}
