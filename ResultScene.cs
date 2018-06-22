using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScene : IScene {

    //TimerFlashing timerFlashing;
    ResultMenu resultMenu;

    /// <summary>
    /// 初期化
    /// </summary>
	void IScene.Initialize()
    {
        AudioManager.Instance.FadeIn((int)SceneController.Instance.FadeTime);
        AudioManager.Instance.Play(AudioManager.BGM.Result);
        resultMenu = UnityEngine.GameObject.FindObjectOfType<ResultMenu>();
        resultMenu.Initialize();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void IScene.Update()
    {
        resultMenu.MyUpdate(
            () =>
            {
                if (resultMenu.menu == ResultMenu.Menu.Continue)
                {
                    AudioManager.Instance.FadeOut((int)SceneController.Instance.FadeTime);
                    SceneController.Instance.LoadLevelFade(new GameScene());
                }
                else
                {
                    AudioManager.Instance.FadeOut((int)SceneController.Instance.FadeTime);
                    SceneController.Instance.LoadLevelFade(new TitleScene());
                }
            }
            );

    }

    /// <summary>
    /// 解放
    /// </summary>
    void IScene.Release()
    {

    }
}
