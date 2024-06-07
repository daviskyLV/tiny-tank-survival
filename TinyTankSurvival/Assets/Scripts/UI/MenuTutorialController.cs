using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuTutorialController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI anyKeyText;

    // Start is called before the first frame update
    void Start()
    {
        GameHandler.OnGameStarted += GameStarted;
    }

    private void GameStarted()
    {
        anyKeyText.text = "Loading... game will begin shortly!";
    }
}
