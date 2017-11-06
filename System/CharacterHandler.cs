using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CharacterHandler : MonoBehaviour
{

    Vector2 leftDefPos, moveDir;
    Vector2 rightDefPos, rightJoyDefPos, rightSwipePos;
    float scWidth = 0;


    public void PointerDown(BaseEventData bed)
    {
        PointerEventData ped = (PointerEventData)bed;
        if (ped.position.x >= scWidth / 2)
            PlayerCharacter.instance.IsAttacking = true;
    }

    public void PointerUp(BaseEventData bed)
    {
        PointerEventData ped = (PointerEventData)bed;
        if (ped.position.x >= scWidth / 2)
            PlayerCharacter.instance.IsAttacking = false;
    }

    public void Drag(BaseEventData bed)
    {
        PointerEventData ped = (PointerEventData)bed;

        if (ped.position.x < scWidth / 2)
        {
            Vector2 touchPos = ped.position;
            Vector2 dir = (touchPos - leftDefPos).normalized;

            float angle = Vector2.Angle(dir, Vector2.right);
            angle = Mathf.Round(angle / 45) * 45;
            Vector2 newDir = Mathf2.DegreeToVector2(angle);
            newDir.y *= Mathf.Sign(dir.y);
            if (newDir.magnitude == 0) return;

            moveDir = newDir;
            if ((touchPos - leftDefPos).magnitude > scWidth / 15)
            {
                leftDefPos = touchPos - dir * scWidth / 20;
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        scWidth = Screen.width;
    }

    void moveCharacter()
    {
        PlayerCharacter.instance.MoveTo(moveDir);
    }

    void CheckButtons()
    {
        Vector2 newDir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            newDir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newDir += Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            newDir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newDir += Vector2.down;
        }
        if (newDir != Vector2.zero)
            moveDir = newDir;
        PlayerCharacter.instance.IsAttacking = Input.GetKey(KeyCode.Space);

    }
    void Update()
    {
        moveCharacter();
        CheckButtons();
    }

}




