using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageManager StageMgr;
    public GameObject Player;
    [SerializeField] Text HpTxt;
    [SerializeField] Text GpTxt;
    [SerializeField] Text ScoreTxt;
    [SerializeField] Image HpGague;
    [SerializeField] Image GpGague;
    [SerializeField] GameObject StageResult;
    [SerializeField] Text ScoreText;
    [SerializeField] Text KillEnemyText;

    public int KillEnemy;
    public int GetItem;

    public float _Hp;
    public float Hp
    {
        get { return _Hp; }
        set
        {
            if (value <= 0)
            {
                _Hp = 0;
                GameOver();
            }
            else if (value >= 100)
            {
                _Hp = 100;
            }
            else
            {
                _Hp = value;
            }
            HpTxt.text = _Hp.ToString() + "%";
            HpGague.fillAmount = _Hp / 100;
        }
    }

    public float _Gp;
    public float Gp
    {
        get { return _Gp; }
        set
        {
            if (value >= 100)
            {
                _Gp = 100;
                GameOver();
            }
            else
            {
                _Gp = value;
            }
            GpTxt.text = _Gp.ToString() + "%";
            GpGague.fillAmount = _Gp / 100;
        }
    }

    public int _Score;
    public int Score
    {
        get { return _Score; }
        set
        {
            if (ScoreTxt.gameObject.activeSelf == true)
            {
                _Score = value;
                ScoreTxt.text = value.ToString();
            }
        }
    }

    public bool ScorePlus = true;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (ScorePlus)
            Score += 100;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 1.4f;
            StartCoroutine(StageClear());
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        Debug.Log("���ӿ���");
    }

    public void GameClear()
    {

    }
    public IEnumerator StageClear()
    {
        StageResult.GetComponent<PlayableDirector>().Play();

        ScoreText.text = "SCORE : 0";
        KillEnemyText.text = "KILL ENEMY : 0";

        yield return new WaitForSeconds(0.2f);
        float Temp = Score * (1 + (Hp / 100) - (Gp / 100) + GetItem/100);
        Score = (int)Temp;
        Camera.main.GetComponent<CameraSystem>().CameraShake(5f, 0.35f);
        ScoreText.text = "SCORE : " + Score.ToString();
        KillEnemyText.text = "KILL ENEMY : " + KillEnemy.ToString();

        //StartCoroutine(TextScroll("Score : ", ScoreText, Score, Score + (int)(Score * Hp) + (int)(Score / Gp)));
    }

    private IEnumerator TextScroll(string insterText, Text text, float current, float target)
    {
        float textValue = current;
        while (true)
        {
            textValue = Mathf.MoveTowards(current, target, Time.deltaTime * 10);
            text.text = insterText + (textValue).ToString();
            if (textValue == target)
                break;
            yield return new WaitForSeconds(0.01f);
        }
    }
}