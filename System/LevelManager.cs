using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public static GameObject activeLevel;
    public GameObject mainCharacter;
    public List<GameObject> levels = new List<GameObject>();
    public List<Transform> activeCenters = new List<Transform>();
    public Dictionary<GameObject, List<Missile>> missiles = new Dictionary<GameObject, List<Missile>>();
    // Use this for initialization
    int levelNum = 0;
    public void loadLevel(int num)
    {
        if (activeLevel != null)
            DestroyObject(activeLevel);
        if (levelNum < levels.Count)
            activeLevel = GameObject.Instantiate(levels[num]);

        GameObject player = GameObject.Instantiate(mainCharacter);
        player.GetComponent<UnitComponent>().OnStart();
        PlayerCharacter.instance.tr.position = new Vector3(0, 0, 0);
        Color tempColor = GameManager.instance.damageEffect.color;
        tempColor.a = 0;
        GameManager.instance.damageEffect.color = tempColor;
        player.transform.SetParent(activeLevel.transform);

        var attackers = GameObject.FindObjectsOfType<GameObject>();
        for (int i = 0; i < attackers.Length; i++)
        {
            AttackingUnitI attacker = attackers[i].GetComponent<AttackingUnitI>();
            if (attacker  == null) continue;
            for(int j = 0; j < 30; j++)
            {
                Missile missile = GameObject.Instantiate(attacker.Missile).GetComponent<Missile>();
                missile.OnInit();
                missile.tr.SetParent(activeLevel.transform);
                InitMissile(attackers[i], missile.GetComponent<Missile>());
            }
            
        }
    }
    public void nextLevel()
    {
        levelNum++;
        loadLevel(levelNum);



    }
    public void resetLevel()
    {
        loadLevel(levelNum);
        

    }

    public void InitMissile(GameObject owner, Missile missile)
    {
        missile.transform.position = new Vector3(300, 300, 300);
        missile.isActive = false;
        missile.rb.velocity = Vector3.zero;
        if (missiles.ContainsKey(owner) == false)
            missiles.Add(owner, new List<Missile>());
        missiles[owner].Add(missile);
    }

    public void Init () {
        instance = this;
        loadLevel(levelNum);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
