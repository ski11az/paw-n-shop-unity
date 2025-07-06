using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreFlow : MonoBehaviour
{
    public GameObject customer1;


    // Start is called before the first frame update
    void Start()
    {
        //startday
        //Loop through all customers
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Kund nr" + i);
            //Handles customers and minigames by calling customerActions
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}


//Create 
/*public void customerActions(Gameobject customer)
{
    //Customer comes in
    //Customer dialogue runs
    //Camera pans to right
    //Enters minigame scene and play minigame
    //Exits minigame and´returns to scene
    //Pans camera to left
    //Run customer exit dialogue
    //Customer exits the shop
}*/