using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //[SerializeField] int lives = 3;
    [SerializeField] Transform[] spawnpoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int score = 0;
    bool gameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start() {
        InstantiateGame();
    }

    void InstantiateGame(){
        gameOver = false;
        score = 0;
    }

    public void SpawnEnemy(){

    }



    public void addScore(int points){
        score += points;
        score = points;
    }

    public void PlayerDie(){
        gameOver = true;
    }
}
