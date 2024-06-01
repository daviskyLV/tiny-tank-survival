using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5;
    [SerializeField]
    private float rotationSpeedDegrees = 120;

    private GameObject tank;
    private NavMeshAgent agent;
    private PlayerInputActions playerControls;
    private InputAction moveAction;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        tank = transform.Find(Constants.TankName).gameObject;
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        moveAction = playerControls.Player.Move;
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    // Update is called once per physics frame
    void Update()
    {
        var tTrans = tank.transform;

        // Movement
        var moveAxis = moveAction.ReadValue<Vector2>();
        tTrans.rotation = Quaternion.Euler(0,
            tTrans.rotation.eulerAngles.y + rotationSpeedDegrees * moveAxis.x * Time.deltaTime,
            0);
        if (moveAxis.y > 0)
            agent.Move(movementSpeed * Time.deltaTime * tTrans.forward);
    }
}
