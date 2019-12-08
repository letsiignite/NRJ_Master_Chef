using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreTrack : MonoBehaviour {

    #region public
    public int TotalGameTime;
     
    

    public Text p1_Score_title;
    public Text p2_Score_title;
    public Text p1_Score;
    public Text p2_Score;
    public Text CountDownTimer;
    public Text player_1_Name;
    public Text player_2_Name;
    public Text winnerText;
    public InputField player_1_Name_Input;
    public InputField player_2_Name_Input;
    public PlayerSaveData playerSaveDataObj;
    #endregion

    #region private
    const int timeCountDownRate = 1;
    int count;
    int p1Score;
    int p2Score;
    #endregion
    // Use this for initialization
    void Start () {
        StartGame();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        p1_Score_title.text = player_1_Name.text = player_1_Name_Input.text;
        p2_Score_title.text = player_2_Name.text = player_2_Name_Input.text;
        
        Debug.Log("player_1_Name.text = " + player_1_Name.text);
        StartCoroutine(TimeCounter());
    }

    IEnumerator TimeCounter()
    {
        count++;
        yield return new WaitForSeconds(timeCountDownRate);
        CountDownTimer.text = "" + (TotalGameTime - count);
        if ((TotalGameTime - count) > 0)
        {
            StartCoroutine(TimeCounter());
        }
        else
        {
            GameOver();
            GameObject.Find("Manager").GetComponent<PlayerSaveData>().EndResult(p1Score,p1_Score_title.text);
            GameObject.Find("Manager").GetComponent<PlayerSaveData>().EndResult(p2Score,p2_Score_title.text );
        }


    }

    public void PlayerOneScore(int scoreToAdd)
    {
        p1Score += scoreToAdd;
        p1_Score.text = "" + p1Score;
    }
    public void PlayerTwoScore(int scoreToAdd)
    {
        p2Score += scoreToAdd;
        p2_Score.text = "" + p2Score;
    }

    public void CustomerLeft(int scoreToAdd)
    {
        p1Score -= scoreToAdd;
        p2Score -= scoreToAdd;
        p1_Score.text = "" + p1Score;
        p2_Score.text = "" + p2Score;
    }
    public void GameOver()
    {
        playerSaveDataObj.EndResult(p1Score, player_1_Name.text);
        playerSaveDataObj.EndResult(p2Score, player_2_Name.text);
        p2Score = 0;
        p1Score = 0;
        p1_Score.text = "";
        p2_Score.text = "";
        winnerText.text = (p1Score > p2Score) ? player_1_Name.text : player_2_Name.text;
        winnerText.text = (p1Score == p2Score) ? "Both Win" : winnerText.text;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
