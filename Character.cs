/*
 * 福永理絵
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Character : MonoBehaviour {
    //-----------------//
    //--- protected ---//
    //-----------------//
    //移動速度
    //protected Vector3 Move_Spd;

     //死亡フラグ--
    protected bool Dead_Flag;

    //体力--
    protected int Hp;

     //ダメージフラグ--
    protected bool Damage_Flag;

    //キャラクターのアニメーション--
    protected Animator Anim;

    //現在の座標
    protected Vector3 Pos;

    ////移動量
    //protected float Move;

    ////移動方向
    //protected Vector3 Move_Dir;

    ////攻撃の接触判定をするタグ
    //protected string Tag_Name;

    ////無敵時間
    //protected int Invincible_Time;

    ////攻撃時間
    //protected int Attack_Time;

    ////攻撃力
    //protected int Attack_Point;

    ////プレイヤーの回転方向
    //protected float Rotate;

    ////移動速度の上限
    //protected float MoveLimit;

    ////移動フラグ
    //protected bool Move_Flag;

    ////パラメータを文字列として受け取るための変数
    //protected String ParamStr;


    //---------------//
    //--- private ---//
    //---------------//

    //--------------//
    //--- public ---//
    //--------------//
    //攻撃フラグ
    //public bool Attack_Flag;


	// Use this for initialization
	//public void Start () {
 //       Tag_Name = "";
 //       Attack_Flag = false;
 //       Hp = 10;
 //       Attack_Time = 0;
 //       Invincible_Time = 0;
 //       Move_Flag = false;
 //       Dead_Flag = false;
 //   }

    // Update is called once per frame
 //   public void Update () {
 //       UpdateAttack();
 //       UpdateDamage();
	//}

    //virtual void MyStart()
    //{

    //}

    //virtual void MyUpdate()
    //{

    //}

    //トリガーとの当たり判定（当たっている間）
    //public void OnTriggerStay(Collider collider)
    //{
    //    //
    //    if (collider.gameObject.tag == Tag_Name)
    //    {
    //        ////
    //        //if (collider.gameObject.GetComponentInParent<Character>().Attack_Flag == true && Invincible_Time <= 0)
    //        //{
    //        //    Damage_Flag = true;
    //        //    Debug.Log("Attackフラグがtrue");
    //        //}
    //    }
    //}

    ////ダメージ処理
    //void UpdateDamage()
    //{
    //    //
    //    if (Damage_Flag == true && Invincible_Time <= 0)
    //    {
    //        Hp--;
    //        Invincible_Time = 40;
    //        Debug.Log("Tag : " + gameObject.tag.ToString() + "の現在のHP : " + Hp);
    //        Damage_Flag = false;
    //    }
    //    else if(Invincible_Time >= 0)
    //    {
    //        Invincible_Time--;
    //    }

    //    if(Hp <= 0)
    //    {
    //        Dead_Flag = true;
    //    }

    //}

    ////攻撃処理
    //void UpdateAttack()
    //{
    //    if(Attack_Time > 0)
    //    {
    //        Attack_Time--;
    //    }
    //    if(Attack_Time <= 0)
    //    {
    //        Attack_Flag = false;
    //        Attack_Time = 0;
    //    }
    //}

}
