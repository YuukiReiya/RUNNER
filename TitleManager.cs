using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //仮シーン遷移
        if (Input.GetAxis("Fire1") == 1)
        {
            SceneManager.LoadScene("GameScene");
        }

    }
}
