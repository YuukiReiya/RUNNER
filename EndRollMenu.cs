using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRollMenu : MonoBehaviour {

    public const int FiveSec = 300;

    int Count = 0;

    private Animator EndRollAnimator;

    private AnimatorStateInfo stateInfo;

    public void Initialize()
    {

        EndRollAnimator = this.GetComponent<Animator>();
        GetAnimState();

    }

    public bool MyUpdate()
    {
        //カウントを加算
        Count++;

        //五秒に一回アニメーションの情報を取得する
        if(FiveSec>=Count)
        {
            GetAnimState();
            Count = 0;
        }

        //アニメーションが終了していたらタイトルへシーン遷移
        if (stateInfo.normalizedTime >= 1.0)
        {
            return true;
        }

        return false;
    }

    //アニメーションの情報を取得
    private void GetAnimState()
    {
        EndRollAnimator.Update(0);
        stateInfo = EndRollAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
