/// <summary>
/// 番場宥輝
/// </summary>
/*
 * 動的なファイル読み込みを行う場合
using System;
using System.IO;
using System.Text;
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM管理
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager> {

    [Header("Parameter")]
    [Header("BGM")]
    [SerializeField, Tooltip("タイトル用BGM")]
    private AudioClip titleClip;
    [SerializeField,Tooltip("ゲーム中BGM")]
    private AudioClip MainClip;
    [SerializeField, Tooltip("リザルト用BGM")]
    private AudioClip resultClip;
    [SerializeField, Tooltip("ゲームオーバー用BGM")]
    private AudioClip gameOverClip;
    [Header("SE")]
    [SerializeField,Tooltip("決定音")]
    private AudioClip decideClip;
    [SerializeField, Tooltip("カーソル音")]
    private AudioClip selectClip;
    [SerializeField, Tooltip("キャンセル用SE")]
    private AudioClip cancelClip;
    [SerializeField, Tooltip("ゲーム開始時SE")]
    private AudioClip gameStartClip;
    [SerializeField,Tooltip("エネミー(銃)の構え時SE")]
    private AudioClip enemyTypeGunStandbyClip;
    [SerializeField, Tooltip("エネミー(銃)の発射SE")]
    private AudioClip enemyTypeGunShotClip;


    //enum
    public enum BGM{
        Title,
        Game,
        Result,
        GameOver,
    };
    public enum SE
    {
        Decide,
        Select,
        Cancel,
        GameStart,
        GunStandby,
        GunShot,
    };

    //Audio Source
    private AudioSource bgmSource;
    private AudioSource seSource;

    //accessor
    public float BGMvol { get { return bgmSource.volume; } set { bgmSource.volume = value; } }
    public float SEvol { get { return seSource.volume; }set { seSource.volume = value; } }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        bgmSource = GetComponent<AudioSource>();

        /*  コンポーネントが無ければ生成    */
        //BGM
        if (!bgmSource) { bgmSource = this.gameObject.AddComponent<AudioSource>(); }
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        //SE
        if (!seSource) { seSource = this.gameObject.AddComponent<AudioSource>(); }
        seSource.loop = false;
        seSource.playOnAwake = false;
    }

    /// <summary>
    /// BGMのフェードアウト
    /// </summary>
    public void FadeOut(int frame)
    {
        IEnumerator coroutine = FadeOutCoroutine(frame);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// BGMのフェードイン
    /// </summary>
    /// <param name="frame"></param>
    public void FadeIn(int frame)
    {
        IEnumerator coroutine = FadeInCoroutine(frame);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// BGMをフェードアウトするコルーチン
    /// </summary>
    /// <param name="frame">フェードのフレーム</param>
    /// <returns></returns>
    private IEnumerator FadeOutCoroutine(float frame)
    {
        float speed = 1 / frame;
        for(int i = 0; i < frame; i++)
        {
            bgmSource.volume -= speed;
            yield return null;
        }
    }

    /// <summary>
    /// BGMをフェードインするコルーチン
    /// </summary>
    /// <param name="frame">フェードのフレーム</param>
    /// <returns></returns>
    private IEnumerator FadeInCoroutine(float frame)
    {
        float speed = 1 / frame;
        for (int i = 0; i < frame; i++)
        {
            bgmSource.volume += speed;
            yield return null;
        }
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="type"></param>
    public void Play(BGM bgm)
    {
        //再生前にBGMを止める
        if (bgmSource.isPlaying) { bgmSource.Stop(); }
        
        //分岐
        switch (bgm)
        {
            case BGM.Title:
                bgmSource.clip = titleClip;
                break;
            case BGM.Game:
                bgmSource.clip = MainClip;
                break;
            case BGM.Result:
                bgmSource.clip = resultClip;
                break;
            case BGM.GameOver:
                bgmSource.clip = gameOverClip;
                break;
        }
        bgmSource.Play();
    }

    public void Play(SE se)
    {
        //再生前にSEを止める
        if (seSource.isPlaying) { seSource.Stop(); }

        //分岐
        switch (se)
        {
            case SE.Decide:
                seSource.clip = decideClip;
                break;
            case SE.Select:
                seSource.clip = selectClip;
                break;
            case SE.Cancel:
                seSource.clip = cancelClip;
                break;
            case SE.GameStart:
                seSource.clip = gameStartClip;
                break;
            case SE.GunShot:
                seSource.clip = enemyTypeGunShotClip;
                break;
            case SE.GunStandby:
                seSource.clip = enemyTypeGunStandbyClip;
                break;
        }
        seSource.Play();
    }
}
