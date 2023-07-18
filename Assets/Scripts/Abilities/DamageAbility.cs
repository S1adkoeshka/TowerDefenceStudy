using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbility : AbilityComponent
{
    [SerializeField]
    private int _damage;

    public new void DealAbility()
    {
        if (GameManager.Instance.GetGold() < _cost)
        {
            GameManager.Instance.ShowMessage("Not enough gold");
            return;
        }

        GameManager.Instance.ReduceGold(_cost);

        foreach (GameObject Enemy in GameManager.Instance.GetEnemies())
        {
            if (Enemy == null) continue;

            EnemyComponent EnemyComponent = Enemy.GetComponentInChildren<EnemyComponent>();
            EnemyComponent.ReduceHealth(_damage);
        }

    }

}
