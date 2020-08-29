using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public static bool paused;
    public static bool dead;

	public GameObject pauseMenuUI;
    public GameObject retryMenuUI;

    public GameObject resumeButton;
    public GameObject retryButton;
    public GameObject quitButton1;
    public GameObject quitButton2;

    void Start() {
        paused = false;
        dead = false;
        Time.timeScale = 1f;
        resumeButton.SetActive(false);
        retryButton.SetActive(false);
        quitButton1.SetActive(false);
        quitButton2.SetActive(false);
        pauseMenuUI.SetActive(false);
        retryMenuUI.SetActive(false);
    }

    void Update() {
        if (Player.getStatusCode() == 11) {
            retryMenuUI.SetActive(true);
            retryButton.SetActive(true);
            quitButton2.SetActive(true);
            Time.timeScale = 0f;
            dead = true;
        }
    }

    public void PauseGame() {
        if (!dead) {
    	   pauseMenuUI.SetActive(true);
           resumeButton.SetActive(true);
           quitButton1.SetActive(true);
    	   Time.timeScale = 0f;
    	   paused = true;
        }
    }

    public void ResumeGame() {
    	pauseMenuUI.SetActive(false);
        resumeButton.SetActive(false);
        quitButton1.SetActive(false);
    	Time.timeScale = 1f;
    	paused = false;
    }

    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void QuitGame() {
        SceneManager.LoadScene(0);
    }

}
