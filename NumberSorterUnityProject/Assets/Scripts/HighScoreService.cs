using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class HighScoreService : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScoreList;
    string getUrl = "https://development.m75.ro/test_mts/public/highscore/5";

    void Start()
    {
        highScoreList.text = "";
        StartCoroutine(GetScores(getUrl, GetPlayers));
    }

    IEnumerator GetScores(string url, System.Action<PlayerList> callback)
    {
        using (UnityWebRequest webAddress = UnityWebRequest.Get(url))
        {
            yield return webAddress.SendWebRequest();

            if (webAddress.isNetworkError)
            {
                Debug.Log(webAddress.error);
            }
            else if (webAddress.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(webAddress.downloadHandler.data);
                PlayerList playerList = JsonUtility.FromJson<PlayerList>(jsonResult);
                callback(playerList);
            }
        }
    }

    public void GetPlayers(PlayerList playerList)
    {
        if (playerList.result == null)
        {
            print("No player found.");
        }
        else
        {
            foreach (Player player in playerList.result)
            {
                highScoreList.text += player.name + ": " + player.value + "\n";
            }
        }
    }

}
