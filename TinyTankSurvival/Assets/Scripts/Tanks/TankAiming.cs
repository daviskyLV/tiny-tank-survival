using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAiming : MonoBehaviour
{
    [SerializeField]
    private GameObject TankTop;
    public Vector3 AimPosition { get; set; } = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        var facingRotation = Quaternion.FromToRotation(TankTop.transform.forward, AimPosition);
        var topRotEul = TankTop.transform.rotation.eulerAngles;
        TankTop.transform.rotation = Quaternion.Euler(topRotEul.x, facingRotation.y, topRotEul.z);
    }
}
