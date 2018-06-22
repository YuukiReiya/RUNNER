/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 突撃時のコライダーの挙動
/// </summary>
public class Charge : MonoBehaviour {

    //Hide variable
    Collider attackCollider;

    //accessor
    public bool isEnable {
        get
        {
            //Nullチェック
            if (!attackCollider)
            {
                //値が入ってなければコンポーネント取得
                attackCollider = GetComponent<Collider>();
            }
            return attackCollider.enabled;
        }
        set
        {
            //Nullチェック
            if (!attackCollider)
            {
                //値が入ってなければコンポーネント取得
                attackCollider = GetComponent<Collider>();
            }
            attackCollider.enabled = value;
        }
    }

}
