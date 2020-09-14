using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager _instance;

    [Header("Game manager variables")]
    [HideInInspector] public bool isEnding;
    [SerializeField] private int pointsPerEnemy;
    [SerializeField] private int pointsPerAlly;
    [SerializeField] private int pointsPerExtraSecond;
    [SerializeField] private float averageTime;
    private int numEnemiesKilled;
    private int numAlliesKilled;
    private float initTime;

    [Space]

    [Header("UI references")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI alliesKilledText;

    [Space]

    [SerializeField] private GameObject endScreen;

    [SerializeField] private GameObject inGameMenu;

    [SerializeField] private TextMeshProUGUI resultTitleText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private GameObject results;
    [SerializeField] private TextMeshProUGUI enemiesKilledTextEnd;
    [SerializeField] private TextMeshProUGUI alliesKilledTextEnd;
    [SerializeField] private TextMeshProUGUI timeTextEnd;
    [SerializeField] private TextMeshProUGUI scoreText;
    #endregion

    #region Methods
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        inGameMenu.SetActive(true);
        endScreen.SetActive(false);
        initTime = Time.time;
        numEnemiesKilled = 0;
        numAlliesKilled = 0;
        isEnding = false;
    }

    private void Update()
    {
        if (!isEnding)
            timeText.text = ((int) Mathf.Round(Time.time - initTime)).ToString();
    }

    public void IncreaseEnemiesKilled()
    {
        numEnemiesKilled++;
        enemiesKilledText.text = numEnemiesKilled.ToString();
    }

    public void IncreaseAlliesKilled()
    {
        numAlliesKilled++;
        alliesKilledText.text = numAlliesKilled.ToString();
    }

    public void EndGame(bool died)
    {
        if (isEnding)
            return;

        isEnding = true;
        endScreen.SetActive(true);
        inGameMenu.SetActive(false);

        if (died)
        {
            results.SetActive(false);
            AudioManager.instance.PlaySound("loseGame");
            resultTitleText.text = "You lost!";
            resultText.text = "The Bat Mafia has killed you and got the demon bat back.";
        }
        else
        {
            int finalTime = (int) Mathf.Round((Time.time - initTime) - averageTime);
            int timeScore = finalTime * pointsPerExtraSecond;
            int enemiesScore = numEnemiesKilled * pointsPerEnemy;
            int alliesScore = numAlliesKilled * pointsPerAlly;
            int finalScore = timeScore + enemiesScore + alliesScore;
            finalScore = Mathf.Max(0, finalScore);

            results.SetActive(true);
            AudioManager.instance.PlaySound("winGame");
            resultTitleText.text = "You won!";
            resultText.text = "You defeated the Bat Mafia and got rid of the curse.";
            enemiesKilledTextEnd.text = numEnemiesKilled.ToString();
            alliesKilledTextEnd.text = numAlliesKilled.ToString();
            timeTextEnd.text = ((int)Mathf.Round(Time.time - initTime)).ToString();
            scoreText.text = finalScore.ToString();
        }
    }
    #endregion
}
