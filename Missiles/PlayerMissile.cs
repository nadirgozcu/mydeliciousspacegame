using System;
using UnityEngine;

public class PlayerMissile : Missile
{
    public override void OnDie()
    {
        base.OnDie();
    }

    public override void OnInit()
    {
        base.OnInit();
        GetComponentInChildren<DamageDealer>().Init(OnGiveDamage);
        DamageTaker damageTaker = GetComponentInChildren<DamageTaker>();
        if (damageTaker)
            damageTaker.Init(OnTakeDamage);
    }

    public virtual void OnGiveDamage(DamageTaker taker, float damage, Vector2 dealPoint)
    {
        base.OnGiveDamage(taker, damage, dealPoint);
        UnitComponent target = taker.GetComponent<UnitComponent>();
        if(target)
            target.AddVelocity("hitted", (target.GetPosition() - (Vector2) tr.position).normalized * 3f, 0.5f, true);
        OnDie();
    }

    public override void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint)
    {
    }

    void Update()
    {
       OnUpdate();
    }

}