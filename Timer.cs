using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static Timer Instance;
    //define的な何か
    public const int OneMinutes = 60;
    public const float SixFrame = 6.0f;

    [Header("CountControl")]
    [SerializeField, Tooltip("レベルごとに、nフレームで1秒進む")]
    public int[] m_iFrameLimit;

    //変数群
    public int m_fFrameCnt { get; private set; }             //フレームカウント用
    public int m_iTimeCntSec { get; private set; }           //時間カウント用(秒)
    public int m_iTimeCntMin { get; private set; }           //時間カウント用（分）
    public Sprite[] Num;                 //
    public Image[] TimeUI;               //

    //タイマーのスプライトカラー変更用変数群
    [SerializeField, Tooltip("タイマーUIの色")]
    private Color[] ObjectiveColor;
    private Color color;                                    //タイマーUIの色
    private Color OriginalColor;                            //originalの色
    private float GradationSpeed=0.1f;                      //グラデーション速度    *0.1をデフォルトとする(6フレームで10%変異先の色に寄る)
    private bool ChangeColorFrag=true;                      //変更先の色か元の色かどちらに変異しているか判別用
    int debagcount=0;

    private int inoperable_Time;

    // Use this for initialization
    public void MyStart()
    {
        //初期化
        m_fFrameCnt = 0;
        m_iTimeCntSec = 0;
        m_iTimeCntMin = 0;
        StatusManager.NowFrame = 60.0f/m_iFrameLimit[StatusManager.PlayerLevel];
        color.a = 1.0f;
        color.r = 1.0f;
        color.b = 1.0f;
        color.g = 1.0f;
        OriginalColor = color;

        inoperable_Time = StatusManager.Inoperable_Time;
    }

    // Update is called once per frame
    public void MyUpdate()
    {
        //スタート時のカメラ移動が終了したらタイマーを動かし始める
        if(!StatusManager.Start_Camera_End)
        {
            return;
        }

        //毎フレーム1加算する
        m_fFrameCnt++;

        //フレームカウントがリミットを越えていたら(秒)
        if (m_iFrameLimit[StatusManager.PlayerLevel] <= m_fFrameCnt)
        {
            //秒加算
            m_iTimeCntSec++;

            //フレームカウントをリセット
            m_fFrameCnt = 0;
        }

        //秒管理変数がリミットを越えたら
        if (m_iTimeCntSec >= OneMinutes)
        {
            //分加算
            m_iTimeCntMin++;

            //秒カウントをリセット
            m_iTimeCntSec = 0;

        }

        //小数点第三位-----------------------------
        TimeUI[2].sprite= Num[(int)(m_fFrameCnt * 100 / (m_iFrameLimit[StatusManager.PlayerLevel] / 10)) % 10];
        //-----------------------------------------

        //小数点第二位-----------------------------
        TimeUI[1].sprite = Num[(int)(m_fFrameCnt*10 / (m_iFrameLimit[StatusManager.PlayerLevel] / 10)) % 10];
        //-----------------------------------------

        //小数点第一位-----------------------------
        TimeUI[0].sprite = Num[(int)(m_fFrameCnt / (m_iFrameLimit[StatusManager.PlayerLevel]/10))% 10];
        //-----------------------------------------

        //一の位の設定（秒）-----------------------
        TimeUI[3].sprite = Num[m_iTimeCntSec % 10];
        //-----------------------------------------

        //十の位の設定（秒）-----------------------
        TimeUI[4].sprite = Num[m_iTimeCntSec / 10];
        //-----------------------------------------

        //一の位の設定（分）-----------------------
        TimeUI[5].sprite = Num[m_iTimeCntMin % 10];
        //-----------------------------------------

        //十の位の設定（分）-----------------------
        TimeUI[6].sprite = Num[m_iTimeCntMin / 10];
        //-----------------------------------------
        

        //フレームが変わったら

        if(StatusManager.NowFrame!=60.0f/m_iFrameLimit[StatusManager.PlayerLevel])

        if(StatusManager.NowFrame!=60/m_iFrameLimit[StatusManager.PlayerLevel])

        if(StatusManager.NowFrame != 60/m_iFrameLimit[StatusManager.PlayerLevel])

        {
            //更新

            StatusManager.NowFrame = 60.0f/m_iFrameLimit[StatusManager.PlayerLevel];

            StatusManager.NowFrame = 60/m_iFrameLimit[StatusManager.PlayerLevel];

            StatusManager.NowFrame = 60.0f / (float)m_iFrameLimit[StatusManager.PlayerLevel];

        }

        TimeColorChangeSeeSpeed();
        
        
    }

    //時間の進行速度の変化をユーザーが感じやすくするための処理
    private void TimeColorChangeSeeSpeed()
    {
        //プレイヤーのレベルが前回より上がっていたら
        ChangeColorLevelUp();

        //現在のフレームレートを参照してグラデーションスピードを変更
        GradationSpeed = 0.1f * StatusManager.NowFrame/(float)(StatusManager.PlayerLevel+2.0f);

        //変更先の色と現在の色を比較して値が同じになるように計算する
        if (ChangeColorFrag == true) CloserObjectiveColor();
        else BackOriginalColor();

        if (ObjectiveColor[StatusManager.PlayerLevel] == color) ChangeColorFrag = false;
        else if (color==OriginalColor) ChangeColorFrag = true;

        for (int i = 0; i < 7; i++)
        {
            //UIの色を変更
            TimeUI[i].color = color;
        }

    }

    //UIの色を目的の色に近づける
    private void CloserObjectiveColor()
    {
        //赤
        if (ObjectiveColor[StatusManager.PlayerLevel].r >= color.r) color.r = ObjectiveColor[StatusManager.PlayerLevel].r;
        else if (ObjectiveColor[StatusManager.PlayerLevel].r < color.r) color.r -= GradationSpeed;
        //青
        if (ObjectiveColor[StatusManager.PlayerLevel].b >= color.b) color.b = ObjectiveColor[StatusManager.PlayerLevel].b;
        else if (ObjectiveColor[StatusManager.PlayerLevel].b < color.b) color.b -= GradationSpeed;
        //緑
        if (ObjectiveColor[StatusManager.PlayerLevel].g >= color.g) color.g = ObjectiveColor[StatusManager.PlayerLevel].g;
        else if (ObjectiveColor[StatusManager.PlayerLevel].g < color.g) color.g -= GradationSpeed;
    }

    //UIの色を元の色に戻す
    private void BackOriginalColor()
    {
        //赤
        if (color.r < OriginalColor.r) color.r += GradationSpeed;
        else if (color.r >= OriginalColor.r) color.r = OriginalColor.r;
        //青
        if (color.b < OriginalColor.b) color.b += GradationSpeed;
        else if (color.b >= OriginalColor.b) color.b = OriginalColor.b;
        //緑
        if (color.g < OriginalColor.g) color.g += GradationSpeed;
        else if (color.g >= OriginalColor.g) color.g = OriginalColor.g;
    }

    //プレイヤーがレベルアップしていた場合
    private void ChangeColorLevelUp()
    {

        if (debagcount!=StatusManager.PlayerLevel)
        {
            color = ObjectiveColor[StatusManager.PlayerLevel];
            ChangeColorFrag = false;
            for (int i = 0; i < 7; i++)
            {
                //UIの色を変更
                TimeUI[i].color = color;
            }
        }

        debagcount = StatusManager.PlayerLevel;
    }

}