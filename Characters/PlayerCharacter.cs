using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCharacter : UnitComponent, AttackingUnitI, AttackedUnitI
{
    public static PlayerCharacter instance;
    AudioSource aSource;
    Animator animator;
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
        aSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        GetComponentInChildren<DamageTaker>().Init(OnTakeDamage);
        instance = this;
        
    }
    IEnumerator OnDieAnimation()
    {
        animator.speed = 0.7f;
        animator.Play("die", 0);
        
        yield return new WaitForSeconds(1f);

        LevelManager.instance.ResetLevel();
        isMoveble = true;
        hpLose = 0;
    }
    public override void OnDie()
    {
        isAlive = false;
        isMoveble = false;
        SetVelocity(Vector2.zero);
        GameManager.instance.healthText.text = "0";
        GameManager.instance.damageEffect.color = new Color(1, 1, 1, 1);
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

    void Update()
    {
        if (isAlive && hpLose > 0)
        {
            hpLose -= Time.deltaTime/12f;
            GameManager.instance.healthText.text = (int)(100 - hpLose * 100 / hp) + "";
            GameManager.instance.damageEffect.color = new Color(1, 1, 1, hpLose / hp);
        }
        Vector3 textPosition = GameManager.instance.healthText.transform.position;
        textPosition.x = tr.position.x + 0.15f;
        textPosition.y = tr.position.y + 0.15f;
        GameManager.instance.healthText.transform.position = textPosition;

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
        missile.InitMissile(tr.position, new int[] { 9, 10 }, new string[] { }, this).OnFired(dir);
        aSource.Play();
    }

    public void Fire()
    {
        if (!isAlive)
            return;
        if (attackTimer < attackSpeed || LevelManager.instance.missiles[gameObject].Count==0) return;
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
        hpLose = Mathf.Min(hpLose + dmg, hp);
        if (hpLose >= hp)
        {
            OnDie();
        }
    }
}
