using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField]
    private float aimRayDistance = 1000.0f;
    [SerializeField]
    private float joystickAimingSpeed = 250f; // Degrees per second

    private TankAiming tankAiming;
    private Transform tankTop;
    private PlayerInputActions playerControls;
    private InputAction aimAction;

    // Start is called before the first frame update
    void Start()
    {
        var tank = transform.Find(Constants.TankName);
        tankAiming = tank.GetComponent<TankAiming>();
        tankTop = tank.Find("Top");
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        aimAction = playerControls.Player.Aim;
        aimAction.Enable();
    }

    private void OnDisable()
    {
        aimAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (tankAiming == null)
            return;

        if (Input.GetJoystickNames().Length == 0)
        {
            // No joysticks present, using mouse to aim
            tankAiming.AimPosition = AimWithMouse();
            return;
        }

        // Using joystick to aim
        tankAiming.AimPosition = AimWithJoystick();
    }

    private Vector3 AimWithJoystick()
    {
        // Aiming
        var aimAxis = aimAction.ReadValue<Vector2>();
        // Calculating new gun rotation
        var rotatedAim = Quaternion.Euler(
            0, tankTop.rotation.eulerAngles.y + joystickAimingSpeed * Time.deltaTime * aimAxis.x, 0
            );

        return tankTop.position + (rotatedAim * Vector3.forward) * 10;
    }

    private Vector3 AimWithMouse()
    {
        Vector3 aimAt = Vector3.zero;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, aimRayDistance))
        {
            aimAt = hit.point;
        }
        else
        {
            aimAt = ray.origin + (ray.direction * aimRayDistance);
        }

        return aimAt;
    }
}
