using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopBord : MonoBehaviour {

    public GameObject chopBordPlaceHolder; // pos to spawn the veg on chopbord
    public GameObject platePlaceHolder;    // pos to spawn the veg on plate
    GameObject playerChopping;
    int vegCount;
    float timer=0;
    List<GameObject> VegOnChopBord;
	// Use this for initialization
	void Start () {
        VegOnChopBord = new List<GameObject>();

    }
	
	// Update is called once per frame
	void Update () {
        if (VegOnChopBord.Count > 0 && playerChopping != null)
        {
            if (timer -Time.time  <= 0)
            {
                UnfrezzPlayer();
            }
        }
	}

    public void DropItems(GameObject veg, GameObject player)
    {

       
        playerChopping.GetComponent<PlayerController>().choping = true;
        if (timer == 0)
        {
            timer = Time.time;
        }
        timer += 2;
        GameObject temp = Instantiate(veg);
        if(vegCount == 0)
            temp.transform.SetParent(chopBordPlaceHolder.transform);
        if(vegCount > 0)
            temp.transform.SetParent(platePlaceHolder.transform);
        temp.transform.localPosition = new Vector3(0,0,0);
        temp.GetComponent<Veg>().isChoped = true;
        VegOnChopBord.Add(temp);
        vegCount++;
        
    }

    public bool CanChop()
    {
        for (int i = 0; i < VegOnChopBord.Count; i++)
        {
            if (!VegOnChopBord[i].activeInHierarchy)
            {
                VegOnChopBord.RemoveAt(i);
                vegCount--;
               
            }
        }
        if (vegCount < 2)
            return true;
        return false;
    }

    void UnfrezzPlayer()
    {
        playerChopping.GetComponent<PlayerController>().choping = false;
        playerChopping = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerChopping = other.gameObject;
        }
    }
}
