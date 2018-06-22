using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオーバーで出すUIのアニメーション
/// </summary>
public class GameOverImageAnimator : MonoBehaviour {

    private Animator anim;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("GmaeOverShowTrigger");
    }
}
