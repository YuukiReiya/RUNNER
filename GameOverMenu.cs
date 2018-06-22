/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームオーバーメニュー
/// </summary>
public class GameOverMenu : MonoBehaviour {

    [Header("Parameter")]
    [SerializeField, Tooltip("コンテニューUIアニメーター")]
    private Animator continueImageAnimator;
    [SerializeField, Tooltip("タイトルへUIアニメーター")]
    private Animator toTitleImageAnimator;
    [SerializeField, Tooltip("ゲームオーバーUIアニメーター")]
    private Animator gameOverImageAnimator;

    //CallBack
    public delegate void DecisionMenu();//決定時呼ばれる

    //Hide variable
    private bool isKey;             //キーの連続入力防止
    private bool isDecision;        //選択中
    private float AxisKey;          //キー入力のポインタ
    private Animator currentState;  //現在選択中のメニュー
    private Animator nonState;      //非選択メニュー

    //constance value
    private const int WAIT_ACTIVATE_FRAME = 100;        //UI表示までの待機フレーム
    private const int WAIT_TRANS_SCENE_FRAME = 40;      //シーン遷移までの待機フレーム
    private const int WAIT_KEY_RESET = 40;
    private const float KEY_INPUT_VALUE = 0.3f;

    //メニューステート
    public enum Menu
    {
        Continue,      //コンテニュー
        ToTitle,       //タイトルへ
    }
    public Menu menu { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        gameOverImageAnimator.SetTrigger("GmaeOverShowTrigger");
        StartCoroutine(WaitForActivate());
        menu = Menu.Continue;
        isKey = false;
        isDecision = false;
    }

    /// <summary>
    /// 更新
    /// </summary>
	public void MyUpdate(DecisionMenu decision = null)
    {
        //アクティブ待機
        if (!continueImageAnimator.gameObject.activeSelf && !toTitleImageAnimator.gameObject.activeSelf) { return; }

        //キー入力更新
        AxisKey = Input.GetAxis("Horizontal");

        //右入力
        if (AxisKey > KEY_INPUT_VALUE && !isKey)
        {
            AudioManager.Instance.Play(AudioManager.SE.Select);
            StartCoroutine(KeyResetCoroutine());
            if (menu == Menu. Continue)
            {
                menu = Menu.ToTitle;
                currentState = toTitleImageAnimator;
                nonState = continueImageAnimator;
            }
            else
            {
                menu = Menu. Continue;
                currentState = continueImageAnimator;
                nonState = toTitleImageAnimator;
            }
            currentState.SetTrigger("Next");
            nonState.SetTrigger("Prev");
        }
        //左入力
        else if (AxisKey < -KEY_INPUT_VALUE && !isKey)
        {
            AudioManager.Instance.Play(AudioManager.SE.Select);
            StartCoroutine(KeyResetCoroutine());
            if (menu == Menu. Continue)
            {
                menu = Menu.ToTitle;
                currentState = toTitleImageAnimator;
                nonState = continueImageAnimator;
            }
            else
            {
                menu = Menu.Continue;
                currentState = continueImageAnimator;
                nonState = toTitleImageAnimator;
            }
            currentState.SetTrigger("Next");
            nonState.SetTrigger("Prev");
        }
        //キーリセット
       // if (Mathf.Abs(AxisKey) <= 0) { isKey = false; }

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
    /// メニュー表示までのフレーム待機
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForActivate()
    {
        for(int i = 0; i < WAIT_ACTIVATE_FRAME; i++) { yield return null; }
        //諸々UI初期化
        continueImageAnimator.gameObject.SetActive(true);
        toTitleImageAnimator.gameObject.SetActive(true);
        currentState = continueImageAnimator;
        nonState = toTitleImageAnimator;
        continueImageAnimator.SetTrigger("Next");
    }

    /// <summary>
    /// 数フレーム待機してメニューを決定
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecisionMenuCoroutine()
    {
        currentState.SetTrigger("Next");
        isKey = true;
        for (int i = 0; i < WAIT_TRANS_SCENE_FRAME; i++) { yield return null; }
        isDecision = true;
    }

    /// <summary>
    /// キー入力リセット
    /// </summary>
    /// <returns></returns>
    private IEnumerator KeyResetCoroutine()
    {
        isKey = true;
        for (int i = 0; i < WAIT_KEY_RESET; i++) { yield return null; }
        isKey = false;
    }
}
