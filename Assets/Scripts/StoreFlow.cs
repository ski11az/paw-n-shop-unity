using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoreFlow : MonoBehaviour
{
    [SerializeField] GameEvent[] gameEvents;
    public GameObject customer1;

    public void StartDay()
    {
        StartCoroutine(Co_PlayInteraction());
    }

    // Start is called before the first frame update
    void Start()
    {
        //startday
        //Loop through all customers
        //for (int i = 0; i < 5; i++)
        //{
        //    Debug.Log("Kund nr" + i);
        //    //Handles customers and minigames by calling customerActions
        //}

    }

    private IEnumerator Co_PlayInteraction()
    {
        foreach(GameEvent gameEvent in gameEvents)
        {
            Debug.Log("Executing first event");
            gameEvent.PlayEvent();
            while (!gameEvent.IsFinished)
            {
                Debug.Log("Waiting on event finish");

                yield return null;
            }
        }
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