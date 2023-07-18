using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemies
{
    public abstract class EnemyComponent : MonoBehaviour
    {
        [SerializeField]
        private int _health;
        [SerializeField]
        private float _movespeed;
        [SerializeField]
        private Transform[] _waypoints;

        [SerializeField]
        private DamageType _resist;
        [SerializeField, Range(0, 100)]
        private int _resistPercent;

        public string Name;

        [SerializeField]
        private int _bounty = 5;

        [SerializeField]
        private int _scorePoints;

        private Animator _animator;

        private bool _die;
        private Coroutine _dieRoutine;

        private Coroutine _hitAnimationRoutine;

        private Coroutine _movementRoutine;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }



        private IEnumerator MovementCorutine()
        {
            for (int i = 0; i < _waypoints.Length; i++)
            {
                transform.LookAt(_waypoints[i]);
                while (Vector3.Distance(transform.position, _waypoints[i].position) > 0.5f)
                {
                    transform.Translate(Vector3.forward * _movespeed * Time.deltaTime);
                    yield return null;
                }
            }
            yield return new WaitForSeconds(1f);

            GameManager.Instance.ReduceHealth();
            Destroy(gameObject);
        }

        public void StartMove()
        {
            _movementRoutine = StartCoroutine(MovementCorutine());
        }

        private void Update()
        {
            if (_health <= 0)
            {
                if(_dieRoutine == null) _dieRoutine = StartCoroutine(UnitDie());
            }
        }

        private IEnumerator UnitDie()
        {
            StopCoroutine(_movementRoutine);

            _die = true;

            GameManager.Instance.IncreaseGold(_bounty);

            SetDieAnimation();

            GameManager.Instance.SetScoreStats(_scorePoints);

            yield return new WaitForSeconds(1.2f);

            Destroy(gameObject);

        }


        private void OnDestroy()
        {
            GameManager.Instance.ReduceNumberOfEnemies();
        }

        public void SetWaypoints(Transform[] Waypoints)
        {
            _waypoints = Waypoints;
        }

        public int GetHealth()
        {
            return _health;
        }

        public void ReduceHealth(int Dmg)
        {
            _health -= Dmg;
        }

        public void IncreaseHealth(int Buff)
        {
            _health += Buff;
        }

        public int GetResistPersent()
        {
            return _resistPercent;
        }

        public DamageType GetResist()
        {
            return _resist;
        }

        public void SetMoveSpeed(float MoveSpeed)
        {
            _movespeed = MoveSpeed;
        }

        public float GetMoveSpeed()
        {
            return _movespeed;
        }

        public void SetHitAnimation()
        {
            if (_hitAnimationRoutine == null) _hitAnimationRoutine = StartCoroutine(HitAnimation());            
        }

        private IEnumerator HitAnimation()
        {
            _animator.SetTrigger("Get Hit");
            _animator.ResetTrigger("Run");
            _animator.ResetTrigger("Die");

            yield return new WaitForSeconds(0.2f);

            SetRunAnimation();

        }

        public void SetRunAnimation()
        {
            _animator.SetTrigger("Run");
            _animator.ResetTrigger("Get Hit");
            _animator.ResetTrigger("Die");
        }

        public void SetDieAnimation()
        {
            _animator.SetTrigger("Die");
            _animator.ResetTrigger("Get Hit");
            _animator.ResetTrigger("Run");
        }

        public bool GetDieState()
        {
            return _die;
        }

    }

   

}

