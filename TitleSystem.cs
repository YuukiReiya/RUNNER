using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour {

    //define的な何か
    public const int GAME_START = 1;
    public const int GAME_EXIT = 0;

    public Image[] UI;
    private int m_iSelectFrag;

    

	// Use this for initialization
	void Start () {

        m_iSelectFrag = GAME_START;
    
	}
	
	// Update is called once per frame
	void Update () {

        MyUpdate();

	}

    void MyUpdate()
    {
        //debag用
        UI[0].transform.localScale = new Vector3(10.0f, 2.0f, 1.0f);
        UI[1].transform.localScale = new Vector3(10.0f, 2.0f, 1.0f);


        //選択の切り替え
        if (Input.GetAxis("Vertical")==-1&&m_iSelectFrag==GAME_START)
        {
            m_iSelectFrag = GAME_EXIT;
        }
        else if(Input.GetAxis("Vertical") ==1&&m_iSelectFrag==GAME_EXIT)
        {
            m_iSelectFrag = GAME_START;
        }

        //debag用
        UI[m_iSelectFrag].transform.localScale = new Vector3(10.0f, 3.0f, 1.0f);


    }
}
