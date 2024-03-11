using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public float rotationSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(
            0, //rotationSpeed*Time.deltaTime,
            rotationSpeed*Time.deltaTime,
            0 //rotationSpeed*Time.deltaTime
            );
    }
}
