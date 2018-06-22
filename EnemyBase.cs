/// <summary>
/// 番場 宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの基底クラス
/// ※インターフェイス継承
/// </summary>
public class EnemyBase : Character
{
    [Header("EnemyParameter")]
    [SerializeField, Tooltip("獲得経験値"), Range(1, 100)]
    private int exp;
    [Header("Stealth")]
    [SerializeField, Tooltip("出現までの透過度の遷移")]
    private AnimationCurve StealthCurve;
    [SerializeField, Tooltip("透過値の変化量"), Range(0, 0.1f)]
    private float CurveStealthAddVal = 0.001f;
    [SerializeField, Tooltip("透過時間(フレーム数)")]
    private int StealthCount = 60;

    //constance value
    private const int FX_FRAME = 40;

    //accessor
    public int Exp { get { return exp; } protected set { exp = value; } }
    public bool isDead { get { return Dead_Flag; } }
    public bool isDestroy { get; private set; }
    public int AttackPower { get { return attack; } }

    //Hide variable
    protected int attack;
    protected LayerMask mask = 1 << 8;
    private float CurveStealthRate = 0;
    private ParticleSystem deadFx;

    //Protected variable
    protected Material          mat;
    protected AnimatorStateInfo stateInfo;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void MyStart()
    {
        mask = EnemyManager.Instance.playerLayer;
        isDestroy = false;
        Dead_Flag = false;
        deadFx = Instantiate(EffectManager.Instance.EnemyDeadFx);
        deadFx.transform.SetParent(this.gameObject.transform);
        if (deadFx.isPlaying) { deadFx.Stop(); }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void MyUpdate()
    {
    }

    /// <summary>
    /// ポップ時のコルーチン
    /// 透過させて表示させる
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Pop()
    {
        for (int count = 0; count < StealthCount; count++)
        {
            CurveStealthRate = Mathf.Clamp(CurveStealthRate + CurveStealthAddVal, 0, 1);
            Color alpha = mat.color;
            alpha.a = StealthCurve.Evaluate(CurveStealthRate);
            mat.color = alpha;
            yield return null;
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public virtual void Dead()
    {
        StartCoroutine(DeadCoroutine());
    }

    /// <summary>
    /// エフェクトの再生後死亡処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeadCoroutine()
    {
        //座標調整
        deadFx.transform.position = this.transform.position;
        mat.color = new Color(0, 0, 0, 0);
        deadFx.Play();
        for(int i=0;i<FX_FRAME;i++)
        { yield return null; }
        isDestroy = true;
    }

    /// <summary>
    /// 現在再生中のアニメーションのステートを取得
    /// </summary>
    public void GetCurrentAnimatorState()
    {
        //必要なら Animator.Update(0)を呼び出す、多分不要
        stateInfo = Anim.GetCurrentAnimatorStateInfo(0);
    }

    public void StopAnim()
    {
        Anim.speed = 0;
    }
}