using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    //--------------------------------------------------------------------------------

    /* Variables that tell wether the game is paused or the player is dead. */
	public static bool paused;
    public static bool dead;

    /* The head and body ragdolls will be instantiated when the player dies. */
    public RagdollHead head;
    public RagdollBody body;

    /* Various utility objects.
     * There are 2 different quit buttons because 'quit' is an option
     * in both the retry menu and in the pause menu. */

	public GameObject pauseMenuUI;
    public GameObject retryMenuUI;

    public GameObject resumeButton;
    public GameObject retryButton;
    public GameObject quitButton1;
    public GameObject quitButton2;
    
    public GameObject playerObj;

    //--------------------------------------------------------------------------------

    void Start() {
        playerObj = GameObject.FindGameObjectsWithTag("Player")[0].gameObject;
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
        if (Player.getStatusCode() == 11 && !dead) {
            Die();
        }
    }

    //--------------------------------------------------------------------------------

    /* Functionality for pause, resume, restart, and quit buttons.
     * 'RestartGame()' restarts just the current level, not the whole game. */

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

    //--------------------------------------------------------------------------------

    /* This tuns upon the death of the player. ShowRetryScreen() is an IEnumerator
     * because the program should stall for 2 seconds while the ragdoll flies around
     * before showing the retry screen. */

    public void Die() {
        RagdollHead thisHead = Instantiate(head);
        RagdollBody thisBody = Instantiate(body);
        thisHead.transform.position = Player.getLocation() + new Vector2(0, 1);
        thisBody.transform.position = Player.getLocation();
        playerObj.SendMessage("Invisible");
        dead = true;
        StartCoroutine(ShowRetryScreen());
    }

    IEnumerator ShowRetryScreen() {
        yield return new WaitForSeconds(2);
        retryMenuUI.SetActive(true);
        retryButton.SetActive(true);
        quitButton2.SetActive(true);
        Time.timeScale = 0f;
    }

    //--------------------------------------------------------------------------------


}
