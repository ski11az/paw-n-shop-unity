using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreFlow : MonoBehaviour
{
    public GameObject nightScene;
    public GameObject customer1;

    public int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount = 1;
            nightScene.SetActive(false);
        }
    }
}
