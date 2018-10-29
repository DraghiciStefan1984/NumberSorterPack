using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerService : MonoBehaviour
{
    public static PlayerService instance = null;
    string playerName = "";
    string timeTaken = "";
    string postUrl = "https://development.m75.ro/test_mts/public/highscore/";

    void Awake()
    {
        GetInstance();
    }

    void Start()
    {
        GetSavedData();
    }

    void GetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void GetSavedData()
    {
        if (PlayerPrefs.GetString("playerName") != null)
        {
            playerName = PlayerPrefs.GetString("playerName");
        }
        
        if (PlayerPrefs.GetString("timeTaken") != null)
        {
            timeTaken = PlayerPrefs.GetString("timeTaken");
        }
    }

    public IEnumerator SavePlayerScore(WWWForm formData)
    {
        using (UnityWebRequest webAddress = UnityWebRequest.Post(postUrl, formData))
        {
            yield return webAddress.SendWebRequest();
 
            if (webAddress.isNetworkError || webAddress.isHttpError)
            {
                print("An error occured!");
                Debug.Log(webAddress.error);
            }
            else if (webAddress.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(webAddress.downloadHandler.data);
                print(jsonResult);
            }
        }
    }

}
