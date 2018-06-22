/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオーバーシーン
/// </summary>
public class GameOverScene : IScene {

    private GameOverMenu gameOverMenu;

    /// <summary>
    /// 初期化
    /// </summary>
    void IScene.Initialize()
    {
        AudioManager.Instance.FadeIn((int)SceneController.Instance.FadeTime);
        AudioManager.Instance.Play(AudioManager.BGM.GameOver);
        gameOverMenu = UnityEngine.GameObject.FindObjectOfType<GameOverMenu>();
        gameOverMenu.Initialize();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void IScene.Update()
    {
        Debug.Log("menuState＝" + gameOverMenu.menu);
        gameOverMenu.MyUpdate(
          () =>
          {
              Debug.Log("決定時は＝" + gameOverMenu.menu);
               //？はダメ！
               if (gameOverMenu.menu == GameOverMenu.Menu.Continue)
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
