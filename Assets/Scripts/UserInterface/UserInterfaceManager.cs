using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using System.Collections;

public class UserInterfaceManager : MonoBehaviour
{

    private Startup startup;

    public GameManager gameManager;

    public GameObject addScoreObject;

    //UI
    public GameObject gameUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreMultiplierText;
    public GameObject pauseUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI yourScoreTextGO;
    public TextMeshProUGUI brokenBlocksTextGO;
    private void Awake()
    {
        startup = GameObject.Find("Startup").GetComponent<Startup>();
    }

    private void Update()
    {
        scoreText.text = gameManager.score.ToString();
        scoreMultiplierText.text = $"{gameManager.scoreMultiplier.ToString()}x";
    }

    public void GameInterfaceEnable()
    {
        gameUI.SetActive(true);
    }

    public void GameInterfaceDisable()
    {
        gameUI.SetActive(false);
    }

    public void GameOverInterfaceEnable(int score, int brokenBlocks)
    {
        yourScoreTextGO.text = score.ToString();
        brokenBlocksTextGO.text = brokenBlocks.ToString();

        gameOverUI.SetActive(true);
    }

    public void GameOverInterfaceDisable()
    {
        gameOverUI.SetActive(false);
    }

    public void spawnAddScoreGameObject(Vector3 pos, int points)
    {
        GameObject newAddScore = Instantiate(addScoreObject, Camera.main.WorldToScreenPoint(pos), Quaternion.identity);
        newAddScore.transform.SetParent(GameObject.Find("GameInterfaceAndroid").transform);
        newAddScore.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newAddScore.GetComponent<TextMeshProUGUI>().text = $"+{points}";
    }

    public void spawnAddNeonBlockGameObject(Vector3 pos, int nb)
    {
        GameObject newAddNeonBlocks = Instantiate(addScoreObject, Camera.main.WorldToScreenPoint(pos), Quaternion.identity);
        newAddNeonBlocks.GetComponent<TextMeshProUGUI>().color = new Color(0.0f, 148.0f, 255.0f);
        newAddNeonBlocks.transform.SetParent(GameObject.Find("GameInterfaceAndroid").transform);
        newAddNeonBlocks.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        newAddNeonBlocks.GetComponent<TextMeshProUGUI>().text = $"+{nb}";
    }

    //UI ACTIONS
    public void NewGame()
    {
        StartCoroutine(NewGameHandler());
    }

    private IEnumerator NewGameHandler()
    {
        yield return null;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuHandler());
    }

    private IEnumerator MainMenuHandler()
    {
        yield return null;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        StartCoroutine(PauseGameHandler());
    }

    private IEnumerator PauseGameHandler()
    {
        pauseUI.SetActive(true);
        GameInterfaceDisable();
        Time.timeScale = 0.0f;
        GameObject.Find("PauseBackground/YourScoreBackground/YourScoreValue").GetComponent<TextMeshProUGUI>().text = gameManager.score.ToString();
        GameObject.Find("PauseBackground/BrokenBlocksBackground/BrokenBlocksValue").GetComponent<TextMeshProUGUI>().text = gameManager.brokenBlocks.ToString();
        yield return null;
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameHandler());
    }

    private IEnumerator ResumeGameHandler()
    {
        pauseUI.SetActive(false);
        GameInterfaceEnable();
        Time.timeScale = 1.0f;
        yield return null;
    }
}