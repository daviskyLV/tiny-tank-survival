using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Tutorials used for custom navmesh navigation:
// https://www.youtube.com/watch?v=scaBHHFKLL0
// https://www.youtube.com/watch?v=QzitQSLhfG0
// https://www.youtube.com/watch?v=o10IwnX1W0A

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform tank;
    [SerializeField]
    [Range(0.1f, 3f)]
    private double pathTimeUpdate = 0.4;

    private Transform playerTank;
    private bool setup;

    private NavMeshPath travelPath;
    private double lastTimePathUpdated;
    private int pathIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent.enabled = true;
        lastTimePathUpdated = 0;
    }

    /// <summary>
    /// Set up the enemy tank, can only be executed once
    /// </summary>
    /// <param name="playerTank">The player tank to chase</param>
    public void Setup(Transform playerTank)
    {
        if (setup)
            return;

        this.playerTank = playerTank;
        RecalculateTravelPath();
        setup = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!setup)
            return;

        RecalculateTravelPath();
    }

    private void FixedUpdate()
    {
        if (!setup)
            return;

        if (RotateTowardsNextPoint())
            MoveTank();
    }

    private void RecalculateTravelPath()
    {
        if (lastTimePathUpdated + pathTimeUpdate > Time.timeAsDouble || playerTank == null)
            return;

        travelPath = new NavMeshPath();
        lastTimePathUpdated = Time.timeAsDouble;
        agent.CalculatePath(
            playerTank.position,
            travelPath);
        pathIndex = 0;
    }

    /// <summary>
    /// Rotates the tank towards the next point
    /// </summary>
    /// <returns>True if facing towards next point</returns>
    private bool RotateTowardsNextPoint()
    {
        if (travelPath.corners.Length <= 0 || playerTank == null || pathIndex >= travelPath.corners.Length)
            return false;

        var desiredRotationY = Quaternion.FromToRotation(
            tank.forward, (travelPath.corners[pathIndex] - tank.position).normalized
            ).eulerAngles.y;
        var appliedRotation = Quaternion.RotateTowards(
            tank.rotation, Quaternion.Euler(0, desiredRotationY, 0), Time.deltaTime * agent.angularSpeed
            );

        var a2 = Quaternion.Lerp(
            tank.rotation, Quaternion.Euler(0, desiredRotationY, 0), (Time.deltaTime * agent.angularSpeed)/360
            );

        var appliedRotationY = (tank.rotation.eulerAngles.y - desiredRotationY) * Time.deltaTime;// * agent.angularSpeed;
        //var appliedRotation = Quaternion.RotateTowards(tank.rotation, desiredRotation, Time.deltaTime * agent.angularSpeed);
        //Debug.Log($"Current rotation: {tank.rotation.eulerAngles}");
        //Debug.Log($"Desired rotation: {desiredRotationY}, applied: {a2.eulerAngles.y}");

        tank.rotation = Quaternion.Euler(0, a2.eulerAngles.y, 0);
        var degreeDif = Mathf.Abs(desiredRotationY - a2.eulerAngles.y);
        
        if (degreeDif > 1.0f)
        {
            // Agent's rotation is still not facing the next point within a degree error
            return false;
        }

        //Debug.Log($"Degree difference less than 1: {degreeDif}");
        return true;
    }

    private void MoveTank() {
        if (playerTank == null && pathIndex < travelPath.corners.Length)
            return;

        if (Vector3.Distance(tank.position, playerTank.position) <= agent.stoppingDistance) {
            // Close to player, but is it in line of sight?
            RaycastHit hit;
            Ray ray = new Ray(tank.position, tank.forward);
            // https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
            // Bit shift the index of the layer (8) to get a bit mask
            // This would cast rays only against colliders in layer 8 (character layer).
            int layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hit, agent.stoppingDistance + 25, layerMask))
            {
                if (playerTank != hit.collider.transform)
                {
                    // nothing is blocking the view, hit player
                    return;
                }
            }
        }

        // Not too close to player
        var movementVector = agent.speed * Time.deltaTime * tank.forward;
        var maxMovementVector = travelPath.corners[pathIndex] - tank.position;
        if (movementVector.sqrMagnitude >= maxMovementVector.sqrMagnitude) {
            // Movement overshoots / reaches destination
            agent.Move(maxMovementVector);
            pathIndex++;
            return;
        }

        // Travelling acros path
        agent.Move(movementVector);

        // Agent is too close to the player, try backing up
        //agent.Move(Vector3.MoveTowards(transform.position, pTrans.position, 1.0f) );
    }
}
