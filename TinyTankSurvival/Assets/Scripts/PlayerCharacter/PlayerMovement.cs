using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5;
    [SerializeField]
    private float rotationSpeedDegrees = 120;

    private GameObject tank;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        tank = transform.Find("Tank").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var tTrans = tank.transform;

        // Movement
        var vertMove = Input.GetAxis("Vertical");
        var horMove = Input.GetAxis("Horizontal");
        tTrans.rotation = Quaternion.Euler(0,
            tTrans.rotation.eulerAngles.y + rotationSpeedDegrees * horMove * Time.deltaTime,
            0);
        if (vertMove > 0)
            agent.Move(movementSpeed * Time.deltaTime * tTrans.forward);
    }
}
