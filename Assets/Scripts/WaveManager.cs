using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public struct Wave
{
    [SerializeField]
    private int _numberOfEnemies;

    [SerializeField]
    private SpawnerComponent[] SpawnPoints;

    public void SetNumberOfEnemies()
    {
        foreach(SpawnerComponent Spawner in SpawnPoints)
        {
            _numberOfEnemies = SpawnPoints.Length;

        }
       
    }

}


