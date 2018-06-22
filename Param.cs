using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;

public class Param : MonoBehaviour {

	protected decimal Parameter;
	
	//パラメータ調整ボタン表示
	protected bool ParamDebug;

	protected float Height;

	//テキストファイル名
	protected string TextName;

	protected string Label;

	protected int ButtonPosX;

	//パラメータの増減値
	protected decimal Min;
	protected decimal Max;

	// Use this for initialization
	protected void Start () {
//		Parameter = 0.05f;		
		//通常移動速度の読み込み
		FileInfo fi = new FileInfo ("Assets/ParameterText/" + TextName);
		StreamReader sr = new StreamReader (fi.OpenRead(), Encoding.UTF8);
		String ParamStr = sr.ReadToEnd ();
		Parameter = Convert.ToDecimal (ParamStr);
		sr.Close ();

		ParamDebug = false;
		ButtonPosX = 0;

		Min = 0.1m;
		Max = 1.0m;

//		Height = 0.0f;
//		TextName = "";
//		Label = "";
	}
	
	// Update is called once per frame
	protected void Update () {
		//パラメーターの変更
		if(Input.GetKeyDown(KeyCode.F1)){
			ParamDebug = !ParamDebug;
		}

	}

	protected void OnGUI(){
		if(ParamDebug){

			//通常移動の速度調整
			if(GUI.Button(new Rect(160 + ButtonPosX, Height, 30, 30), "<<")){
				Parameter -= Max;
			}
			if(GUI.Button(new Rect(190 + ButtonPosX, Height, 30, 30), "<")){
				Parameter -= Min;
			}
			if(GUI.Button(new Rect(310 + ButtonPosX, Height, 30, 30), ">")){
				Parameter += Min;
			}
			if(GUI.Button(new Rect(340 + ButtonPosX, Height, 30, 30), ">>")){
				Parameter += Max;
			}

			Parameter = Math.Round (Parameter, 3);
				
			if(Parameter <= 0.0m){
				Parameter = 0.0m;
			}

			this.GetComponent<Text>().text = Label + "           " + Parameter.ToString();

			//ParameterをTextで書き出し
			if(GUI.Button(new Rect(375 + ButtonPosX, Height, 40, 30), "OK")){
				StreamWriter sw = new StreamWriter("Assets/ParameterText/" + TextName, false);
				sw.WriteLine(Parameter);
				sw.Flush();
				sw.Close();
			}			
		}
	}

}
