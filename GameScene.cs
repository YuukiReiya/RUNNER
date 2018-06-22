/// <summary>
/// 番場宥輝
/// 福永理絵
/// </summary>
#define _DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームシーンの初期化
/// </summary>
public class GameScene :IScene {

    //Hide variable
    private int StartTimer;
    Player player;
    MainCamera mainCamera;
    CameraParent cameraParent;      //追加
    StartEvent startEvent;          //追加
    ReadyGo readyGo;
    
    /// <summary>
    /// 初期化
    /// </summary>
    void IScene.Initialize()
    {
        AudioManager.Instance.FadeIn((int)SceneController.Instance.FadeTime);
        AudioManager.Instance.Play(AudioManager.SE.GameStart);
        AudioManager.Instance.Play(AudioManager.BGM.Game);
        StartTimer = 90;

        StatusManager.ClearMinitue = 0;
        StatusManager.ClearSecond = 0;
        StatusManager.Start_Camera_End = false;
        StatusManager.Start_Camera_Skip = false;
        StatusManager.Player_Inoperable_Time = 0;

        BulletManager.Instance.Initialize();
        EnemyManager.Instance.MyStart();
        if (player == null)
        {
            player =UnityEngine.GameObject.FindObjectOfType<Player>();
        }
        player.Initialized();
        Debug.Log("name=" + player.gameObject);
        if (Timer.Instance == null)
        {
            Timer.Instance = UnityEngine.GameObject.FindObjectOfType<Timer>();
        }
        Timer.Instance.MyStart();

        if (mainCamera == null)
        {
            mainCamera = UnityEngine.GameObject.FindObjectOfType<MainCamera>();
        }
        mainCamera.Initialize();

        //追加
        if (cameraParent == null)
        {
            cameraParent = UnityEngine.GameObject.FindObjectOfType<CameraParent>();
        }
        cameraParent.Initialize();
        if (startEvent == null)
        {
            startEvent = UnityEngine.GameObject.FindObjectOfType<StartEvent>();
        }
        startEvent.Initialize();
        if (readyGo == null)
        {
            readyGo = UnityEngine.GameObject.FindObjectOfType<ReadyGo>();
        }
        readyGo.Initialize();


    }

    /// <summary>
    /// 更新
    /// </summary>
    void IScene.Update()
    {
        if(StartTimer  > 0)
        {
            StartTimer--;
            StatusManager.StartTimer = StartTimer;
        }

        //キャンセルボタンで一番初めのカメラをスキップする
        if (Input.GetButtonDown("Cancel") && !StatusManager.Start_Camera_End)
        {
            //StatusManager.Player_Inoperable_Time = (int)(60 * 2.5f);
            StatusManager.Start_Camera_Skip = true;
        }

        //スタート時のカメラ時の操作不可能時間を減らしていく
        if (StatusManager.Player_Inoperable_Time > 0)
        {
            StatusManager.Player_Inoperable_Time--;
        }

        cameraParent.MyUpdate();        //追加
        player.MyUpdate();
        mainCamera.MyUpdate();
        startEvent.MyUpdate();
        readyGo.MyUpdate();
        Timer.Instance.MyUpdate();
        EnemyManager.Instance.MyUpdate();
        BulletManager.Instance.MyUpdate();
        Debug.Log("ゲームシーンの更新");

#if _DEBUG
        //-------------------------------------------------------------
        /*                       デバッグ                            */
        //タイトルへ
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.LoadLevelFade(new TitleScene());
        }

        //リザルトへ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneController.Instance.LoadScene(new ResultScene());
        }
        //-------------------------------------------------------------
#endif
        //ゲームクリア
        if (IsGameClear())
        {
            StatusManager.ClearMinitue = Timer.Instance.m_iTimeCntMin;
            StatusManager.ClearSecond = Timer.Instance.m_iTimeCntSec;
            StatusManager.ClearFrame = Timer.Instance.m_fFrameCnt;
            AudioManager.Instance.FadeOut((int)SceneController.Instance.FadeTime);
            SceneController.Instance.LoadScene(new ResultScene());
        }

        //ゲームオーバー
        if (IsGameOver())
        {
            AudioManager.Instance.FadeOut((int)SceneController.Instance.FadeTime);
            SceneController.Instance.LoadScene(new GameOverScene());
        }
    }

    /// <summary>
    /// 解放
    /// </summary>
    void IScene.Release()
    {
        UnityEngine.GameObject.Destroy(EnemyManager.Instance.gameObject);
    }

    /// <summary>
    /// ゲームオーバー判定
    /// </summary>
    /// <returns></returns>
    private bool IsGameOver()
    {
        return player.Dead();
    }
    /// <summary>
    /// ゲームクリア判定
    /// </summary>
    /// <returns></returns>
    private bool IsGameClear()
    {
        return player.Clear();
    }
}
