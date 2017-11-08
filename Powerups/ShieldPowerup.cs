using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : MonoBehaviour {
    public GameObject shieldEffect;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ShieldEffect se = PlayerCharacter.instance.GetComponentInChildren<ShieldEffect>();
            if(se)
            {
                se.timer = 0;
                DestroyObject(gameObject);
                return;
            }
            GameObject shield = GameObject.Instantiate(shieldEffect);
            shield.transform.position = PlayerCharacter.instance.GetPosition();
            shield.transform.parent = PlayerCharacter.instance.tr;
            DestroyObject(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
