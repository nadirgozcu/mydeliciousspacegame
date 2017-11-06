using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface AttackedUnitI
{
    void OnTakeDamage(DamageDealer dealer, float dmg, Vector2 dealPoint);
}

