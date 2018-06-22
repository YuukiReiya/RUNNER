using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : IScene {

    /// <summary>
    /// 初期化
    /// </summary>
	void IScene.Initialize()
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    void IScene.Update()
    {
        Debug.Log("ロード");

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneController.Instance.LoadLevelFade(new TitleScene());
        }
    }

    /// <summary>
    /// 解放
    /// </summary>
    void IScene.Release()
    {

    }
}
