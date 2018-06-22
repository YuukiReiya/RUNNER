using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMove : Param
{

	// Use this for initialization
	new void Start () {
		TextName = "PlayerMove.txt";
		base.Start ();
//		Parameter = 0.05f;
		Height = 65.0f;
		Label = "PlayerMove    ";
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
	}
}
