/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームオーバーUI
/// </summary>
public class GameOverImage : MonoBehaviour {

    [SerializeField, Tooltip("ゲームオーバーのUIアニメーションオブジェクト")]
    GameOverImageAnimator gameOverImageAnimator;

    private Image image;
    [SerializeField, Tooltip("フェードフレーム")]
    int frame = 60;
    public delegate void EndFadeFunc();

    public void Initialize(EndFadeFunc endFadeFunc=null)
    {
        //  image = GetComponent<Image>();
        //  StartCoroutine(FadeStartCoroutine(endFadeFunc));
        gameOverImageAnimator.Initialize();
    }

    /// <summary>
    /// フェードの開始
    /// </summary>
    public virtual IEnumerator FadeStartCoroutine(EndFadeFunc endFadeFunc)
    {
        float speed = 1.0f / frame;
        Debug.Log(speed);
        for (int i = 0; i < frame; i++)
        {
            Color color = image.color;
            color.a += speed;
            image.color = color;
            yield return null;
        }
        if (endFadeFunc != null) { endFadeFunc(); }
        yield break;
    }
}
