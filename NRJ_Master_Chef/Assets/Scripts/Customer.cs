using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    #region public
    public GameObject[] vegPrefab;

    #endregion

    #region private
    int waitTime;
    List<GameObject> order;
    bool isAngry;
    GameObject targetPlayer;
    const int waitTimeMultiplyer = 20;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        // Set power up for player
        targetPlayer.GetComponent<PlayerController>().SetPowerup();
    }

    public void SetMyOrder(List<GameObject> orderObj)
    {
        order = orderObj;
        waitTime = orderObj.Count * waitTimeMultiplyer; // Set wait time for the customer
    }
}
