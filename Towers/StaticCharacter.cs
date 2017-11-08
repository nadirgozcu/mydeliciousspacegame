using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

public abstract class StaticCharacter : MonoBehaviour
{
    public List<StaticCharacter> childrenCharacters = new List<StaticCharacter>();
    public StaticCharacter parentCharacter;
    public SpriteRenderer sr;
    public Transform tr;
    public Animator animator;
    public bool isAlive = true;
    public int temp;
#if UNITY_EDITOR
    public Texture2D spriteRoot;
    public bool west = false, east = false, south = false, north = false;
    public enum DIRECTION
    {
        NULL,
        NORTH,
        SOUTH,
        EAST,
        WEST
    }
    public enum INSTYPE
    {
        Tube,
        Tower,
        Center
    }
    public INSTYPE instanceType;
    public DIRECTION parentDir;
    public void UpdateSprite()
    {
        if(parentDir!= DIRECTION.NULL)
        {
            spriteRoot = parentCharacter.spriteRoot;
        }
        PrefabUtility.DisconnectPrefabInstance(gameObject);
        string sheet = AssetDatabase.GetAssetPath(spriteRoot);  
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(sheet).OfType<Sprite>().ToArray();
        tr = transform;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
        west = false;
        east = false;
        south = false;
        north =false;
        int numOfConnections = childrenCharacters.Count;
        if (parentDir != DIRECTION.NULL) numOfConnections += 1;
        for(int i = 0; i  < childrenCharacters.Count; i++)
        {
            switch (childrenCharacters[i].parentDir)
            {
                case DIRECTION.EAST:
                    west = true;
                    break;
                case DIRECTION.WEST:
                    east = true;
                    break;
                case DIRECTION.SOUTH:
                    north = true;
                    break;
                case DIRECTION.NORTH:
                    south = true;
                    break;
            }
        }
        if(parentDir != DIRECTION.NULL)
        {
            switch (parentDir)
            {
                case DIRECTION.EAST:
                    east = true;
                    break;
                case DIRECTION.WEST:
                    west = true;
                    break;
                case DIRECTION.SOUTH:
                    south = true;
                    break;
                case DIRECTION.NORTH:
                    north = true;
                    break;
            }
        }



        if (numOfConnections == 1)
        {
            if (north)
            {
                switch (instanceType)
                {
                    case INSTYPE.Tube:
                        sr.sprite = sprites[0];
                        break;
                    case INSTYPE.Tower:
                        sr.sprite = sprites[8];
                        break;
                }

            }
            else if (east)
            {
                switch (instanceType)
                {
                    case INSTYPE.Tube:
                        sr.sprite = sprites[1];
                        break;
                    case INSTYPE.Tower:
                        sr.sprite = sprites[9];
                        break;
                }
            }
            else if (south)
            {
                switch (instanceType)
                {
                    case INSTYPE.Tube:
                        sr.sprite = sprites[2];
                        break;
                    case INSTYPE.Tower:
                        sr.sprite = sprites[11];
                        break;
                }
            }
            else if (west)
            {
                switch (instanceType)
                {
                    case INSTYPE.Tube:
                        sr.sprite = sprites[3];
                        break;
                    case INSTYPE.Tower:
                        sr.sprite = sprites[15];
                        break;
                }
            }
        }
        else if (numOfConnections == 2)
        {
            if (north && east)
            {
                sr.sprite = sprites[10];
            }
            else if (south && east)
            {
                sr.sprite = sprites[13];
            }
            else if (north && west)
            {
                sr.sprite = sprites[16];
            }
            else if (south && west)
            {
                sr.sprite = sprites[19];
            }
            else if (east && west)
            {
                if (instanceType == INSTYPE.Center)
                    sr.sprite = sprites[7];
                else if (parentCharacter.instanceType == INSTYPE.Center)
                    sr.sprite = sprites[5];
                else
                    sr.sprite = sprites[17];
            }
            else if (south && north)
            {
                if (instanceType == INSTYPE.Center)
                    sr.sprite = sprites[6];
                else if (parentCharacter.instanceType == INSTYPE.Center)
                    sr.sprite = sprites[4];
                else
                    sr.sprite = sprites[12];
            }
        }
        else if (numOfConnections == 3)
        {
            if (west == false)
            {
                sr.sprite = sprites[14];
            }
            else if (south == false)
            {
                sr.sprite = sprites[18];
            }
            else if (east == false)
            {
                sr.sprite = sprites[20];
            }
            else
                sr.sprite = sprites[21];
        }
        else sr.sprite = sprites[22];

        DamageDealer dd = gameObject.GetComponentInChildren<DamageDealer>();
        DamageTaker dt = gameObject.GetComponentInChildren<DamageTaker>();
        DestroyImmediate(gameObject.GetComponent<Collider2D>());
        if(dd)
            DestroyImmediate(dd.GetComponent<Collider2D>());
        if(dt)
            DestroyImmediate(dt.GetComponent<Collider2D>());

        PolygonCollider2D root = gameObject.AddComponent<PolygonCollider2D>();
        if (dd)
        {
            PolygonCollider2D ddCollider = dd.gameObject.AddComponent<PolygonCollider2D>();
            ddCollider.pathCount = root.pathCount;
            for (int p = 0; p < root.pathCount; p++)
            {
                ddCollider.SetPath(p, root.GetPath(p));
            }
            ddCollider.isTrigger = true;
        }
        if (dt)
        {
            PolygonCollider2D dtCollider = dt.gameObject.AddComponent<PolygonCollider2D>();
            dtCollider.pathCount = root.pathCount;
            for (int p = 0; p < root.pathCount; p++)
            {
                dtCollider.SetPath(p, root.GetPath(p));
            }
            dtCollider.isTrigger = true;
        }
        DestroyImmediate(root);
    }
    public int listIndex = 0;
#endif
    public virtual void OnChildDie(StaticCharacter child)
    {
        childrenCharacters.Remove(child);
        if (childrenCharacters.Count == 0 && instanceType!= INSTYPE.Center)
        {
            OnDie();
        }
    }

    IEnumerator Die()
    {
        tr.localScale = new Vector3(1.5f, 1.5f, 1);
        animator.Play("die", 0);
        yield return new WaitForSeconds(0.15f);
        if (parentCharacter != null)
        {
            parentCharacter.OnChildDie(this);
        }
        for (int i = 0; i < childrenCharacters.Count; i++)
            childrenCharacters[i].OnDie();
        yield return new WaitForSeconds(0.35f);
        GameObject.Destroy(gameObject);


    }
    public virtual void OnDie()
    {
        if (!isAlive) return;
        GetComponentInChildren<DamageDealer>().enabled = false;
        isAlive = false;
        StartCoroutine(Die());

    }

    public virtual void OnInit()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        tr = transform;
    }


}