using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerFlashing : MonoBehaviour {

    public static TimerFlashing Instance;

    public Timer timer;

    private int m_iCnt=0;


    public void MyStart()
    {
    }


    public void Flashing()
    {
        m_iCnt++;

        if (m_iCnt==6)
        {
            for (int i = 0; i < 6; i++)
            {
                timer.TimeUI[i].color =new Color(1, 1, 1, 0);
            }
        }
        else if(m_iCnt==12)
        {
            for (int i = 0; i < 6; i++)
            {
                timer.TimeUI[i].color = new Color(1, 1, 1, 1);
            }

            m_iCnt = 0;
        }
        

    }
   
}
