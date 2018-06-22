using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParent : MonoBehaviour {
    //入力参照
    private Vector3 Axis;

    public float Rot_Speed;
    public GameObject Pos;

    public int ReadyaGo_Timer;


    // Use this for initialization
    //   void Start () {

    //}

    public void Initialize()
    {
        ReadyaGo_Timer = StatusManager.Inoperable_Time;
    }
	
    public void MyUpdate()
    {
        //スタート時のカメラが終了したら入力を受け付ける
        if (StatusManager.Start_Camera_End)
        {
            Axis.x = Input.GetAxis("Horizontal2");
            Axis.z = Input.GetAxis("Vertical2");
        }

        this.transform.position = Pos.transform.position;
        Vector3 sa = Vector3.Lerp(this.transform.forward, Pos.transform.forward, 0.2f);
        transform.rotation = Quaternion.LookRotation(sa);

    }
}
