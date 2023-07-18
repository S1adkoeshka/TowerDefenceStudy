using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsComponent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject _bar;

    public void OnPointerClick(PointerEventData eventData)
    {
        _bar.SetActive(!_bar.activeSelf);
    }
}
