/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームシーンのUI
/// </summary>
public class GameSceneUI : MonoBehaviour {

    [SerializeField,Tooltip("タイムのオブジェクト")]
    private GameObject times;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
    }

    /// <summary>
    /// 解放 
    /// </summary>
    public void Release()
    {

    }

    public void Activeate(bool isActive)
    {
        times.SetActive(isActive);
    }
}
