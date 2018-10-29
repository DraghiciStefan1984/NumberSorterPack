using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberManager : MonoBehaviour
{
    public static NumberManager instance = null;

    [SerializeField] List<Button> numberButtonList;
    [SerializeField] AudioClip[] buttonSounds;
    [SerializeField] GameObject winParticles;
    AudioSource audioSource;

    Button selectedButton;
    TextMeshProUGUI infoText;

    int inactivatedButtonNumber;
    bool gameWon = false;

    void Awake()
    {
        GetInstance();
        ShuffleButtons();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        infoText = GameObject.Find("InstructionsText").GetComponent<TextMeshProUGUI>();
        WWWForm formData = GetPlayerFormData();
    }

    void Update()
    {
        if(!gameWon)
        {
            CheckVictoryCondition();
        }
    }

    void GetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ShuffleButtons()
    {
        List<Sprite> tempSpriteList = new List<Sprite>();
        numberButtonList.ForEach((button)=>tempSpriteList.Add(button.GetComponent<Image>().sprite));

        foreach(Button button in numberButtonList)
        {
            int randomSpriteIndex = Random.Range(0, tempSpriteList.Count);
            button.GetComponent<Image>().sprite = tempSpriteList[randomSpriteIndex];
            tempSpriteList.RemoveAt(randomSpriteIndex);
            button.onClick.AddListener(()=> ValidateButton(button));
            button.onClick.AddListener(() => PlayButtonSelectedSound(button));
        }
    }

    void ValidateButton(Button currentButton)
    {
        string currentButtonSpriteName = currentButton.GetComponent<Image>().sprite.name;
        if (!DeactivateValidButton(currentButton))
        {
            if (selectedButton != null)
            {
                SwapSprites(currentButton);
                selectedButton.transform.localScale = new Vector3(1f, 1f, 1f);

                DeactivateValidButton(currentButton);
                DeactivateValidButton(selectedButton);
                selectedButton = null;
            }
            else
            {
                selectedButton = currentButton;
                currentButton.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }
        
    }

    void SwapSprites(Button currentButton)
    {
        Sprite tempSprite = currentButton.GetComponent<Image>().sprite;
        currentButton.GetComponent<Image>().sprite = selectedButton.GetComponent<Image>().sprite;
        selectedButton.GetComponent<Image>().sprite = tempSprite;
    }

    bool DeactivateValidButton(Button button)
    {
        if (button.name == button.GetComponent<Image>().sprite.name)
        {
            button.interactable = false;
            inactivatedButtonNumber++;
            return true;
        }
        return false;
    }

    void CheckVictoryCondition()
    {
        if (inactivatedButtonNumber == numberButtonList.Count)
        {
            gameWon = true;
            StartWinSequence();
            StartCoroutine(PlayerService.instance.SavePlayerScore(GetPlayerFormData()));
            StartCoroutine(LoadHighScoreScene());
        }
    }

    void StartWinSequence()
    {
        ChangeUIForVictory();
        TimerManager.instance.StopTimer();
        PlayerPrefs.SetFloat("timeTaken", TimerManager.instance.GameTimer);
        //print(PlayerPrefs.GetFloat("timeTaken"));
        gameObject.GetComponent<AudioSource>().Play();
        GameObject instantiatedParticleObject = Instantiate(winParticles, infoText.transform.position, Quaternion.identity);
        instantiatedParticleObject.GetComponent<ParticleSystem>().Play();
    }

    private void ChangeUIForVictory()
    {
        infoText.text = "Congratulations, you won!";
        infoText.fontSize = 60;
        GameObject.Find("GoofyImage").GetComponent<Image>().enabled = false;
        GameObject.Find("BackButton").SetActive(false);
    }

    IEnumerator LoadHighScoreScene()
    {
        yield return new WaitForSeconds(2f);
        GameManager.instance.LoadHighScoreScene();
    }

    void PlayButtonSelectedSound(Button button)
    {
        if (button.interactable)
        {
            audioSource.PlayOneShot(buttonSounds[0]);
        }
        else if(!button.interactable)
        {
            audioSource.PlayOneShot(buttonSounds[1]);
        }
    }

    public WWWForm GetPlayerFormData()
    {
        WWWForm formData = new WWWForm();
        string currentPlayerName = PlayerPrefs.GetString("playerName");
        double scoreValue = (double)PlayerPrefs.GetFloat("timeTaken");
        formData.AddField("name", currentPlayerName);
        formData.AddField("value", scoreValue.ToString());
        return formData;
    }
}
