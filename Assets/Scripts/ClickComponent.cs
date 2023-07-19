using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickComponent : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _cells;

    void Update()
    {       

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (GameManager.Instance._gameIsPaused) return;
            TowerManager.Instance.SelectedTower = null;
            TowerManager.Instance.HideElements();
            if (GameManager.Instance.GetTowerToSet() == null) return;
            foreach (var Cell in GameManager.Instance.GetCellsList())
            {
                Cell.SetInactiveMaterial();
            }          

        }
        
    }


}
