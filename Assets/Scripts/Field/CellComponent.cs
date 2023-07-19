using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cells
{
    public class CellComponent : MonoBehaviour, IPointerClickHandler
    {

        private GameObject _tower;

        [SerializeField]
        private Material _activeMaterial;
        [SerializeField]
        private Material _inactiveMaterial;

        private MeshRenderer _mesh;

        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            
        }

        public void SetTower(GameObject Tower)
        {
            _tower = Tower;
        }
        public GameObject GetTower()
        {
            return _tower;
        }

        public void SetActiveMaterial()
        {
            _mesh.material = _activeMaterial;
        }

        public void SetInactiveMaterial()
        {
            _mesh.material = _inactiveMaterial;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_tower != null) return;

            var TowerBilderComponent = GameManager.Instance.GetTowerToSet();

            if (TowerBilderComponent == null) return;

            var NewTower = Instantiate(TowerBilderComponent.GetTowerPrefab());

            NewTower.transform.position = transform.position;

            TowerComponent NewTowerComponent = NewTower.GetComponentInChildren<TowerComponent>();

            NewTowerComponent.SetCell(this);

            _tower = NewTower;

            GameManager.Instance.ReduceGold(TowerBilderComponent.GetCost());

            GameManager.Instance.SetTowerToSet(null);

            foreach (var Cell in GameManager.Instance.GetCellsList())
            {
                Cell.SetInactiveMaterial();
            }

        }
    }

}

