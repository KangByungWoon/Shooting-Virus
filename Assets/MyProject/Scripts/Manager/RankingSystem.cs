using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class RankingSystem : MonoBehaviour
{
    public List<string> Ranknames = new List<string>();
    public List<int> Rankscores = new List<int>();

    [SerializeField] PlayableDirector ResultWindow;
    [SerializeField] PlayableDirector RankJoinWindow;
    [SerializeField] TimelineAsset ResultCloseWindow;
    [SerializeField] TimelineAsset CloseRankJoinWindow;
    [SerializeField] TimelineAsset OpenRankJoinWindow;

    public void RankingUpdate()
    {

    }

    private void SortRanking()
    {

    }

    public void RankJoinWindowOpen()
    {
        RankJoinWindow.playableAsset = OpenRankJoinWindow;
        RankJoinWindow.Play();
    }

    public void GoTitle()
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
