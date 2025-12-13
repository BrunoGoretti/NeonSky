using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject playButton;
    public GameObject mainMenuButton;

    public GameObject gameOver;
    private bool isGameOver = false;
    public bool IsGameOver => isGameOver;

private void Awake()
{
    Application.targetFrameRate = 60;

    gameOver.SetActive(false);
    playButton.SetActive(false);
    mainMenuButton.SetActive(false);

    Time.timeScale = 1f;  
}
    public void Play()
    {
                isGameOver = false;

        FindObjectOfType<GameOverScreen>().FadeToClear();

        playButton.SetActive(false);
        mainMenuButton.SetActive(false);
        gameOver.SetActive(false);

        player.gameObject.SetActive(true);
        player.enabled = true;

        Time.timeScale = 1f;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
            Destroy(pipes[i].gameObject);

        player.transform.position = new Vector3(0, 0, 0);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void GameOver()
    {
         isGameOver = true;
        FindObjectOfType<GameOverScreen>().FadeToBlack();

        gameOver.SetActive(true);
        playButton.SetActive(true);
        mainMenuButton.SetActive(true);

        Pause();
    }

    public void MainMenu()
    {
         Time.timeScale = 1f;
         SceneManager.LoadScene("MainMenu");
    }
}
