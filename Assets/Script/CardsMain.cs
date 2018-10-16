using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsMain : MonoBehaviour
{
    static bool DEBUG = true;
    static bool READY = false;
    static float DISTANCE_DELTA = 1.1F;
    static float X_OFFSET = 19f;
    static float ACCE_MIN = 0.015f;
    static float ACCE_MAX = 0.15f;

    GameObject navi;
    GameObject media;
    GameObject phone;
    GameObject car;
    GameObject umetrip;
    GameObject store;
    GameObject file;
    GameObject tachograh;
    GameObject kuwo;

    ArrayList cubesList = new ArrayList();
    CubeWrap cubeWrap0 = new CubeWrap();
    CubeWrap cubeWrap1 = new CubeWrap();
    CubeWrap cubeWrap2 = new CubeWrap();
    CubeWrap cubeWrap3 = new CubeWrap();
    CubeWrap cubeWrap4 = new CubeWrap();
    CubeWrap cubeWrap5 = new CubeWrap();
    CubeWrap cubeWrap6 = new CubeWrap();
    CubeWrap cubeWrap7 = new CubeWrap();
    CubeWrap cubeWrap8 = new CubeWrap();


    static Vector3 Position0 = new Vector3(0f - X_OFFSET, 0.1f, 0f);
    static Vector3 Position1 = new Vector3(-2f - X_OFFSET, 0f, 10f);
    static Vector3 Position2 = new Vector3(4f - X_OFFSET, 0f, 9.5f);
    static Vector3 Position3 = new Vector3(10f - X_OFFSET, 0f, 8f);
    static Vector3 Position4 = new Vector3(15f - X_OFFSET, 0f, 6f);
    static Vector3 Position5 = new Vector3(20f - X_OFFSET, 0f, 3.5f);
    static Vector3 Position6 = new Vector3(47f - X_OFFSET, 3f, -5f);
    static Vector3 Position7 = new Vector3(50f - X_OFFSET, 3f, -5f);
    static Vector3 Position8 = new Vector3(-60f - X_OFFSET, 3f, 0f);
    static Vector3 LeftoutsideP = new Vector3(-55f - X_OFFSET, 2f, -5f);
    static Vector3 RightoutsideP = new Vector3(47f - X_OFFSET, 3f, -5f);

    static Vector3 Angle0 = new Vector3(0f, 0f, 0f);
    static Vector3 Angle1 = new Vector3(0f, 0f, 0f);
    static Vector3 Angle2 = new Vector3(0f, 9f, 0f);
    static Vector3 Angle3 = new Vector3(0f, 0f, 0f);
    static Vector3 Angle4 = new Vector3(0f, 27f, 0f);
    static Vector3 Angle5 = new Vector3(0f, 29f, 0f);
    static Vector3 Angle6 = new Vector3(0f, 29f, 0f);
    static Vector3 Angle7 = new Vector3(0f, 29f, 0f);
    static Vector3 Angle8 = new Vector3(0f, 29f, 0f);
    static Vector3 AngleLeftoutside = new Vector3(0f, 10f, 0f);
    static Vector3 AngleRightoutside = new Vector3(0f, 39f, 0f);
    static Vector3 Angle_1 = new Vector3(0f, 0f, 0f);

    static Vector3 Scal0 = new Vector3(1, 1, 1);
    static Vector3 Scal1 = new Vector3(1, 1, 1);
    static Vector3 Scal2 = new Vector3(1, 1, 1);
    static Vector3 Scal3 = new Vector3(1, 1, 1);
    static Vector3 Scal4 = new Vector3(1, 1, 1);
    static Vector3 Scal5 = new Vector3(1, 1, 1);
    static Vector3 Scal6 = new Vector3(1, 1, 1);
    static Vector3 Scal7 = new Vector3(1, 1, 1);
    static Vector3 Scal8 = new Vector3(1, 1, 1);
    static Vector3 ScalLeftoutside = new Vector3(1, 1, 1);
    static Vector3 ScalRightoutside = new Vector3(1, 1, 1);

    CubePosion cubePosion0 = new CubePosion(0, Position0, Angle0, Scal0);
    CubePosion cubePosion1 = new CubePosion(1, Position1, Angle1, Scal1);
    CubePosion cubePosion2 = new CubePosion(2, Position2, Angle2, Scal2);
    CubePosion cubePosion3 = new CubePosion(3, Position3, Angle3, Scal3);
    CubePosion cubePosion4 = new CubePosion(4, Position4, Angle4, Scal4);
    CubePosion cubePosion5 = new CubePosion(5, Position5, Angle5, Scal5);
    CubePosion cubePosion6 = new CubePosion(6, Position6, Angle6, Scal6);
    CubePosion cubePosion7 = new CubePosion(7, Position7, Angle7, Scal7);
    CubePosion cubePosion8 = new CubePosion(8, Position8, Angle8, Scal8);
    CubePosion cubeLeftoutside = new CubePosion(-1, LeftoutsideP, AngleLeftoutside, ScalLeftoutside);
    CubePosion cubeRightoutside = new CubePosion(-2, RightoutsideP, AngleRightoutside, ScalRightoutside);

    CubePosion focusedCubePosion;
    CubeWrap focusedCubeWrap;

    float moveDelta = -1;
    float speed = 0;
    float startSpeed = 0;
    float acce = ACCE_MIN;
    enum State {LEFT,RIGHT,STOP};
    State state = State.STOP;

    enum ClickArea {Zero,One,Two,Three,Four,Five,Six };
    ClickArea clickArea = ClickArea.Zero;

    enum slideVector { nullVector, up, down, left, right };
    private Vector2 touchDown = Vector2.zero; //手指开始按下的位置
    private Vector2 touchUp = Vector2.zero; //手指开始按下的位置
    private Vector2 touchFirst = Vector2.zero; //手指开始按下的位置
    private Vector2 slowTouchStart = Vector2.zero; //手指开始按下的位置
    private Vector2 touchSecond = Vector2.zero; //手指拖动的位置
    private Vector2 touchThird = Vector2.zero; //手指抬起的位置
    private slideVector currentVector = slideVector.nullVector;//当前滑动方向
    private float downtime;
    private bool mouseDown = false;
    private bool longPressed = false;
    private float clicktime;
    private float timer;//时间计数器  
    public float offsetTime = 0.1f;//判断的时间间隔 
    public float SlidingDistance = 5f;
    private float dragDetaDistance = 0f;
    private float dragDetaTime = 0f;
    private float dragTime = 0f;

    bool slowMove = false;
    bool sorting = false;
    private string language = "zh";

    AndroidJavaClass jc ;
    AndroidJavaObject jo;

    public Texture2D img;
    void slideGesture()   // 滑动方法
    {
        if (Event.current.type == EventType.MouseDown)
        //判断当前手指是按下事件 
        {
            touchDown = Event.current.mousePosition;
            touchFirst = Event.current.mousePosition;//记录开始按下的位置
            touchThird = Event.current.mousePosition;
            slowTouchStart = Event.current.mousePosition;
            log("Gesture.............................. MouseDown   mousePosition:" + touchFirst.ToString());
            mouseDown = true;
            downtime = Time.time;

            if (state == State.STOP)
            {
                focusedCubePosion = getTouchedCube(touchFirst);
                if(focusedCubePosion != null)
                {
                    focusedCubeWrap = focusCube(focusedCubePosion);
                }
            }
        }

        if (Event.current.type == EventType.MouseUp)
        {
            mouseDown = false;
            longPressed = false;
            clicktime = Time.time - downtime;
            touchUp = Event.current.mousePosition;
            touchThird = Event.current.mousePosition;
            float distance = touchThird.x - touchFirst.x;


            log("Gesture.............................. MouseUp   clicktime:" + clicktime  + "  distance:" + distance  + "  touchUp:" + touchUp);
            if (focusedCubeWrap != null && clicktime > 0.02 && clicktime < 0.2 && distance < 5 && distance > -5 && state == State.STOP)
            {
                onClick(focusedCubeWrap.cube.name);
            }
            unfocusCube();
            slowMove = false;


            if (Mathf.Abs(dragDetaDistance) > 1)
            {
                float x = dragDetaDistance;
                acce = ACCE_MIN;
                startSpeed = Mathf.Abs(x / (clicktime * 100));
                if (x < 0) //左滑
                {
                    if (speed > 0)
                    {
                        if (state == State.RIGHT)
                        {
                            log("Gesture...........RIGHT >> LEFT ");
                            speed = 0;
                            state = State.LEFT;
                            startMove();
                        }
                        else if (state == State.LEFT)
                        {
                            speed = speed + 1;
                            log("Gesture...........left acce speed ! ");
                        }
                    }
                    else
                    {
                        state = State.LEFT;
                        log("Gesture left , startSpeed = " + startSpeed);
                        startMove();
                    }

                }
                else//右滑
                {
                    if (speed > 0)
                    {
                        if (state == State.LEFT)
                        {
                            log("Gesture...........LEFT >> RIGHT ");
                            speed = 0;
                            state = State.RIGHT;
                            startMove();

                        }
                        else if (state == State.RIGHT)
                        {
                            speed = speed + 1;
                            log("Gesture...........right acce speed ! ");
                        }

                    }
                    else
                    {
                        state = State.RIGHT;
                        log("Gesture right , startSpeed = " + startSpeed);
                        startMove();
                    }
                }
            }
        }

        if (Event.current.type == EventType.MouseDrag)
        //判断当前手指是拖动事件
        {
            if (Event.current.mousePosition.x != touchSecond.x)
            {
                dragDetaDistance = Event.current.mousePosition.x - touchSecond.x;
            }
            touchSecond = Event.current.mousePosition;
            log("Gesture........... MouseDrag  touchSecond=" + touchSecond.ToString());

            dragDetaTime = Time.time - dragTime;
            dragTime = Time.time;

            if (slowMove == false && state != State.STOP)
            {
                speed = 0;
                /*
                touchFirst = touchSecond;
                if(state == State.RIGHT)
                {
                    touchFirst.x = touchSecond.x - moveDelta * 300;
                }
                else if (state == State.LEFT)
                {
                    touchFirst.x = touchSecond.x + moveDelta * 300;
                }
                log("Gesture........... MouseDrag  stop,  state=" + state);*/

                state = State.STOP;
                return;
            }

            slowMove = true;
            Vector2 slideDirection = touchSecond - touchFirst;
            if (slideDirection.x > 2)
            {
                state = State.RIGHT;
            } else if (slideDirection.x < -2)
            {
                state = State.LEFT;
            } else
            {
                state = State.STOP;
            }
            log("Gesture..................................... MouseDrag  slowMove=" + slowMove + "  ,state=" + state);

        }   // 滑动方法
    }

    CubePosion getTouchedCube(Vector2 position)
    {
        if (position.x > 80 && touchFirst.x < 456 &&
            position.y > 138 && touchFirst.y < 595)
        {
            return cubePosion0;
        }
        if (position.x > 538 && touchFirst.x < 750 &&
            position.y > 290 && touchFirst.y < 545)
        {
            return cubePosion1;
        }
        if (position.x > 810 && touchFirst.x < 1020 &&
            position.y > 298 && touchFirst.y < 560)
        {
            return cubePosion2;
        }
        if (position.x > 1089 && touchFirst.x < 1300 &&
            position.y > 305 && touchFirst.y < 575)
        {
            return cubePosion3;
        }
        if (position.x > 1375 && touchFirst.x < 1655 &&
            position.y > 300 && touchFirst.y < 600)
        {
            return cubePosion4;
        }
        if (position.x > 1738 && touchFirst.x < 1900 &&
            position.y > 280 && touchFirst.y < 655)
        {
            return cubePosion5;
        }

        return null;
    }
    void onClick(string cube)
    {
        log("onClick");
        onUnityClick(cube);
    }

    void onPressed()
    {
        log("onPressed");
        //如果在运动中按下屏幕，则停止运动
        if (speed > 0)
        {
            speed = 0;
        }

    }

    void onLongPressed()
    {
        log("onLongPressed");
        onUnityLongPressed();

    }

    CubeWrap focusCube(CubePosion cubePosion)
    {
        foreach (CubeWrap cubeWrap in cubesList)
        {
            if (cubeWrap.startPosion == cubePosion)
            {
                cubeWrap.focused = true;
                cubeWrap.cube.transform.localScale = cubePosion.scal * 1.1f;
                return cubeWrap;
            }
        }
        return null;
    }

    void unfocusCube()
    {
        foreach (CubeWrap cubeWrap in cubesList)
        {
            if (cubeWrap.focused)
            {
                cubeWrap.focused = false;
                cubeWrap.cube.transform.localScale = cubeWrap.startPosion.scal;
                break;
            }
        }
        focusedCubeWrap = null;
        focusedCubePosion = null;
    }

    //cards: navi#media#hphone#car#umetrip#store#file#tachograh#kuwo
    void sortCards(string cards)
    {
        sorting = true;
        log("sortCards, cards: " + cards);
        if (state != State.STOP)
        {
            sorting = false;
            log("sortCards, resort error, cards moving!");
            return;
        }
        if (cards == null || cards.Equals(""))
        {
            sorting = false;
            log("sortCards, input sort string is empty!");
            return;
        }
        string[] cardsList = cards.Split('#');
        if (cardsList.Length != 9)
        {
            sorting = false;
            log("sortCards, input cards error,  size: " + cardsList.Length);
            return;
        }
        GameObject cube0 = getCubeWithTag(cardsList[0]);
        updateCubePosion(cube0, cubePosion0);
        cubeWrap0.cube = cube0;
        cubeWrap0.startPosion = cubePosion0;

        GameObject cube1 = getCubeWithTag(cardsList[1]);
        updateCubePosion(cube1, cubePosion1);
        cubeWrap1.cube = cube1;
        cubeWrap1.startPosion = cubePosion1;

        GameObject cube2 = getCubeWithTag(cardsList[2]);
        updateCubePosion(cube2, cubePosion2);
        cubeWrap2.cube = cube2;
        cubeWrap2.startPosion = cubePosion2;

        GameObject cube3 = getCubeWithTag(cardsList[3]);
        updateCubePosion(cube3, cubePosion3);
        cubeWrap3.cube = cube3;
        cubeWrap3.startPosion = cubePosion3;

        GameObject cube4 = getCubeWithTag(cardsList[4]);
        updateCubePosion(cube4, cubePosion4);
        cubeWrap4.cube = cube4;
        cubeWrap4.startPosion = cubePosion4;

        GameObject cube5 = getCubeWithTag(cardsList[5]);
        updateCubePosion(cube5, cubePosion5);
        cubeWrap5.cube = cube5;
        cubeWrap5.startPosion = cubePosion5;

        GameObject cube6 = getCubeWithTag(cardsList[6]);
        updateCubePosion(cube6, cubePosion6);
        cubeWrap6.cube = cube6;
        cubeWrap6.startPosion = cubePosion6;

        GameObject cube7 = getCubeWithTag(cardsList[7]);
        updateCubePosion(cube7, cubePosion7);
        cubeWrap7.cube = cube7;
        cubeWrap7.startPosion = cubePosion7;

        GameObject cube8 = getCubeWithTag(cardsList[8]);
        updateCubePosion(cube8, cubePosion8);
        cubeWrap8.cube = cube8;
        cubeWrap8.startPosion = cubePosion8;

        updateNextLeft();
        sorting = false;
        log("sortCards, end: ");
    }

    void updateCubePosion(GameObject cube, CubePosion cubePosion )
    {
        if (cube == null)
        {
            log("updateCubePosion, cube is null");
            return;
        }
        cube.transform.position = cubePosion.position;
        cube.transform.localEulerAngles = cubePosion.angle;
        cube.transform.localScale = cubePosion.scal;
    }


    GameObject getCubeWithTag(string card)
    {
        if (card.StartsWith("navi"))
        {
            return navi;
        }
        if (card.StartsWith("media"))
        {
            return media;
        }
        if (card.StartsWith("phone"))
        {
            return phone;
        }
        if (card.StartsWith("car"))
        {
            return car;
        }
        if (card.StartsWith("umetrip"))
        {
            return umetrip;
        }
        if (card.StartsWith("store"))
        {
            return store;
        }
        if (card.StartsWith("file"))
        {
            return file;
        }
        if (card.StartsWith("tachograh"))
        {
            return tachograh;
        }
        if (card.StartsWith("kuwo"))
        {
            return kuwo;
        }
        return null;
    }

    //切换语言language: eng, cn
    void onLanguageChanged(string language)
    {
        log("onLanguageChanged, language:" + language);
        this.language = language;
        if (cubesList.Count != 9)
        {
            return;
        }
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.cube.SendMessage("changeLanguage", language);
        }
    }

    //设置多媒体卡片上的信息,infor: music,radio
    void setMediaCardsInfor(string infor)
    {
        log("setMediaCardsInfor, infor:" + infor);
        if (infor == null)
        {
            return;
        }
        if(infor.Equals("radio"))
        {
            if (language.Equals("zh"))
            {
                media.SendMessage("updateTexture", 0);

            } else
            {
                media.SendMessage("updateTexture", 1);
            }
        } else if (infor.Equals("music"))
        {
            if (language.Equals("zh"))
            {
                media.SendMessage("updateTexture", 2);

            }
            else
            {
                media.SendMessage("updateTexture", 3);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        callAndroid();
        onUnityBeforeStart();
        navi = GameObject.Find("navi");
        media = GameObject.Find("media");
        phone = GameObject.Find("phone");
        car = GameObject.Find("car");
        umetrip = GameObject.Find("umetrip");
        store = GameObject.Find("store");
        file = GameObject.Find("file");
        tachograh = GameObject.Find("tachograh");
        kuwo = GameObject.Find("kuwo");

        Position0 = navi.transform.position;
        Angle0 = navi.transform.localEulerAngles;
        Scal0 = navi.transform.localScale;
        updateKeyPosition(cubePosion0, navi);
        cubeWrap0.cube = navi;
        cubeWrap0.startPosion = cubePosion0;
       // cubes.AddLast(cubeWrap0);
        cubesList.Add(cubeWrap0);

        Position1 = media.transform.position;
        Angle1 = media.transform.localEulerAngles;
        Scal1 = media.transform.localScale;
        updateKeyPosition(cubePosion1, media);
        cubeWrap1.cube = media;
        cubeWrap1.startPosion = cubePosion1;
        // cubes.AddLast(cubeWrap1);
        cubesList.Add(cubeWrap1);

        Position2 = phone.transform.position;
        Angle2 = phone.transform.localEulerAngles;
        Scal2 = phone.transform.localScale;
        updateKeyPosition(cubePosion2, phone);
        cubeWrap2.cube = phone;
        cubeWrap2.startPosion = cubePosion2;
      //  cubes.AddLast(cubeWrap2);
        cubesList.Add(cubeWrap2);

        Position3 = car.transform.position;
        Angle3 = car.transform.localEulerAngles;
        Scal3 = car.transform.localScale;
        updateKeyPosition(cubePosion3, car);
        cubeWrap3.cube = car;
        cubeWrap3.startPosion = cubePosion3;
        //cubes.AddLast(cubeWrap3);
        cubesList.Add(cubeWrap3);

        Position4 = umetrip.transform.position;
        Angle4 = umetrip.transform.localEulerAngles;
        Scal4 = umetrip.transform.localScale;
        updateKeyPosition(cubePosion4, umetrip);
        cubeWrap4.cube = umetrip;
        cubeWrap4.startPosion = cubePosion4;
     //   cubes.AddLast(cubeWrap4);
        cubesList.Add(cubeWrap4);

        Position5 = store.transform.position;
        Angle5 = store.transform.localEulerAngles;
        Scal5 = store.transform.localScale;
        updateKeyPosition(cubePosion5, store);
        cubeWrap5.cube = store;
        cubeWrap5.startPosion = cubePosion5;
    //    cubes.AddLast(cubeWrap5);
        cubesList.Add(cubeWrap5);

        Position6 = file.transform.position;
        Angle6 = file.transform.localEulerAngles;
        Scal6 = file.transform.localScale;
        updateKeyPosition(cubePosion6, file);
        cubeWrap6.cube = file;
        cubeWrap6.startPosion = cubePosion6;
        cubesList.Add(cubeWrap6);

        Position7 = tachograh.transform.position;
        Angle7 = tachograh.transform.localEulerAngles;
        Scal7 = tachograh.transform.localScale;
        updateKeyPosition(cubePosion7, tachograh);
        cubeWrap7.cube = tachograh;
        cubeWrap7.startPosion = cubePosion7;
        cubesList.Add(cubeWrap7);

        Position8 = kuwo.transform.position;
        Angle8 = kuwo.transform.localEulerAngles;
        Scal8 = kuwo.transform.localScale;
        updateKeyPosition(cubePosion8, kuwo);
        cubeWrap8.cube = kuwo;
        cubeWrap8.startPosion = cubePosion8;
        cubesList.Add(cubeWrap8);

        updateNextLeft();
        updateLeftAndRight();
        /*
        log("。。。start  cube0:" + cube0.transform.position.ToString() + cube0.name);
        log("。。。start  cube1:" + cube1.transform.position.ToString() + cube1.name);
        log("。。。start  cube2:" + cube2.transform.position.ToString() + cube2.name);
        log("。。。start  cube3:" + cube3.transform.position.ToString() + cube3.name);
        log("。。。start  cube4:" + cube4.transform.position.ToString() + cube4.name);
        log("。。。start  cube5:" + cube5.transform.position.ToString() + cube5.name);
        */
        onUnityStart();
        READY = true;
    }

    void updateKeyPosition(CubePosion cubePosion, GameObject gameObject)
    {
        cubePosion.position = gameObject.transform.position;
        cubePosion.angle = gameObject.transform.localEulerAngles;
        cubePosion.scal = gameObject.transform.localScale;
    }

    void onUnityClick(string cube)
    {
        if (DEBUG) Debug.Log("onUnityClick " + cube);
        if (jo != null)
        {
            jo.Call("onUnityClick", cube);
        }
    }

    void onUnityLongPressed()
    {
        if (DEBUG) Debug.Log("onUnityLongPressed");
        if (jo != null)
        {
            jo.Call("onUnityLongPressed");
        }
    }

    void onUnityBeforeStart()
    {
        if (DEBUG) Debug.Log("onUnityBeforeStart");
        if (jo != null)
        {
            jo.Call("onUnityBeforeStart");
        }
    }

    void onUnityStart()
    {
        if (DEBUG) Debug.Log("onUnityStart");
        if (jo != null)
        {
            jo.Call("onUnityStart");
        }
    }
    void callAndroid()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (jo == null)
        {
            jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }
        #endif
    }
    void startApp(string activity)
    {
        if (DEBUG) Debug.Log("startApp, activity:" + activity);
        if (jo != null)
        {
            jo.Call("launcherApp", activity);
        }
    }

    void log(string msg)
    {
        if (!DEBUG) return;
        Debug.Log(msg);
        /*
        if (jo != null) { 
           jo.Call("ULog", msg);
        }*/
    }

    void onMoveStart(int index)
    {
        if (DEBUG) Debug.Log("onMoveStart, index:" + index);
        if (jo != null)
        {
            jo.Call("onMoveStart", index);
        }
    }

    void onMoveEnd(int index)
    {
        if (DEBUG) Debug.Log("onMoveEnd, index:" + index);
        if (jo != null)
        {
            jo.Call("onMoveEnd", index);
        }
    }
    /*
    private void FixedUpdate()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        log("FixedUpdate position:" + Input.GetTouch(0).position);
        
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            log("TouchPhase.Began");
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            log("TouchPhase.Moved");
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            log("TouchPhase.Ended");
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Canceled)
        {
            log("TouchPhase.Canceled");
        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        if (!READY) return;
        if (mouseDown)
        {
            float longClickTime = Time.time - downtime;
            float distance = touchSecond.x - touchDown.x;
            log("Update: longPressed distance= " + distance);
            if (!longPressed && longClickTime > 0.5 && state == State.STOP && distance < 10 && distance > -10)
            {
                longPressed = true;
                onLongPressed();
            }
        }
        speed = speed - acce;
        //  log("Update:  speed= " + speed);
       // log("Update:  speed= " + speed);
        if (speed > 4f)
        {
            speed = 4f;
        }
        if (moveDelta >= 1)
        {
            if (slowMove)
            {
                touchFirst = touchSecond;
                log("Update:   slowMove reset !");
            }
            moveDelta = 0;
            foreach (CubeWrap cube in cubesList)
            {
                cube.startPosion = currentKeyPosion(cube.cube.transform.position); 
            }
            if (state != State.STOP)
            {
                updateLeftAndRight();
            } else
            {
                log("Update，Stop 0 ");
                return;
            }
            if (speed <= 0)
            {
                state = State.STOP;
                int index = getLeftsideCubeIndex();
                onMoveEnd(index);
                log("[[ move stop ]]]]]]]]]]]]]]]]]]]]]]]");
                return;
            }
        }
        if (speed > 1.8f)
        {
            moveDelta = moveDelta + Time.deltaTime * speed;
            if (state == State.LEFT)
            {
                moveToLeft(moveDelta);
            } else if (state == State.RIGHT)
            {
                moveToRight(moveDelta);
            } else
            {
                log("Update，Stop 1 ");
                return;
            }

        } else
        {
            if (slowMove && mouseDown)
            {
                float d = Mathf.Abs(touchSecond.x - touchFirst.x);
                moveDelta = d / 300;
                log("Update:  slowMove   moveDelta=" + moveDelta  + "  state=" + state);
            } else
            {
                if (state == State.STOP)
                {
                    return;
                }
                if (moveDelta > 0f)
                {
                    moveDelta = moveDelta + Time.deltaTime * 1.8f;
                }
                // log("Update:  speed= " + speed + "  moveDelta=" + moveDelta);
              
            }

            if (moveDelta == 0)
            {
                return;
            }
            if (state == State.LEFT)
            {
                moveToLeft(moveDelta);
            }
            else if (state == State.RIGHT)
            {
                moveToRight(moveDelta);
            }
           

        }

    }

    void OnGUI()
    {
        slideGesture();
       // setBackground();

    }

    void setBackground()
    {
        string aa = "";
        //这里我的Unity3d教程要继续构造一个空的GUIStyle
        GUIStyle bb = new GUIStyle();
        //设置“bb”在正常显示的时候是背景图片
        bb.normal.background = img;
        GUI.Label(new Rect(0, 0, 1370, 780), aa, bb);
    }

    void updateLeftAndRight()
    {
        log("updateLeftAndRight ");
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.leftPosion = nextLeftPosion(cubeWrap.cube.transform.position);
            cubeWrap.rightPosion = nextRightPosion(cubeWrap.cube.transform.position);
        }
    }
    void updateNextLeft()
    {
        log("updateNextLeft ");
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.nextPosion = nextLeftPosion(cubeWrap.cube.transform.position);
           // log("updateNextLeft: " + cubeWrap.toString());
        }
    }

    void updateNextRight()
    {
        log("updateNextRight ");
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.nextPosion = nextRightPosion(cubeWrap.cube.transform.position);
           // log("updateNextRight: " + cubeWrap.toString());
        }
    }

    void startMove()
    {
        log("startMove,  >>>>>>>>>>>>>");
        if (sorting)
        {
            log("startMove,  sorting, not start.");
            return;
        }
        //if (speed > 0 || moveDelta > 0) return;
       
        //updateLeftAndRight();
       
        int startIndex = getLeftsideCubeIndex();
        onMoveStart(startIndex);
        moveDelta = Time.deltaTime;
        speed = startSpeed;
        log("startMove,  >>>>>>>>>>>>>  startSpeed: " + startSpeed + "  startIndex: " + startIndex);
    }

    void moveToLeft(float delta)
    {
        log("---moveToLeft---  delta:" + delta );
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.cube.transform.position = Vector3.Lerp(cubeWrap.startPosion.position, cubeWrap.leftPosion.position, delta);
            Vector3 startAnge = cubeWrap.startPosion.angle;
            /*
            if (startAnge.Equals(Angle_1))
            {
                startAnge = new Vector3(0f, 359f, 0f);
            }*/
            cubeWrap.cube.transform.localEulerAngles = Vector3.Slerp(startAnge, cubeWrap.leftPosion.angle, delta);
            cubeWrap.cube.transform.localScale = Vector3.Lerp(cubeWrap.startPosion.scal, cubeWrap.leftPosion.scal, delta);
            int index = cubesList.IndexOf(cubeWrap);
        }
    }

    void moveToRight(float delta)
    {
         log("---moveToRight---  delta:" + delta  );
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.cube.transform.position = Vector3.Lerp(cubeWrap.startPosion.position, cubeWrap.rightPosion.position, delta);
            Vector3 nextAnge = cubeWrap.rightPosion.angle;
            /*
            if (nextAnge.Equals(Angle_1))
            {
                nextAnge = new Vector3(0f, 359f, 0f);
            }*/
            cubeWrap.cube.transform.localEulerAngles = Vector3.Slerp(cubeWrap.startPosion.angle, nextAnge, delta);
            cubeWrap.cube.transform.localScale = Vector3.Lerp(cubeWrap.startPosion.scal, cubeWrap.rightPosion.scal, delta);
        }
    }

    int getLeftsideCubeIndex()
    {
        foreach (CubeWrap cube in cubesList)
        {
            if (Vector3.Distance(cube.cube.transform.position, Position0) < DISTANCE_DELTA)
            {
                return cubesList.IndexOf(cube);
            }
        }
        return -1;
    }

    CubePosion currentKeyPosion(Vector3 currentP)
    {

        if (Vector3.Distance(currentP, Position0) < DISTANCE_DELTA)
        {
            return cubePosion0;
        }

        if (Vector3.Distance(currentP, Position1) < DISTANCE_DELTA)
        {
            return cubePosion1;
        }

        if (Vector3.Distance(currentP, Position2) < DISTANCE_DELTA)
        {
            return cubePosion2;
        }

        if (Vector3.Distance(currentP, Position3) < DISTANCE_DELTA)
        {
            return cubePosion3;
        }

        if (Vector3.Distance(currentP, Position4) < DISTANCE_DELTA)
        {
            return cubePosion4;
        }


        if (Vector3.Distance(currentP, Position5) < DISTANCE_DELTA)
        {
            return cubePosion5;
        }

        if (Vector3.Distance(currentP, Position6) < DISTANCE_DELTA)
        {
            return cubePosion6;
        }

        if (Vector3.Distance(currentP, Position7) < DISTANCE_DELTA)
        {
            return cubePosion7;
        }

        if (Vector3.Distance(currentP, Position8) < DISTANCE_DELTA)
        {
            return cubePosion8;
        }
        log("----currentKeyPosion default:cubePosion0 ");
        return cubePosion0;
    }

    CubePosion nextLeftPosion(Vector3 currentP)
    {

        if (Vector3.Distance(currentP, Position0) < DISTANCE_DELTA)
        {
            //log("nextLeftPosion: cubePosion8");
            return cubePosion8;
        }
 
        if (Vector3.Distance(currentP, Position1) < DISTANCE_DELTA)
        {
           // log("nextLeftPosion: cubePosion0");
            return cubePosion0;
        }

        if (Vector3.Distance(currentP, Position2) < DISTANCE_DELTA)
        {
            //log("nextLeftPosion: cubePosion1");
            return cubePosion1;
        }

        if (Vector3.Distance(currentP, Position3) < DISTANCE_DELTA)
        {
           // log("nextLeftPosion: cubePosion2");
            return cubePosion2;
        }

        if (Vector3.Distance(currentP, Position4) < DISTANCE_DELTA)
        {
           // log("nextLeftPosion: cubePosion3");
            return cubePosion3;
        }


        if (Vector3.Distance(currentP, Position5) < DISTANCE_DELTA)
        {
            //log("nextLeftPosion: cubePosion4");
            return cubePosion4;
        }

        if (Vector3.Distance(currentP, Position6) < DISTANCE_DELTA)
        {
            //log("nextLeftPosion: cubePosion5");
            return cubePosion5;
        }

        if (Vector3.Distance(currentP, Position7) < DISTANCE_DELTA)
        {
            //log("nextLeftPosion: cubePosion6");
            return cubePosion6;
        }

        if (Vector3.Distance(currentP, Position8) < DISTANCE_DELTA)
        {
           // log("nextLeftPosion: cubePosion7");
            return cubePosion7;
        }
        /*
        if (Vector3.Distance(currentP, LeftoutsideP) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion5");
            return cubePosion5;
        }
        */
        log("----nextLeftPosion default:cubePosion0 ");

        return cubePosion0;
    }
    CubePosion nextRightPosion(Vector3 currentP)
    {

        if (Vector3.Distance(currentP, Position0) < DISTANCE_DELTA)
        {
            //log("nextRightPosion: cubePosion1");
            return cubePosion1;
        }
        if (Vector3.Distance(currentP, Position1) < DISTANCE_DELTA)
        {
           // log("nextRightPosion: cubePosion2");
            return cubePosion2;
        }

        if (Vector3.Distance(currentP, Position2) < DISTANCE_DELTA)
        {
            //log("nextRightPosion: cubePosion3");
            return cubePosion3;
        }

        if (Vector3.Distance(currentP, Position3) < DISTANCE_DELTA)
        {
           // log("nextRightPosion: cubePosion4");
            return cubePosion4;
        }

        if (Vector3.Distance(currentP, Position4) < DISTANCE_DELTA)
        {
           // log("nextRightPosion: cubePosion5");
            return cubePosion5;
        }

        if (Vector3.Distance(currentP, Position5) < DISTANCE_DELTA)
        {
           // log("nextLeftPosion: cubePosion6");
            return cubePosion6;
        }

        if (Vector3.Distance(currentP, Position6) < DISTANCE_DELTA)
        {
          //  log("nextLeftPosion: cubePosion7");
            return cubePosion7;
        }

        if (Vector3.Distance(currentP, Position7) < DISTANCE_DELTA)
        {
           // log("nextLeftPosion: cubePosion8");
            return cubePosion8;
        }
        if (Vector3.Distance(currentP, Position8) < DISTANCE_DELTA)
        {
           // log("nextRightPosion: cubePosion0");
            return cubePosion0;
        }
        log("----nextRightPosion default:cubePosion0 ");

        return cubePosion0;
    }

    class CubePosion
    {
        public int index = -3;
        public Vector3 position;
        public Vector3 angle;
        public Vector3 scal;
        public CubePosion()
        {

        }
        public CubePosion(int index, Vector3 position, Vector3 angle, Vector3 scal)
        {
            this.index = index;
            this.position = position;
            this.angle = angle;
            this.scal = scal;
        }

        public bool Equals2(Vector3 cubePosion)
        {
            return position.Equals(cubePosion);
        }
    }

    class CubeWrap{
        public GameObject cube;
        public CubePosion startPosion;
        public CubePosion nextPosion;
        public CubePosion leftPosion;
        public CubePosion rightPosion;
        public bool focused = false;
        public CubeWrap()
        {

        }

        public string toString()
        { 

            return cube.name + "  position:" + cube.transform.position.ToString() + "  startPosion:" + startPosion.position.ToString()
                + "  nextPosion:" + nextPosion.position.ToString() + "  localEulerAngles:" + cube.transform.localEulerAngles.ToString();
        }
      

    }
}
