using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject levelCompleteUI;

    private bool gameOver = false;
    private bool gamePaused = false;
    private float timeRestart = 5;

    private void Update()
    {
        if (Input.GetButtonDown("Pause") == true)
        {
            gamePaused = !gamePaused;

            if (gamePaused == true)
            {
                GamePause();
            }
            else
            {
                GameResume();
            }
        }
    }

    public void GameOver()
    {
        if (gameOver == false)
        {
            gameOver = true;
            Debug.Log("Game Over!");
            Invoke("LevelRestart", timeRestart);
        }
    }

    private void GamePause()
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void GameResume()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LevelComplete()
    {
        levelCompleteUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
