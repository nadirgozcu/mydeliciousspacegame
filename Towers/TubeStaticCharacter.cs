using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class TubeStaticCharacter : StaticCharacter, AttackedUnitI
{
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

    public void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint)
    {
    }
}
