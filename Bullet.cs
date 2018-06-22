/// <summary>
/// 番場宥輝
/// </summary>
using UnityEngine;

/// <summary>
/// 弾丸
/// </summary>
public class Bullet : MonoBehaviour {

    //Hide variable
    private float bulletSpeed;
    private Vector3 lockPos;

    //accessor
    public bool isDestroy { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="target"></param>
    public void MyStart(Vector3 v3, float Speed, LayerMask player, LayerMask build)
    {

        lockPos = v3;
        isDestroy = false;
        bulletSpeed = Speed;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void MyUpdate()
    {
        float speed = bulletSpeed * StatusManager.NowFrame;
        speed = speed < EnemyManager.Instance.SpeedLimit ? EnemyManager.Instance.SpeedLimit : speed;
        this.gameObject.transform.position += lockPos * speed * Time.deltaTime;
    }

    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("トリガーえんたー"+other.gameObject.name);
        //プレイヤーとの衝突
        if (1 << other.gameObject.layer == EnemyManager.Instance.playerLayer)
        {
        }
        //建物と衝突
        else if (1 << other.gameObject.layer == EnemyManager.Instance.stageLayer)
        {
            Debug.Log("ステージ触れましたTri");
            isDestroy = true;
        }
    }

    /// <summary>
    /// 衝突(コリジョン)
    /// </summary>
    /// <param name="collision"></param>

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("破棄"+collision.gameObject.name);
        //建物と衝突
        if (1 << collision.gameObject.layer == EnemyManager.Instance.stageLayer)
        {
            Debug.Log("ステージ触れましたCol");
            isDestroy = true;
        }

    }

}
