using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAbility : AbilityComponent
{
    [SerializeField]
    private float Slow;
    [SerializeField]
    private float SlowTime;


    public new void DealAbility()
    {
        if (GameManager.Instance.GetGold() < _cost)
        {
            GameManager.Instance.ShowMessage("Not enough gold");
            return;
        }
        StartCoroutine(SetSlow());
    }

    private IEnumerator SetSlow()
    {
        List<EnemyComponent> Enemies = new List<EnemyComponent>();
        GameManager.Instance.ReduceGold(_cost);
        float EnemyMoveSpeedValue = 0f;

        foreach (GameObject Enemy in GameManager.Instance.GetEnemies())
        {
            if (Enemy == null) continue;
            EnemyComponent EnemyComponent = Enemy.GetComponentInChildren<EnemyComponent>();
            Enemies.Add(EnemyComponent);
            EnemyMoveSpeedValue = EnemyComponent.GetMoveSpeed();
            EnemyComponent.SetMoveSpeed(Slow);
        }

        yield return new WaitForSeconds(SlowTime);

        foreach(EnemyComponent EnemyOnList in Enemies)
        {
            if (EnemyOnList == null) continue;
            EnemyOnList.SetMoveSpeed(EnemyMoveSpeedValue);
        }



    }



}
