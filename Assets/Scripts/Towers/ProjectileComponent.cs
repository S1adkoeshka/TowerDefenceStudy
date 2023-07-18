using Enemies;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{

    public GameObject Target;

    public float ProjectileSpeed;

    public int Damage;
    public DamageType DamageType;
    public float AOE;

    void Update()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) < 0.1f)
        {
            if(AOE > 0)
            {
                DoAOEDamage();
            }
            else
            {
                EnemyComponent TargetComponent = Target.GetComponentInChildren<EnemyComponent>();
                TargetComponent.SetHitAnimation();
                CalculateDamage(TargetComponent.GetResist(), TargetComponent.GetResistPersent(), TargetComponent);
            }
            
            Destroy(this.gameObject);
            return;
        }
            
       
        transform.LookAt(Target.transform);
        transform.Translate(Vector3.forward * ProjectileSpeed * Time.deltaTime);
        
    }

    public void DoAOEDamage()
    {
        
        foreach (var Enemy in GameManager.Instance.GetEnemies())
        {
            if (Enemy == null) continue;

            if (Vector3.Distance(Target.transform.position, Enemy.transform.position) <= AOE)
            {
                EnemyComponent EnemyComponent = Enemy.GetComponentInChildren<EnemyComponent>();
                EnemyComponent.SetHitAnimation();
                CalculateDamage(EnemyComponent.GetResist(), EnemyComponent.GetResistPersent(), EnemyComponent);
            }
        }
    }

    private void CalculateDamage(DamageType ResistType, int ResistPerscent, EnemyComponent TargetComponent)
    {
        int Dmg = Damage;
        if(ResistType == DamageType)
        {
            Dmg = Damage * ResistPerscent / 100;
        }

        TargetComponent.ReduceHealth(Dmg);
    }

}
