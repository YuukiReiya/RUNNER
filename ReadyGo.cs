using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyGo : MonoBehaviour {

    public float Wait_Play_Time;
    private int wait_play_time;
    private float duration;
    private bool inoperable;
    // Use this for initialization
    void Start()
    {
        this.GetComponent<Animator>().speed = 0.0f;
    }

    public void Initialize()
    {
        inoperable = false;
        this.GetComponent<Animator>().speed = 0.0f;

        AnimatorStateInfo currentState = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        duration = currentState.length;

        //StatusManager.Readygo_Wait_Time = Wait_Play_Time;
        wait_play_time = StatusManager.Player_Inoperable_Time;
    }

    // Update is called once per frame
    public void MyUpdate () {

        //ゲーム開始時のカメラの残り時間がReadyGoの再生時間以下になるか、カメラがスキップされる
        if (((StatusManager.Start_Camera_Skip && this.GetComponent<Animator>().speed <= 0.0f) || StatusManager.Player_Inoperable_Time == (int)(60 * duration)) && !inoperable)
        {
            StatusManager.Player_Inoperable_Time = (int)(60 * duration);       //ReadyGoの再生時間分の残り時間にする
            wait_play_time = (int)(60 * 2.5f);
            this.GetComponent<Animator>().speed = 1.0f;                         //アニメーションを再生させる
            StatusManager.Start_Camera_Skip = false;
            inoperable = true;
        }

        if (wait_play_time > 0)
        {
            wait_play_time--;
        }

        if(wait_play_time <= 0)
        {
            StatusManager.Start_Camera_End = true;
        }
    }
}
