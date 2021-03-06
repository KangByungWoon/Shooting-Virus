using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSystem : Singleton<JsonSystem>
{
    // Json을 사용하여 파일 입출력을 구현했습니다.
    public GameInformation Information;

    private void Awake()
    {
        Information = new GameInformation();

        Information = DataLoadText<GameInformation>("Info.text");
    }

    public void Save()
    {
        DataSaveText(Information, "Info.text");
    }

    // 경로를 지정해서 데이터를 저장합니다.
    public void DataSaveText<T>(T data, string fileName)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);

            if (json.Equals("{}"))
            {
                Debug.Log("json null");
                return;
            }
            string path = Application.dataPath + "/" + fileName;
            File.WriteAllText(path, json);

            Debug.Log(json);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("not found file" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("not found directorty" + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("not found IO" + e.Message);
        }
    }

    // 경로를 지정해서 데이터를 불러옵니다.
    public T DataLoadText<T>(string fileName)
    {
        try
        {
            string path = Application.dataPath + "/" + fileName;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log(json);
                T t = JsonUtility.FromJson<T>(json);
                return t;
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("file not found" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("file nt directory" + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("file not IO" + e.Message);
        }
        return default;
    }
}

[System.Serializable]
public class GameInformation
{
    public List<RankData> rankData = new List<RankData>();

    public int PlayerHp;
    public int PlayerDamage;
    public float PlayerBulletMoveSpeed;
    public float PlayerBulletAttackSpeed;
    public float PlayerMoveSpeed;

    public int Bacteria_Hp;
    public int Germ_Hp;
    public int Cancer_Cells_Hp;
    public int Virus_Hp;
    public int Stage1Boss_HP;
    public int Stage2Boss_HP;

    public int Bacteria_Damage;
    public int Germ_Damage;
    public int Cancer_Cells_Damage;
    public int Virus_Damage;
    public int Stage1Boss_Damage;
    public int Stage2Boss_Damage;

    public float Bacteria_BulletSpeed;
    public float Germ_BulletSpeed;
    public float Cancer_Cells_BulletSpeed;
    public float Virus_BulletSpeed;
    public float Stage1Boss_BulletSpeed;
    public float Stage2Boss_BulletSpeed;

    public float Bacteria_Speed;
    public float Germ_Speed;
    public float Cancer_Cells_Speed;
    public float Virus_Speed;
    public float Leukocyte_Speed;
    public float RedBlood_Cells_Speed;
    public float Stage1Boss_Speed;
    public float Stage2Boss_Speed;

    public float Leukocyte_SpawnTimer;
    public float RedBlood_Cells_SpawnTimer;
    public float Leukocyte_SpawnPer;
    public float RedBlood_Cells_SpawnPer;
}

