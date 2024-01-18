using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject player;
    public GameObject deadPanel;
    public GameObject tutorPanel;


    public void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
         if (context.started)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    public void OnPull()
    {
        if ( tutorPanel.activeSelf) 
        {
            tutorPanel.SetActive(false);
        }

    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void OnEnable()
    {
        CharacterEvent.characterDead += CharacterDead;

    }

    private void OnDisable()
    {

        CharacterEvent.characterDead -= CharacterDead;
    }

    public void CharacterDead(GameObject character)
    {
        if (character == player)
        {
            StartCoroutine(UpdateDead());
        }
    }
    IEnumerator UpdateDead()
    {
        deadPanel.SetActive(true);
        while (0.01 < Time.timeScale)
        {
            Time.timeScale -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        
    }
}