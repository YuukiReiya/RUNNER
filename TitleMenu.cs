/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルメニューのUI
/// </summary>
public class TitleMenu : MonoBehaviour {

    [Header("Parameter")]
    [SerializeField, Tooltip("ゲームスタートUIアニメーター")]
    private Animator StartImageAnimator;
    [SerializeField, Tooltip("ゲーム終了UIアニメーター")]
    private Animator EndImageAnimator;
    [SerializeField, Tooltip("エンドロールのアニメーター")]
    private Animator EndRollImageAnimator;

    //CallBack
    public delegate void DecisionMenu();//決定時呼ばれる

    //Hide variable
    private bool isKey;             //キーの連続入力防止
    private bool isDecision;        //選択中
    private float AxisKey;          //キー入力のポインタ
    private Animator currentState;  //現在選択中のメニュー
    private Animator nonState;    //非選択中のメニュー

    //constance value
    private const int WAIT_FRAME = 40;  //シーン遷移するまでの待機フレーム
    private const int WAIT_KEY_RESET = 40;

    //メニューステート
    public enum Menu
    {
        GameStart,      //ゲームスタート
        GameEnd,        //ゲーム終了
        EndRoll,        //エンドロール
    }
    public Menu menu { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        menu = Menu.GameStart;
        isKey = false;
        isDecision = false;

        currentState = StartImageAnimator;
        currentState.SetTrigger("Next");
        nonState = EndImageAnimator;
    }

    /// <summary>
    /// 更新
    /// </summary>
	public void MyUpdate(DecisionMenu decision = null)
    {
        //キー入力更新
        AxisKey = Input.GetAxis("Vertical");

        //上入力
        if (AxisKey > 0 && !isKey)
        {
            AudioManager.Instance.Play(AudioManager.SE.Select);
            StartCoroutine(KeyResetCoroutine());
            if (menu == Menu.GameStart)
            {
                menu = Menu.GameEnd;
                currentState = EndImageAnimator;
                nonState = StartImageAnimator;
            }
            else if(menu==Menu.EndRoll)
            {
                menu = Menu.GameStart;
                currentState = StartImageAnimator;
                nonState = EndRollImageAnimator;
            }
            else
            {
                menu = Menu.EndRoll;
                currentState = EndRollImageAnimator;
                nonState = EndImageAnimator;

            }
            currentState.SetTrigger("Next");
            nonState.SetTrigger("Prev");
        }
        //下入力
        else if (AxisKey < 0 && !isKey)
        {
            AudioManager.Instance.Play(AudioManager.SE.Select);
            StartCoroutine(KeyResetCoroutine());
            if (menu == Menu.GameStart)
            {
                menu = Menu.EndRoll;
                currentState = EndRollImageAnimator;
                nonState = StartImageAnimator;
            }
            else if(menu==Menu.EndRoll)
            {
                menu = Menu.GameEnd;
                currentState = EndImageAnimator;
                nonState = EndRollImageAnimator;
            }
            else
            {
                menu = Menu.GameStart;
                currentState = StartImageAnimator;
                nonState = EndImageAnimator;
            }
            currentState.SetTrigger("Next");
            nonState.SetTrigger("Prev");
        }
        //キーリセット
       // if (Mathf.Abs(AxisKey)<=0) { isKey = false; }


        //決定
        if (Input.GetButtonDown("Fire1"))
        {
            AudioManager.Instance.Play(AudioManager.SE.Decide);
            StartCoroutine(DecisionMenuCoroutine());
        }

        //コールバック関数
        if (isDecision && decision != null)
        {
            decision();
        }
    }

    /// <summary>
    /// 数フレーム待機してメニューを決定
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecisionMenuCoroutine()
    {
        currentState.SetTrigger("Next");
        isKey = true;
        for(int i=0;i<WAIT_FRAME;i++) { yield return null; }
        isDecision = true;
    }

    /// <summary>
    /// キー入力リセット
    /// </summary>
    /// <returns></returns>
    private IEnumerator KeyResetCoroutine()
    {
        isKey = true;
        for(int i = 0; i < WAIT_KEY_RESET; i++) { yield return null; }
        isKey = false;
    }
}
