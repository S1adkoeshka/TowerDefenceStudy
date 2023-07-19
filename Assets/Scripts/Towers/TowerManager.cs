using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Towers
{
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager Instance;

        public TowerComponent SelectedTower;
   
        [SerializeField]
        private Button _upgradeButton;
        [SerializeField]
        private TMP_Text _upgradeCost;
        [SerializeField]
        private Button _destroyButton;
        [SerializeField]
        private TMP_Text _towerName;


        private void Start()
        {
            Instance = this;
            HideElements();
        }

        public void UpgradeTower()
        {
            if (GameManager.Instance._gameIsPaused) return;

            if (SelectedTower == null) return;

            if (GameManager.Instance.GetGold() < SelectedTower.GetUpgradeCost())
            {
                GameManager.Instance.ShowMessage("Not enough gold");
                return;
            }

            GameManager.Instance.ReduceGold(SelectedTower.GetUpgradeCost());
            SelectedTower.UpgradeTower();
            HideElements();
        }

        public void DestroyTower()
        {
            if (GameManager.Instance._gameIsPaused) return;
            if (SelectedTower == null) return;
            Destroy(SelectedTower.gameObject);
            HideElements();
        }

        public void ShowElements()
        {
            if (SelectedTower == null) return;

            if(SelectedTower.GetUpgradeModel() == null)
            {
                _upgradeButton.gameObject.SetActive(false);
            }            
            else 
            {
                _upgradeButton.gameObject.SetActive(true);
                _upgradeCost.text = SelectedTower.GetUpgradeCost().ToString();
            }

            _destroyButton.gameObject.SetActive(true);
            _towerName.gameObject.SetActive(true);
            _towerName.text = SelectedTower.TowerName;          
        }

        public void HideElements()
        {
            _upgradeButton.gameObject.SetActive(false);
            _towerName.text = null;
            _upgradeCost.text = null;
            _destroyButton.gameObject.SetActive(false);
            _towerName.gameObject.SetActive(false);
        }

    }

}

