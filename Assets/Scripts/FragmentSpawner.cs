using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentSpawner : MonoBehaviour
{
    [SerializeField] Assemblage assemblage;

    [SerializeField] float spawnInterval = 5;
    [SerializeField] float spawnHeight = 10;
    [SerializeField] float spawnWidth = 3;

    List<Attachable> fragments;

    float timeOfLastSpawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        fragments = assemblage.GetFragments();

        foreach (Attachable fragment in fragments)
        {
            fragment.transform.parent = null;
        }

        timeOfLastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (fragments.Count > 0 && Time.time >= timeOfLastSpawn + spawnInterval)
        {
            Vector3 spawnPos = new(Random.Range(-spawnWidth, spawnWidth), spawnHeight, 0);

            Attachable spawnedFragment = fragments[0];
            spawnedFragment.transform.position = spawnPos;
            spawnedFragment.gameObject.SetActive(true);
            fragments.RemoveAt(0);

            timeOfLastSpawn = Time.time;
        }
    }
}
