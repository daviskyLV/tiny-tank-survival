using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject playerTank;

    private void Start()
    {
        PlayerControllerOLD.OnPlayerSpawned += UpdatePlayerTank;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacterCamera();
    }

    private void UpdateCharacterCamera()
    {
        if (playerTank == null)
            return;

        var tTrans = playerTank.transform;

        // Updating camera position
        transform.SetPositionAndRotation(
            tTrans.position + new Vector3(0, 14, 0) + tTrans.forward * -2,
            Quaternion.Euler(78, tTrans.rotation.eulerAngles.y, 0)
        );
    }

    private void UpdatePlayerTank(GameObject playerCharacter)
    {
        playerTank = playerCharacter.transform.Find("Tank").gameObject;
    }
}
