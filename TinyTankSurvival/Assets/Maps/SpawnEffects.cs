using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffects : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    }
}
