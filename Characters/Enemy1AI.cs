using System;
using UnityEngine;
public class Enemy1AI : MonoBehaviour
{
    UnitComponent uComponent;
    AttackingUnitI attackingU; 
    bool isLocked;
    public float randomPointRange = 5;

    public float randomMoveInterval = 5;
    float randomMoveTimer = -5;

    public float folllowUpdateInterval = 1;
    float followUpdateTimer = 0;

    private void Start()
    {
        uComponent = transform.parent.GetComponent<UnitComponent>();
        attackingU = uComponent.GetComponent<AttackingUnitI>();
    }

    private void Update()
    {
        if (!isLocked)
        {
            if(Time.time > randomMoveTimer + randomMoveInterval)
            {
                randomMoveTimer = Time.time;
                Vector2 tarPoint = PlayerCharacter.instance.GetPosition() +
                                   Mathf2.SelectRandomPoint(randomPointRange);
                Vector2 dir = tarPoint - uComponent.GetPosition();
                uComponent.MoveTo (dir);
            }
        }
        else
        {
            if(Time.time > folllowUpdateInterval + followUpdateTimer)
            {
                followUpdateTimer = Time.time;
                randomMoveTimer = Time.time;
                Vector2 tarPoint = PlayerCharacter.instance.GetPosition();
                Vector2 dir = tarPoint - uComponent.GetPosition();
                uComponent.MoveTo(dir);
            }

            //F
            if(InAttackInterval(25))
                attackingU.IsAttacking = true;
            else
                attackingU.IsAttacking = false;
        }
    }

    bool InAttackInterval(float angle)
    {
        return Vector2.Angle(PlayerCharacter.instance.GetPosition() - uComponent.GetPosition(),
                        uComponent.GetLookingDirection()) < angle;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            isLocked = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isLocked = false;
            attackingU.IsAttacking = false;
        }
            
    }

}