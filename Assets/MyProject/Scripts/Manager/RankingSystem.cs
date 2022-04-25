using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;

[System.Serializable]
public struct RankData
{
    public int Score;
    public string Name;
}

public class RankingSystem : MonoBehaviour
{
    public Text Score;
    public Text KillEnemy;

    public List<RankData> rankData = new List<RankData>();

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
        rankData = JsonSystem.Instance.Information.rankData;
    }

    public void RankingUpdate()
    {
        RankingUpdate_Title();

        Score.text = "SCORE : " + GameManager.Instance._Score.ToString();
        KillEnemy.text = "KILL ENEMY : " + GameManager.Instance.KillEnemy.ToString();
    }

    public void Setdir()
    {
        ResultWindow.playableAsset = ResultOpenWindow;
    }

    public void RankingUpdate_Title()
    {
        SortRanking();

        for (int i = 0; i < rankData.Count && i < ScoreTexts.Count; i++)
        {
            NameTexts[i].text = "| " + rankData[i].Name;
            ScoreTexts[i].text = "¡Ú SCORE : " + rankData[i].Score;
        }
    }

    private void SortRanking()
    {
        rankData.Sort((s1, s2) => s1.Score.CompareTo(s2.Score));
        rankData.Reverse();
    }

    public void RankJoinWindowOpen()
    {
        RankJoinWindow.playableAsset = OpenRankJoinWindow;
        RankJoinWindow.Play();
    }

    public void GoTitle()
    {
        JsonSystem.Instance.Information.rankData = rankData;
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
        RankData rank = new RankData();
        rank.Name = Join.text;
        rank.Score = GameManager.Instance.Score;
        rankData.Add(rank);
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
