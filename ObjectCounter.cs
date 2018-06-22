/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ObjectCounter : MonoBehaviour {

    [SerializeField, Tooltip("親オブジェクト")]
    private GameObject parent;

    [ContextMenu("Childs GameObject Counter")]
    private void GetChildsObject()
    {
        Renderer[] childs = parent.GetComponentsInChildren<Renderer>();
        int num = 0;
        foreach(var i in childs)
        {
            num++;
        }
      //  Debug.Log(parent.gameObject.name + "のメッシュレンダラー" + num + "個");
    }
}
