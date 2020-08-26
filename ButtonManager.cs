using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	public static bool paused;
    public static bool dead;

	public GameObject pauseMenuUI;
    public GameObject retryMenuUI;

    public GameObject resumeButton;
    public GameObject retryButton;
    public GameObject quitButton;

    void Start() {
        paused = false;
        dead = false;
        resumeButton.SetActive(false);
        retryButton.SetActive(false);
        quitButton.SetActive(false);
        pauseMenuUI.SetActive(false);
        retryMenuUI.SetActive(false);
    }

    void Update() {
        if (Player.getStatusCode() == 11) {
            retryMenuUI.SetActive(true);
            retryButton.SetActive(true);
            quitButton.SetActive(true);
            Time.timeScale = 0f;
            dead = true;
        }
    }

    public void PauseGame() {
        if (!dead) {
    	   pauseMenuUI.SetActive(true);
           resumeButton.SetActive(true);
           quitButton.SetActive(true);
    	   Time.timeScale = 0f;
    	   paused = true;
        }
    }

    public void ResumeGame() {
    	pauseMenuUI.SetActive(false);
        resumeButton.SetActive(false);
        quitButton.SetActive(false);
    	Time.timeScale = 1f;
    	paused = false;
    }

    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
    }

}
