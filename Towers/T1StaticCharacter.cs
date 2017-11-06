using System;
using UnityEngine;

public class T1StaticCharacter : StaticCharacter, AttackingUnitI, AttackedUnitI
{
    public float missileSpeed = 2;

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

    public override void OnChildDie(StaticCharacter child)
    {
        base.OnChildDie(child);
    }

    void OnEnable()
    {
        Start();
    }

    public override void OnInit()
    {
        base.OnInit();
        DamageDealer dd = GetComponentInChildren<DamageDealer>();
        GetComponentInChildren<DamageTaker>().Init(OnTakeDamage);
        dd.Init(OnGiveDamage);
        dd.SetDamage(999);
        dd.AddTargetLayer(8);
        dd.AddTargetLayer(10);
    }

    void Start()
    {
        OnInit();
    }

    public void OnMissileSpawn(Missile missile)
    {
        missile.tr.position = tr.position;
        Vector3 tarPos = PlayerCharacter.instance.GetPosition();
        Vector2 dir = tarPos - tr.position;
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

    private void Update()
    {
        UpdateAttacker();
    }

    public void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint)
    {
        OnDie();
    }

    public void OnGiveDamage(DamageTaker taker, float damage, Vector2 dealPoint)
    {
    }
}
