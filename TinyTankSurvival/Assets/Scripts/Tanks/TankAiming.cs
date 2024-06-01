using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankAiming : MonoBehaviour
{
    [SerializeField]
    private GameObject TankTop;
    public Vector3 AimPosition { get; set; } = Vector3.zero;

    private Transform tTopTrans;

    private void Start()
    {
        tTopTrans = TankTop.transform;
    }

    // Update is called once per frame
    void Update()
    {
        tTopTrans.LookAt( new Vector3(AimPosition.x, tTopTrans.position.y, AimPosition.z) );
    }
}
