/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト管理クラス
/// ※再生するエフェクトの変数を持っている
/// 　読み取り側でポインタを生成
/// </summary>
public class EffectManager :SingletonMonoBehaviour<EffectManager> {

    [Header("Parameter")]
    [Header("Enemy")]
    [SerializeField,Tooltip("エネミー(銃)弾発射")]
    private ParticleSystem shotMotionFx;
    [SerializeField, Tooltip("エネミー死亡時エフェクト")]
    private ParticleSystem enemyDeadFx;
    [SerializeField, Tooltip("エネミー(剣)攻撃時")]
    private ParticleSystem slashMotionFx;

    //accessor
    public ParticleSystem ShotMotionFx { get { return shotMotionFx; } }
    public ParticleSystem SlashFx { get { return slashMotionFx; } }
    public ParticleSystem EnemyDeadFx { get { return enemyDeadFx; } }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
    }

}
