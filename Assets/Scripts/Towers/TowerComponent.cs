using Cells;
using Enemies;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Towers
{
    public abstract class TowerComponent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private int _damage;
        [SerializeField]
        private AttackType _attactType;
        [SerializeField]
        protected DamageType _damageType;
        [SerializeField]
        private float _aoe = 10f;

        public string TowerName;
        [SerializeField]
        private string _towerPrefabPath;

        [SerializeField]
        private GameObject _canon;

        [SerializeField]
        private Sprite _towerSprite;

        private CellComponent _cell;

        [SerializeField]
        private GameObject _upgradeModel;
        [SerializeField]
        private int _upgradeCost;

        [SerializeField]
        private GameObject _projectile;

        [SerializeField]
        private float _attackRange = 2f;

        [SerializeField]
        private float _attackRate;

        private GameObject _target;

        private bool CanAttack;


        void Start()
        {
            CanAttack = true;
        }

        void Update()
        {

            if (_target != null)
            {
                if(Vector3.Distance(transform.position, _target.transform.position) > _attackRange)
                {
                    _target = null;
                    return;
                }
                else if(_target.GetComponentInChildren<EnemyComponent>().GetDieState() == true)
                {
                    _target = null;
                    return;
                }
                else
                {
                    _canon.transform.LookAt(_target.transform.position);
                    if (CanAttack)
                    {
                        StartCoroutine(Attack(_target));
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
             
            }

            _target = FindNearestEnemy();
        }

        private GameObject FindNearestEnemy()
        {
            
            foreach(GameObject Enemy in GameManager.Instance.GetEnemies())
            {
                if (Enemy == null) continue;
                if(Vector3.Distance(transform.position, Enemy.transform.position) <= _attackRange)
                {
                    EnemyComponent NearEnemyComponent = Enemy.GetComponentInChildren<EnemyComponent>();
                    if (NearEnemyComponent.GetDieState() == true) continue;
                    if(NearEnemyComponent.GetResist() == _damageType)
                    {
                        if(NearEnemyComponent.GetResistPersent() >= 100)
                        {
                            continue;
                        }
                    }
                    return Enemy;
                }
            }

            return null;



        }
    
        private IEnumerator Attack(GameObject Target)
        {
            CanAttack = false;

            GameObject NewProjectile = Instantiate(_projectile);

            ProjectileComponent NewProjectileComponent = NewProjectile.GetComponentInChildren<ProjectileComponent>();
            NewProjectileComponent.Target = Target;

            NewProjectileComponent.Damage = _damage;
            NewProjectileComponent.DamageType = _damageType;
            NewProjectileComponent.AOE = _aoe;

            NewProjectile.transform.position = _canon.transform.position;

            yield return new WaitForSeconds(_attackRate);

            CanAttack = true;

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            TowerManager.Instance.SelectedTower = this;
            TowerManager.Instance.ShowElements();
        }

        public void UpgradeTower()
        {
            if(_upgradeModel != null)
            {             
                var NewTower = Instantiate(_upgradeModel);
                NewTower.transform.position = gameObject.transform.position;
                var NewTowerComponent = NewTower.GetComponentInChildren<TowerComponent>();
                NewTowerComponent.SetCell(_cell);
                _cell.SetTower(NewTower);
                Destroy(gameObject);
                StartCoroutine(ScaleTower(NewTower));               
            }
        }

        private IEnumerator ScaleTower(GameObject Tower)
        {
            var TimeToBuild = 0f;
            while (TimeToBuild < 4f)
            {
                Tower.transform.localScale = new Vector3(TimeToBuild, TimeToBuild, TimeToBuild);
                TimeToBuild += Time.deltaTime;
            }
            yield return null;
        }

        public int GetUpgradeCost()
        {
            return _upgradeCost;
        }

        public GameObject GetUpgradeModel()
        {
            return _upgradeModel;
        }

        public Sprite GetTowerSprite()
        {
            return _towerSprite;
        }

        public string GetTowerPrefabPath()
        {
            return _towerPrefabPath;
        }

        public void SetCell(CellComponent Cell)
        {
            _cell = Cell;
        }

        public CellComponent GetCell()
        {
            return _cell;
        }

    }

}


