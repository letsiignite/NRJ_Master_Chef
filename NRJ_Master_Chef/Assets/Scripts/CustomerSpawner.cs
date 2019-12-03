using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerSpawner : MonoBehaviour
{

    #region public
    public Sprite[] vegPrefabImages;
    public GameObject[] vegPrefab;
    public GameObject[] customer_UI;
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
        activeCustomerList = new GameObject[customerSittingPositions.Length];
        for (int i = 0; i < customerSittingPositions.Length; i++)
        {
            SpawnCustomer();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnCustomer()
    {
        for (int it = 0; it < activeCustomerList.Length; it++)     // itterate to get the empty seat
        {
            if (activeCustomerList[it] == null)
            {
                GameObject temp = Instantiate(customerPrefab);
                temp.transform.position = customerSittingPositions[it].transform.position;
                GetOrder(it);
                temp.GetComponent<Customer>().SetMyOrder(order);
            }
        }
    }

    void GetOrder(int UiIndex)
    {
        int orderItemsCount = 2;
        int subUiIndex = 0;
        while (orderItemsCount > -1)
        {
            int num = Random.Range(0, vegPrefab.Length + 2);
            if (num < vegPrefab.Length)
            {
                customer_UI[UiIndex].transform.GetChild(subUiIndex++).GetComponent<Image>().sprite = vegPrefabImages[num];  // Show the image of order on UI
                order.Add(vegPrefab[num]);
            }
        }
        if (subUiIndex == 0)
        {
            customer_UI[UiIndex].transform.GetChild(subUiIndex++).GetComponent<Image>().sprite = vegPrefabImages[0];  //Default order
        }
    }
}