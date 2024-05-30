using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelEndMenu : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TMP_Text levelComplete;
    [SerializeField] TMP_Text timeElapsed;
    [SerializeField] TMP_Text timeScore;
    [SerializeField] TMP_Text killsText;
    [SerializeField] TMP_Text killsScore;
    [SerializeField] GameObject buttons;
    [SerializeField] PlayerDataScrObj playerData;
    string targetText;

    void Start()
    {
        levelComplete.gameObject.SetActive(false);
        timeElapsed.gameObject.SetActive(false);
        timeScore.gameObject.SetActive(false);
        killsText.gameObject.SetActive(false);
        killsScore.gameObject.SetActive(false);
        buttons.gameObject.SetActive(false);
        StartCoroutine("LoadMenu");
    }



    //wait for a split second
    //all the assets are invisible
    //set the score and time numbers
    //"LevelComplete" text rolls in
    //time text rolls in
    //time score pops in
    //kills text rolls in
    //kills score pops in
    //buttons pop in

    IEnumerator LoadMenu()
    {
        Debug.Log("coroutine start");
        yield return new WaitForSeconds(0.3f);
        //set time and score
        Debug.Log("seconds: " + playerData.levelTime);
        timeScore.text = "";
        if ((int)(playerData.levelTime / 60) < 10)
        {
            timeScore.text += "0";
        }
        timeScore.text += ((int)(playerData.levelTime / 60)).ToString() + ":";
        if ((int)playerData.levelTime - (int)(playerData.levelTime / 60) < 10)
        {
            timeScore.text += "0";

        }
        timeScore.text += (((int)playerData.levelTime - (int)(playerData.levelTime / 60))).ToString() + ":";
        timeScore.text += ((playerData.levelTime - (int)playerData.levelTime).ToString()).Substring(2, 2);

        killsScore.text = playerData.levelKills.ToString();
        targetText = "Level completed.";
        levelComplete.gameObject.SetActive(true);
        levelComplete.text = "";
        while (levelComplete.text.Length < targetText.Length)
        {
            levelComplete.text = targetText.Substring(0, levelComplete.text.Length + 1);
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.5f);
        targetText = "Time Elapsed:";
        timeElapsed.gameObject.SetActive(true);
        timeElapsed.text = "";
        while (timeElapsed.text.Length < targetText.Length)
        {
            timeElapsed.text = targetText.Substring(0, timeElapsed.text.Length + 1);
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.5f);
        timeScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        targetText = "Kills: ";
        killsText.gameObject.SetActive(true);
        killsText.text = "";
        while (killsText.text.Length < targetText.Length)
        {
            killsText.text = targetText.Substring(0,killsText.text.Length + 1);
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.5f);
        killsScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        buttons.gameObject.SetActive(true);


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
