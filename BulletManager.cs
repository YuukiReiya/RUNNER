/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 撃ちだした弾丸のデータ
/// </summary>
public struct BulletData
{
    public Bullet bullet;
    public int shotID;
}

/// <summary>
/// 発射した弾丸のマネージャー
/// </summary>
public class BulletManager : SingletonMonoBehaviour<BulletManager>{

    //Hide variable
    private List<BulletData>        bullets;
    private List<BulletData>        destroyBulletList;       //コレクション操作の回避策
    private Dictionary<int, Shot>   shotDic;


    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        bullets = new List<BulletData>();
        destroyBulletList = new List<BulletData>();
        shotDic = new Dictionary<int, Shot>();
        bullets.Clear();
        destroyBulletList.Clear();
        shotDic.Clear();
    }

    /// <summary>
    /// /更新
    /// </summary>
    public void MyUpdate()
    {
        //更新するバレットがなければ処理しない
        if (bullets.Count == 0) { return; }

        //更新※コレクションでエラーになったらfor文に！
        foreach(var bullet in bullets)
        {
            //更新
            bullet.bullet.MyUpdate();
            if (bullet.bullet.isDestroy) { destroyBulletList.Add(bullet); }
        }

        //削除チェック
        foreach(var i in destroyBulletList )
        {
            DeleteBullet(i);
        }
        destroyBulletList.Clear();
    }

    /// <summary>
    /// Shotの登録
    /// </summary>
    /// <param name="shot"></param>
    /// <param name="id"></param>
    public void AddShot(Shot shot,int id)
    {
        shotDic.Add(id, shot);
    }

    /// <summary>
    /// 弾追加
    /// </summary>
    public void AddBullet(Bullet bullet, int id)
    {
        BulletData data;
        data.bullet = bullet;
        data.shotID = id;
        bullets.Add(data);
    }

    /// <summary>
    /// 弾の削除通知
    /// </summary>
    public void DeleteBullet(BulletData data)
    {
        //減算処理
        shotDic[data.shotID].BulletsNum--;//生成数 減算
        bullets.Remove(data);
        Destroy(data.bullet.gameObject);
    }
}
