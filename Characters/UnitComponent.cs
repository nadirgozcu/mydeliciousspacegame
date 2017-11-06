using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class UnitComponent : MonoBehaviour
{
    public class VelocityEffector
    {
        public bool isStatic;
        public Vector3 velocity;
        public float duration;
        float initialDuration = 0;
        Vector3 initialVelocity;
        public VelocityEffector(Vector3 vel, float dur)
        {
            isStatic = false;
            duration = dur;
            initialDuration = dur;
            initialVelocity = vel;
            velocity = vel;
        }

        public VelocityEffector(Vector3 vel)
        {
            isStatic = true;
            velocity = vel;
            initialVelocity = vel;

        }
        public float getDuration()
        {
            if (isStatic) return -999;
            return duration;
        }
        public float updateDuration()
        {
            if (isStatic) return -999;
            duration -= Time.deltaTime;
            velocity = velocity.normalized * (duration * initialVelocity.magnitude / initialDuration);

            return duration;
        }
    }
    Rigidbody2D rb;
    public Transform tr;
    public float hp;
    public float ms;//movespeed
    Dictionary<string, VelocityEffector> veloList = new Dictionary<string, VelocityEffector>();
    // velo name, direction + duration
    Vector2 unitLookDirection = new Vector2(0, 1);
    public bool isMoving = false;
    public bool isMoveble = true;
    public bool isAlive = true;


    public Vector2 GetVelocityDirection()
    {
        return rb.velocity.normalized;
    }
    public Vector2 GetLookingDirection()
    {
        return unitLookDirection.normalized;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    public Vector2 GetPosition()
    {
        return rb.position;
    }
    public UnitComponent SetVelocity(Vector2 vel)
    {
        veloList.Clear();
        addVelocity("fixed", vel, false);
        return this;
    }
    public UnitComponent RotateTo(Vector2 dir)
    {
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        unitLookDirection = dir;
        return this;
    }
    
    
    public UnitComponent MoveTo(Vector2 dir)
    {
        if (!isMoveble) return this;
        addVelocity("move", dir.normalized * ms, false);
        RotateTo(dir);
     

        return this;

    }
    public string addVelocity(string key, Vector2 vel, bool stack)
    {
        vel.x = vel.x / rb.mass;
        vel.y = vel.y / rb.mass;
        if (veloList.ContainsKey(key)) {
            if (!stack)
            {
                veloList[key] = new VelocityEffector(vel);
            }
            else
            {
                key = addVelocity(key + "1", vel, stack);
            }
        }
            
        else
            veloList.Add(key, new VelocityEffector(vel));
        UpdateVelocity();
        return key;
    }
    public string AddVelocity(string key, Vector2 vel, float dur, bool stack)
    {

        vel.x = vel.x / rb.mass;
        vel.y = vel.y / rb.mass;
        if (veloList.ContainsKey(key))
        {
            if (!stack)
            {
                veloList[key] = new VelocityEffector(vel, dur);
            }
            else
            {
                key = AddVelocity(key + "1", vel, dur, stack);
            }
        }

        else
            veloList.Add(key, new VelocityEffector(vel, dur));
        UpdateVelocity();
        return key;
    }
    public UnitComponent ClearVelocities()
    {
        veloList.Clear();
        rb.velocity = new Vector2(0, 0);
        return this;
    }
    public UnitComponent RemoveVelocity(string key)
    {
        if (veloList.ContainsKey(key))
            veloList.Remove(key);
        UpdateVelocity();
        return this;
    }
    public UnitComponent UpdateVelocity()
    {
        Vector3 veloSum = new Vector3(0, 0, 0);

        foreach (var item in veloList)
        {
            veloSum += item.Value.velocity;
        }
        Vector3 temp = new Vector3(veloSum.x, veloSum.y, 0);
        rb.velocity = temp;

        if (temp.magnitude != 0)
        {
            isMoving = true;
            OnMove();
        }

        else
        {
            isMoving = false;
            OnStop();
        }
            
        return this;
    }

    public UnitComponent UpdateVelocityDuration()
    {

        List<string> keys = new List<string>(veloList.Keys);
        foreach (string key in keys)
        {
            if (!veloList[key].isStatic)
            {
                float newDur = veloList[key].updateDuration();
                if (newDur < 0)
                {
                    veloList.Remove(key);
                    
                }
                UpdateVelocity();
            }

        }

        return this;
    }

    public abstract UnitComponent GetOwner();
   // public abstract void OnGiveDamage(UnitComponent tar, float damage, Vector2 dealPoint);
    public abstract void OnMove();
   // public abstract void OnTakeDamage(float dmg, DamageDealer dealerUnit, Vector2 dealPoint);
    public abstract void OnDie();
    public abstract void OnStop();
    


    public virtual void OnUpdate()
    {
        if (veloList.Count > 0) UpdateVelocityDuration();
    }
    public virtual void OnStart()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();

    }

}
