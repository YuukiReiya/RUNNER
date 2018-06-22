using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour {

    Image BoostGauge;

    float m_fSavePlayerExp;

    float m_fAdd;

	// Use this for initialization
	void Start () {

        BoostGauge = GetComponent<Image>();
        m_fAdd = 0;
        m_fSavePlayerExp = StatusManager.PlayerEx;
	}
	
	// Update is called once per frame
	void Update () {


        //ゲージの伸びは５フレームで必ず完了するように加算する値を変える
        if(StatusManager.PlayerEx!=m_fSavePlayerExp)
        {
            //加算値を変更
            m_fAdd = (StatusManager.PlayerEx - BoostGauge.fillAmount)/5.0f;

            //saveの値を更新
            m_fSavePlayerExp = StatusManager.PlayerEx;

        }
        

        //現在のゲージ表示割合が現在のプレイヤーの経験値割合より小さかったら
        if(BoostGauge.fillAmount< StatusManager.PlayerEx)
        {

            BoostGauge.fillAmount += m_fAdd;

        }
        //現在のゲージ表示割合が現在のプレイヤーの経験値割合より小さかったら
        else if(BoostGauge.fillAmount > StatusManager.PlayerEx)
        {

            BoostGauge.fillAmount -= 0.01f;

        }

        //近似値になったら値を経験値の割合を代入
        if(StatusManager.PlayerEx+0.01f>=BoostGauge.fillAmount&&StatusManager.PlayerEx-0.01f<=BoostGauge.fillAmount)
        {

            BoostGauge.fillAmount = StatusManager.PlayerEx;

        }


        //ゲージがMAXになったら
        if (BoostGauge.fillAmount == 1)
        {

            StatusManager.PlayerEx = 0;

        }


    }
}
