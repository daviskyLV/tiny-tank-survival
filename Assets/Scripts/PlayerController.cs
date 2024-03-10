using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject tankPrefab;
    private GameObject[] spawns;
    public Rigidbody playerRigidbody;
    private GameObject tank;

    public float rotationSpeed = 3;
    public float movementSpeed = 6;
    // Start is called before the first frame update
    void Start()
    {
        var spawnHolder = GameObject.Find("/Map/Spawnpoints");
        spawns = new GameObject[spawnHolder.transform.childCount];

        for (int i = 0; i < spawns.Length; i++)
        {
            spawns[i] = spawnHolder.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Respawn();
        }

        // movement and rotation
        if (tank != null)
        {
            float rot = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            float mov = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

            tank.transform.Rotate(0, rot, 0);
            tank.transform.Translate(0, 0, mov);
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawning player!");
        playerRigidbody.isKinematic = true;
        // Destroying old stuff
        if (tank != null)
        {
            Destroy(tank);
        }

        // choosing spawn
        var chosen = Random.Range(0, spawns.Length);

        // spawning
        transform.position = spawns[chosen].transform.position;
        transform.rotation = spawns[chosen].transform.rotation;
        tank = Instantiate(tankPrefab, transform);
        tank.transform.name = "Tank";
        playerRigidbody.isKinematic = false;
    }
}
