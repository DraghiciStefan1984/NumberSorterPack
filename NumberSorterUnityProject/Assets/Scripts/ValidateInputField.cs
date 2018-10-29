using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidateInputField : MonoBehaviour
{
    Button playButton;
    InputField inputField;
    string playerName;

    void Awake()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }

    void Start ()
    {
        IntitializeInputFieldValue();
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckInputField();
	}

    void CheckInputField()
    {
        if (inputField.text.Length < 3)
        {
            TogglePlayButton(false, 0.5f);
        }

        else
        {
            playerName = inputField.text;
            PlayerPrefs.SetString("playerName", playerName);
            TogglePlayButton(true, 1f);
        }
    }

    void TogglePlayButton(bool isInteractable, float alpha)
    {
        var buttonColors = playButton.colors;
        var buttonNormalColor=buttonColors.normalColor;
        buttonNormalColor.a = alpha;
        playButton.interactable = isInteractable;
    }

    void IntitializeInputFieldValue()
    {
        playerName = PlayerPrefs.GetString("playerName");

        if (playerName != null)
        {
            inputField.text = playerName;
            TogglePlayButton(true, 1f);
        }
        else
        {
            inputField.text = "";
            TogglePlayButton(false, 0.5f);
        }
    }
}
