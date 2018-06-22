/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マニュアルを表示するキャンバス
/// </summary>
public class ManualCanvas : SingletonMonoBehaviour<ManualCanvas> {

    [Header("Parameter")]
    [SerializeField, Tooltip("ヒエラルキー内のCanvasGroupアタッチ ※CanvasのSortOrderは1000以上")]
    private CanvasGroup canvasGroup;
    [SerializeField, Tooltip("フェードにかけるフレーム")]
    private int fadeFrame = 40;

    //accessor
    public float alpha { get { return canvasGroup.alpha; } }

    //callback
    public delegate void FadeFinishFunc();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// フェードイン呼び出し用の関数
    /// </summary>
    public void StartFadeIn(FadeFinishFunc fadeFinishFunc = null)
    {
        StartCoroutine(FadeInCanvas(fadeFinishFunc));
    }

    /// <summary>
    /// フェードアウト呼び出し用の関数
    /// </summary>
    public void StartFadeOut(FadeFinishFunc fadeFinishFunc = null)
    {
        StartCoroutine(FadeOutCanvas(fadeFinishFunc));
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInCanvas(FadeFinishFunc finishFunc)
    {
        float speed = 1 / (float)fadeFrame;
        for(int i = 0; i < fadeFrame; i++)
        {
            canvasGroup.alpha += speed;
            yield return null;
        }
        if (finishFunc != null)
        {
            finishFunc();
        }
        yield break;
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutCanvas(FadeFinishFunc finishFunc)
    {
        float speed = 1 / (float)fadeFrame;
        for (int i = 0; i < fadeFrame; i++)
        {
            canvasGroup.alpha -= speed;
            yield return null;
        }
        if (finishFunc != null)
        {
            finishFunc();
        }
        yield break;
    }
}
