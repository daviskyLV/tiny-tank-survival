using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

// Singleton to handle some game related stuff
public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
