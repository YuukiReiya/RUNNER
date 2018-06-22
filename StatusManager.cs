/// <summary>
/// 番場 宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusManager{

    public static float PlayerSpeed { get; set; }
    public static int PlayerLevel { get; set; }
    public static bool PlayerLevelUp { get; set; }
    public static float PlayerEx { get; set; }
   
    public static int ClearMinitue { get; set; }    //分
    public static int ClearSecond { get; set; }     //秒
    public static int ClearFrame { get; set; }     //

    public static float NowFrame { get; set; }

    public static float Time { get; set; }

    public static int StartTimer { get; set; }

    public static int Readygo_Wait_Time { get; set; }           //ReadyGoが再生されるまでの時間
    public static int Player_Inoperable_Time { get; set; }      //操作不可能な時間
    public static bool Start_Camera_Skip { get; set; }          //ゲーム開始時のカメラスキップフラグ
    public static bool Start_Camera_End { get; set; }           //ゲーム開始時のカメラが終了したフラグ

    //ゲーム開始までの操作不可能なトータル時間
    public static int Inoperable_Time
    {
        get
        {
            return Readygo_Wait_Time + Player_Inoperable_Time;
        }
    }

}