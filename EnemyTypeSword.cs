/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 剣で襲ってくる敵
/// </summary>
public class EnemyTypeSword : EnemyBase
{

    [Header("EnemySwordParameter")]
    private GameObject TargetObject;
    [SerializeField, Tooltip("移動速度"), Range(1.0f, 50.0f)]
    private float baseSpeed = 5;
    [SerializeField, Tooltip("回転速度"), Range(100.0f, 500.0f)]
    private float rotateSpeed = 300;

    [SerializeField, Tooltip("移動先座標のリストが格納されたオブジェクト")]
    private GameObject TargetListObject;
    [SerializeField, Tooltip("攻撃しているフレーム")]
    private int attackFrame;
    
    //Hide variable
    private TargetSearch targetSearch;
    private Charge attackTrigger;
    private ParticleSystem slashFx;
    private float attackCount;
    private int targetPosIndex;

    //List
    private List<Vector3> targetPosList;

    //Flags
    private bool isRotate;  //回転中
    private bool isDameged; //被ダメ判定
    private bool NowattackFrag=false;       //アニメーション遷移管理フラグ
    private bool BforeattackFrag=false;     //アニメーション遷移管理フラグ
    private bool AttackFrag = false;        //攻撃フラグ
    private bool EffeckFrag = false;

    //constance value
    public const float AnimPlayTime=1.0f;   //1=アニメーション1ループ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void MyStart()
    {
        base.MyStart();
        attack = EnemyManager.Instance.SwordAttackPower;
        TargetObject = EnemyManager.Instance.targetObject;
        attackTrigger = GetComponentInChildren<Charge>();
        targetSearch = GetComponentInChildren<TargetSearch>();
        targetSearch.searchFinishedFunc = ChargingInit;
        Anim = GetComponent<Animator>();

        Debug.Log(attackTrigger);
        attackTrigger.isEnable = false;
        targetPosIndex = 0;
        targetPosList = new List<Vector3>();
        targetPosList.Clear();

        isRotate = true;
        isDameged = false;

        //エフェクト
        slashFx = Instantiate(EffectManager.Instance.SlashFx);
        slashFx.transform.SetParent(this.gameObject.transform);
        slashFx.transform.localPosition = new Vector3(-0.08f, 1.62f, 1.75f);
        slashFx.transform.rotation = this.transform.rotation;

        //移動先の座標をリストに格納
        foreach (var i in TargetListObject.GetComponentsInChildren<Transform>())
        {
            targetPosList.Add(i.position);
        }
        targetPosList.RemoveAt(0);//親オブジェクトをリストから削除


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

        //現在再生アニメーションの情報を取得
        GetAnimationState();

        ChangeAttackFrag();

        //索敵対象のプレイヤーを検知
        if (targetSearch.IsSearch)
        {
            //攻撃構えのモーションへの遷移フラグをオンに
            BforeattackFrag = true;
            //回転
            if (isRotate) { TargetRotation(EnemyManager.Instance.targetObject.transform.position); }
            //回転終了
            else if (AttackFrag == true) { Attack(); }

        }
        //プレイヤーを見つけていない
        else
        {
            BforeattackFrag = false;
            NowattackFrag = false;

            //回転
            if (isRotate) { TargetRotation(targetPosList[targetPosIndex]); }
            //回転終了
            else { Move(); }
        }

        //アニメーションの速度変更
        ChangeAnimationSpeed();

        //アニメーションの遷移フラグを更新
        AnimatonFragUpdate();

    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        //攻撃モーションへの遷移フラグをオンに
        NowattackFrag = true;

        if (EffeckFrag == false)
        {
            EffeckFrag = true;
            attackTrigger.isEnable = true;
            slashFx.Play();
        }

        if (stateInfo.normalizedTime>=AnimPlayTime&&stateInfo.IsTag("nowattack")==true) {
            targetSearch.IsSearch = false;
            isRotate = true;
            AttackFrag = false;
            EffeckFrag = false;
            attackTrigger.isEnable = false;
            slashFx.Stop();
        }

        
 
        float speed = baseSpeed * StatusManager.NowFrame;//速度計算
        this.gameObject.transform.position += this.transform.forward * speed * Time.deltaTime;

    }

    /// <summary>
    /// 突撃方向の決定
    /// </summary>
    private void ChargingInit()
    {
        isRotate = true;
        TargetRotation(EnemyManager.Instance.targetObject.transform.position);
    }

    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
    }

    /// <summary>
    /// 衝突(Collision)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //player
        if (1 << collision.gameObject.layer == EnemyManager.Instance.playerLayer)
        {
            isDameged = true;
        }
        //建物にぶつかったら破棄
        if (1 << collision.gameObject.layer == EnemyManager.Instance.stageLayer) 
        {
            if (isDameged)
            {
                Dead();
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float speed = baseSpeed * StatusManager.NowFrame * Time.deltaTime;
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPosList[targetPosIndex], speed);
        if (this.transform.position == targetPosList[targetPosIndex])
        {
            IndexUpdate();
            isRotate = true;
        }
    }

    /// <summary>
    /// 移動先のインデックス更新
    /// </summary>
    private void IndexUpdate()
    {
        targetPosIndex++;
        if (targetPosIndex == targetPosList.Count)
        {
            targetPosIndex = 0;
        }
    }

    /// <summary>
    /// 目的のTransformに向けて回転処理(補完)を行う
    /// </summary>
    /// <param name="trans"></param>
    private void TargetRotation(Vector3 target)
    {
        float speed = rotateSpeed * StatusManager.NowFrame * Time.deltaTime;
        Vector3 vec = target - this.transform.position;
        Quaternion qua = Quaternion.LookRotation(new Vector3(vec.x, 0, vec.z));
        this.gameObject.transform.rotation = Quaternion.RotateTowards(this.gameObject.transform.rotation, qua, speed);
        if (this.gameObject.transform.rotation == qua)
        {
            isRotate = false;
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
    private void AnimatonFragUpdate()
    {
        Anim.SetBool("isNowattack", NowattackFrag);
        Anim.SetBool("isbforeattack", BforeattackFrag);
    }

    //条件を満たしていれば攻撃フラグを変更
    private void ChangeAttackFrag()
    {
        //攻撃構えモーションが終わっていたら攻撃フラグをtrueに
        if (stateInfo.IsTag("bforeattack") == true && stateInfo.normalizedTime >= AnimPlayTime)
        {
            AttackFrag = true;
        }
    }

    //時間の速度に合わせてアニメーションの速度を変更
    private void ChangeAnimationSpeed()
    {
        if(Anim.speed!=StatusManager.NowFrame)
        {
            Anim.speed = StatusManager.NowFrame;
        }
    }
}
