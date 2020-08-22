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
    #endregion

    #region Methods
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        initTime = Time.time;
        numEnemiesKilled = 0;
        numAlliesKilled = 0;
        isEnding = false;
    }

    private void Update()
    {
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
        float finalTime = Time.time - initTime;
        int timeScore = ((int) Mathf.Round(finalTime - averageTime)) * pointsPerExtraSecond;
        int enemiesScore = numEnemiesKilled * pointsPerEnemy;
        int alliesScore = numAlliesKilled * pointsPerAlly;
        int finalScore = timeScore + enemiesScore + alliesScore;
        finalScore = Mathf.Max(0, finalScore);


    }
    #endregion
}
