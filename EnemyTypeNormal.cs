/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// モブ敵
/// </summary>
public class EnemyTypeNormal : EnemyBase {

    [Header("EnemyNormalParameter")]
    [SerializeField, Tooltip("初期の移動速度")]
    private float initSpeed;
    [SerializeField, Tooltip("移動先座標のリストが格納されたオブジェクト")]
    private GameObject TargetListObject;

    //Hide variable
    private int targetPosIndex;

    //List
    private List<Vector3> targetPosList;


    /// <summary>
    /// 初期化
    /// </summary>
    public override void MyStart()
    {
        targetPosIndex = 0;//インデックスの初期化

        targetPosList = new List<Vector3>();
        targetPosList.Clear();

        foreach (var i in TargetListObject.GetComponentsInChildren<Transform>())
        {
            targetPosList.Add(i.position);
        }
        targetPosList.RemoveAt(0);//親オブジェクトをリストから削除

        //マテリアルの取得
        Renderer[] render;
        render = GetComponentsInChildren<Renderer>();
        StartCoroutine(Pop());
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void MyUpdate()
    {
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float speed = initSpeed * StatusManager.NowFrame * Time.deltaTime;//速度計算
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPosList[targetPosIndex], speed);
        if (this.transform.position == targetPosList[targetPosIndex])
        {
            IndexUpdate();
        }
    }

    /// <summary>
    /// インデックスの更新
    /// </summary>
    private void IndexUpdate()
    {
        targetPosIndex++;
        if (targetPosIndex == targetPosList.Count)
        {
            targetPosIndex = 0;
        }
    }

    /// <summary>
    /// 衝突(コリジョン)
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (1 << collision.gameObject.layer == 1 << 8)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 衝突(トリガー)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == 1 << 8)
        {
            Destroy(this.gameObject);
        }
    }
}
