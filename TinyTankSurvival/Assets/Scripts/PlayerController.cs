using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tankPrefabs;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private float movementSpeed = 2;
    [SerializeField]
    private float rotationSpeedDegrees = 72;

    private GameObject tank;
    private AsyncOperation asyncLoad;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        asyncLoad = SceneManager.LoadSceneAsync("SandyPingPong", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && asyncLoad.isDone) {
            if (tank != null)
                Destroy(tank);

            // Instantiating the new tank
            tank = Instantiate(tankPrefabs[0]);
            tank.name = "PlayerTank";

            // Move the GameObject to the newly loaded Scene
            Scene pingPongScene = SceneManager.GetSceneByName("SandyPingPong");
            SceneManager.MoveGameObjectToScene(tank, pingPongScene);

            tank.transform.position = GameObject.Find("PlayerSpawn").transform.position + new Vector3(0, 0.5f, 0);
        }



        if (tank != null)
        {
            var tTrans = tank.transform;
            var rb = tank.GetComponent<Rigidbody>();

            // Movement
            var vertMove = Input.GetAxis("Vertical");
            var horMove = Input.GetAxis("Horizontal");
            if (vertMove > 0)
                rb.velocity = tTrans.forward * movementSpeed * Time.deltaTime;

            rb.MoveRotation(Quaternion.Euler(0,
                tTrans.rotation.eulerAngles.y + rotationSpeedDegrees * horMove * Time.deltaTime,
                0));
            //tTrans.Rotate(0, rotationSpeedDegrees * horMove * Time.deltaTime, 0);

            // Updating camera position
            mainCamera.transform.SetPositionAndRotation(
                tTrans.position + new Vector3(0, 11, 0) + tTrans.forward*-5,
                Quaternion.Euler(69, tTrans.rotation.eulerAngles.y, 0)
            );
        }
    }
}
