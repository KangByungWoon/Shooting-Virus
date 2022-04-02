using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class TitleSystem : MonoBehaviour
{
    [SerializeField] PlayableDirector Start;
    [SerializeField] PlayableDirector Rank;

    public void GameStart()
    {
        Start.Play();
    }

    public void SceneMove()
    {
        SceneManager.LoadScene("Ingame");
    }

    public void RankingOpen()
    {
        Rank.Play();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
