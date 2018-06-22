/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定したオブジェクトのコライダーとのトリガー衝突検知
/// </summary>
public class TargetSearch : MonoBehaviour {

    [Header("SearchParameter")]
    [SerializeField, Tooltip("サーチ対象のレイヤーマスク")]
    private LayerMask mask = 1 << 8;

    //Hide variable
    private bool isSearch = false;
    private bool isSearchStay = false;

    //accessor
    public bool IsSearch { get { return isSearch; }set { isSearch = value; } }  
    public bool IsSearchStay { get { return isSearchStay; } set { isSearchStay = value; } }

    //CallBack
    public delegate void SearchFinishedFunc();
    public delegate void SearchingFunc();
    public SearchFinishedFunc searchFinishedFunc = null;
    public SearchingFunc searchingFunc = null;
    /// <summary>
    /// コライダーの範囲に入ったらフラグをON
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == mask)
        {
            isSearch = true;
            if (searchFinishedFunc != null)
            {
                searchFinishedFunc();
            }
        }
    }

    /// <summary>
    /// コライダーの範囲内ならフラグをON
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (1 << other.gameObject.layer == mask)
        {
            isSearchStay = true;
            if (searchingFunc != null)
            {
                searchingFunc();
            }
        }
    }
}
