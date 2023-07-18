using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
using TMPro;

namespace Towers
{
    public class ToweBuilder : MonoBehaviour
    {
        [SerializeField]
        private GameObject _towerPrefab;

        [SerializeField]
        private int _cost;

        [SerializeField]
        private TMP_Text _costField;

        private void Start()
        {
            _costField.text = _cost.ToString();
        }


        public void SetTowerToManager()
        {
            if (GameManager.Instance.GetGold() < _cost) 
            {
                GameManager.Instance.ShowMessage("Not enough gold");
                return;
            }
            

            GameManager.Instance.SetTowerToSet(this);
            foreach(CellComponent Cell in GameManager.Instance.GetCellsList())
            {
                if (Cell.GetTower() == null) Cell.SetActiveMaterial();
            }
        }

        public int GetCost()
        {
            return _cost;
        }

        public GameObject GetTowerPrefab()
        {
            return _towerPrefab;
        }


    }

}
