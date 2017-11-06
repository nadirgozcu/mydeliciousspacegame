using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Enemy1Character : UnitComponent, AttackingUnitI, AttackedUnitI
{
    Animator animator;
    public GameObject shieldSpawn;
    public float spawnChance = 1;
    public float hpLose = 0;

    public bool isAttacking;
    public float attackSpeed;
    float attackTimer;

    public Missile missile;

    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }

        set
        {
            attackSpeed = value;
        }
    }

    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }

        set
        {
            isAttacking = value;
        }
    }

    public float AttackTimer
    {
        get
        {
            return attackTimer;
        }

        set
        {
            attackTimer = value;
        }
    }

    public Missile Missile
    {
        get
        {
            return missile;
        }

        set
        {
            missile = value;
        }
    }

    GameObject AttackingUnitI.gameObject
    {
        get
        {
            return gameObject;
        }
    }

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
        UpdateAttacker();
    }

    public override UnitComponent GetOwner()
    {
        return this;
    }

    public void OnMissileSpawn(Missile missile)
    {
        missile.tr.position = tr.position;
        Vector2 dir = GetLookingDirection();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        missile.tr.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        missile.InitMissile(tr.position, new int[] { 8, 10 }, new string[] { }, this).OnFired(dir);
    }

    public void Fire()
    {
        if (!isAlive)
            return;
        if (attackTimer < attackSpeed || LevelManager.instance.missiles[gameObject].Count == 0) return;
        else attackTimer = 0;
        Missile mermi = LevelManager.instance.missiles[gameObject][0];
        LevelManager.instance.missiles[gameObject].RemoveAt(0);
        OnMissileSpawn(mermi);
    }

    public void UpdateAttacker()
    {
        if (attackTimer < attackSpeed) attackTimer += Time.deltaTime;
        else if (isAttacking) Fire();
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
