/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JoystickManager : MonoBehaviour {

    public static JoystickManager instance;
    public bool moveable = true;
    public class MyTouch
    {
        public Vector2 position;
        public MyPhase phase;
        public enum MyPhase
        {
            Began,
            Moved,
            Ended,
            Stationary
        }
    }
    public Vector2 centralPoint = new Vector2(0,0);
    

    Vector2[] directions = {new Vector2(1,0), new Vector2(1,1), new Vector2(0, 1),  new Vector2(-1, 1),
                            new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1,-1) };
    int octant = -1;
    RectTransform tr;
    PlayerCharacter cm;
    float screenWidth;
	// Use this for initialization

    void Init()
    {
        tr = GetComponent<RectTransform>();
        cm = GameObject.Find("Player").GetComponent<PlayerCharacter>();
        screenWidth = Screen.width;
        instance = this;
    }
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }
    void OnEnable()
    {
        Start();
    }
    void Start () {
        Init();
	}
	
    void checkTouch()
    {
        List<MyTouch> activeTouches = new List<MyTouch>();



        bool addPc = true;
        MyTouch activeTouchPC = new MyTouch();
        activeTouchPC.position = Input.mousePosition;
        
        if (Input.GetMouseButtonUp(0))
            activeTouchPC.phase = MyTouch.MyPhase.Ended;
        else if (Input.GetMouseButtonDown(0))
        {
            activeTouchPC.phase = MyTouch.MyPhase.Began;
        }

        else if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
            activeTouchPC.phase = MyTouch.MyPhase.Moved;
        else if (Input.GetMouseButton(0))
            activeTouchPC.phase = MyTouch.MyPhase.Stationary;
        else addPc = false;
        if (addPc)
        {
            activeTouches.Add(activeTouchPC);
        }
            

        for(int i = 0; i < Input.touchCount; i++)
        {
            MyTouch activeTouch = new MyTouch();
            activeTouch.position = Input.GetTouch(i).position;

            switch (Input.GetTouch(i).phase)
            {
                case TouchPhase.Began:
                    activeTouch.phase = MyTouch.MyPhase.Began;
                    break;
                case TouchPhase.Moved:
                    activeTouch.phase = MyTouch.MyPhase.Moved;
                    break;
                case TouchPhase.Ended:
                    activeTouch.phase = MyTouch.MyPhase.Ended;
                    break;
                case TouchPhase.Stationary:
                    activeTouch.phase = MyTouch.MyPhase.Stationary;
                    break;
            }
            activeTouches.Add(activeTouch);
        }
        for(int i = 0; i < activeTouches.Count; i++)
        {
            
            checkOrders(activeTouches[i]);

        }
    }

    void fireOrder()
    {
        //cm.Fire();
    }



    void checkOrders(MyTouch activeTouch)
    {
        if (activeTouch.position.x < screenWidth * 0.6f)
        {
            fireOrder();
            return;
        }
            
        switch (activeTouch.phase)
        {
            case MyTouch.MyPhase.Began:
                centralPoint = activeTouch.position;
                moveable = true;
                //GameObject.Find("denemee").transform.position = centralPoint;
                break;
            case MyTouch.MyPhase.Moved:
                if (!moveable) return;
                Vector2 newPos = activeTouch.position;
               
                Vector2 vDir = (newPos - centralPoint);

                if (vDir.magnitude < 50) return;
                moveable = false;

                Vector2 nDir = vDir.normalized;

                float angle = Mathf.Atan2(nDir.y, nDir.x);
                int temp = (int)Mathf.Round(8 * angle / (2 * Mathf.PI) + 8) % 8;
                if (temp == octant) return;
                else octant = temp;
                nDir = directions[octant].normalized;

                //cm.moveCharacterToward(nDir);
                //8 yönlü
                //cm.moveCharacterToward(vDir.normalized);
                //360 dereceli
                break;
            case MyTouch.MyPhase.Ended:

                //cm.stopCharacter();
                break;
        }
    }

	// Update is called once per frame
	void Update () {
        if(Input.touchCount > 0 || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))  checkTouch();

    }
}
*/