/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Imageを扱う基底クラス
/// </summary>
public class ImageBase : MonoBehaviour {

    private Image image;
    [SerializeField, Tooltip("フェードフレーム")]
    int frame = 60;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        image = GetComponent<Image>();
    }

   
}
