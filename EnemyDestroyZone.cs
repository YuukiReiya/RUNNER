/// <summary>
/// 番場 宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミー削除ゾーン
/// </summary>
public class EnemyDestroyZone : MonoBehaviour {

    [Header("EnemyDestroy")]
    [SerializeField, Tooltip("衝突")]
    private LayerMask mask = 1 << 9;

    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == mask)
        {
            Destroy(other.gameObject);
        }
    }

    /*
    [SerializeField, Header("FollowObject"), Tooltip("追従するオブジェクトをアタッチ!")]
    private GameObject m_Target;
    private Vector3 m_Directtion;
    /// <summary>
    /// Debugギズモ パラメータ
    /// </summary>
    [SerializeField, Header("Spherecast"), Tooltip("当たり判定レイヤーマスク")]
    private LayerMask m_RayMask = ~0;
    [SerializeField, Header("Position"), Tooltip("レイの原点")]
    private Vector3 m_RayPos;
    [SerializeField, Tooltip("球の半径")]
    private float m_RayRadius;
    private RaycastHit hitInfo;
    [SerializeField, Header("Debug on Gizmos")]
    protected Color m_RayColor = Color.white;
    */
    /*
    public void MyStart()
    {
    }

    /// <summary>
    /// レイ判定を行うだけ
    /// </summary>
    public void MyUpdate()
    {
        m_Directtion = m_Target.transform.forward * (-1);
        if (Physics.SphereCast(this.transform.position + m_RayPos, m_RayRadius, m_Directtion, out hitInfo, m_RayRadius, m_RayMask, QueryTriggerInteraction.Ignore))
        {
            EnemyBase enemy = hitInfo.collider.gameObject.GetComponent<EnemyBase>();
            bool isEnemy = enemy is EnemyBase;
            if (!isEnemy) { return; }
            if (!enemy) { return; }

            //エネミー破棄処理
            Destroy(enemy.gameObject);
        }

    }
    /// <summary>
    /// ギズモ表示
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = m_RayColor;
        m_Directtion = m_Target.transform.forward * (-1);
        Gizmos.DrawWireSphere(this.transform.position + m_RayPos + m_Directtion * m_RayRadius, m_RayRadius);
    }
    */

}
