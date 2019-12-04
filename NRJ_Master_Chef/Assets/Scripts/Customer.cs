using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    #region public
    public GameObject[] vegPrefab;

    #endregion

    #region private
    const int waitTimeMultiplyer = 20;
    int waitTime;
    bool isAngry;
    float timeOfOrder = 0;

    List<GameObject> order;
    GameObject targetPlayer;
    Text waitTimeDispaly;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeOfOrder > 0)
        {
            float waitTimeRemaining = (waitTime - (float)Mathf.Round((Time.time - timeOfOrder) * 100f) / 100f);
            waitTimeDispaly.text = "" + waitTimeRemaining;
        }
    }

    public void ItemDroped(GameObject veg, GameObject player)
    {
        foreach (GameObject t in order)
        {
            if (t.name == veg.name)
            {
                order.Remove(t);
                if (order.Count == 0)
                {
                    targetPlayer = player;
                    OrderComplete();
                }
                return;             // if item placed on plate is in the order then everything is fine.
            }
        }
        isAngry = true;             // else the customer will be angry
        targetPlayer = player;      // with player
    }

    void OrderComplete()
    {
        isAngry = false;
        timeOfOrder = 0;
        waitTimeDispaly.text = "--";
        float waitTimeRemaining = (waitTime - (float)Mathf.Round((Time.time - timeOfOrder) * 100f) / 100f);
        if ((waitTimeRemaining / waitTime) >= 0.7f)
        {
            // Set power up for player
            targetPlayer.GetComponent<PlayerController>().SetPowerup();
        }
    }

    public void SetMyOrder(List<GameObject> orderObj)
    {
        order = orderObj;
        waitTime = orderObj.Count * waitTimeMultiplyer; // Set wait time for the customer
        timeOfOrder = Time.time;
    }
}
