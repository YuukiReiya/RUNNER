/*
 * 福永理絵
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CametaState
{
    //加速時にカメラが離れる距離
    [Tooltip("加速時にカメラがどれだけ離れるか")]
    public GameObject Leava_Diretion;

    //加速時のカメラの注視点
    [Tooltip("加速時のカメラの注視点")]
    public GameObject Look_Point;

    //加速時にカメラが離れる速度
    [Tooltip("加速時にカメラが離れていく速度")]
    public float Leava_Speed;

    //加速後にカメラが戻る位置
    //[Tooltip("加速後にカメラが戻る位置")]
    //public GameObject Return_Position;

    //カメラが離れ切った時に\nその場にとどまる時間
    [Tooltip("カメラが離れ切った時に\nその距離を維持する時間\n(Wait_Time - 1 している)")]
    public int Wait_Time;

    //加速時にカメラが離れていく速度
    [Tooltip("カメラが近づく速度")]
    public float Approach_Speed;
}

public class MainCamera : MonoBehaviour 
{

    //入力参照
    private Vector3 Axis;

    //プレイヤーオブジェクト
    public GameObject Player;

    //カメラの移動速度
    private Vector3 Move;

    //カメラの移動時間
    private int Timer;

    //カメラがその場にとどまる時間
    private int wait_time;

    //注視点の移動速度
    private float look_speed;

    private GameObject Child;

    private int Level;

    public CametaState[] Camera_State;

    [Tooltip("カメラの注視点となるオブジェクト")]
    public GameObject Look_Point;

    //カメラの操作方法
    [Tooltip("カメラの操作方法\ntrue : 右スティックでカメラ操作\nfalse : LBボタンでカメラ操作")]  
    public bool Select_Camera_Type;
    
    //加速時にカメラが離れていく速度
    [Tooltip("通常時のプレイヤーとカメラの距離\n(Look_Pointの座標 + Normal_Direction)")]
    public Vector3 Normal_Direction;

    [Tooltip("カメラの回転速度")]
    public float Camera_Rotat_Speed;

    //カメラの初期座標
    [Tooltip("カメラの通常座標")]
    public GameObject Default_Pos;

    //カメラの初期注視点
    [Tooltip("カメラの初期注視点")]
    public GameObject Default_LookPos;

    //集中線の表示時間
    [Tooltip("集中線の表示時間")]
    public int Render_Line_Time;
    private int render_line_time;

    [Tooltip("集中線パーティクル")]
    public GameObject Line_Effect;      //lv1時常に表示
    private ParticleSystem Effect;

    private Vector3 Camera_Look_Pos;
    public GameObject Pos;

    private bool skip_Start;

    //レベルアップフラグを記憶する
    private bool levelUp;

    public int ReadyaGo_Timer;

    private float num;

    private bool look_arrive;
    private bool pos_arrive;

    private float before_mag;

    private int arrive;

    public void Initialize()
    {
        Timer = 0;
        wait_time = 0;
        arrive = 0;

        Effect = Line_Effect.GetComponent<ParticleSystem>();
        Effect.Stop();

        render_line_time = 0;
        levelUp = false;

        look_arrive = false;
        pos_arrive = false;

        Camera_Look_Pos = Default_LookPos.transform.position;
        //Camera_Look_Pos = Look_Point.transform.position;

        num = 0.0001f;
        skip_Start = false;

        //カメラを移動させ続ける時間を求める
        float dis = Vector3.Distance(Camera_State[Level].Leava_Diretion.transform.position, Default_Pos.transform.position);       //Player.transform.position - 
        float timer = Mathf.Floor(dis / Camera_State[Level].Approach_Speed);
        //Timer = (int)timer;

        //注視点の移動速度
        dis = Vector3.Distance(Default_LookPos.transform.position, Camera_State[Level].Look_Point.transform.position);
        look_speed = Mathf.Floor(dis / timer);

        this.transform.position = Camera_State[Level].Leava_Diretion.transform.position;
        //Player = GameObject.Find("MainPlayer");
        //Start_Pos = this.transform.position;
        //Operat_Camera = false;
        //Child = transform.FindChild("MainCamera").gameObject;
        //Child.transform.LookAt(this.transform.position);
        ReadyaGo_Timer = StatusManager.Player_Inoperable_Time;

        Vector3 mag = Camera_Look_Pos - Look_Point.transform.position;
        before_mag = mag.magnitude;
    }

    public void MyUpdate()
    {

        //プレイヤーのレベルが上がったら、現在のレベルを取得する
        if (StatusManager.PlayerLevelUp)
        {
            Level = StatusManager.PlayerLevel;
        }

        //スタート時のカメラがスキップされたら、カメラをデフォルトの位置に持っていく
        if (StatusManager.Start_Camera_Skip && !skip_Start)
        {
            skip_Start = true;
            this.transform.position = Default_Pos.transform.position;
        }

        //スタート時のカメラの移動
        if (!StatusManager.Start_Camera_End && !StatusManager.Start_Camera_Skip && !skip_Start)
        {
            UpdateCamera(this.gameObject, Camera_State[0].Leava_Diretion.transform.position, Default_Pos.transform.position, Camera_State[0].Look_Point.transform.position, 0, 0, Camera_State[0].Approach_Speed);
        }
        //通常時のカメラ移動
        else
        {
            UpdateCamera(this.gameObject, Camera_State[Level]);
            Camera_Look_Pos = Look_Point.transform.position;
        }
        this.transform.LookAt(Camera_Look_Pos);

        //パーティクル表示タイマーが0になったら、停止させる
        if (render_line_time == 0)
        {
            Effect.Stop();
        }
        else if (render_line_time > 0)
        {
            render_line_time--;
        }

    }


    //カメラの更新
    void UpdateCamera_Lerp()
    {
        //this.transform.LookAt(Look_Point.transform.position);
        //プレイヤーが加速した時の、カメラの動き
        if (StatusManager.PlayerLevelUp == true)
        {
            levelUp = StatusManager.PlayerLevelUp;
            StatusManager.PlayerLevelUp = false;

            //集中線パーティクル再生
            Effect.Play();
            render_line_time = Render_Line_Time;
        }

        if(levelUp == true)
        {
            Vector3 sa = Vector3.Lerp(this.transform.position.normalized, Camera_State[Level].Leava_Diretion.transform.position.normalized, 0.0001f);
            if(sa.magnitude >= 0.1f)
            {
                this.transform.position += sa;
            }
            else
            {
                levelUp = false;
                wait_time = Camera_State[Level].Wait_Time;
            }
        }

        if(wait_time > 0)
        {
            this.transform.position = Camera_State[Level].Leava_Diretion.transform.position;
            wait_time--;
        }
        else if(wait_time == 0)
        {
            Vector3 sa = Vector3.Lerp(Default_Pos.transform.position.normalized, this.transform.position.normalized, 0.01f);
            this.transform.position += sa;
            wait_time = -1;
        }

    }

    //カメラの更新
    void UpdateCamera()
    {
        //this.transform.LookAt(Look_Point.transform.position);
        //プレイヤーが加速した時の、カメラの動き
        if (StatusManager.PlayerLevelUp == true)
        {
            //カメラを移動させ続ける時間を求める
            float dis = Vector3.Distance(this.transform.position, Camera_State[Level].Leava_Diretion.transform.position);       //Player.transform.position - 
            float timer = Mathf.Floor(dis / Camera_State[Level].Leava_Speed);
            Timer = (int)timer;
            //StatusManager.PlayerLevelUp = false;

            //集中線パーティクル再生
            Effect.Play();
            render_line_time = Render_Line_Time;
        }

        //タイマーが0になったら、カメラをその場にとどまらせる時間を設定する
        if (Timer == 0 && wait_time == -1)
        {
            wait_time = Camera_State[Level].Wait_Time;
        }
        else if (wait_time > 0)
        {
            wait_time--;
        }

        //タイマーが0になるまで、カメラを移動させ続ける
        if (Timer > 0)
        {
            Vector3 ndir = Camera_State[Level].Leava_Diretion.transform.position - this.transform.position;
            this.transform.position += ndir.normalized * Camera_State[Level].Leava_Speed;
            Timer--;
        }
        //タイマーが0で、その場で待機時間も0になったらカメラをデフォルトの位置に戻す
        else if (Timer == 0 && wait_time == 0)
        {
            Vector3 dis = Default_Pos.transform.position - this.transform.position;       //Player.transform.position - 

            Vector3 ndir = Default_Pos.transform.position - this.transform.position;
            Vector3 dis2 = ndir.normalized * Camera_State[Level].Approach_Speed;

            if (dis.magnitude > dis2.magnitude)
            {
                this.transform.position += ndir.normalized * Camera_State[Level].Approach_Speed;
                //Debug.Log("調整");
            }
            //カメラとデフォルトの距離が、移動量よりも短くなった場合カメラをぢフォルトの座標にする
            else
            {
                this.transform.position = Default_Pos.transform.position;
                Timer = -1;
                wait_time = -1;
                //Debug.Log("TimerStop");
            }
        }
    }

    //カメラの更新
    void UpdateCamera(GameObject obj, CametaState cs)
    {
        UpdateCamera(obj, Default_Pos.transform.position, cs.Leava_Diretion.transform.position, cs.Look_Point.transform.position, cs.Leava_Speed, cs.Wait_Time, cs.Approach_Speed);
    }
    
    //カメラの更新
    void UpdateCamera(GameObject obj, Vector3 sp, Vector3 gp, Vector3 lp, float speed, int wait, float aps)
    {
        //this.transform.LookAt(Look_Point.transform.position);
        //プレイヤーが加速した時の、カメラの動き
        arrive = 0;
        if (StatusManager.PlayerLevelUp == true)
        {
            //カメラを移動させ続ける時間を求める
            float dist = Vector3.Distance(sp, gp);       //Player.transform.position - 
            float timer = Mathf.Floor(dist / speed);
            Timer = (int)timer;

            //注視点の移動速度
            dist = Vector3.Distance(lp, Look_Point.transform.position);
            look_speed = Mathf.Floor(dist / timer);

            //Debug.Log("Timer : " + Timer);
            //Debug.Log("PlayerLevelUp : " + StatusManager.PlayerLevelUp);
            //StatusManager.PlayerLevelUp = false;

            //集中線パーティクル再生
            Effect.Play();
            render_line_time = Render_Line_Time;
            StatusManager.PlayerLevelUp = false;
        }

        //タイマーが0になったら、カメラをその場にとどまらせる時間を設定する
        if (Timer == 0 && wait_time == -1)
        {
            wait_time = wait;
        }
        else if (wait_time > 0)
        {
            wait_time--;
        }

        Vector3 dis;
        Vector3 dis2;
        Vector3 ndir;

        //タイマーが0になるまで、カメラを移動させ続ける
        if (Timer > 0)
        {
            ndir = gp - obj.transform.position;
            obj.transform.position += ndir.normalized * speed;
            Timer--;
        }
        //タイマーが0で、その場で待機時間も0になったらカメラをデフォルトの位置に戻す
        else if (Timer == 0 && wait_time == 0)
        {
            dis = Default_Pos.transform.position - obj.transform.position;       //Player.transform.position - 

            ndir = Default_Pos.transform.position - obj.transform.position;
            dis2 = ndir.normalized * aps;

            if (dis.magnitude > dis2.magnitude && pos_arrive == false)
            {
                obj.transform.position += ndir.normalized * aps;

                //ndir = Look_Point.transform.position - Camera_Look_Pos;
                //Camera_Look_Pos += ndir.normalized * aps;
                //Debug.Log("調整");
            }
            //カメラとデフォルトの距離が、移動量よりも短くなった場合カメラをぢフォルトの座標にする
            else
            {
                obj.transform.position = Default_Pos.transform.position;
                pos_arrive = true;
                //Camera_Look_Pos = Look_Point.transform.position;
                //Timer = -1;
                //wait_time = -1;
                //Debug.Log("TimerStop");
            }

            //カメラの注視点を移動させる
            dis = lp - Camera_Look_Pos;

            ndir = lp - Camera_Look_Pos;
            dis2 = ndir.normalized * look_speed;

            if (dis.magnitude > dis2.magnitude && look_arrive == false)
            {
                Camera_Look_Pos += ndir.normalized * look_speed;
                //Debug.Log("調整");
            }
            //カメラとデフォルトの距離が、移動量よりも短くなった場合カメラをぢフォルトの座標にする
            else
            {
                Camera_Look_Pos = lp;
                look_speed = 0;
                look_arrive = true;
                //Debug.Log("TimerStop");
            }

            if (look_arrive && pos_arrive)
            {
                //obj.transform.position = Default_Pos.transform.position;
                Timer = -1;
                wait_time = -1;
                skip_Start = true;

            }
        }

    }


    public Vector2 GetRightAxis()
    {
        return Axis * Camera_Rotat_Speed;
    }
}
