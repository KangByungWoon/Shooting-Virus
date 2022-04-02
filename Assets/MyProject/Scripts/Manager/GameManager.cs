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
    [SerializeField] Text GetItemTxt;

    [SerializeField] GameObject Smoke;
    [SerializeField] GameObject Pack;

    public int KillEnemy;
    public int GetItem;

    bool isEnd = false;

    public float _Hp;
    public float Hp
    {
        get { return _Hp; }
        set
        {
            if (value <= 0)
            {
                _Hp = 0;
                if (!isEnd)
                {
                    StartCoroutine(GameOver());
                    isEnd = true;
                }
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
                if (!isEnd)
                {
                    StartCoroutine(GameOver());
                    isEnd = true;
                }
            }
            else if (value <= 0)
            {
                _Gp = 0;
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
    [SerializeField] GameObject OverWindow;
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (ScorePlus)
            Score += 123;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Time.timeScale = 1.4f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Time.timeScale = 1;
        }
    }

    private void Start()
    {
        _Hp = JsonSystem.Instance.Information.PlayerHp;
    }

    public void GetItemTxtOutput(string typeName, bool isSpecial = false)
    {
        GetItemTxt.text = "ITEM GET!\n" + typeName;
        if (isSpecial)
            GetItemTxt.text = "LEVEL UP!";
        GetItemTxt.GetComponent<PlayableDirector>().Play();
    }

    public IEnumerator StageClear()
    {
        StageResult.GetComponent<PlayableDirector>().Play();

        ScoreText.text = "SCORE : 0";
        KillEnemyText.text = "KILL ENEMY : 0";

        yield return new WaitForSeconds(0.2f);
        float Temp = Score * (1 + (Hp / 100) - (Gp / 100) + GetItem / 100);
        Score = (int)Temp;
        Camera.main.GetComponent<CameraSystem>().CameraShake(5f, 0.35f);
        ScoreText.text = "SCORE : " + Score.ToString();
        KillEnemyText.text = "KILL ENEMY : " + KillEnemy.ToString();
    }

    public IEnumerator GameOver()
    {
        Player.GetComponent<AirPlaneController>().MoveSpeed = 0;
        Pack.SetActive(false);
        GameObject smoke = Instantiate(Smoke); 
        smoke.transform.position = Player.GetComponent<AirPlaneController>().transform.position + new Vector3(0,0,-0.5f);

        Player.GetComponent<AirPlaneController>().NOSHOTTING = true;
        ScorePlus = false;

        StageMgr.StopAllCoroutines();
        var enemys = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemys)
        {
            enemy.EixtEnemy();
        }

        ScrollMap map = FindObjectOfType<ScrollMap>();
        float tempSpeed = map.Map_MoveSpeed;
        for (int i = 0; i < 100; i++)
        {
            map.Map_MoveSpeed -= tempSpeed * 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3f);
        OverWindow.GetComponent<PlayableDirector>().Play();
    }

    public IEnumerator GameClear()
    {
        OverWindow.GetComponent<PlayableDirector>().Play();
        Camera.main.GetComponent<CameraSystem>().CameraShake(100f, 0.35f);
        yield return null;
    }
}