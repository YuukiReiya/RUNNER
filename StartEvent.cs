using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : MonoBehaviour {

    //プレイヤーがReadyGo終了後にいるべき座標
    [Tooltip("プレイヤーがReadyGo終了後にいるべき座標")]
    public GameObject Player_GameStart_Pos;

    //ReadyGo終了地点から離れる距離
    private float pos;

    //初期速度
    [Tooltip("ReadyGo中の速度")]
    public float Speed;

    //時間
    [Tooltip("ReadyGo終了までの時間")]
    private float time;

    private bool skip_Start;

    // Use this for initialization
    public void Initialize () {
        skip_Start = false;
        time = StatusManager.Player_Inoperable_Time;// - (int)(60 * 2.5f);
        pos = Speed * time;
        this.transform.position = new Vector3(Player_GameStart_Pos.transform.position.x, Player_GameStart_Pos.transform.position.y, Player_GameStart_Pos.transform.position.z - (pos * 0.01f));
    }

    // Update is called once per frame
    public void MyUpdate()
    {
        Vector3 len = this.transform.position - Player_GameStart_Pos.transform.position;
        if (len.magnitude <= 0.2f)
        {
            StatusManager.Start_Camera_Skip = true;
        }
        //スタート時のカメラがスキップされたら、指定された位置まで移動する
        if (StatusManager.Start_Camera_Skip && !skip_Start)
        {
            skip_Start = true;
            this.transform.position = Player_GameStart_Pos.transform.position;
        }
    }
}
