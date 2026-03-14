using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public GameObject startPanel;
    public GameObject deathPanel;
    public GameObject playerPanel;
    public Boolean showing;
    public GameObject continuePanel;
    public GameObject winPanel;
    public GameObject hitMarker;

    public TMP_Text scoreText;
    public TMP_Text winScoreText;

    void ShowPanel(GameObject panel) {
        winPanel.SetActive(false);
        startPanel.SetActive(false);
        deathPanel.SetActive(false);
        playerPanel.SetActive(false);
        continuePanel.SetActive(false);

        showing = false;
        if (panel != null)
        {
           showing = true;
            panel.SetActive(true);
            if (panel == playerPanel)
               showing = false;
        }
    }

    public void setScore(int n)
    {
        winScoreText.text = "SCORE: " + n.ToString();
        scoreText.text = "SCORE: " + n.ToString();
    }

    public void continueGame()
    {
        AudioManager.Instance.PlayMusic("gameSong");
        ShowPanel(playerPanel);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void startGame()
    {
        ShowPanel(startPanel);
        Time.timeScale = 0f;
        
    }

    public void deathReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void pauseGame()
    {
        if (!showing)
            ShowPanel(continuePanel);
        AudioManager.Instance.PlayMusic("menuSong");
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void death()
    {
        ShowPanel(deathPanel);
        AudioManager.Instance.PlayMusic("menuSong");
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startGame();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            pauseGame();
        }
    }

    public void win()
    {
        ShowPanel(winPanel);
        AudioManager.Instance.PlayMusic("menuSong");
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void showHitMarker()
    {
        hitMarker.SetActive(true);
        Invoke(nameof(HideHitMarker), 0.1f);
    }

    void HideHitMarker()
    {
        hitMarker.SetActive(false);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
