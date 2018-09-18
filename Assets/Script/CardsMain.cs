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

    GameObject cube0;
    GameObject cube1;
    GameObject cube2;
    GameObject cube3;
    GameObject cube4;
    GameObject cube5;

    ArrayList cubesList = new ArrayList();
    CubeWrap cubeWrap0 = new CubeWrap();
    CubeWrap cubeWrap1 = new CubeWrap();
    CubeWrap cubeWrap2 = new CubeWrap();
    CubeWrap cubeWrap3 = new CubeWrap();
    CubeWrap cubeWrap4 = new CubeWrap();
    CubeWrap cubeWrap5 = new CubeWrap();


    static Vector3 Position0 = new Vector3(0f - X_OFFSET, 0.1f, 0f);
    static Vector3 Position1 = new Vector3(-2f - X_OFFSET, 0f, 10f);
    static Vector3 Position2 = new Vector3(4f - X_OFFSET, 0f, 9.5f);
    static Vector3 Position3 = new Vector3(10f - X_OFFSET, 0f, 8f);
    static Vector3 Position4 = new Vector3(15f - X_OFFSET, 0f, 6f);
    static Vector3 Position5 = new Vector3(20f - X_OFFSET, 0f, 3.5f);
    static Vector3 LeftoutsideP = new Vector3(-55f - X_OFFSET, 2f, -5f);
    static Vector3 RightoutsideP = new Vector3(47f - X_OFFSET, 3f, -5f);

    static Vector3 Angle0 = new Vector3(0f, 0f, 0f);
    static Vector3 Angle1 = new Vector3(0f, 0f, 0f);
    static Vector3 Angle2 = new Vector3(0f, 9f, 0f);
    static Vector3 Angle3 = new Vector3(0f, 0f, 0f);
    static Vector3 Angle4 = new Vector3(0f, 27f, 0f);
    static Vector3 Angle5 = new Vector3(0f, 29f, 0f);
    static Vector3 AngleLeftoutside = new Vector3(0f, 10f, 0f);
    static Vector3 AngleRightoutside = new Vector3(0f, 39f, 0f);
    static Vector3 Angle_1 = new Vector3(0f, 0f, 0f);

    static Vector3 Scal0 = new Vector3(1, 1, 1);
    static Vector3 Scal1 = new Vector3(1, 1, 1);
    static Vector3 Scal2 = new Vector3(1, 1, 1);
    static Vector3 Scal3 = new Vector3(1, 1, 1);
    static Vector3 Scal4 = new Vector3(1, 1, 1);
    static Vector3 Scal5 = new Vector3(1, 1, 1);
    static Vector3 ScalLeftoutside = new Vector3(1, 1, 1);
    static Vector3 ScalRightoutside = new Vector3(1, 1, 1);

    CubePosion cubePosion0 = new CubePosion(0, Position0, Angle0, Scal0);
    CubePosion cubePosion1 = new CubePosion(1, Position1, Angle1, Scal1);
    CubePosion cubePosion2 = new CubePosion(2, Position2, Angle2, Scal2);
    CubePosion cubePosion3 = new CubePosion(3, Position3, Angle3, Scal3);
    CubePosion cubePosion4 = new CubePosion(4, Position4, Angle4, Scal4);
    CubePosion cubePosion5 = new CubePosion(5, Position5, Angle5, Scal5);
    CubePosion cubeLeftoutside = new CubePosion(-1, LeftoutsideP, AngleLeftoutside, ScalLeftoutside);
    CubePosion cubeRightoutside = new CubePosion(-2, RightoutsideP, AngleRightoutside, ScalRightoutside);

    CubePosion focusedCubePosion;

    float moveDelta = -1;
    float speed = 0;
    float startSpeed = 0;
    float acce = ACCE_MIN;
    enum State {LEFT,RIGHT,STOP};
    State state = State.STOP;

    enum ClickArea {Zero,One,Two,Three,Four,Five,Six };
    ClickArea clickArea = ClickArea.Zero;

    enum slideVector { nullVector, up, down, left, right };
    private Vector2 touchFirst = Vector2.zero; //手指开始按下的位置
    private Vector2 touchSecond = Vector2.zero; //手指拖动的位置
    private slideVector currentVector = slideVector.nullVector;//当前滑动方向
    private float downtime;
    private bool mouseDown = false;
    private bool longPressed = false;
    private float clicktime;
    private float timer;//时间计数器  
    public float offsetTime = 0.1f;//判断的时间间隔 
    public float SlidingDistance = 0f;


    AndroidJavaClass jc ;
    AndroidJavaObject jo;

    public Texture2D img;
    void slideGesture()   // 滑动方法
    {
        if (Event.current.type == EventType.MouseDown)
        //判断当前手指是按下事件 
        {
            touchFirst = Event.current.mousePosition;//记录开始按下的位置
            log("..................................... MouseDown   mousePosition:" + touchFirst.ToString());
            mouseDown = true;
            downtime = Time.time;

            //如果在运动中按下屏幕，则停止运动
            if (speed > 0)
            {
                speed = 0;
            }

            if (state == State.STOP)
            {
                focusedCubePosion = getTouchedCube(touchFirst);
                if(focusedCubePosion != null)
                {
                    focusCube(focusedCubePosion);
                }
            }
        }

        if (Event.current.type == EventType.MouseUp)
        {
            mouseDown = false ;
            longPressed = false;
            clicktime = Time.time - downtime;
            log("..................................... MouseUp   clicktime:" + clicktime);
            if (clicktime >0.8 && clicktime < 0.2)
            {
                onClick(focusedCubePosion.index);
            }
            unfocusCube();
        }

        if (Event.current.type == EventType.MouseDrag)
        //判断当前手指是拖动事件
        {
            log("..................................... MouseDrag");
            touchSecond = Event.current.mousePosition;

            timer += Time.deltaTime;  //计时器

            if (timer > offsetTime)
            {
                touchSecond = Event.current.mousePosition; //记录结束下的位置
                Vector2 slideDirection = touchFirst - touchSecond;
                float x = slideDirection.x;
                float y = slideDirection.y;

                if (y + SlidingDistance < x && y > -x - SlidingDistance)
                {
                    /*
                    if (currentVector == slideVector.left)
                    {
                        return;
                    }
                    */
  
                   if (state == State.STOP)
                   {
                        state = State.LEFT;
                    }
                    currentVector = slideVector.left;

                    if (speed > 0)
                    {
                        log("left......................................already start !, speed = " + speed);
                        return;
                    }
                    acce = ACCE_MIN;
                    startSpeed = x /(timer * 400);
                    log("left , startSpeed = " + startSpeed);
                    startMove();
                }
                else if (y > x + SlidingDistance && y < -x - SlidingDistance)
                {
                    /*
                    if (currentVector == slideVector.right)
                    {
                        return;
                    }
                    */
                    if (state == State.STOP)
                    {
                        state = State.RIGHT;
                    }
                    currentVector = slideVector.right;

                    if (speed > 0)
                    {
                        log("right......................................already start !, speed = " + speed);
                        return;
                    }
                    acce = ACCE_MIN;
                    startSpeed =0 - x / (timer * 400);
                    log("right ,startSpeed = " + startSpeed);
                    startMove();
                }
                else if (y > x + SlidingDistance && y - SlidingDistance > -x)
                {
                    if (currentVector == slideVector.up)
                    {
                        return;
                    }

                    log("up");

                    currentVector = slideVector.up;
                }
                else if (y + SlidingDistance < x && y < -x - SlidingDistance)
                {
                    if (currentVector == slideVector.down)
                    {
                        return;
                    }

                    log("Down");

                    currentVector = slideVector.down;
                }

                timer = 0;
                touchFirst = touchSecond;
            }
            if (Event.current.type == EventType.MouseUp)
            {//滑动结束  
                currentVector = slideVector.nullVector;
            }
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
    void onClick(int index)
    {
        log("onClick");
        onUnityClick(index);
    }

    void onLongPressed()
    {
        log("onLongPressed");
        onUnityLongPressed();

    }

    void focusCube(CubePosion cubePosion)
    {
        foreach (CubeWrap cubeWrap in cubesList)
        {
            if (cubeWrap.startPosion == cubePosion)
            {
                cubeWrap.focused = true;
                cubeWrap.cube.transform.localScale = cubePosion.scal * 1.1f;
                break;
            }
        }
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
    }

    void sortCards(string cards)
    {
        log("sortCards, cards: " + cards);
    }

    void onLanguageChanged(string language)
    {
        log("onLanguageChanged, language:" + language);
        if (cubesList.Count != 6)
        {
            return;
        }
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.cube.SendMessage("changeLanguage", language);
        }
    }

    // Use this for initialization
    void Start()
    {
        callAndroid();
        onUnityBeforeStart();
        cube0 = GameObject.Find("Cube0");
        cube1 = GameObject.Find("Cube1");
        cube2 = GameObject.Find("Cube2");
        cube3 = GameObject.Find("Cube3");
        cube4 = GameObject.Find("Cube4");
        cube5 = GameObject.Find("Cube5");

        Position0 = cube0.transform.position;
        Angle0 = cube0.transform.localEulerAngles;
        Scal0 = cube0.transform.localScale;
        cubePosion0.position = cube0.transform.position;
        cubePosion0.angle = cube0.transform.localEulerAngles;
        cubePosion0.scal = cube0.transform.localScale;
        cubeWrap0.cube = cube0;
        cubeWrap0.startPosion = cubePosion0;
       // cubes.AddLast(cubeWrap0);
        cubesList.Add(cubeWrap0);

        Position1 = cube1.transform.position;
        Angle1 = cube1.transform.localEulerAngles;
        Scal1 = cube1.transform.localScale;
        cubePosion1.position = cube1.transform.position;
        cubePosion1.angle = cube1.transform.localEulerAngles;
        cubePosion1.scal = cube1.transform.localScale;
        cubeWrap1.cube = cube1;
        cubeWrap1.startPosion = cubePosion1;
        // cubes.AddLast(cubeWrap1);
        cubesList.Add(cubeWrap1);

        Position2 = cube2.transform.position;
        Angle2 = cube2.transform.localEulerAngles;
        Scal2 = cube2.transform.localScale;
        cubePosion2.position = cube2.transform.position;
        cubePosion2.angle = cube2.transform.localEulerAngles;
        cubePosion2.scal = cube2.transform.localScale;
        cubeWrap2.cube = cube2;
        cubeWrap2.startPosion = cubePosion2;
      //  cubes.AddLast(cubeWrap2);
        cubesList.Add(cubeWrap2);

        Position3 = cube3.transform.position;
        Angle3 = cube3.transform.localEulerAngles;
        Scal3 = cube3.transform.localScale;
        cubePosion3.position = cube3.transform.position;
        cubePosion3.angle = cube3.transform.localEulerAngles;
        cubePosion3.scal = cube3.transform.localScale;
        cubeWrap3.cube = cube3;
        cubeWrap3.startPosion = cubePosion3;
        //cubes.AddLast(cubeWrap3);
        cubesList.Add(cubeWrap3);

        Position4 = cube4.transform.position;
        Angle4 = cube4.transform.localEulerAngles;
        Scal4 = cube4.transform.localScale;
        cubePosion4.position = cube4.transform.position;
        cubePosion4.angle = cube4.transform.localEulerAngles;
        cubePosion4.scal = cube4.transform.localScale;
        cubeWrap4.cube = cube4;
        cubeWrap4.startPosion = cubePosion4;
     //   cubes.AddLast(cubeWrap4);
        cubesList.Add(cubeWrap4);

        Position5 = cube5.transform.position;
        Angle5 = cube5.transform.localEulerAngles;
        Scal5 = cube5.transform.localScale;
        cubePosion5.position = cube5.transform.position;
        cubePosion5.angle = cube5.transform.localEulerAngles;
        cubePosion5.scal = cube5.transform.localScale;
        cubeWrap5.cube = cube5;
        cubeWrap5.startPosion = cubePosion5;
    //    cubes.AddLast(cubeWrap5);
        cubesList.Add(cubeWrap5);
        updateNextLeft();
        log("。。。start  cube0:" + cube0.transform.position.ToString());
        log("。。。start  cube1:" + cube1.transform.position.ToString());
        log("。。。start  cube2:" + cube2.transform.position.ToString());
        log("。。。start  cube3:" + cube3.transform.position.ToString());
        log("。。。start  cube4:" + cube4.transform.position.ToString());
        log("。。。start  cube5:" + cube5.transform.position.ToString());
        onUnityStart();
        READY = true;
    }

    void onUnityClick(int index)
    {
        if (DEBUG) Debug.Log("onUnityClick " + index);
        if (jo != null)
        {
            jo.Call("onUnityClick",index);
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
        if (jo != null) { 
           jo.Call("ULog", msg);
        }
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

    // Update is called once per frame
    void Update()
    {
        if (!READY) return;
        if (mouseDown)
        {
            float longClickTime = Time.time - downtime;
            if (!longPressed && longClickTime > 0.3)
            {
                longPressed = true;
                onLongPressed();
            }
        }
        speed = speed - acce;
        //  log("Update:  speed= " + speed);
       // log("Update:  speed= " + speed);
        if (speed > 2f)
        {
            speed = 2f;
        }
        if (moveDelta >= 1)
        {
            moveDelta = 0;
            foreach (CubeWrap cube in cubesList)
            {
                cube.startPosion = cube.nextPosion;
            }
            if (state == State.LEFT)
            {
                updateNextLeft();
            } else if (state == State.RIGHT)
            {
                updateNextRight();
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
        if (speed > 1f)
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
            if (moveDelta > 0f)
            {
                moveDelta = moveDelta + Time.deltaTime * 1f;
            }
           // log("Update:  speed= " + speed + "  moveDelta=" + moveDelta);
            speed = 0;
            if (state == State.LEFT)
            {
                moveToLeft(moveDelta);
            }
            else if (state == State.RIGHT)
            {
                moveToRight(moveDelta);
            } else
            {
                return;
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

    void updateNextLeft()
    {
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.nextPosion = nextLeftPosion(cubeWrap.cube.transform.position);
            log("updateNextLeft: " + cubeWrap.toString());
        }
    }

    void updateNextRight()
    {
        foreach (CubeWrap cubeWrap in cubesList)
        {
            cubeWrap.nextPosion = nextRightPosion(cubeWrap.cube.transform.position);
            log("updateNextRight: " + cubeWrap.toString());
        }
    }

    void startMove()
    {
        log("startMove,  >>>>>>>>>>>>>");
        if (speed > 0 || moveDelta > 0) return;
        if (state == State.LEFT)
        {
            updateNextLeft();
        } else if (state == State.RIGHT)
        {
            updateNextRight();
        } else
        {
            log("startMove,  >>>>>>>>>>>>>  return");
            return;
        }
        int startIndex = getLeftsideCubeIndex();
        onMoveStart(startIndex);
        moveDelta = Time.deltaTime;
        speed = startSpeed;
        log("startMove,  >>>>>>>>>>>>>  startSpeed: " + startSpeed + "  startIndex: " + startIndex);
    }

    void moveToLeft(float delta)
    {
       // log("---moveToLeft---  delta:" + delta  + "   cubes.Count=" + cubes.Count);
        foreach (CubeWrap cubeWrap in cubesList)
        {
            //log("---moveToLeft---  foreach:" + delta);

            if (cubeWrap.nextPosion.Equals2(LeftoutsideP) && delta > 0.4)
            {
                cubeWrap.startPosion = cubeRightoutside;
                cubeWrap.nextPosion = cubePosion5;
                log("moveToLeft  leftoutside to right outside: " + cubeWrap.toString());
            }
            cubeWrap.cube.transform.position = Vector3.Lerp(cubeWrap.startPosion.position, cubeWrap.nextPosion.position, delta);
            Vector3 startAnge = cubeWrap.startPosion.angle;
            if (startAnge.Equals(Angle_1))
            {
                startAnge = new Vector3(0f, 359f, 0f);
            }
            cubeWrap.cube.transform.localEulerAngles = Vector3.Slerp(startAnge, cubeWrap.nextPosion.angle, delta);
            cubeWrap.cube.transform.localScale = Vector3.Lerp(cubeWrap.startPosion.scal, cubeWrap.nextPosion.scal, delta);
            int index = cubesList.IndexOf(cubeWrap);
        }
    }

    void moveToRight(float delta)
    {
        // log("---moveToLeft---  delta:" + delta  + "   cubes.Count=" + cubes.Count);
        foreach (CubeWrap cubeWrap in cubesList)
        {
            //log("---moveToLeft---  foreach:" + delta);

            if (cubeWrap.nextPosion.Equals2(RightoutsideP) && delta > 0.4)
            {
                cubeWrap.startPosion = cubeLeftoutside;
                cubeWrap.nextPosion = cubePosion0;
                log("moveToLeft  leftoutside to right outside: " + cubeWrap.toString());
            }
            cubeWrap.cube.transform.position = Vector3.Lerp(cubeWrap.startPosion.position, cubeWrap.nextPosion.position, delta);
            Vector3 nextAnge = cubeWrap.nextPosion.angle;
            if (nextAnge.Equals(Angle_1))
            {
                nextAnge = new Vector3(0f, 359f, 0f);
            }
            cubeWrap.cube.transform.localEulerAngles = Vector3.Slerp(cubeWrap.startPosion.angle, nextAnge, delta);
            cubeWrap.cube.transform.localScale = Vector3.Lerp(cubeWrap.startPosion.scal, cubeWrap.nextPosion.scal, delta);
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

    CubePosion nextLeftPosion(Vector3 currentP)
    {

        if (Vector3.Distance(currentP, Position0) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubeLeftoutside");
            return cubeLeftoutside;
        }
 
        if (Vector3.Distance(currentP, Position1) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion0");
            return cubePosion0;
        }

        if (Vector3.Distance(currentP, Position2) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion1");
            return cubePosion1;
        }

        if (Vector3.Distance(currentP, Position3) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion2");
            return cubePosion2;
        }

        if (Vector3.Distance(currentP, Position4) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion3");
            return cubePosion3;
        }


        if (Vector3.Distance(currentP, Position5) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion4");
            return cubePosion4;
        }

        if (Vector3.Distance(currentP, LeftoutsideP) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubePosion5");
            return cubePosion5;
        }
        log("----nextLeftPosion default:cubePosion0 ");

        return cubePosion0;
    }
    CubePosion nextRightPosion(Vector3 currentP)
    {

        if (Vector3.Distance(currentP, Position0) < DISTANCE_DELTA)
        {
            log("nextRightPosion: cubePosion1");
            return cubePosion1;
        }
        if (Vector3.Distance(currentP, Position1) < DISTANCE_DELTA)
        {
            log("nextRightPosion: cubePosion2");
            return cubePosion2;
        }

        if (Vector3.Distance(currentP, Position2) < DISTANCE_DELTA)
        {
            log("nextRightPosion: cubePosion3");
            return cubePosion3;
        }

        if (Vector3.Distance(currentP, Position3) < DISTANCE_DELTA)
        {
            log("nextRightPosion: cubePosion4");
            return cubePosion4;
        }

        if (Vector3.Distance(currentP, Position4) < DISTANCE_DELTA)
        {
            log("nextRightPosion: cubePosion5");
            return cubePosion5;
        }

        if (Vector3.Distance(currentP, Position5) < DISTANCE_DELTA)
        {
            log("nextLeftPosion: cubeRightoutside");
            return cubeRightoutside;
        }

        if (Vector3.Distance(currentP, RightoutsideP) < DISTANCE_DELTA)
        {
            log("nextRightPosion: cubePosion0");
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
