using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    void Awake()
    {
        GetInstance();
        gameObject.GetComponent<AudioSource>().Play();
    }
	
    void GetInstance()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadPlayerNameMenuScene()
    {
        SceneManager.LoadScene("PlayerNameMenu");
    }

    public void LoadHighScoreScene()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
