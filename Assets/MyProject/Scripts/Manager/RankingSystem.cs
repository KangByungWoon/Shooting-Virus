using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingSystem : MonoBehaviour
{
    public Text Score;
    public Text KillEnemy;

    public List<string> Ranknames = new List<string>();
    public List<int> Rankscores = new List<int>();

    public List<Text> NameTexts = new List<Text>();
    public List<Text> ScoreTexts = new List<Text>();

    public InputField Join;

    [SerializeField] PlayableDirector ResultWindow;
    [SerializeField] PlayableDirector RankJoinWindow;
    [SerializeField] TimelineAsset ResultCloseWindow;
    [SerializeField] TimelineAsset ResultOpenWindow;
    [SerializeField] TimelineAsset CloseRankJoinWindow;
    [SerializeField] TimelineAsset OpenRankJoinWindow;

    private void Start()
    {
        Ranknames = JsonSystem.Instance.Information.rankNames;
        Rankscores = JsonSystem.Instance.Information.rankScores;
    }

    public void RankingUpdate()
    {
        SortRanking();

        Score.text = "SCORE : " + GameManager.Instance._Score.ToString();
        KillEnemy.text = "KILL ENEMY : " + GameManager.Instance.KillEnemy.ToString();

        for (int i = 0; i < Rankscores.Count && i < ScoreTexts.Count; i++)
        {
            NameTexts[i].text = "| " + Ranknames[i];
            ScoreTexts[i].text = "★ SCORE : " + Rankscores[i];
        }
    }

    public void Setdir()
    {
        ResultWindow.playableAsset = ResultOpenWindow;
    }

    public void RankingUpdate_Title()
    {
        SortRanking();

        for (int i = 0; i < Rankscores.Count && i < ScoreTexts.Count; i++)
        {
            NameTexts[i].text = "| " + Ranknames[i];
            ScoreTexts[i].text = "★ SCORE : " + Rankscores[i];
        }
    }

    //버블 정렬을 사용해서 랭킹을 정렬합니다.
    private void SortRanking()
    {
        for (int i = 0; i < Rankscores.Count - 1; i++)
        {
            for (int j = i + 1; j < Rankscores.Count; j++)
            {
                if (Rankscores[j] > Rankscores[i])
                {
                    int temp = Rankscores[j];
                    string stemp = Ranknames[j];

                    Rankscores[j] = Rankscores[i];
                    Ranknames[j] = Ranknames[i];

                    Rankscores[i] = temp;
                    Ranknames[i] = stemp;
                }
            }
        }
    }

    public void RankJoinWindowOpen()
    {
        RankJoinWindow.playableAsset = OpenRankJoinWindow;
        RankJoinWindow.Play();
    }

    public void GoTitle()
    {
        JsonSystem.Instance.Information.rankNames = Ranknames;
        JsonSystem.Instance.Information.rankScores = Rankscores;
        JsonSystem.Instance.Save();
        ResultWindow.playableAsset = ResultCloseWindow;
        ResultWindow.Play();
    }

    public void GoTitle_Title()
    {
        ResultWindow.playableAsset = ResultCloseWindow;
        ResultWindow.Play();
    }

    public void TitleSceneMove()
    {
        SceneManager.LoadScene("Title");
    }

    public void RankJoinConfirm()
    {
        Ranknames.Add(Join.text);
        Rankscores.Add(GameManager.Instance.Score);
        RankingUpdate();
        RankJoinWindow.playableAsset = CloseRankJoinWindow;
        RankJoinWindow.Play();
    }

    public void RankJoinCancel()
    {
        RankJoinWindow.playableAsset = CloseRankJoinWindow;
        RankJoinWindow.Play();
    }

}
