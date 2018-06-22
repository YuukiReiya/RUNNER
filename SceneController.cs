/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン管理
/// </summary>
public class SceneController :SingletonMonoBehaviour<SceneController> {

    [SerializeField,Tooltip("フェードさせるフレーム")]
    private float fadeTime = 60;
    public float FadeTime { get { return fadeTime; } }

    //現在のシーン
    IScene root;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        //シーンの初期化
        Instance.root = ConvertScene(SceneManager.GetActiveScene().name);
        root.Initialize();
    }

    /// <summary>
    /// シーンの更新
    /// </summary>
    public void SceneUpdate()
    {
        //フェード中は更新しない
        if (!FadeManager.Instance.IsFade)
        {
            root.Update();
        }
    }

    /// <summary>
    /// シーン(State)をレベル(.unity)と紐づける
    /// </summary>
    /// <param name="scene"></param>
    /// <returns>シーンに対応するレベルの名前</returns>
    private string ConvertScene(IScene scene)
    {
        if(scene is TitleScene) { return "TitleScene"; }
        else if(scene is GameScene) { return "GameScene"; }
        else if(scene is EndRollScene) { return "EndRollScene"; }

        Debug.LogError("this is not convert scene! ");
        return null;
    }

    /// <summary>
    /// シーン(State)をレベル(.unity)と紐づける
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns>レベルの名前に対応するシーン</returns>
    private IScene ConvertScene(string sceneName)
    {
        switch (sceneName)
        {
            case "TitleScene":
                return new TitleScene();
            case "GameScene":
                return new GameScene();
            case "GameScene_GD":
                return new GameScene();
            case "EndRollScene":
                return new EndRollScene();
            default:
                break;
        }

        Debug.LogError("<color=red>this is not convert scene!</color>");
        return null;
    }

    /// <summary>
    /// フェードを使ったレベル＋シーンのロード
    /// </summary>
    /// <param name="nextScene"></param>
    public void LoadLevelFade(IScene nextScene)
    {
        //コルーチンの登録
        IEnumerator FadeInFinishedCoroutine = WaitForSceneLoaded(nextScene);
        //匿名関数をコルーチンの引数に
        IEnumerator coroutine = FadeManager.Instance.SceneFadeCoroutine(
            fadeTime,
            () =>
            {
                SceneManager.LoadScene(ConvertScene(nextScene));
                StartCoroutine(FadeInFinishedCoroutine);
            }
            );
        //コルーチンを開始
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// シーンのロード
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="isTransScene"></param>
    public void LoadScene(IScene nextScene)
    {
        root.Release();
        root = nextScene;
        root.Initialize();
    }

    /// <summary>
    /// シーンが遷移するまで待機
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForSceneLoaded(IScene scene)
    {
        root.Release();
        //シーンのロードが終わるまで待機
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == ConvertScene(scene));
        root = scene;
        scene.Initialize();
    }

    /// <summary>
    /// フェードインしてゲームを終了させる
    /// </summary>
    public void GameEnd()
    {
        IEnumerator coroutine = FadeManager.Instance.FadeInCoroutine(
            fadeTime,
            () =>
            {
                Application.Quit();
            }
            );
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// マニュアルシーンのロード
    /// </summary>
    public void ManualSceneLoad()
    {
        IEnumerator coroutine = FadeManager.Instance.FadeInCoroutine(
            fadeTime,
            () => { LoadScene(new ManualScene()); }
            );
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// マニュアルシーンのアンロード
    /// </summary>
    public void ManualSceneUnload()
    {
        IScene scene = new GameScene();
        //シーンを遷移
        SceneManager.LoadScene(ConvertScene(scene));
        //コルーチンの登録
        IEnumerator FadeOutFinishedCoroutine = WaitForSceneLoaded(scene);
        //匿名関数をコルーチンの引数に
        IEnumerator coroutine = FadeManager.Instance.FadeOutCoroutine(
            fadeTime,
            () => 
            {
                StartCoroutine(FadeOutFinishedCoroutine);
            }
            );
        //コルーチンを開始
        StartCoroutine(coroutine);
    }
}
