using UnityEngine;
using System.Collections;
using System;

public abstract class Missile : MonoBehaviour {

    public bool isActive = false;
    protected DamageDealer dDealer;
    public Transform tr;
    public Rigidbody2D rb;
    float dieTimer = 2.5f;
    public float ms;
    AttackingUnitI owner;

    public virtual void OnFired(Vector2 dir)
    {
        rb.velocity = (dir.normalized * ms);
        dDealer.enabled = true;
    }

    public virtual void OnDie()
    {
        if ((MonoBehaviour)owner)
            LevelManager.instance.InitMissile(owner.gameObject, this);
        else
            DestroyObject(gameObject);
    }

    public virtual void OnGiveDamage(DamageTaker taker, float damage, Vector2 dealPoint)
    {
        owner.OnGiveDamage(taker, damage, dealPoint);
    }

    public abstract void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint);
    public virtual void OnInit()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void OnUpdate() {
        if (isActive)
        {
            dieTimer -= Time.deltaTime;
            if (dieTimer <= 0)
                OnDie();
        }
    }
    
    // Use this for initialization
    public virtual Missile InitMissile(Vector3 position, string[] layers, string[] tags, AttackingUnitI owner)
    {
        isActive = true;
        dieTimer = 2.5f;
        transform.position = position;
        dDealer = GetComponentInChildren<DamageDealer>();
        foreach(var layer in layers)
        {
            dDealer.AddTargetLayer(layer);
        }
        foreach(var tag in tags)
        {
            dDealer.AddTargetTag(tag);
        }
        dDealer.SetDamage(1);
        dDealer.enabled = false;
        this.owner = owner;
        return this;

    }

    public virtual Missile InitMissile(Vector3 position, int[] layers, string[] tags, AttackingUnitI owner)
    {
        isActive = true;
        dieTimer = 2.5f;
        transform.position = position;
        dDealer = GetComponentInChildren<DamageDealer>();
        foreach (var layer in layers)
        {
            dDealer.AddTargetLayer(layer);
        }
        foreach (var tag in tags)
        {
            dDealer.AddTargetTag(tag);
        }
        this.owner = owner;
        dDealer.SetDamage(1);
        dDealer.enabled = false;
        return this;

    }
}
