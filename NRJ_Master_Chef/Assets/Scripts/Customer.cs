using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Once the customer is created, this takes over
/// This class takes care of wait time countdown, items served, state of the customer.
/// </summary>
public class Customer : MonoBehaviour
{
    #region public
    public GameObject[] vegPrefab;
    public Text timerText;
    #endregion

    #region private
    const int waitTimeMultiplyer = 30;
    float timeCountDownRate =1;
    int waitTime;
    int count;
    int sno;
    bool isAngry;
    float timeOfOrder = 0;

    List<GameObject> order;
    GameObject[] orderPlaceHolders;
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
        if (timeOfOrder > 0 && waitTimeDispaly != null)
        {
            Debug.Log("Wait time = "+ waitTimeDispaly.text);
            float waitTimeRemaining = (waitTime - (float)Mathf.Round((Time.time - timeOfOrder) * 100f) / 100f);
            waitTimeDispaly.text = "" + waitTimeRemaining;
        }
    }
    /// <summary>
    /// Checks what was served and updates UI and state od customer
    /// </summary>
    /// <param name="veg">The veg that was served</param>
    /// <param name="player"> The player who served</param>
    public void ItemDroped(GameObject veg, GameObject player)
    {
        foreach (GameObject t in order)
        {
            string tName = t.name + "(Clone)";
            string vegName = veg.name;
            Debug.Log("<> <> <> Check order for " + vegName + "/ "+ (tName));
            if (tName == vegName)
            {
                if (!veg.GetComponent<Veg>().isChoped)
                {
                    Debug.LogError("Veg not chopped");
                    Debug.Log(veg.name + " Not choped"); // Warning msg
                    return;
                }
                order.Remove(t);
                veg.SetActive(true);
                veg.transform.SetParent(transform);
                veg.transform.localPosition = new Vector3(order.Count+1,0,0);
                Debug.Log("Placed veg");
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
        Debug.LogError("Customer is angry");
        timeCountDownRate *= 0.7f;
    }


    /// <summary>
    /// Once order is complete, it grants score to player and asks to create new customer.
    /// </summary>
    void OrderComplete()
    {
        isAngry = false;
        Debug.Log("Order complited");
        //waitTimeDispaly.text = "--";
        float waitTimeRemaining = (waitTime - (float)Mathf.Round((Time.time - timeOfOrder) * 100f) / 100f);
        if ((waitTimeRemaining / waitTime) >= 0.7f)
        {
            // Set power up for player
            targetPlayer.GetComponent<PlayerController>().SetPowerup();
        }
        timeOfOrder = 0;
        waitTime = 0;
        order.Clear();
        targetPlayer.GetComponent<PlayerController>().GrantScore();
    }

    /// <summary>
    /// Sets wait time, order details and starts the timer
    /// </summary>
    /// <param name="orderObj">list of order</param>
    /// <param name="img"> list of UI images</param>
    /// <param name="sno">Sno of customer</param>
    public void SetMyOrder(List<GameObject> orderObj, GameObject[] img, int sno)
    {
        order = orderObj;
        waitTime = orderObj.Count * waitTimeMultiplyer; // Set wait time for the customer
        timeOfOrder = Time.time;
        orderPlaceHolders = img;
        this.sno = sno;
        StartCoroutine(TimeCounter());
    }

    IEnumerator TimeCounter()
    {
        count++;
        yield return new WaitForSeconds(timeCountDownRate);
        timerText.text = "" + (waitTime - count);
        if ((waitTime - count) > 0)
        {
            StartCoroutine(TimeCounter());
        }
        else
        {
            timeOfOrder = 0;
            waitTime = 0;
            order.Clear();
            for (int i = 0; i < orderPlaceHolders.Length; i++)
            {
                orderPlaceHolders[i].SetActive(false);
            }
            GameObject.Find("Manager").GetComponent<CustomerSpawner>().SpawnNewCustomer(sno);
            GameObject.Find("Manager").GetComponent<ScoreTrack>().CustomerLeft(50);             //penalty for not serving the customer
            Debug.Log(" +++ Object destroyed +++");
            Destroy(gameObject);
        }
        
    }

}
