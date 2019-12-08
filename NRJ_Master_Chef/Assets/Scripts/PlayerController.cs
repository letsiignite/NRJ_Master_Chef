using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    #region public 
    public int speed;
    public string moveForward;
    public string moveBack;
    public string moveRight;
    public string moveleft;
    public string drop;
    public bool choping = false;        // set and reset by chopBord script
    public bool isPlayer1;              // Set if this is player one
    public Image[] foodIconDisplay;
    public ScoreTrack scoreTrackObj;
    #endregion

    #region private
    List<GameObject> items;
    int foodCountIndex;
    bool nearTable;
    bool nearDustbin;
    const float distanceForPickup = 2f;
    GameObject currentCustomer;
    GameObject chopBord;
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

        if (nearTable && !choping)
        {
            //Debug.Log(" --  Near table");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Raycasting");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    
                    if (hit.collider.tag == "veg")
                    {
                        Debug.Log("Hit on " + hit.collider.name);
                        if (items.Count < 2) // If he has less than 2 vegitables, then he can take from table
                        {
                            Debug.Log("Hit on " + hit.collider.name+"  Dist = "+ Vector3.Distance(hit.collider.gameObject.transform.position, transform.position));
                            if (Vector3.Distance(hit.collider.gameObject.transform.position, transform.position) < distanceForPickup)// If player is close to the veg
                            {
                                if (!(hit.collider.gameObject.GetComponent<Veg>().isChoped))
                                {
                                    items.Add(hit.collider.gameObject.GetComponent<Veg>().prefab);// Player can take the veg
                                }
                                else if ((hit.collider.gameObject.GetComponent<Veg>().isChoped))
                                {
                                    items.Add(hit.collider.gameObject);
                                    hit.collider.gameObject.SetActive(false);
                                    hit.collider.gameObject.transform.SetParent(null);
                                }
                                Debug.Log("Picked up " + hit.collider.name);
                                foodIconDisplay[foodCountIndex].sprite = hit.collider.gameObject.GetComponent<Veg>().myIcon;
                                foodIconDisplay[foodCountIndex].gameObject.SetActive(true);
                                foodCountIndex++;
                                Debug.Log("--------------Veg in hand------------");
                            }
                            Debug.Log("<< DONE pickup >> " );
                        }
                    }
                }
            }
        }
        #endregion
        #region walk
        if (!choping)
        {
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
        }
        #endregion
        #region drop
        if (Input.GetKeyUp(drop) && currentCustomer !=null) // Drop on customer plate
        {
            int i = 0;
            while (items.Count > 0)
            {
                Debug.Log("---- Droping "+items[0].name);
                currentCustomer.GetComponent<Customer>().ItemDroped(items[0], this.gameObject);
                items.RemoveAt(0);
                foodIconDisplay[i++].gameObject.SetActive(false);
                foodCountIndex--;
            }
            
        }

        if (Input.GetKeyUp(drop) && chopBord != null)
        {
            int i = 0;
            Debug.Log(" -- Droping at chopbord -- ");
            while (items.Count > 0)
            {
                if (chopBord.GetComponent<ChopBord>().CanChop())
                {
                    chopBord.GetComponent<ChopBord>().DropItems(items[0], this.gameObject);
                    items.RemoveAt(0);
                    foodIconDisplay[i++].gameObject.SetActive(false);
                    foodCountIndex--;
                }
                
            }
            Debug.Log(" -- Droping at chopbord DONE -- ");

           
        }

        if (Input.GetKeyDown(drop) && nearDustbin)
        {
            if (items.Count > 0)
            {
                items.Clear();
                punish();
                foodIconDisplay[0].gameObject.SetActive(false);
                foodIconDisplay[1].gameObject.SetActive(false);
            }

        }
        #endregion
    }

    public List<GameObject> GetItems()
    {
        return items;
    }
    public void punish()
    {
        if (isPlayer1)
        {
            scoreTrackObj.PlayerOneScore(-20);       // -20 score for not serving the customer
        }
        else
        {
            scoreTrackObj.PlayerTwoScore(-20);
        }
        //scoreTrackObj.GameOver();
    }
    public void GrantScore()
    {
        if (isPlayer1)
        {
            scoreTrackObj.PlayerOneScore(50);       // 50 score for comp-liting the order
        }
        else
        {
            scoreTrackObj.PlayerTwoScore(50);
        }
        //scoreTrackObj.GameOver();
    }

    public void SetPowerup()
    {
        int powerUp = Random.Range(1, 2);
        switch (powerUp)
        {
            case 1:
                speed *= 2;
                Invoke("ResetSpeed", 5);
                break;

            case 2:
                if (isPlayer1)
                {
                    scoreTrackObj.PlayerOneScore(10); // 10 score bonus for quickly compliting order 
                }
                else
                {
                    scoreTrackObj.PlayerTwoScore(10);
                }
                break;
        };
        
    }

    void ResetSpeed()
    {
        speed *= (int)0.5f;
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

        if (other.tag == "chopbord")
        {
            chopBord = other.gameObject;
        }

        if (other.tag == "dustbin")
        {
            nearDustbin = true;
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
        if (other.tag == "chopbord")
        {
            chopBord = null;
        }

        if (other.tag == "dustbin")
        {
            nearDustbin = false;
        }
    }


}
