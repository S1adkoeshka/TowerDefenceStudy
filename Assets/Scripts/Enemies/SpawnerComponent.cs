using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour
{

    [SerializeField]
    private Transform[] _waypoints;

    public void FillEnemies(int Count, GameObject EnemyPrefab, int HpBonuse)
    {
        StartCoroutine(SpawnEnemies(Count, EnemyPrefab, HpBonuse));
    }

    private IEnumerator SpawnEnemies(int Count, GameObject EnemyPrefab, int HpBonuse)
    {
        for (int i = 0; i < Count; i++)
        {
            var NewEnemy = Instantiate(EnemyPrefab);
            NewEnemy.transform.position = transform.position;
            EnemyComponent EnemyComponent = NewEnemy.GetComponentInChildren<EnemyComponent>();

            EnemyComponent.IncreaseHealth(HpBonuse);           
            EnemyComponent.SetWaypoints(_waypoints);
            EnemyComponent.StartMove();
            GameManager.Instance.AddUnitToList(NewEnemy);
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
}
