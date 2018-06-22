using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleRanking : MonoBehaviour {

    [SerializeField, Tooltip("数字のリソース")]
    private Sprite[] Num;
    [SerializeField, Tooltip("ランキングUI")]
    private Image[] RankingImage;

    //ハイスコア保存用
    private int[] HighScore;
    private int[] HighScoreFrame;

    //初期化
    public void Start()
    {

        HighScore = new int[5];
        HighScoreFrame = new int[5];

        //保存されているハイスコアを取得
        HighScore[0] = PlayerPrefs.GetInt("HIGHSCORE_1");
        HighScore[1] = PlayerPrefs.GetInt("HIGHSCORE_2");
        HighScore[2] = PlayerPrefs.GetInt("HIGHSCORE_3");
        HighScore[3] = PlayerPrefs.GetInt("HIGHSCORE_4");
        HighScore[4] = PlayerPrefs.GetInt("HIGHSCORE_5");

        HighScoreFrame[0] = PlayerPrefs.GetInt("HIGHSCOREFRAME_1");
        HighScoreFrame[1] = PlayerPrefs.GetInt("HIGHSCOREFRAME_2");
        HighScoreFrame[2] = PlayerPrefs.GetInt("HIGHSCOREFRAME_3");
        HighScoreFrame[3] = PlayerPrefs.GetInt("HIGHSCOREFRAME_4");
        HighScoreFrame[4] = PlayerPrefs.GetInt("HIGHSCOREFRAME_5");

        //ランキングのタイムをUIにセット
        SetRanking();

    }


    //ランキングをUIにセット
    private void SetRanking()
    {
        RankingImage[0].sprite = Num[HighScore[0] / 36000];
        HighScore[0] = HighScore[0] - ((HighScore[0] / 36000) * 36000);
        RankingImage[1].sprite = Num[HighScore[0] / 3600];
        HighScore[0] = HighScore[0] - ((HighScore[0] / 3600) * 3600);
        RankingImage[2].sprite = Num[HighScore[0] / 600];
        HighScore[0] = HighScore[0] - ((HighScore[0] / 600) * 600);
        RankingImage[3].sprite = Num[HighScore[0] / 60];
        HighScore[0] = HighScore[0] - ((HighScore[0] / 60) * 60);
        RankingImage[4].sprite = Num[HighScoreFrame[0] / 100 % 10];
        RankingImage[5].sprite = Num[HighScoreFrame[0] / 10 % 10];
        RankingImage[6].sprite = Num[HighScoreFrame[0] % 10];

        RankingImage[7].sprite = Num[HighScore[1] / 36000];
        HighScore[1] = HighScore[1] - ((HighScore[1] / 36000) * 36000);
        RankingImage[8].sprite = Num[HighScore[1] / 3600];
        HighScore[1] = HighScore[1] - ((HighScore[1] / 3600) * 3600);
        RankingImage[9].sprite = Num[HighScore[1] / 600];
        HighScore[1] = HighScore[1] - ((HighScore[1] / 600) * 600);
        RankingImage[10].sprite = Num[HighScore[1] / 60];
        HighScore[1] = HighScore[1] - ((HighScore[1] / 60) * 60);
        RankingImage[11].sprite = Num[HighScoreFrame[1] / 100 % 10];
        RankingImage[12].sprite = Num[HighScoreFrame[1] / 10 % 10];
        RankingImage[13].sprite = Num[HighScoreFrame[1] % 10];

        RankingImage[14].sprite = Num[HighScore[2] / 36000];
        HighScore[2] = HighScore[2] - ((HighScore[2] / 36000) * 36000);
        RankingImage[15].sprite = Num[HighScore[2] / 3600];
        HighScore[2] = HighScore[2] - ((HighScore[2] / 3600) * 3600);
        RankingImage[16].sprite = Num[HighScore[2] / 600];
        HighScore[2] = HighScore[2] - ((HighScore[2] / 600) * 600);
        RankingImage[17].sprite = Num[HighScore[2] / 60];
        HighScore[2] = HighScore[2] - ((HighScore[2] / 60) * 60);
        RankingImage[18].sprite = Num[HighScoreFrame[2] / 100 % 10];
        RankingImage[19].sprite = Num[HighScoreFrame[2] / 10 % 10];
        RankingImage[20].sprite = Num[HighScoreFrame[2] % 10];

        RankingImage[21].sprite = Num[HighScore[3] / 36000];
        HighScore[3] = HighScore[3] - ((HighScore[3] / 36000) * 36000);
        RankingImage[22].sprite = Num[HighScore[3] / 3600];
        HighScore[3] = HighScore[3] - ((HighScore[3] / 3600) * 3600);
        RankingImage[23].sprite = Num[HighScore[3] / 600];
        HighScore[3] = HighScore[3] - ((HighScore[3] / 600) * 600);
        RankingImage[24].sprite = Num[HighScore[3] / 60];
        HighScore[3] = HighScore[3] - ((HighScore[3] / 60) * 60);
        RankingImage[25].sprite = Num[HighScoreFrame[3] / 100 % 10];
        RankingImage[26].sprite = Num[HighScoreFrame[3] / 10 % 10];
        RankingImage[27].sprite = Num[HighScoreFrame[3] % 10];

        RankingImage[28].sprite = Num[HighScore[4] / 36000];
        HighScore[4] = HighScore[4] - ((HighScore[4] / 36000) * 36000);
        RankingImage[29].sprite = Num[HighScore[4] / 3600];
        HighScore[4] = HighScore[4] - ((HighScore[4] / 3600) * 3600);
        RankingImage[30].sprite = Num[HighScore[4] / 600];
        HighScore[4] = HighScore[4] - ((HighScore[4] / 600) * 600);
        RankingImage[31].sprite = Num[HighScore[4] / 60];
        HighScore[4] = HighScore[4] - ((HighScore[4] / 60) * 60);
        RankingImage[32].sprite = Num[HighScoreFrame[4] / 100 % 10];
        RankingImage[33].sprite = Num[HighScoreFrame[4] / 10 % 10];
        RankingImage[34].sprite = Num[HighScoreFrame[4] % 10];
    }

}
