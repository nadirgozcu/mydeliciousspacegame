using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarManager : MonoBehaviour
{
    List<Transform> starCache = new List<Transform>();
    public GameObject starObj;
    public GameObject starRoot;
    public int starCount = 500;
    Transform tr;
    float radius;
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "star")
        {
            Vector2 tar = tr.position;
            Vector3 str = other.transform.position;
            Vector2 dir = (Vector2) str - tar;
            dir = dir * -1;
            Vector2 newPos = tar + dir;
            other.transform.position = new Vector3(newPos.x, newPos.y, str.z);
            //starCache.Add(other.transform);
        }
    }

    void SpawnStar()
    {
        if (starCache.Count == 0)
            return;
        Transform target = starCache[0];
        starCache.RemoveAt(0);

    }
    
    void Start()
    {
        tr = transform;
        radius = GetComponent<CircleCollider2D>().bounds.size.x/2;
        for (int i = 0; i < starCount; i++)
        {
            Vector2 randomPoint = Mathf2.SelectRandomPoint(radius);
            GameObject star = GameObject.Instantiate(starObj, new Vector3(randomPoint.x + tr.position.x, randomPoint.y + tr.position.y, UnityEngine.Random.Range(10, 30)), Quaternion.identity);
            star.transform.SetParent(starRoot.transform);
            star.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }
    }

}