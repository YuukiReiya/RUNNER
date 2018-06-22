/*
 * 福永理絵
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Player : Character {

    //---------------//
    //--- private ---//
    //---------------//
    //手の位置
    private Transform Hand;

    //メインカメラ
    private Camera Main_Camera;

    //プレイヤーのレベル
    private int Level;

    //現在の経験値
    private int Point;


    //移動量
    private float Move;

    //移動方向
    private Vector3 Move_Dir;

    //攻撃の接触判定をするタグ
    private string Tag_Name;

    //無敵時間
    private int Invincible_Time;

    //攻撃時間
    private int Attack_Time;

    //攻撃力
    private int Attack_Point;

    //プレイヤーの回転方向
    private float Rotate;

    //移動速度の上限
    private float MoveLimit;

    //移動フラグ
    private bool Move_Flag;

    //パラメータを文字列として受け取るための変数
    private String ParamStr;

    //クリアオブジェクトに触れたフラグ
    private bool Clear_Flag;

    //プレイヤーの進行方向
    //public GameObject 

    //--------------//
    //--- public ---//
    //--------------//
    //MEMO:[System.NonSerialized] を先頭につけることで、publicにしても
    //      inspector上で表示されない

    //プレイヤーの回転
    [Tooltip("プレイヤーの回転")]
    public GameObject Player_Rotate;

    //最大レベル
    [Tooltip("プレイヤーの最大レベル")] 
    public int Level_Max;

    [Tooltip("プレイヤー正面から90°以上回転する際にAxis.xに代入する値。\nこの値を変更すると、90°以上の回転時の速度が変化する。")]
    [Range(0.0f, 1.0f)]
    public float Rotate_Mul;

    [Tooltip("プレイヤーの90°以下の回転補間速度。\n1.0fに近いほど早く補間する。")]
    public float[] Lerp_Time;

    [Tooltip("プレイヤーの90°以上の回転補間速度。\n1.0fに近いほど早く補間する。")]
    public float[] Lerp_Time_Turn;

    //スタート時の操作不可能時間
    [Tooltip("スタート時の操作不可能時間")]
    public int Inoperable_Time;

    //プレイヤーのよろめく時間
    [Tooltip("プレイヤーのよろめく時間\n(フレーム数を指定してください)")]
    public int Stagger_Time = 10;

    //ノックバックさせるときに加える力
    [Tooltip("ノックバックさせるときに加える力")]
    public float Stagger_Power = 2;

    //レベルごとの移動速度
    [Tooltip("レベルごとの移動速度")] 
    public float[] Move_Speed;

    //レベルごとのモーション再生速度
    [Tooltip("レベルごとのモーション再生速度")]
    public float[] Motion_Speed;

    //各レベルごとに必要な経験値
    [Tooltip("レベルごとに必要な経験値")] 
    public int[] Level_Point;

    //レベルごとの敵が吹っ飛ぶ速度
    [Tooltip("レベルごとの敵が吹っ飛ぶ速度\nデフォルト:1000")]
    public float[] Enemy_Fly;



    //effectの列挙体
    enum EffectNum : int
    {
        //パーティクル
        Level1 = 0,         //Level1以上の時表示するエフェクト
        Level3,         //Level3以上の時表示するエフェクト
        Dead,           //死亡時に表示するエフェクト
        ParticleNum,    //使用するパーティクルエフェクトの合計

        Level2_L = 0,       //Level2以上の時表示するトレイル(左足)
        Level2_R,       //Level2以上の時表示するトレイル(右足)
        TrailNum,       //使用するトレイルエフェクトの合計
    }

    //プレイヤーエフェクト
    [Tooltip("プレイヤーで使用するエフェクト\n現在は3種のみ使用できる")]
    public GameObject[] Player_Effect;      //lv1時常に表示
    private ParticleSystem[] Effect = new ParticleSystem[(int)EffectNum.ParticleNum];                //Lv2に常に表示
    private TrailRenderer[] Effect_Trail = new TrailRenderer[(int)EffectNum.TrailNum];            //Lv2に常に表示

    //攻撃フラグ
    [System.NonSerialized]  public bool Attack_Flag;

    //入力参照
    [System.NonSerialized] public Vector3 Axis;
    [System.NonSerialized] public Vector3 AxisRaw;

    private LevelupUI LevelUp_UI;

    private Vector3 beforPos;

    private Vector3 cameraForward;
    private Vector3 moveForward;

    private Rigidbody rb;

    private bool skip_Start;

    //public AnimationState GameStartEffect_Time;
    public int ReadyaGo_Timer;
    
    private int layer_Num;

    //無敵状態フラグ
    private bool invincible;

    private int stagger_time;

    //プレイヤーマテリアル
    private Material Default_Material;



    public void Initialized()
    {
        Anim = GetComponent<Animator>();

        Level = 0;
        Point = 0;
         
        Tag_Name = "Enemy";
        Hand = Anim.GetBoneTransform(HumanBodyBones.RightHand);
        Main_Camera = Camera.main;
        Anim.speed = Motion_Speed[Level];

        Attack_Flag = false;
        Hp = 10;
        Attack_Time = 0;
        Invincible_Time = 0;
        Move_Flag = false;
        Dead_Flag = false;
        skip_Start = false;
        invincible = false;

        Effect[(int)EffectNum.Level1] = Player_Effect[0].GetComponent<ParticleSystem>();
        Effect[(int)EffectNum.Level1].Stop();
        Effect[(int)EffectNum.Level3] = Player_Effect[3].GetComponent<ParticleSystem>();
        Effect[(int)EffectNum.Level3].Stop();
        Effect[(int)EffectNum.Dead] = Player_Effect[4].GetComponent<ParticleSystem>();
        Effect[(int)EffectNum.Dead].Stop();

        Effect_Trail[(int)EffectNum.Level2_L] = Player_Effect[1].GetComponent<TrailRenderer>();
        Effect_Trail[(int)EffectNum.Level2_L].enabled = false;
        Effect_Trail[(int)EffectNum.Level2_R] = Player_Effect[2].GetComponent<TrailRenderer>();
        Effect_Trail[(int)EffectNum.Level2_R].enabled = false;

        LevelUp_UI = new LevelupUI();
        LevelUp_UI.MyStart();

        rb = GetComponent<Rigidbody>();

        Axis.z = 1.0f;

        Level = 0;

        StatusManager.PlayerSpeed = 0.0f;
        StatusManager.PlayerLevel = Level;
        StatusManager.PlayerLevelUp = false;
        StatusManager.PlayerEx = 0;
        StatusManager.Player_Inoperable_Time = Inoperable_Time;

        ReadyaGo_Timer = StatusManager.Inoperable_Time;

        layer_Num = LayerMask.NameToLayer("Attack");

    }


    public void MyUpdate()
    {
        //base.Update();
        UpdateAttack();
        UpdateDamage();

        Vector3 camForward = Camera.main.transform.forward;

        //スタート時のカメラがスキップされたら、指定された位置まで移動する
        if(StatusManager.Start_Camera_Skip && !skip_Start)
        {
            skip_Start = true;
            Level = 1;          //レベルを1にする
            StatusManager.PlayerLevel = Level;
        }
        //スタート時のカメラが終了するまで、正面方向(+Z方向)に移動し続ける
        if (!StatusManager.Start_Camera_End)
        {
            camForward = new Vector3(0, 0, 1);
        }
        //スタート時のカメラが終了したら操作可能にする
        else
        {
            camForward = Camera.main.transform.forward;
            Axis.x = Input.GetAxis("Horizontal");
            Axis.z = Input.GetAxis("Vertical");
        }

        //Axis *= 0.5f;
        //Anim.SetFloat("Speed", 1.0f);
        //アニメーションの再生速度を変更する
        //Anim.speed += 0.001f;

        //プレイヤーの回転補間速度
        float lerp_t = Lerp_Time[Level];

        //よろめく時間を減らす
        if(stagger_time > 0)
        {
            stagger_time--;
        }
        else if(stagger_time <= 0)
        {
            stagger_time = 0;
            invincible = false;
            Anim.SetBool("knockback", false);
        }

        if (Axis.magnitude > 0.1f)
        {
            float sum = 0.0f;
            if (Axis.x < 0.0f)
            {
                sum +=  Axis.x * -1.0f;
            }
            else
            {
                sum += Axis.x;
            }

            if (Axis.z < 0.0f)
            {
                sum += Axis.z * -1.0f;
            }
            else
            {
                sum += Axis.z;
            }

            Axis.Set(Axis.x, 0, Axis.z / sum);
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            cameraForward = Vector3.Scale(camForward, new Vector3(1, 0, 1)).normalized;

            //真後ろに移動する際、X方向のスティックの傾きが0になってしまい、回転しなかったので
            //Zが0以下(後方へ回転)だった場合、Xに1.0fを代入し回転させるようにしている
            float sign = 0.0f;
            if(Axis.z < 0.0f)
            {
                if(Axis.x >= 0)
                {
                    sign = Rotate_Mul;
                }
                else if (Axis.x < 0)
                {
                    sign = -Rotate_Mul;
                }
                lerp_t = Lerp_Time_Turn[Level];
            }
            else
            {
                lerp_t = Lerp_Time[Level];
            }

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            moveForward = cameraForward * Axis.z + Camera.main.transform.right * (Axis.x + sign); // + new Vector3(sign, 0, 0) * Axis.x);
            //Debug.Log("Axis.x : " + Axis.x);
            //Debug.Log("Axis.z : " + Axis.z);
        }
        else
        {
            moveForward = this.transform.forward * Move_Speed[Level];
        }

        if(!Dead_Flag && !Clear_Flag && !invincible)
        {
            //プレイヤーの向いている方向に移動する
            rb.velocity = this.transform.forward * Move_Speed[Level];
        }
        else if(Clear_Flag)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            Vector3 sa = Vector3.Lerp(this.transform.forward, moveForward, lerp_t);
            sa.y = 0.0f;
            transform.rotation = Quaternion.LookRotation(sa);
        }

        UpdateInput();
        if (Input.GetButtonDown("ReLoad") == true)
        {
            ReLoad();
        }

        //StatusManager.PlayerLevelUp = false;

        //敵を倒したら経験値が入る---------------------
        if (Input.GetButtonDown("EnemyKill") == true)
        {
            //Point++;
            //StatusManager.PlayerEx = (float)Point / (float)Level_Point[Level];
            ////各レベルに設定された経験値以上を取得したらレベルアップフラグを立てる
            //if (Point >= Level_Point[Level])
            //{
            //    StatusManager.PlayerLevelUp = true;
            //    Point = 0;
            //}
            LevelUpdate(1);
        }

        if(Dead_Flag || Clear_Flag)
        {
            Anim.speed = 0;
            Effect[(int)EffectNum.Level1].Stop();
            Effect[(int)EffectNum.Level2_L].Stop();
            Effect[(int)EffectNum.Level2_R].Stop();
            Effect[(int)EffectNum.Level3].Stop();
            Axis = new Vector3(0, 0, 0);
        }

        EffectUpdate();

        //------------------------------------------------

        //if (Input.GetButtonDown("Jump") == true)
        //{
        //    this.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.VelocityChange);
        //}
        //if (StatusManager.PlayerLevelUp == true)
        //{
        //    Anim.speed += 0.05f;
        //    MoveLimit += 0.5f;
        //}
    }

    //プレイヤーの攻撃とのあたり判定
    private void OnTriggerEnter(Collider collider)
    {
        //とりあえずタグで敵とのあたり判定
        //if(collider.gameObject.tag == Tag_Name)
        //{
        //    EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();
        //    if (enemy == null)
        //    {
        //        return;
        //    }
        //    enemy.StopAnim();
        //    enemy.Dead();
        //    LevelUpdate(collider.gameObject.GetComponent<EnemyBase>().Exp);
        //    //collider.GetComponent<EnemyBase>().Dead();
        //    //プレイヤーの攻撃と接触したら吹っ飛ばす
        //    Vector3 force = collider.gameObject.transform.position - this.gameObject.transform.position;
        //    collider.gameObject.GetComponent<Rigidbody>().AddForce(force.normalized * Enemy_Fly[Level], ForceMode.Acceleration);
        //}

        if (!invincible)
        {
            //敵の攻撃との当たり判定
            if (collider.tag == "Attack")
            {
                LevelDownUpdate(2);         //敵の攻撃力を入れる
                Effect[(int)EffectNum.Dead].Play();
                //敵の攻撃に触れたらノックバックさせる
                this.gameObject.GetComponent<Rigidbody>().AddForce((this.transform.forward * -1.0f) * Move_Speed[Level] * Stagger_Power, ForceMode.VelocityChange);
                invincible = true;
                stagger_time = Stagger_Time;
                //ノックバックモーションにする
                Anim.SetBool("knockback", true);
            }
        }

        if (collider.tag == "Clear")
        {
            Clear_Flag = true;
        }
        ////タグでステージとのあたり判定
        //if (collider.tag == "Stage")
        //{
        //    Dead_Flag = true;
        //}

    }

    //プレイヤーの攻撃とのあたり判定
    private void OnCollisionEnter(Collision collision)
    {
        //とりあえずタグで敵とのあたり判定
        if (collision.gameObject.tag == Tag_Name)
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy == null)
            {
                return;
            }
            enemy.StopAnim();
            enemy.Dead();
            LevelUpdate(collision.gameObject.GetComponent<EnemyBase>().Exp);
            //collider.GetComponent<EnemyBase>().Dead();
            //プレイヤーの攻撃と接触したら吹っ飛ばす
            Vector3 force = collision.gameObject.transform.position - this.gameObject.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(force.normalized * Enemy_Fly[Level], ForceMode.Acceleration);
        }

        if (!invincible)
        {
            if (collision.gameObject.tag == "Stage")        //collision.gameObject.tag == "Attack" || 
            {
                LevelDownUpdate(2);         //敵の攻撃力を入れる
                Effect[(int)EffectNum.Dead].Play();
                //建物に触れたらノックバックさせる
                this.gameObject.GetComponent<Rigidbody>().AddForce((this.transform.forward * -1.0f) * Move_Speed[Level] * Stagger_Power, ForceMode.VelocityChange);
                invincible = true;
                stagger_time = Stagger_Time;
                //ノックバックモーションにする
                Anim.SetBool("knockback", true);
            }
        }
        if (collision.gameObject.tag == "Clear")
        {
            Clear_Flag = true;
        }

        //タグでステージとのあたり判定
        //if (collision.gameObject.tag == "Stage")
        //{
        //    Dead_Flag = true;
        //}

    }


    //入力の更新
    void UpdateInput()
    {
        if (Input.GetAxis("Fire1") == 1)
        {
            Attack_Flag = true;
            Attack_Time = 10;
        }
    }

    //ファイルの内容再読み込み
    void ReLoad()
    {
        //通常移動速度の読み込み
        FileInfo fi = new FileInfo("Assets/ParameterText/PlayerMove.txt");
        StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8);
        ParamStr = sr.ReadToEnd();
        MoveLimit = Convert.ToSingle(ParamStr);
        sr.Close();
    }

    //ダメージ処理
    void UpdateDamage()
    {
        //
        if (Damage_Flag == true && Invincible_Time <= 0)
        {
            Hp--;
            Invincible_Time = 40;
            //Debug.Log("Tag : " + gameObject.tag.ToString() + "の現在のHP : " + Hp);
            Damage_Flag = false;
        }
        else if (Invincible_Time >= 0)
        {
            Invincible_Time--;
        }

        if (Hp <= 0)
        {
            //Dead_Flag = true;
        }

    }

    //攻撃処理
    void UpdateAttack()
    {
        if (Attack_Time > 0)
        {
            Attack_Time--;
        }
        if (Attack_Time <= 0)
        {
            Attack_Flag = false;
            Attack_Time = 0;
        }
    }

    //レベルアップ
    private void LevelUpdate(int point)
    {
        Point += point;
        StatusManager.PlayerEx = (float)Point / (float)Level_Point[Level];

        StatusManager.PlayerLevelUp = false;
        //各レベルに設定された経験値以上を取得したらレベルアップフラグを立てる
        if (Point >= Level_Point[Level])
        {
            StatusManager.PlayerLevelUp = true;
            Point = 0;
        }
        //レベルアップ処理をする
        if (Input.GetButtonDown("LevelUp") == true || StatusManager.PlayerLevelUp == true)
        {
            //レベルが最大でなければレベルを上げる
            if (Level < Level_Max)
            {
                Level++;
                //Effect[0].Play();

                LevelUp_UI.LevelUpEffect(Level);
            }
            //Debug.Log("レベルアップ");
            StatusManager.PlayerLevelUp = true;
            StatusManager.PlayerLevel = Level;
            Anim.speed = Motion_Speed[Level];
        }

    }

    //レベルダウン処理
    private void LevelDownUpdate(int point)
    {
        //経験値を下げる
        Point -= point;
        //経験値が0未満になったら前のレベルの必要経験値の半分を、現在の所持経験値にする
        if(Point < 0)
        {
            //レベルを下げる
            if(Level > 0)
            {
                Level--;
                //レベルが下がった場合、一つ下のレベルで必要な合計経験値の半分の状態から始める
                Point = Level_Point[Level] / 2;
                if(Level > 0)
                {
                    //Level0のレベルアップ用UIがないので、Level0の場合表示しない
                    LevelUp_UI.LevelUpEffect(Level);
                }
            }
            else
            {
                Point = 0;
            }
        }

        StatusManager.PlayerEx = (float)Point / (float)Level_Point[Level];
        StatusManager.PlayerLevel = Level;
        Anim.speed = Motion_Speed[Level];

    }

    //死亡フラグ
    public bool Dead()
    {
        return Dead_Flag;
    }

    //クリアフラグ
    public bool Clear()
    {
        return Clear_Flag;
    }


    //エフェクト表示
    private void EffectUpdate()
    {
        if(Dead_Flag || Clear_Flag)
        {
            return;
        }

        //Level1のエフェクト再生
        if (Level >= 1)
        {
            Effect[(int)EffectNum.Level1].Play();
        }
        else
        {
            Effect[(int)EffectNum.Level1].Stop();
        }

        //Level2のトレイル再生
        if (Level >= 2)
        {
            Effect_Trail[(int)EffectNum.Level2_L].enabled = true;
            Effect_Trail[(int)EffectNum.Level2_R].enabled = true;
        }
        else
        {
            Effect_Trail[(int)EffectNum.Level2_L].enabled = false;
            Effect_Trail[(int)EffectNum.Level2_R].enabled = false;
        }

        //Level3のエフェクト再生
        if (Level == 3 && !Effect[(int)EffectNum.Level3].isPlaying)
        {
            Effect[(int)EffectNum.Level3].Play();
        }
        else if(Level < 3)
        {
            Effect[(int)EffectNum.Level3].Stop();
        }

    }

    public Vector3 GetMoveForward()
    {
        return moveForward;
    }
}
