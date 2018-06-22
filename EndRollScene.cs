using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndRollScene :IScene {

    EndRollMenu endrollMenu;

    //初期化
    void IScene.Initialize()
    {
        endrollMenu = UnityEngine.GameObject.FindObjectOfType<EndRollMenu>();
        endrollMenu.Initialize();
    }

    //更新
    void IScene.Update()
    {
        Debug.Log("エンドロール");
        if(endrollMenu.MyUpdate()==true)
        {
            SceneController.Instance.LoadLevelFade(new TitleScene());
        }
    }

    //解放
    void IScene.Release()
    {

    }

}
