using System;
using System.Collections.Generic;

using UnityEngine;
public class DamageDealer : MonoBehaviour
{
    public List<int> targetLayers = new List<int>();
    public List<string> targetTags = new List<string>();
    float damage;
    Transform tr;
    Action<DamageTaker, float, Vector2> OnGiveDamage;
    public void DamageTo(DamageTaker tar)
    {
        tar.TakeDamage(this, damage, tr.position);
        if(OnGiveDamage != null)
            OnGiveDamage(tar, damage, tr.position);
        
    }
    public void Init(Action<DamageTaker, float, Vector2> dmgGiveFunc)
    {
        OnGiveDamage = dmgGiveFunc;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<DamageTaker>() == null) return;
        
        if (targetLayers.Contains(col.gameObject.layer))
        {
            DamageTo(col.GetComponent<DamageTaker>());
        }
        else if (targetTags.Contains(col.tag))
        {
            DamageTo(col.GetComponent<DamageTaker>());
        }
    }
    public void SetDamage(float dmg)
    {
        damage = dmg;

    }
    public DamageDealer AddTargetTag(string tar)
    {
        targetTags.Add(tar);
        return this;
    }
    public void RemoveTargetTag(string tar)
    {
        targetTags.Remove(tar);
    }
    public DamageDealer AddTargetLayer(string tar)
    {
        targetLayers.Add(GameManager.instance.layers[tar]);
        return this;   
    }
    public DamageDealer AddTargetLayer(int tar)
    {
        targetLayers.Add(tar);
        return this;
    }
    public void RemoveTargetLayer(string tar)
    {
        targetLayers.Remove(GameManager.instance.layers[tar]);
    }
    void Start()
    {
        tr = transform;
    }
}
