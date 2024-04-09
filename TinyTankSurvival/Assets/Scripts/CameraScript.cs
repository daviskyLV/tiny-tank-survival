using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject PlayerTank { get; set; }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacterCamera();
    }

    private void UpdateCharacterCamera()
    {
        if (PlayerTank == null)
            return;

        var tTrans = PlayerTank.transform;

        // Updating camera position
        transform.SetPositionAndRotation(
            tTrans.position + new Vector3(0, 11, 0) + tTrans.forward * -5,
            Quaternion.Euler(69, tTrans.rotation.eulerAngles.y, 0)
        );
    }
}
