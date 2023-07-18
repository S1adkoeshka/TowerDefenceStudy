using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : AbilityComponent
{
    [SerializeField]
    private int _heal;

    public new void DealAbility()
    {
        if (GameManager.Instance.GetGold() < _cost)
        {
            GameManager.Instance.ShowMessage("Not enough gold");
            return;
        }

        GameManager.Instance.ReduceGold(_cost);

        GameManager.Instance.IncreaseHealth(_heal);

    }
}
