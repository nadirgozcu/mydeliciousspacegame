using System;
using System.Collections.Generic;

using UnityEngine;
public class DamageTaker : MonoBehaviour
{
    Action<DamageDealer, float, Vector2> OnTakeDamage;
    public void Init(Action<DamageDealer, float, Vector2> dmgTakeFunc)
    {
        OnTakeDamage = dmgTakeFunc;
    }
    public void TakeDamage(DamageDealer dealer, float dmg, Vector2 pos)
    {
        OnTakeDamage(dealer, dmg, pos);
    }

}