/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作説明を出すシーン
/// </summary>
public class ManualScene : IScene {

    private bool isKey;

    /// <summary>
    /// 初期化
    /// </summary>
    void IScene.Initialize()
    {
        isKey = false;
        ManualCanvas.Instance.StartFadeIn();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void IScene.Update()
    {
        if (isKey) { return; }
        if (Input.GetButtonDown("Fire1"))
        {
            isKey = true;
            ManualCanvas.Instance.StartFadeOut(
                () =>
                {
                    SceneController.Instance.ManualSceneUnload();
                }
                );
        }
    }

    /// <summary>
    /// 解放
    /// </summary>
    void IScene.Release()
    {

    }
}
