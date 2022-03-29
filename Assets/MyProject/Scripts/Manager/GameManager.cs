using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            _Score = value;
            ScoreTxt.text = value.ToString();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Score += 100;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 1.4f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        Debug.Log("게임오버");
    }

    public void GameClear()
    {

    }
}
