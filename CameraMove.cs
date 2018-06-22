using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMove : Param
{
    // Use this for initialization
    new void Start()
    {
        TextName = "CameraMove.txt";
        base.Start();
        //		Parameter = 0.05f;
        Height = 100.0f;
        Label = "CameraMove    ";
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
