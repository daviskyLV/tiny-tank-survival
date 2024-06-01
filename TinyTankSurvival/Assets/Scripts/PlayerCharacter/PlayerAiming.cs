using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField]
    private float aimRayDistance = 1000.0f;

    private TankAiming tankAiming;

    // Start is called before the first frame update
    void Start()
    {
        var tank = transform.Find(Constants.TankName);
        tankAiming = tank.GetComponent<TankAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tankAiming != null)
        {
            tankAiming.AimPosition = GetTargetPoint();
        }
    }

    private Vector3 GetTargetPoint()
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
