using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRanking : MonoBehaviour {

    private string[] HighScoreKey;                 //スコアキー(整数用)
    private string[] HighScoreFrameKey;            //スコアキー(少数用)
    private int[] HighScore;                       //ランキング保存用変数(整数)
    private int[] HighScoreFrame;                  //ランキング保存用変数(少数)

    //ランクイン関連変数群
    private int SaveRankInScore=0;
    private int SaveRankInScoreFrame=0;
    private int SaveNum=-1;
    private int FlashCnt = 0;

    [SerializeField]
    private Image[] HighScoreImage;
    [SerializeField]
    private Sprite[] Num;

    [SerializeField, Tooltip("移動させるオブジェクト")]
    private GameObject[] Ranking;

    [SerializeField, Tooltip("移動目標")]
    private Vector3 MoveToPos = new Vector3(-14332, 0);
    [SerializeField, Tooltip("移動速度")]
    private float moveSpeed=6500;
    [SerializeField, Tooltip("次のUIが移動を開始するまでの時間")]
    private int NextUIMove = 6;
    [SerializeField, Tooltip("UIの変更カラー")]
    private Color UIcolor;

    //define定義的ななにか
    public const int TenMin = 36000;
    public const int OneMin = 3600;
    public const int TenSec = 600;
    public const int OneSec = 60;
    public const int NoRecord = 0;

    enum TimeUIImage
    {
        min_10,
        min_1,
        sec_10,
        sec_1,
        few_1,
        few_2,
        few_3,

        min_10_2,
        min_1_2,
        sec_10_2,
        sec_1_2,
        few_1_2,
        few_2_2,
        few_3_2,

        min_10_3,
        min_1_3,
        sec_10_3,
        sec_1_3,
        few_1_3,
        few_2_3,
        few_3_3,

        min_10_4,
        min_1_4,
        sec_10_4,
        sec_1_4,
        few_1_4,
        few_2_4,
        few_3_4,

        min_10_5,
        min_1_5,
        sec_10_5,
        sec_1_5,
        few_1_5,
        few_2_5,
        few_3_5
    }

    enum HighScoreRank
    {
        One,
        Two,
        Three,
        Four,
        Five
    }

	//初期化
	public void Initialize(){

        HighScore = new int[5];
        HighScoreFrame = new int[5];
        HighScoreKey = new string[5];
        HighScoreFrameKey = new string[5];

        //キー設定
        HighScoreKey[0] = "HIGHSCORE_1";
        HighScoreKey[1] = "HIGHSCORE_2";
        HighScoreKey[2] = "HIGHSCORE_3";
        HighScoreKey[3] = "HIGHSCORE_4";
        HighScoreKey[4] = "HIGHSCORE_5";

        HighScoreFrameKey[0] = "HIGHSCOREFRAME_1";
        HighScoreFrameKey[1] = "HIGHSCOREFRAME_2";
        HighScoreFrameKey[2] = "HIGHSCOREFRAME_3";
        HighScoreFrameKey[3] = "HIGHSCOREFRAME_4";
        HighScoreFrameKey[4] = "HIGHSCOREFRAME_5";

        //debag
        //for (int i = 0; i < 5; i++)
        //{
        //    PlayerPrefs.SetInt(HighScoreKey[i], 0);
        //    PlayerPrefs.SetFloat(HighScoreFrameKey[i], 0);
        //}

        //現在のランキングスコアを取得
        for (int i=0;i<5;i++)
        {
            HighScore[i] = PlayerPrefs.GetInt(HighScoreKey[i]);
            HighScoreFrame[i] = PlayerPrefs.GetInt(HighScoreFrameKey[i]);
        }

        //NoRecordがあれば今回のスコアと差し替えたのちソート
        if (ChangeNoRecord() == false)
        {

            //今回のクリアスコアがランクインしていれば保存したのちソート
            HighScoreSave();

        }

        //ハイスコアの値をUIにセット
        HighScoreSet();

        StartCoroutine(MoveImageCoroutine());

    }

    //ハイスコアUI設定
    private void HighScoreSet()
    {
        //一位のスコアセット
        HighScoreImage[(int)TimeUIImage.min_10].sprite = Num[HighScore[(int)HighScoreRank.One] / TenMin];
        HighScore[(int)HighScoreRank.One] = HighScore[(int)HighScoreRank.One] - ((HighScore[(int)HighScoreRank.One] / TenMin) * TenMin);
        HighScoreImage[(int)TimeUIImage.min_1].sprite = Num[HighScore[(int)HighScoreRank.One] / OneMin];
        HighScore[(int)HighScoreRank.One] = HighScore[(int)HighScoreRank.One] - ((HighScore[(int)HighScoreRank.One] / OneMin) * OneMin);
        HighScoreImage[(int)TimeUIImage.sec_10].sprite = Num[HighScore[(int)HighScoreRank.One] / TenSec];
        HighScore[(int)HighScoreRank.One] = HighScore[(int)HighScoreRank.One] - ((HighScore[(int)HighScoreRank.One] / TenSec) * TenSec);
        HighScoreImage[(int)TimeUIImage.sec_1].sprite = Num[HighScore[(int)HighScoreRank.One] / OneSec];
        HighScore[0] = HighScore[(int)HighScoreRank.One] - ((HighScore[(int)HighScoreRank.One] / OneSec) * OneSec);
        HighScoreImage[(int)TimeUIImage.few_1].sprite = Num[HighScoreFrame[(int)HighScoreRank.One] / 100 % 10];
        HighScoreImage[(int)TimeUIImage.few_2].sprite = Num[HighScoreFrame[(int)HighScoreRank.One] / 10 % 10];
        HighScoreImage[(int)TimeUIImage.few_3].sprite = Num[HighScoreFrame[(int)HighScoreRank.One] % 10];

        //二位のスコアセット
        HighScoreImage[(int)TimeUIImage.min_10_2].sprite = Num[HighScore[(int)HighScoreRank.Two] / TenMin];
        HighScore[(int)HighScoreRank.Two] = HighScore[(int)HighScoreRank.Two] - ((HighScore[(int)HighScoreRank.Two] / TenMin) * TenMin);
        HighScoreImage[(int)TimeUIImage.min_1_2].sprite = Num[HighScore[(int)HighScoreRank.Two] / OneMin];
        HighScore[(int)HighScoreRank.Two] = HighScore[(int)HighScoreRank.Two] - ((HighScore[(int)HighScoreRank.Two] / OneMin) * OneMin);
        HighScoreImage[(int)TimeUIImage.sec_10_2].sprite = Num[HighScore[(int)HighScoreRank.Two] / TenSec];
        HighScore[(int)HighScoreRank.Two] = HighScore[(int)HighScoreRank.Two] - ((HighScore[(int)HighScoreRank.Two] / TenSec) * TenSec);
        HighScoreImage[(int)TimeUIImage.sec_1_2].sprite = Num[HighScore[(int)HighScoreRank.Two] / OneSec];
        HighScore[0] = HighScore[(int)HighScoreRank.Two] - ((HighScore[(int)HighScoreRank.Two] / OneSec) * OneSec);
        HighScoreImage[(int)TimeUIImage.few_1_2].sprite = Num[HighScoreFrame[(int)HighScoreRank.Two] / 100 % 10];
        HighScoreImage[(int)TimeUIImage.few_2_2].sprite = Num[HighScoreFrame[(int)HighScoreRank.Two] / 10 % 10];
        HighScoreImage[(int)TimeUIImage.few_3_2].sprite = Num[HighScoreFrame[(int)HighScoreRank.Two] % 10];

        //三位のスコアセット
        HighScoreImage[(int)TimeUIImage.min_10_3].sprite = Num[HighScore[(int)HighScoreRank.Three] / TenMin];
        HighScore[(int)HighScoreRank.Three] = HighScore[(int)HighScoreRank.Three] - ((HighScore[(int)HighScoreRank.Three] / TenMin) * TenMin);
        HighScoreImage[(int)TimeUIImage.min_1_3].sprite = Num[HighScore[(int)HighScoreRank.Three] / OneMin];
        HighScore[(int)HighScoreRank.Three] = HighScore[(int)HighScoreRank.Three] - ((HighScore[(int)HighScoreRank.Three] / OneMin) * OneMin);
        HighScoreImage[(int)TimeUIImage.sec_10_3].sprite = Num[HighScore[(int)HighScoreRank.Three] / TenSec];
        HighScore[(int)HighScoreRank.Three] = HighScore[(int)HighScoreRank.Three] - ((HighScore[(int)HighScoreRank.Three] / TenSec) * TenSec);
        HighScoreImage[(int)TimeUIImage.sec_1_3].sprite = Num[HighScore[(int)HighScoreRank.Three] / OneSec];
        HighScore[0] = HighScore[(int)HighScoreRank.Three] - ((HighScore[(int)HighScoreRank.Three] / OneSec) * OneSec);
        HighScoreImage[(int)TimeUIImage.few_1_3].sprite = Num[HighScoreFrame[(int)HighScoreRank.Three] / 100 % 10];
        HighScoreImage[(int)TimeUIImage.few_2_3].sprite = Num[HighScoreFrame[(int)HighScoreRank.Three] / 10 % 10];
        HighScoreImage[(int)TimeUIImage.few_3_3].sprite = Num[HighScoreFrame[(int)HighScoreRank.Three] % 10];

        //四位のスコアセット
        HighScoreImage[(int)TimeUIImage.min_10_4].sprite = Num[HighScore[(int)HighScoreRank.Four] / TenMin];
        HighScore[(int)HighScoreRank.Four] = HighScore[(int)HighScoreRank.Four] - ((HighScore[(int)HighScoreRank.Four] / TenMin) * TenMin);
        HighScoreImage[(int)TimeUIImage.min_1_4].sprite = Num[HighScore[(int)HighScoreRank.Four] / OneMin];
        HighScore[(int)HighScoreRank.Four] = HighScore[(int)HighScoreRank.Four] - ((HighScore[(int)HighScoreRank.Four] / OneMin) * OneMin);
        HighScoreImage[(int)TimeUIImage.sec_10_4].sprite = Num[HighScore[(int)HighScoreRank.Four] / TenSec];
        HighScore[(int)HighScoreRank.Four] = HighScore[(int)HighScoreRank.Four] - ((HighScore[(int)HighScoreRank.Four] / TenSec) * TenSec);
        HighScoreImage[(int)TimeUIImage.sec_1_4].sprite = Num[HighScore[(int)HighScoreRank.Four] / OneSec];
        HighScore[0] = HighScore[(int)HighScoreRank.Four] - ((HighScore[(int)HighScoreRank.Four] / OneSec) * OneSec);
        HighScoreImage[(int)TimeUIImage.few_1_4].sprite = Num[HighScoreFrame[(int)HighScoreRank.Four] / 100 % 10];
        HighScoreImage[(int)TimeUIImage.few_2_4].sprite = Num[HighScoreFrame[(int)HighScoreRank.Four] / 10 % 10];
        HighScoreImage[(int)TimeUIImage.few_3_4].sprite = Num[HighScoreFrame[(int)HighScoreRank.Four] % 10];

        //五位のスコアセット
        HighScoreImage[(int)TimeUIImage.min_10_5].sprite = Num[HighScore[(int)HighScoreRank.Five] / TenMin];
        HighScore[(int)HighScoreRank.Five] = HighScore[(int)HighScoreRank.Five] - ((HighScore[(int)HighScoreRank.Five] / TenMin) * TenMin);
        HighScoreImage[(int)TimeUIImage.min_1_5].sprite = Num[HighScore[(int)HighScoreRank.Five] / OneMin];
        HighScore[(int)HighScoreRank.Five] = HighScore[(int)HighScoreRank.Five] - ((HighScore[(int)HighScoreRank.Five] / OneMin) * OneMin);
        HighScoreImage[(int)TimeUIImage.sec_10_5].sprite = Num[HighScore[(int)HighScoreRank.Five] / TenSec];
        HighScore[(int)HighScoreRank.Five] = HighScore[(int)HighScoreRank.Five] - ((HighScore[(int)HighScoreRank.Five] / TenSec) * TenSec);
        HighScoreImage[(int)TimeUIImage.sec_1_5].sprite = Num[HighScore[(int)HighScoreRank.Five] / OneSec];
        HighScore[0] = HighScore[(int)HighScoreRank.Five] - ((HighScore[(int)HighScoreRank.Five] / OneSec) * OneSec);
        HighScoreImage[(int)TimeUIImage.few_1_5].sprite = Num[HighScoreFrame[(int)HighScoreRank.Five] / 100 % 10];
        HighScoreImage[(int)TimeUIImage.few_2_5].sprite = Num[HighScoreFrame[(int)HighScoreRank.Five] / 10 % 10];
        HighScoreImage[(int)TimeUIImage.few_3_5].sprite = Num[HighScoreFrame[(int)HighScoreRank.Five] % 10];
    }

    //ハイスコアのセーブ
    private void HighScoreSave()
    {
        int Score = StatusManager.ClearMinitue * OneMin;
        Score += StatusManager.ClearSecond * OneSec;
        int fScore = StatusManager.ClearFrame;

        //低いスコアから比べていく
        for (int i = 4; i > -1; i--)
        {
            //整数の大小比較
            if (Score < HighScore[i])
            {
                NewRecord(i, Score, fScore);
                ScoreSortAndSave();
                break;
            }
            else if (Score == HighScore[i])
            {
                //少数の大小比較
                if (fScore < HighScoreFrame[i])
                {
                    NewRecord(i, Score, fScore);
                    ScoreSortAndSave();
                    break;
                }
            }
        }

    }

    //ハイスコアのソート（昇順）後に値を保存
    private void ScoreSortAndSave()
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = i + 1; j < 5; ++j)
            {
                //整数の大小比較
                if (HighScore[i] > HighScore[j]&&HighScore[j]!=0)
                {
                    ScoreSwap(i,j);
                }
                else if(HighScore[i]==HighScore[j])
                {
                    //少数の大小比較
                    if(HighScoreFrame[i]>HighScoreFrame[j])
                    {
                        ScoreSwap(i,j);
                    }
                }
            }
        }

        //ソート後の数値格納
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(HighScoreKey[i], HighScore[i]);
            PlayerPrefs.SetInt(HighScoreFrameKey[i], HighScoreFrame[i]);

            //UIの演出用
            if (HighScore[i] == SaveRankInScore && HighScoreFrame[i] == SaveRankInScoreFrame)
            {
                SaveNum = i;
            }
        }

    }

    //スコアの入れ替え
    private void ScoreSwap(int i,int j)
    {
        //一時保存用変数
        int save = 0;
        int fsave = 0;

        //整数の入れ替え
        save = HighScore[i];
        HighScore[i] = HighScore[j];
        HighScore[j] = save;

        //少数の入れ替え
        fsave = HighScoreFrame[i];
        HighScoreFrame[i] = HighScoreFrame[j];
        HighScoreFrame[j] = fsave;
    }

    //ニューレコードの保存
    private void NewRecord(int i,int Score,int fScore)
    {
        //今回のスコアを保存
        HighScore[i] = Score;
        HighScoreFrame[i] = fScore;

        //UI演出用に保存
        SaveRankInScore = Score;
        SaveRankInScoreFrame = fScore;

    }

    //ランキングにNO Recordがあった場合
    private bool ChangeNoRecord()
    {
        int Score = StatusManager.ClearMinitue * OneMin;
        Score += StatusManager.ClearSecond * OneSec;
        int fScore = StatusManager.ClearFrame;
        bool NoRecordFrag = false;

        for (int i=0;i<5;i++)
        {
            if(HighScore[i]==NoRecord&&HighScoreFrame[i]==NoRecord)
            {
                NewRecord(i, Score, fScore);
                NoRecordFrag = true;
                break;
            }
        }

        //トゥルーならソートを実行
        if(NoRecordFrag==true)
        {
            ScoreSortAndSave();
        }

        return NoRecordFrag;

    }

    //UIの色変更
    public void ChangeUIcolorAndFlash()
    {

        if (SaveNum != -1)
        {

            //フラッシュカウントを加算
            FlashCnt++;

            if (FlashCnt > 12) UIcolor.a = 0f;
            if (FlashCnt > 24)
            {
                UIcolor.a = 1f;
                FlashCnt = 0;
            }

            switch (SaveNum)
            {
                case (int)HighScoreRank.One://一位のUIの色変更
                    for (int i = 0; i < 7; i++)
                    {
                        HighScoreImage[i].color = UIcolor;
                    }
                    break;
                case (int)HighScoreRank.Two://二位のUIの色変更
                    for (int i = 7; i < 14; i++)
                    {
                        HighScoreImage[i].color = UIcolor;
                    }
                    break;
                case (int)HighScoreRank.Three://三位のUIの色変更
                    for (int i = 14; i < 21; i++)
                    {
                        HighScoreImage[i].color = UIcolor;
                    }
                    break;
                case (int)HighScoreRank.Four://四位のUIの色変更
                    for (int i = 21; i < 28; i++)
                    {
                        HighScoreImage[i].color = UIcolor;
                    }
                    break;
                case (int)HighScoreRank.Five://五位のUIの色変更
                    for (int i = 28; i < 35; i++)
                    {
                        HighScoreImage[i].color = UIcolor;
                    }
                    break;
            }
        }
    }


    /// <summary>
    /// UIの移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveImageCoroutine()
    {
        float speed = moveSpeed * Time.deltaTime;
        int Count = 0;
       while (Ranking[0].gameObject.transform.localPosition != MoveToPos)
       {
            //カウントを加算
            Count++;

            //五位のランキングUI移動
            Ranking[4].gameObject.transform.localPosition = Vector3.MoveTowards(Ranking[4].gameObject.transform.localPosition, MoveToPos, speed);
            //四位のランキングUI移動
            if(Count> NextUIMove) Ranking[3].gameObject.transform.localPosition = Vector3.MoveTowards(Ranking[3].gameObject.transform.localPosition, MoveToPos, speed);
            //三位のランキングUI移動
            if(Count> NextUIMove*2) Ranking[2].gameObject.transform.localPosition = Vector3.MoveTowards(Ranking[2].gameObject.transform.localPosition, MoveToPos, speed);
            //二位のランキングUI移動
            if(Count> NextUIMove*3) Ranking[1].gameObject.transform.localPosition = Vector3.MoveTowards(Ranking[1].gameObject.transform.localPosition, MoveToPos, speed);
            //一位のランキングUI移動
            if(Count> NextUIMove*4) Ranking[0].gameObject.transform.localPosition = Vector3.MoveTowards(Ranking[0].gameObject.transform.localPosition, MoveToPos, speed);

            yield return null;
       }
        yield break;
    }

}
