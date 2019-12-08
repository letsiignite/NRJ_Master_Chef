using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class spawns new customer,
/// Get order from them,
/// Set UI accordingly,
/// Then hopes everything will work perfectly.
/// </summary>
public class CustomerSpawner : MonoBehaviour
{

    #region public
    public Sprite[] vegPrefabImages;
    public GameObject[] vegPrefab;
    public GameObject[] customer_UI;
    public Text[] customer_Timers;
    public GameObject customerPrefab;
    public GameObject[] customerSittingPositions;
    public GameObject[] activeCustomerList;
    #endregion

    #region private
    int index;
    List<GameObject> order;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        order = new List<GameObject>();
        activeCustomerList = new GameObject[customerSittingPositions.Length];
        for (int i = 0; i < customerSittingPositions.Length; i++)
        {
            SpawnCustomer();
        }
        //SpawnNewCustomer(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Spawn 4 customers to start
    /// </summary>
   void SpawnCustomer()
    {
        for (int it = 0; it < activeCustomerList.Length; it++)    
        {
            if (activeCustomerList[it] == null)
            {
                GameObject temp = Instantiate(customerPrefab);
                temp.transform.position = customerSittingPositions[it].transform.position;
                temp.GetComponent<Customer>().timerText = customer_Timers[it];
                GetOrder(it);
                GameObject[] OrderPlaceHolders = new GameObject[customer_UI[it].transform.childCount];
                for (int i = 0; i < customer_UI[it].transform.childCount; i++)
                {
                    OrderPlaceHolders[i] = customer_UI[it].transform.GetChild(i).gameObject;
                }
                Debug.Log(it+") Order count = "+order.Count);
                temp.GetComponent<Customer>().SetMyOrder(order, OrderPlaceHolders,it);
                activeCustomerList[it] = temp;
                
            }
        }
    }

    /// <summary>
    /// Spawns new customer when requested
    /// </summary>
    /// <param name="index"> Position index </param> 

    public void SpawnNewCustomer(int index)
    {
        Debug.Log("New Customer --- "+index);
        GameObject temp = Instantiate(customerPrefab);
        temp.transform.position = customerSittingPositions[index].transform.position;
        temp.GetComponent<Customer>().timerText = customer_Timers[index];
        GetOrder(index);
        GameObject[] OrderPlaceHolders = new GameObject[customer_UI[index].transform.childCount];
        for (int i = 0; i < customer_UI[index].transform.childCount; i++)
        {
            OrderPlaceHolders[i] = customer_UI[index].transform.GetChild(i).gameObject;
        }
        Debug.Log("Assigning new order");
        temp.GetComponent<Customer>().SetMyOrder(order, OrderPlaceHolders, index);
       
    }


    /// <summary>
    /// Generates orders for specified customer
    /// </summary>
    /// <param name="UiIndex"> index of the customer </param>
    void GetOrder(int UiIndex)
    {
        Debug.Log("Get order for :"+UiIndex);
        int orderItemsCount = 2;
        order.Clear();
        int subUiIndex = 0;
        while (orderItemsCount > -1)
        {
            int num = Random.Range(0, vegPrefab.Length * 2);
            if (num < vegPrefab.Length)
            {
                customer_UI[UiIndex].transform.GetChild(subUiIndex).GetComponent<Image>().sprite = vegPrefabImages[num];  // Show the image of order on UI
                customer_UI[UiIndex].transform.GetChild(subUiIndex).gameObject.SetActive(true);
                subUiIndex++;
                order.Add(vegPrefab[num]);
                Debug.Log("  ----------  Order  -->> "+vegPrefab[num].name+" &  "+ vegPrefabImages[num]);
            }
            orderItemsCount--;
        }
        if (subUiIndex == 0)
        {
            customer_UI[UiIndex].transform.GetChild(subUiIndex).GetComponent<Image>().sprite = vegPrefabImages[0];  //Default order
            customer_UI[UiIndex].transform.GetChild(subUiIndex).gameObject.SetActive(true);
            order.Add(vegPrefab[0]);
        }
    }
}