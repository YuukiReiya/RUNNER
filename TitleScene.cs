/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルシーン
/// </summary>
public class TitleScene : IScene {

    TitleMenu titleMenu;

    IEnumerator coroutine;

    /// <summary>
    /// 初期化
    /// </summary>
    void IScene.Initialize()
    {
        ManualCanvas.Instance.Initialize();
        titleMenu = UnityEngine.GameObject.FindObjectOfType<TitleMenu>();
        titleMenu.Initialize();
        AudioManager.Instance.FadeIn((int)SceneController.Instance.FadeTime);
        AudioManager.Instance.Play(AudioManager.BGM.Title);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void IScene.Update()
    {
        titleMenu.MyUpdate(
           () =>
           {
               //？はダメ！
               if (titleMenu.menu == TitleMenu.Menu.GameStart)
               {
                   //SceneController.Instance.LoadLevelFade(new GameScene());
                   SceneController.Instance.ManualSceneLoad();
                   AudioManager.Instance.FadeOut((int)SceneController.Instance.FadeTime);
               }
               else if(titleMenu.menu==TitleMenu.Menu.EndRoll)
               {
                   
                   SceneController.Instance.LoadLevelFade(new EndRollScene());
               }
               else
               {
                   SceneController.Instance.GameEnd();
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
