using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

// Singleton to handle some game related stuff
public class GameHandler : MonoBehaviour
{
    private static GameHandler _instance;

    public static GameHandler Instance {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
