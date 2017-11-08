using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarManager : MonoBehaviour
{
    public static StarManager instance;
    public GameObject starObj;
    public GameObject starRoot;
    public int starCount = 500;
    Transform tr;
    float radius;

    void OnTriggerExit2D(Collider2D other)
    {
        Vector2 tar = tr.position;
        Vector3 str = other.transform.position;
        Vector2 dir = (Vector2)str - tar;
        dir = dir * -1;
        Vector2 newPos = tar + dir;
        other.transform.position = new Vector3(newPos.x, newPos.y, str.z);
    }

    IEnumerator InstantiateStarsOverTime()
    {
        for (int i = 0; i < starRoot.transform.childCount; i++)
        {
            if (!PlayerCharacter.instance)
                break;
            Vector2 randomPoint = Mathf2.SelectRandomPoint(radius);
            Vector3 randomPos = new Vector3(randomPoint.x + PlayerCharacter.instance.tr.position.x, randomPoint.y + PlayerCharacter.instance.tr.position.y, UnityEngine.Random.Range(10, 30));
            starRoot.transform.GetChild(i).transform.position = randomPos;
            if(i % 20 == 0)
                yield return new WaitForEndOfFrame();
        }
    }

    public void RePositionStars()
    {
        StartCoroutine(InstantiateStarsOverTime());
    }

    public void Init()
    {
        instance = this;
        tr = transform;
        radius = GetComponent<CircleCollider2D>().bounds.size.x/2;
        for (int i = 0; i < starCount; i++)
        {
            Vector2 randomPoint = Mathf2.SelectRandomPoint(radius);
            Vector3 randomPos = new Vector3(randomPoint.x + PlayerCharacter.instance.tr.position.x, randomPoint.y + PlayerCharacter.instance.tr.position.y, UnityEngine.Random.Range(10, 30));
            GameObject star = GameObject.Instantiate(starObj, randomPos, Quaternion.identity);
            star.transform.SetParent(starRoot.transform);
            star.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
    }

}