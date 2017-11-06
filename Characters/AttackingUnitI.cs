using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface AttackingUnitI
{
    float AttackSpeed { get; set; }
    bool IsAttacking { get; set; }
    float AttackTimer { get; set; }
    Missile Missile { get; set; }
    GameObject gameObject { get; }
    void OnMissileSpawn(Missile missile);

    void OnGiveDamage(DamageTaker taker, float damage, Vector2 dealPoint);

    void Fire();

    void UpdateAttacker();

}

