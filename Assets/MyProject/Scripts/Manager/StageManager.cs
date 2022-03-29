using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] float Stage1ProgressTime;
    [SerializeField] float Stage2ProgressTime;

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

    private int Stage = 1;
    private IEnumerator Spawn;

    private void Start()
    {
        StartCoroutine(StageProgress(Stage1ProgressTime, 1));
    }

    private IEnumerator StageProgress(float progressTime, int Stage)
    {
        Spawn = SpawnCoroutine();
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
                StageProgressUI.SetActive(false);
                BossProgressUI.SetActive(true);
                StopCoroutine(Spawn);
                Instantiate(Stage == 1 ? Stage1Boss : Stage2Boss);
                break;
            }
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            int randomspawn = Random.Range(0, Stage * 2);
            for (int i = 0; i < randomspawn; i++)
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

    public void ClearCallBack()
    {
        //¿¬Ãâ
        StartCoroutine(StageProgress(Stage2ProgressTime, 2));
    }
}
