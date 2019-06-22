using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDefaultPlaceholder : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        //if can get variable and its = 1, alreacy launched, dont do shit
        if(!PlayerPrefs.HasKey("GameLauncedBefore"))
        {
            PlayerPrefs.SetInt("GameLauncedBefore", 1);
            PlayerPrefs.Save();
            //this code is only on first launch, should be atleast
            string firstNamePart = "DancingStar#";
            firstNamePart += Random.Range(1, 10000).ToString();
            Debug.Log("New name is: "+firstNamePart);
            //set that shit
            GetComponent<InputField>().text = firstNamePart;
            PlayerPrefs.SetString("ScoreName", firstNamePart);
            PlayerPrefs.Save();
            Debug.Log("first time randomizing name to: " + PlayerPrefs.GetString("ScoreName"));
        }
        else
        {
            Debug.Log("already played, setting name to: "+PlayerPrefs.GetString("ScoreName"));
            GetComponent<InputField>().text = PlayerPrefs.GetString("ScoreName");
        }
	}

    public void SetScoreName()
    {
        string name = GetComponent<InputField>().text;
        PlayerPrefs.SetString("ScoreName", name);
        Debug.Log("setting playerpref name to: " + PlayerPrefs.GetString("ScoreName"));
        PlayerPrefs.Save();
    }
}
