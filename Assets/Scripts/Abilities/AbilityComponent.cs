using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class AbilityComponent : MonoBehaviour
{
    [SerializeField]
    protected int _cost;

    [SerializeField]
    private TMP_Text _costField;

    public void DealAbility() { }

    private void Start()
    {
        _costField.text = _cost.ToString();
    }
}
