using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSaveData : MonoBehaviour
{
    public Text[] snoPlayerName;
    public Text[] playerScore; 
    

    public GameObject leaderboard;
    
    [HideInInspector]
    public int[] score = new int[10];
    [HideInInspector]
    public string[] playerName = new string[10];
    // Start is called before the first frame update

    private void OnEnable()
    {
        LoadPlayerData();
    }
    void Start()
    {
        
        LoadPlayerData();
        //gameObject.SetActive(false);
        Debug.Log(Application.persistentDataPath);
       // Invoke("test", 2);
    }
    public void test()
    {
        DisplayLeaderboard();
    }
    public void SavePlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/NRJ_gamesave.save", FileMode.Create);

        StorageData data = new StorageData();
        
        data.score = this.score;
        data.playerName = this.playerName;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("---------->>>  File saved");
    }

    public void LoadPlayerData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/NRJ_gamesave.save"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/NRJ_gamesave.save", FileMode.Open);

            StorageData data = new StorageData();
            data = (StorageData)bf.Deserialize(file);
            
            this.score = data.score;
            this.playerName = data.playerName;
            file.Close();
            //DisplayLeaderboard();

            Debug.Log("#############################################");
            for (int i = 0; i < this.score.Length; i++)
            {
                Debug.Log(i + ") " + this.playerName[i]);
            }

            Debug.Log("#############################################");
        }
        else
        {
            Debug.Log("----------No File-----------");
        }
    }

    public void EndResult(int playerScore, string id)
    {
        float playerTime = (float)Math.Round(Time.time * 100f) / 100f; 
        int index = 0;
        Debug.Log("______________________________________________________________");
        Debug.Log("Submit for "+id);
        while (index < this.score.Length)
        {
            Debug.Log(index+". "+ playerScore+" >= "+ this.score[index]);
            if (playerScore >= this.score[index])
            {
                //Debug.Log("playerTime = "+ playerTime);
                if (playerScore == this.score[index] )
                {
                    index++;
                    continue;
                }
                else
                {
                    Debug.Log("Current player is in at - "+index);
                    break;// if current players score is better than current entry at the index, then it should be replaced.
                }
            }
            index++;
        }
        if (index == this.score.Length)
        {
            Debug.Log("Current player did not enter top 10");
            DisplayLeaderboard();
            return;
        }


        int tempScore= playerScore, currentScore=0;
        float tempTime= playerTime, currentTime=0;
        string tempName=id, currentName="";
        while (index < this.score.Length)
        {
            currentScore = this.score[index];
            
            currentName = this.playerName[index];

            this.score[index] = tempScore;
            
            this.playerName[index] = tempName;

           
                tempScore = currentScore;
                tempTime = currentTime;
                tempName = currentName;
           
            index++;
        }
           
        Debug.Log("");
        Debug.Log("");
        Debug.Log("#############################################");
        for (int i = 0; i < this.score.Length; i++)
        {
            Debug.Log(i+") "+this.playerName[i]);
        }
        
        Debug.Log("#############################################");
        Debug.Log("");
        Debug.Log("");
        SavePlayerData();
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        Debug.Log("<<--  DisplayLeaderboard  -->>");
        leaderboard.SetActive(true);
        int index = 0;
        for (; index < 10; index++)
        {
            snoPlayerName[index].text = playerName[index];
            playerScore[index].text = ""+ score[index];
            
        }
    }
}
[System.Serializable]
public class StorageData
{

    
    public int[] score = new int[10];
    public string[] playerName = new string[10];
}
