/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 撃ちだした弾の管理を行うクラス
/// </summary>
public class Shot : MonoBehaviour {

    [Header("ShotParameter")]
    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject bulletObj;
    [SerializeField, Tooltip("弾速"), Range(0.1f, 100.0f)]
    private float speed;

    //Hide variable
    private int ID;
    private Transform target;
    private LayerMask playerMask;
    private LayerMask buildMask;

    //accessor
    public int BulletsNum { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void MyStart()
    {
        ID = this.gameObject.GetInstanceID();
        BulletsNum = 0;
        //bullets = new List<Bullet>();
        playerMask = EnemyManager.Instance.playerLayer;
        buildMask = EnemyManager.Instance.stageLayer;
        BulletManager.Instance.AddShot(this, ID);
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    /// <param name="target"></param>
   public void CreateBullet(Transform target)
    {
        GameObject tmp = Instantiate(bulletObj);
        tmp.gameObject.transform.position = this.gameObject.transform.position;
        tmp.transform.SetParent(null);
        Bullet bullet = tmp.GetComponent<Bullet>();
        bullet.MyStart(this.gameObject.transform.forward, speed, playerMask, buildMask);
        BulletsNum++;//生成数 加算
        BulletManager.Instance.AddBullet(bullet, ID);
    }

}
