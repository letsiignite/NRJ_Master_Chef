using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    #region public 
    public int speed;
    public string moveForward;
    public string moveBack;
    public string moveRight;
    public string moveleft;
    public string drop;
    #endregion

    #region private
    List<GameObject> items;
    bool nearTable;
    const float distanceForPickup = 1;
    GameObject currentCustomer;
    #endregion
    // Use this for initialization
    void Start()
    {
        items = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        #region pickup

        if (nearTable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "veg")
                    {
                        if (items.Count < 2) // If he has less than 2 vegitables, then he can take from table
                        {
                            if (Vector3.Distance(hit.transform.position, transform.position) < distanceForPickup)// If player is close to the veg
                            {
                                items.Add(hit.collider.gameObject);// Player can take the veg
                                Debug.Log("Picked up " + hit.collider.name);
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region walk
        if (Input.GetKey(moveForward))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(moveBack))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(moveRight))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(moveleft))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        #endregion
        #region drop
        if (Input.GetKey(drop) && currentCustomer !=null)
        {
            currentCustomer.GetComponent<Customer>().ItemDroped(items[0], this.gameObject);
        }
        #endregion
    }

    public List<GameObject> GetItems()
    {
        return items;
    }

    public void SetPowerup()
    {
        //Choose a powerup at random
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("At " + other.name);
        if (other.tag == "table")
        {
            nearTable = true;
        }

        if (other.tag == "customer")
        {
            currentCustomer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "table")
        {
            nearTable = false;
        }

        if (other.tag == "customer")
        {
            currentCustomer = null;         // reset current customer
        }
    }


}
