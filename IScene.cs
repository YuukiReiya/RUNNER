using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンのインターフェース
/// </summary>
public interface IScene {

    void Initialize();  //初期化
    void Update();      //更新
    void Release();     //解放
}
