using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager: MonoBehaviour {
    public static GameManager instance;
    public StarManager starManager;
    public Image damageEffect;
    public TextMesh healthText;
    public Dictionary<string, int> layers = new Dictionary<string, int>();
    void Start()
    {
        instance = this;
        layers.Add("Player", 8);
        layers.Add("Enemy", 9);
        layers.Add("Nature", 10);
        layers.Add("PlayerMissile", 11);
        layers.Add("EnemyMissile", 12);
        layers.Add("NatureMissile", 13);
        GameObject.FindObjectOfType<CameraManager>().Init();
        GetComponent<LevelManager>().Init();
        starManager.Init();
    }

}
