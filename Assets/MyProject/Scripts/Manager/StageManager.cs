using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class StageManager : MonoBehaviour
{
    [SerializeField] float Stage1ProgressTime;
    [SerializeField] float Stage2ProgressTime;

    [SerializeField] GameObject Stage1BossEffect;
    [SerializeField] GameObject Stage2BossEffect;

    [SerializeField] GameObject Stage1Boss;
    [SerializeField] GameObject Stage2Boss;

    [SerializeField] Image PlayerIcon;
    [SerializeField] Image BossIcon;
    [SerializeField] Image FillStage;
    [SerializeField] Text StageText;

    [SerializeField] GameObject StageProgressUI;
    [SerializeField] GameObject BossProgressUI;

    [SerializeField] Sprite CovidBoss;
    [SerializeField] Sprite CovidChangeBoss;

    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject LockOn;
    [SerializeField] GameObject CMCam;

    [SerializeField] GameObject FireWorks;
    [SerializeField] GameObject DustStorm;

    [SerializeField] GameObject OverWindow;

    private int Stage = 1;
    private IEnumerator Spawn;

    private void Start()
    {
        StartCoroutine(StageProgress(Stage1ProgressTime, 1));
        //Stage1BossEffect.GetComponent<PlayableDirector>().Play();
    }

    private IEnumerator StageProgress(float progressTime, int Stage)
    {
        Spawn = SpawnCoroutine();
        StageProgressUI.SetActive(true);
        BossProgressUI.SetActive(false);

        StartCoroutine(Spawn);
        StageText.text = "Stage" + Stage.ToString();
        BossIcon.sprite = Stage == 1 ? CovidBoss : CovidChangeBoss;
        BossIcon.SetNativeSize();
        float now_progressTime = 0;

        while (true)
        {
            now_progressTime += 0.01f;
            FillStage.fillAmount = now_progressTime / progressTime;
            PlayerIcon.rectTransform.anchoredPosition = new Vector3(FillStage.fillAmount * FillStage.rectTransform.rect.width, 0, 0);
            yield return new WaitForSeconds(0.01f);

            if (now_progressTime >= progressTime)
            {
                StopCoroutine(Spawn);
                if (Stage == 1)
                    Stage1BossEffect.GetComponent<PlayableDirector>().Play();
                else
                    Stage2BossEffect.GetComponent<PlayableDirector>().Play();
                break;
            }
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            int randomspawn = Random.Range(0, Stage * 2);
            for (int i = 0; i <= randomspawn; i++)
            {
                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Bacterias, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                    case 1:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Germs, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                    case 2:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Viruses, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                    case 3:
                        ObjectPool.Instance.GetObject(ObjectPool.Instance.Cancer_Cellses, new Vector3(Random.Range(-30, 30), Random.Range(10, 30), -10));
                        break;
                }

            }
            yield return new WaitForSeconds(Random.Range(0.4f / Stage, 2f / Stage));
        }
    }

    public void BossArrive()
    {
        var enemys = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
        {
            enemy.Die();
        }

        GameUI.SetActive(false);
        LockOn.SetActive(false);
        StageProgressUI.SetActive(false);
        BossProgressUI.SetActive(true);

        if (Stage == 1)
        {
            Stage1Boss.SetActive(true);
            Stage1BossEffect.SetActive(false);
            Stage1Boss.GetComponent<PlayableDirector>().Play();
            CMCam.GetComponent<CinemachineVirtualCamera>().LookAt = Stage1Boss.gameObject.transform;
        }
        else
        {
            Stage2Boss.SetActive(true);
            Stage2BossEffect.SetActive(false);
            Stage2Boss.GetComponent<PlayableDirector>().Play();
            CMCam.GetComponent<CinemachineVirtualCamera>().LookAt = Stage2Boss.gameObject.transform;
        }

        CMCam.SetActive(true);
    }

    public void BossStart()
    {
        GameUI.SetActive(true);
        LockOn.SetActive(true);
        CMCam.SetActive(false);

    }

    public IEnumerator ClearCallBack()
    {
        var enemys = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
        {
            enemy.Die();
        }

        GameManager.Instance.Player.GetComponent<AirPlaneController>().NOSHOTTING = true;
        GameManager.Instance.ScorePlus = false;

        FireWorks.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(4f);
        FireWorks.GetComponent<ParticleSystem>().Stop();

        DustStorm.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2f);

        if (Stage == 1)
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.StageClear());
            yield return new WaitForSeconds(5f);

            DustStorm.GetComponent<ParticleSystem>().Stop();
            yield return new WaitForSeconds(2f);

            StartCoroutine(StageProgress(Stage2ProgressTime, 2));
            GameManager.Instance.Player.GetComponent<AirPlaneController>().NOSHOTTING = false;
            GameManager.Instance.ScorePlus = true;
            GameManager.Instance.Hp = 100;
            GameManager.Instance.Gp = 30;
            Stage++;
        }
        else
        {
            OverWindow.GetComponent<PlayableDirector>().Play();
        }
    }

}
