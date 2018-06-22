using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPoint : MonoBehaviour {

    //プレイヤーオブジェクト
    public GameObject Player;
    public GameObject MainCamera;

    [Tooltip("注視点をずらす量")]
    public Vector3 Shift_Pos;

    private Vector3 Axis;

    private Vector3 cameraForward;
    private Vector3 moveForward;

    private Rigidbody rb;

    public int ReadyaGo_Timer;

    public GameObject Goal_Arrow;
    public GameObject Goal_Pos;

    // Use this for initialization
    void Start () {
        this.Initialize();
	}

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        //Player = GameObject.Find("MainPlayer");
        //MainCamera = GameObject.Find("MainCamera");

        ReadyaGo_Timer = StatusManager.Inoperable_Time;
    }

    // Update is called once per frame
    void Update ()
    {
        var dir = Vector3.Scale(cameraForward.normalized, Axis);
        cameraForward = Vector3.Lerp(cameraForward, dir, 0.01f);

        this.transform.position = Player.transform.position;

        transform.rotation = Player.transform.rotation;
       // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            if (ReadyaGo_Timer <= 0)
            {
            }
        }

        Goal_Arrow.transform.LookAt(Goal_Pos.transform.position);

        //FBXがうまく出力できないため、方向転換させたあとx軸を-90°している
        Goal_Arrow.transform.Rotate(new Vector3(Goal_Arrow.transform.rotation.x - 90, Goal_Arrow.transform.rotation.y, Goal_Arrow.transform.rotation.z));

    }
}
