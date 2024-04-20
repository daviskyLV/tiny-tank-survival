using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform PlayerTank { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        PlayerController.OnPlayerSpawned += UpdatePlayerTank;
    }

    // Update is called once per frame
    void Update()
    {
        // As a safe measure making sure the tank doesnt flip
        //Tank.transform.rotation = Quaternion.Euler(0, Tank.transform.rotation.eulerAngles.y, 0);
        UpdateDestination();
    }

    private void UpdatePlayerTank(GameObject playerCharacter)
    {
        PlayerTank = playerCharacter.transform.Find("Tank");
    }

    private void UpdateDestination() {
        if (PlayerTank == null)
            return;

        if (Vector3.Distance(transform.position, PlayerTank.position) >= agent.stoppingDistance)
        {
            // Updating destination's position
            agent.destination = PlayerTank.position;
            return;
        }

        // Agent is too close to the player, try backing up
        //agent.Move(Vector3.MoveTowards(transform.position, pTrans.position, 1.0f) );
    }
}
