using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeDetector : MonoBehaviour {
    AttackingUnitI ac;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
            ac.IsAttacking = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
            ac.IsAttacking = false;
    }
    // Use this for initialization
    void Start () {
        ac = transform.parent.GetComponent<AttackingUnitI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
