using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemySuicideCharacter : UnitComponent, AttackedUnitI
{
    Animator animator;
    public GameObject shieldSpawn;
    public float spawnChance = 1;
    public float hpLose = 0;

    public override void OnStart()
    {
        base.OnStart();
        animator = GetComponent<Animator>();
        DamageDealer dd = GetComponentInChildren<DamageDealer>();
        GetComponentInChildren<DamageTaker>().Init(OnTakeDamage);
        dd.Init(null);
        dd.SetDamage(999);
        dd.AddTargetLayer(8);
        dd.AddTargetLayer(10);

    }
    IEnumerator OnDieAnimation()
    {
        animator.speed = 0.7f;
        animator.Play("die", 0);

        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
    }
    public override void OnDie()
    {
        if (!isAlive)
            return;
        isAlive = false;
        isMoveble = false;
        SetVelocity(Vector2.zero);
        if (UnityEngine.Random.Range(0, 1) <= spawnChance)
            Instantiate(shieldSpawn, tr.position, Quaternion.identity);
        StartCoroutine(OnDieAnimation());
    }

    public override void OnMove()
    {

    }

    public void OnAttack(Vector2 dir)
    {
        
    }

    public override void OnStop()
    {

    }

    void Awake()
    {
        OnStart();
    }

    void Update()
    {
        base.OnUpdate();
    }

    public override UnitComponent GetOwner()
    {
        return this;
    }

    public void OnGiveDamage(DamageTaker taker, float damage, Vector2 dealPoint)
    {
        //taker.TakeDamage(damage, dealer, dealPoint);
    }

    public void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint)
    {
        hpLose += dmg;

        if (hpLose > hp)
        {
            OnDie();
        }
    }
}
