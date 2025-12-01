using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    private int score;

private void Awake()
{
    Application.targetFrameRate = 60;

    gameOver.SetActive(false);
    playButton.SetActive(true);

    Pause();
}
public void Play()
{
    score = 0;
    scoreText.text = score.ToString();

    playButton.SetActive(false);
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
        gameOver.SetActive(true);
        playButton.SetActive(true);

        Pause();
    }

    public void ExitGame()
    {
        Application.Quit();
    }



    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
