using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject PlayerTank { get; set; }
    public GameObject Tank { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // As a safe measure making sure the tank doesnt flip
        //Tank.transform.rotation = Quaternion.Euler(0, Tank.transform.rotation.eulerAngles.y, 0);
        UpdateDestination();
    }

    private void UpdateDestination() {
        if (PlayerTank == null)
            return;

        var pTrans = PlayerTank.transform;
        if (Vector3.Distance(transform.position, pTrans.position) >= agent.stoppingDistance)
        {
            // Updating destination's position
            agent.destination = pTrans.position;
            return;
        }

        // Agent is too close to the player, try backing up
        //agent.Move(Vector3.MoveTowards(transform.position, pTrans.position, 1.0f) );
    }
}
