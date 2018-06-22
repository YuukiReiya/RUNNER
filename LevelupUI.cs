using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelupUI{

    private GameObject[] LevelUpEff=new GameObject[4];

    private ParticleSystem ps;

	// Use this for initialization
	public void MyStart () {

        LevelUpEff[1] = GameObject.Find("Lv1FX");
        LevelUpEff[2] = GameObject.Find("Lv2FX");
        LevelUpEff[3] = GameObject.Find("Lv3FX");

    }


    public void LevelUpEffect(int PlayerLevel)
    {
        //effect呼び出し
         LevelUpEff[PlayerLevel].GetComponent<ParticleSystem>().Play();
    }
}
