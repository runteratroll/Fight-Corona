using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab = null;
    public GameObject AkumaPre = null;
    public GameObject OctoberPre = null;
    public Transform[] spawnPoints;
    public ObjectManager poolManager;
    [SerializeField]
    private GameObject trapPrefab = null;
    private float maxSpawnDelay;
    private float curSpawnDelay;
    public int ranPoint;
    public string[] enemyobjs;
    private float hardTime = 0f;
    enum Difficulty {Easy, Nomal, Hard, SuperHard}; //0, 1, 2,3
    Difficulty difficulty = Difficulty.Easy;
    private int life = 3;
    public long highScore = 0;
    private long score = 0;
    private bool isEasy = false;
    private bool isNomal =false;
    private bool isHard = false;
    private bool isSuperHard =false;
    private float enemySpawnTime = 5f;
    private float akumaSpawnTime = 10f;
    private float octoberSpawnTime = 7f;
    [SerializeField]
    Text textLife;
    [SerializeField]
    Text textScore;
    [SerializeField]
    Text textHighScore = null;
    [SerializeField]
    Text[] textDifficultyValue;
    [SerializeField]
    Text textDifficulty;


    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPosition { get; private set; }
    void Start()
    {
        enemyobjs = new string[] { "enemyL", "enemyAkuma", "enemyOctober" };
        highScore = PlayerPrefs.GetInt("HIGHSCORE");
        poolManager = FindObjectOfType<ObjectManager>();
        //textHighScore.text = string.Format("HIGHSCORE\n{0}", ); //PlayerPrefs.GetInt("HIGHSCORE")
        MinPosition = new Vector2(-8.7f, -5f);
        MaxPosition = new Vector2(8.7f, 5f);
        StartCoroutine(EnemySpawn());
        StartCoroutine(EnemySpawnAkumas());
        StartCoroutine(EnemySpawnOctober());
        UpdateUI();

    }

    private void DifficulyCheck()
    {
        if (hardTime > 15f && difficulty == Difficulty.Easy)
        {
            difficulty = Difficulty.Nomal; //노말 바뀜
        }

        if (hardTime > 60f && difficulty == Difficulty.Nomal)
        {
            difficulty = Difficulty.Hard; //하드로 바뀜
        }
        if (hardTime > 90f && difficulty == Difficulty.Hard)
        {
            difficulty = Difficulty.SuperHard; //초하드로 바뀜

        }
            
        
    }

    public void AddScore(long addScore) // 함수로 만드는 이유를 알아보자
    {
        
        score += addScore;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HIGHSCORE", (int)highScore);
        }
        UpdateUI();
    }
    void Update()
    {
        hardTime += Time.deltaTime;
        //Debug.Log("hardTime : "+ hardTime);
        curSpawnDelay += Time.deltaTime;
        if(curSpawnDelay > maxSpawnDelay)
        {
            trapPrefab.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            SpawnTrap();
            maxSpawnDelay = Random.Range(30f, 40f);
            curSpawnDelay = 0;
        }
        DifficulyCheck();



    }

    void SpawnTrap()
    {
        int ranPoint = Random.Range(0, 2);

        if(ranPoint == 0)
        {
            Instantiate(trapPrefab, spawnPoints[ranPoint]);
        }
        else if(ranPoint == 1)
        {
            trapPrefab.transform.rotation = Quaternion.Euler(0f, 0f, -180f);
            Instantiate(trapPrefab, spawnPoints[ranPoint]);
        }
    }
    IEnumerator EnemySpawn()
    {
        float randomY;
        //기존의 instantiate()를 오브젝트 풀링으로 교체
        
        while (true)
        {

            randomY = Random.Range(-3.8f, 4.5f);
            Instantiate(enemyPrefab, new Vector2(14f, randomY), Quaternion.identity);
            if (difficulty == Difficulty.Nomal && !isNomal) //조건 추가하셈
            {
                enemySpawnTime -= 1f;
                isNomal = true;
            }
            if (difficulty == Difficulty.Hard && !isHard)
            {
                enemySpawnTime -= 1f;
                isHard = true;
            }

            if (difficulty == Difficulty.SuperHard && !isSuperHard)
            {
                enemySpawnTime -= 1f;
                isSuperHard = true;
            }

            yield return new WaitForSeconds(enemySpawnTime);
            Debug.Log("enemySpawnTime : " + enemySpawnTime);
        } 
    }

    IEnumerator EnemySpawnAkumas()
    {
        float randomY;

        while (true)
        {
            randomY = Random.Range(-3.8f, 4.5f);
            Instantiate(AkumaPre, new Vector2(14f, randomY), Quaternion.identity);
            if (difficulty == Difficulty.Nomal && !isNomal) //조건 추가하셈
            {
                akumaSpawnTime -= 2f;
                isNomal = true;
            }
            if (difficulty == Difficulty.Hard && !isHard)
            {
                akumaSpawnTime -= 2f;
                isHard = true;
            }

            if (difficulty == Difficulty.SuperHard && !isSuperHard)
            {
                akumaSpawnTime -= 1f;
                isSuperHard = true;
            }
            yield return new WaitForSeconds(akumaSpawnTime);

        }
    }

    IEnumerator EnemySpawnOctober()
    {
        float randomY;
        while (true)
        {
            randomY = Random.Range(-3.8f, 4.5f);
            Instantiate(OctoberPre, new Vector2(14f, randomY), Quaternion.identity);
            //enemyOctober = objectManager.MakeObj("enemyOctober");
            //enemyOctober.transform.position = new Vector2(14f, randomY);
            if (difficulty == Difficulty.Nomal && !isNomal) //조건 추가하셈
            {
                octoberSpawnTime -= 2f;
                isNomal = true;
            }
            if (difficulty == Difficulty.Hard && !isHard)
            {
                octoberSpawnTime -= 2f;
                isHard = true;
            }

            if (difficulty == Difficulty.SuperHard && !isSuperHard)
            {
                octoberSpawnTime -= 2f;
                isSuperHard = true;
            }
            yield return new WaitForSeconds(octoberSpawnTime);

        }
    }

    void UpdateUI()
    {
        textLife.text = string.Format("LIFE : {0}", life);
        textScore.text = string.Format("SCORE : {0}", score);
        textHighScore.text = string.Format("HIGH SCORE : {0}", highScore);

        if(difficulty == Difficulty.Easy)
            textDifficulty.text = textDifficultyValue[0].text;
        if (difficulty == Difficulty.Nomal)
            textDifficulty.text = textDifficultyValue[1].text;
        if (difficulty == Difficulty.Hard)
            textDifficulty.text = textDifficultyValue[2].text;
        if (difficulty == Difficulty.SuperHard)
            textDifficulty.text = textDifficultyValue[3].text;


    }

    public void Dead()
    {
        life--;
        UpdateUI();
        if (life <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
