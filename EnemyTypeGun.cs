/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を撃ってくるエネミー
/// </summary>
public class EnemyTypeGun :EnemyBase {

    [Header("EnemyGunnerParameter")]
    [SerializeField,Tooltip("プレイヤーの方を向く速度")]
    private float   rotateSpeed = 2.0f;
    [SerializeField,Tooltip("攻撃間隔 ※フレーム単位"), Range(60, 60 * 10)]
    private int     AttackFrame = 60;
    private int     AttackCount = 0;
    [SerializeField, Tooltip("最大生成数 ※ターゲットしていてもこの数以上生成出来ない"), Range(1, 5)]
    private int     shotNum = 2;

    //Hide variable
    private GameObject          TargetObject;
    private TargetSearch        targetSearch;
    private Shot                shot;
    private ParticleSystem      shotFx;

    //Flags
    private bool AttackafterFrag;
    private bool BforeattackFrag;
    private bool FireFrag;
    private bool FindFrag;

    /*
    [Header("Raycast")]
    [SerializeField,Tooltip("索敵範囲マスク")]
    private LayerMask layerMask = 1 << 8;
    [SerializeField, Tooltip("レイを撃つ座標")]
    private Vector3 rayPos;
    [SerializeField, Tooltip("半径"), Range(0.1f, 20.0f)]
    private float radius;
    private RaycastHit hitInfo;
    */
    /// <summary>
    /// 初期化
    /// </summary>
    public override void MyStart()
    {
        base.MyStart();
        attack = EnemyManager.Instance.GunAttackPower;
        targetSearch = GetComponentInChildren<TargetSearch>();
        shot = GetComponentInChildren<Shot>();
        Anim = GetComponent<Animator>();
        TargetObject = EnemyManager.Instance.targetObject;
        shot.MyStart();
        AttackafterFrag = false;
        //エフェクト
        shotFx = Instantiate(EffectManager.Instance.ShotMotionFx);
        shotFx.transform.SetParent(this.gameObject.transform);
        shotFx.transform.position = shot.transform.position;             //エフェクトの座標
        //α値変更
        Renderer render = GetComponentInChildren<Renderer>();
        mat = render.material;
        Color alpha = mat.color;
        alpha.a = 0;
        mat.color = alpha;
        StartCoroutine(Pop());
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void MyUpdate()
    {
        if (Dead_Flag) { return; }

        //現在再生中のアニメーションの情報を取得
        GetAnimationState();//こいつ重い

        //アニメーションの遷移フラグのリセット
        AnimationFragReset();

        if (targetSearch.IsSearchStay)
        {
            Targetting();

            //弾を撃てるかチェック
            if (shotNum <= shot.BulletsNum) { return; }

            FindFrag = true;

            if (stateInfo.IsTag("bforeattack") == true)
            {
                if (stateInfo.normalizedTime >= 0.4f && AttackafterFrag == false)
                {
                    FireFrag = true;
                    Attack();
                    AttackafterFrag = true;
                }
            }
        }
        else
        {
        }

        CoolTime();

        //アニメーションの速度変更
        ChangeAnimationSpeed();

        //animationの遷移条件フラグ更新
        AnimationFragUpdate();//特に重い
    }

    /// <summary>
    /// ギズモ表示
    /// </summary>
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(rayPos, radius);
    }
    */

    /// <summary>
    /// ターゲティング
    /// </summary>
    private void Targetting()
    {
        float speed = rotateSpeed * StatusManager.NowFrame;
        Vector3 vec = TargetObject.transform.position - this.transform.position;
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.LookRotation(new Vector3(vec.x, 0, vec.z)), speed * Time.deltaTime);
    }

    /// <summary>
    /// レイを撃つ
    /// </summary>
    /*
    private void RayShoot()
    {
        if (Physics.SphereCast(rayPos, radius, Vector3.up, out hitInfo, radius, layerMask, QueryTriggerInteraction.Ignore))
        {
            isTargeting = true;
        }
    }
    */

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        AudioManager.Instance.Play(AudioManager.SE.GunShot);
        shotFx.Play();
        targetSearch.IsSearchStay = false;
        shot.CreateBullet(TargetObject.transform);
        
    }

    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤー
        //if (1 << other.gameObject.layer == EnemyManager.Instance.playerLayer)
        //{
        //  //  Dead_Flag = true;
        //}
    }

    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤー
        //ステージ
        if (1 << collision.gameObject.layer == EnemyManager.Instance.stageLayer)
        {
            Dead();
        }
    }

    /// <summary>
    /// 攻撃後のクールタイム
    /// </summary>
    private void CoolTime()
    {
        if (AttackafterFrag == true)
        {
            AttackCount++;
            if (AttackCount >= AttackFrame)
            {
                AttackCount = 0;
                BforeattackFrag = true;
                AttackafterFrag = false;
            }
        }
    }
   
    //-----------------------------------------------------------------
    //      以下 有吉さん
    //-----------------------------------------------------------------

    //現在再生されているアニメーションの情報を取得
    private void GetAnimationState()
    {
        Anim.Update(0);
        stateInfo = Anim.GetCurrentAnimatorStateInfo(0);
    }

    //アニメーションの遷移フラグ更新
    private void AnimationFragUpdate()
    {
        Anim.SetBool("isFind", FindFrag);
        Anim.SetBool("isFire", FireFrag);
        Anim.SetBool("isBforeattack", BforeattackFrag);
    }

    //アニメーションの速度変更
    private void ChangeAnimationSpeed()
    {
        //---------------------------------------------
        //そもそもこのif文は意味がない
        if (StatusManager.PlayerSpeed!=Anim.speed)
        {
            //---------------------------------------------
            //StatusManager.NowFrameを直に代入しちゃダメ!!
            Anim.speed = StatusManager.NowFrame;
            //---------------------------------------------
        }
        //---------------------------------------------
    }

    //アニメーションの遷移フラグのリセット
    private void AnimationFragReset()
    {
        FireFrag = false;
        FindFrag = false;
        BforeattackFrag = false;
    }
}
