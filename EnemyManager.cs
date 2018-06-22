/// <summary>
/// 番場 宥輝
/// </summary>

//#define DisableON //コメントアウトするとActiveは切らない
#define TypeCollision

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// エネミーの管理クラス
/// </summary>
public class EnemyManager : SingletonMonoBehaviour<EnemyManager> {

    [Header("ALL Enemy Parameter")]
    [SerializeField, Tooltip("プレイヤーの当たり判定マスク")]
    public LayerMask playerLayer = 1 << 8;
    [SerializeField, Tooltip("建物(ステージ)の当たり判定マスク")]
    public LayerMask stageLayer = 1 << 10;
    [SerializeField,Tooltip("この値より遅くならない"), Range(0.0f, 10.0f)]
    private float MinSpeed = 0.0f;
    [SerializeField, Tooltip("プレイヤーとエネミーの距離がこの設定値以上離れていたらエネミーは更新しない ※負数はエラー")]
    private float activateDistance = 100.0f;
    [SerializeField, Tooltip("アニメーションを同期させるフレーム ※小さいほどアニメーションの速度の反映が早くなるが、重くもなる"), Range(1, 60)]
    private int animSyncFrame = 30;
    [SerializeField, Tooltip("銃のエネミーの攻撃力")]
    private int gunEnemyAttack = 1;
    [SerializeField]
    private int swordEnemyAttack = 1;

    //Hide variable
    private Dictionary<int, EnemySpawner> spownDic;

    //constance value
    private const int NONE_SPOWN_ENEMY_ID = -1;

    //accessor
    public GameObject targetObject { get; private set; }
    public float SpeedLimit { get { return MinSpeed; } }
    public float ActivateDistance { get { return activateDistance; } }
    public int GunAttackPower { get { return gunEnemyAttack; } }
    public int SwordAttackPower { get { return swordEnemyAttack; } }

    //List
    private List<EnemyData> enemyList = new List<EnemyData>();          //エネミーのリスト
    private List<EnemyData> activateEnemys  = new List<EnemyData>();    //更新するエネミーのリスト

    /// <summary>
    /// エネミーの構造体
    /// </summary>
    struct EnemyData
    {
        public EnemyBase data;
        public int id;
    }

    /// <summary>
    /// EnemyのMyStart()を回す
    /// </summary>
    public void MyStart () {

        spownDic = new Dictionary<int, EnemySpawner>();
        spownDic.Clear();
        targetObject = GameObject.Find("Player/MainPlayer");

        //配列にエネミーが入ってなければ処理を抜ける
        enemyList.Clear();

        //シーン上に配置されているエネミーをリストに格納
        foreach(EnemyBase enemy in UnityEngine.Object.FindObjectsOfType(typeof(EnemyBase))){
            AddEnemy(enemy, NONE_SPOWN_ENEMY_ID);
        }

        //スポーン処理
        foreach(EnemySpawner spawn in UnityEngine.Object.FindObjectsOfType(typeof(EnemySpawner)))
        {
            //スポナーのリスト登録
            spawn.Initialize();
            spownDic.Add(spawn.ID, spawn);
        }

        //登録済みエネミーの初期化
        foreach(var enemy in enemyList)
        {
            enemy.data.MyStart();
        }
    }

    /// <summary>
    /// EnemyのUpdate_()を回す
    /// </summary>
	public void MyUpdate () {

        //配列にエネミーが入ってなければ処理を抜ける
        if (enemyList.Count == 0) { return; }

        //Activateするエネミーの登録
        foreach(var enemy in enemyList)
        {
            if (RegistEnemyList(enemy.data))
            {
                activateEnemys.Add(enemy);
            }
#if DisableON
            else
            {
                enemy.gameObject.SetActive(false);
            }
#endif
        }

        //Activateしたエネミーのアニメーションステートの取得
        foreach(var enemy in activateEnemys)
        {
            if (Time.frameCount % animSyncFrame == 0)
            {
                //現在のアニメーションステートの取得だったり反映処理
            }
        }


        //Activateしたエネミーの更新
        foreach (var enemy in activateEnemys)
        {
            enemy.data.MyUpdate();
            //死んだらリストから削除
            if (enemy.data.isDestroy) { DeleteEnemy(enemy); }
        }
        activateEnemys.Clear();
    }

    /// <summary>
    /// リスト追加
    /// </summary>
    /// <param name="add"></param>
    public void AddEnemy(EnemyBase add,int id)
    {
        EnemyData data;
        data.data = add;
        data.id = id;
        enemyList.Add(data);
    }

    /// <summary>
    /// リスト削除とオブジェクト破棄
    /// </summary>
    /// <param name="enemy"></param>
    private void DeleteEnemy(EnemyData enemy)
    {
        //エネミーの登録IDにスポナーが登録されていれば
        if (enemy.id != NONE_SPOWN_ENEMY_ID)
        {
            //スポナーの生成数を減算する
            spownDic[enemy.id].PopNum--;
        }
        enemyList.Remove(enemy);
        Destroy(enemy.data.gameObject);
    }

    /// <summary>
    /// プレイヤーとエネミーの距離がエネミーをActivateする距離より短いかチェック
    /// </summary>
    /// <param name="enemy">比較対象エネミー</param>
    /// <returns>true:対象距離以下 false:超過</returns>
    private bool RegistEnemyList(EnemyBase enemy)
    {
        return Vector3.Distance(targetObject.transform.position, enemy.gameObject.transform.position) <= ActivateDistance;
    }
}
