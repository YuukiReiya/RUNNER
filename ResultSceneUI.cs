/// <summary>
/// 番場 宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルトシーンのUI
/// </summary>
public class ResultSceneUI : MonoBehaviour {

    [Header("ResultParameter")]
    [SerializeField, Tooltip("タイムのUIを格納したゲームオブジェクト")]
    private GameObject times;
   
    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        IsActive(false);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void MyUpdate()
    {

    }

    /// <summary>
    /// アクティブの切り替え
    /// </summary>
    /// <param name="isActive"></param>
    public void IsActive(bool isActive)
    {
        times.SetActive(isActive);
    }
}
