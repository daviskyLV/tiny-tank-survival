using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private TankShooting tankShooting;
    private InputAction fire;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        fire.Disable();
        fire.performed -= Fire;
    }

    // Start is called before the first frame update
    void Start()
    {
        var tank = transform.Find(Constants.TankName);
        tankShooting = tank.GetComponent<TankShooting>();
    }

    private void Fire(InputAction.CallbackContext context)
    {
        tankShooting.Shoot();
    }
}
