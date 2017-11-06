using System;
using UnityEngine;
public class EnemySuicideAI : MonoBehaviour
{
    UnitComponent uComponent;
    bool isLocked;
    public float randomPointRange = 5;

    public float randomMoveInterval = 5;
    float randomMoveTimer = -5;

    public float folllowUpdateInterval = 1;
    float followUpdateTimer = 0;

    private void Start()
    {
        uComponent = transform.parent.GetComponent<UnitComponent>();
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
        }
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
        }
            
    }

}