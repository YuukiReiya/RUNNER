/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// エネミーのスポナー
/// </summary>
public class EnemySpawner : MonoBehaviour {

    [Header("SettingParameter")]
    [SerializeField, Tooltip("出現させるエネミーのPrefab")]
    private GameObject enemyPrefab;
    [SerializeField, Tooltip("出現させるエネミーの総数"), FormerlySerializedAs("m_TotalEnemyNum"), Range(1, 8)]
    private int totalEnemyNum = 3;
    [SerializeField, Tooltip("出現場所"), FormerlySerializedAs("m_popPos")]
    private Vector3 popPos;

    //constance value
    private const int MAX_POP_NUM = 10;
    private int enemyNum;

    //Hide variable
    private LayerMask layerMask = 1 << 8;

    //accessor
    public int ID { get; private set; }
    public int PopNum { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        ID = this.gameObject.GetInstanceID();
        PopNum = 0;
        layerMask = EnemyManager.Instance.playerLayer;
        enemyNum = totalEnemyNum;

        //上限設定 ※必要なら更新
        enemyNum = totalEnemyNum > MAX_POP_NUM ? MAX_POP_NUM : totalEnemyNum;

    }

    private void OnTriggerEnter(Collider other)
    {
        //生成可能か判定
        if (PopNum >= totalEnemyNum) { return; }

        if (1 << other.gameObject.layer == layerMask)
        {
            EnemyBase instance =
            Instantiate(enemyPrefab, popPos, new Quaternion()).GetComponent<EnemyBase>();
            if (!instance)
            {
                Destroy(instance.gameObject);
                return;
            }
            PopNum++;
            instance.MyStart();//初期化
            EnemyManager.Instance.AddEnemy(instance, ID);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(popPos, 1.5f);
    }
}
