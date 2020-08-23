using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public static LevelChanger _instance;

    Animator anim;
    int levelToLoad;
    [HideInInspector] public float soundLevel = 0;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //SoundsManager._instance.levelChangerVolume = soundLevel;
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        anim.SetTrigger("FadeOut");
    }

    public void FadeToLevel()
    {
        levelToLoad = SceneManager.GetActiveScene().buildIndex;
        anim.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel()
    {
        levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        anim.SetTrigger("FadeOut");
    }

    public void ResetLevel()
    {
        levelToLoad = SceneManager.GetActiveScene().buildIndex;
        anim.SetTrigger("FadeOut");
    }

    public void ExitGame()
    {
        levelToLoad = -1;
        anim.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        if (levelToLoad == -1)
            Application.Quit();
        else
            SceneManager.LoadScene(levelToLoad);
    }

}
