using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class CenterStaticCharacter : StaticCharacter, AttackedUnitI
{
    public override void OnDie()
    {
        base.OnDie();
    }

    public override void OnInit()
    {
        base.OnInit();
        DamageDealer dd = GetComponentInChildren<DamageDealer>();
        GetComponentInChildren<DamageTaker>().Init(OnTakeDamage);
        dd.Init(null);
        dd.SetDamage(999);
        dd.AddTargetLayer(8);
        dd.AddTargetLayer(10);
    }

    public override void OnChildDie(StaticCharacter child)
    {
        base.OnChildDie(child);
    }

    void OnEnable()
    {
        Start();
    }

    void Start()
    {
        OnInit();
    }

    public void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint)
    {
        OnDie();
        for (int i = childrenCharacters.Count - 1; i >= 0; i--)
        {
            childrenCharacters[i].OnDie();
        }
    }
}
