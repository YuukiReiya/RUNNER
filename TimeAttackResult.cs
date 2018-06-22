/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト時のタイム
/// </summary>
public class TimeAttackResult : MonoBehaviour {

    [Header("Parameter")]
    [SerializeField, Tooltip("移動目標")]
    private Vector3 MoveToPos = new Vector3(664, -100);
    [SerializeField, Tooltip("移動速度")]
    private float moveSpeed = 6500.0f;

    [SerializeField]
    private Image[] scoreImage;
    [SerializeField]
    private Sprite[] Num;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        scoreImage[0].sprite = Num[StatusManager.ClearMinitue / 10 % 10];
        scoreImage[1].sprite = Num[StatusManager.ClearMinitue % 10];
        scoreImage[2].sprite = Num[StatusManager.ClearSecond / 10 % 10];
        scoreImage[3].sprite = Num[StatusManager.ClearSecond % 10];
        scoreImage[4].sprite = Num[StatusManager.ClearFrame / 100 % 10];
        scoreImage[5].sprite = Num[StatusManager.ClearFrame / 10 % 10];
        scoreImage[6].sprite = Num[StatusManager.ClearFrame % 10];
        StartCoroutine(MoveImageCoroutine());
    }
	
    /// <summary>
    /// 解放
    /// </summary>
    public void Release()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// UIの移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveImageCoroutine()
    {
        float speed = moveSpeed * Time.deltaTime;
        while (this.gameObject.transform.localPosition != MoveToPos)
        {
            this.gameObject.transform.localPosition = Vector3.MoveTowards(this.gameObject.transform.localPosition, MoveToPos, speed);
            yield return null;
        }
        yield break;
    }
}
