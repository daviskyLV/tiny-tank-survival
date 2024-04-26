using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    // Upgradeable values
    /// <summary>
    /// Rocket travel speed
    /// </summary>
    public float Speed { get; set; } = 3.0f;
    /// <summary>
    /// How fast the rocket rotates towards its TargetTank. Measured in degrees per second
    /// </summary>
    public float HeatSeekingRotation { get; set; } = 72.7f;
    /// <summary>
    /// Target Tank for the rocket to aim towards, null if heat seeking is disabled
    /// </summary>
    public GameObject TargetTank { get; set; }
    /// <summary>
    /// The lifetime of the rocket
    /// </summary>
    public double Lifetime { get; set; } = 10.0;

    private double startTime = 0;

    private void Start()
    {
        startTime = Time.timeAsDouble;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (null == null)
        //    return;

        //CheckLifetime();
        //MoveRocket();
    }

    private void CheckLifetime()
    {
        if (Time.timeAsDouble - startTime > Lifetime)
            Destroy(gameObject);
    }

    //private void MoveRocket()
    //{
    //// handling rotation
    //var aimtowards = transform.position + transform.up * 3;
    //    if (targettank != null)
    //    {
    //        aimtowards = targettank.transform.position;
    //    }

    //    // https://discussions.unity.com/t/how-to-slow-down-transform-lookat/126080/2
    //    //Vector3 direction = aimTowards - transform.position;
    //    //Quaternion toRotation = Quaternion.LookRotation(direction, -transform.forward); //Quaternion.FromToRotation(transform.up, direction);
    //    //var easedHeatRotation = Quaternion.Lerp(transform.rotation, toRotation, HeatSeekingRotation * Time.fixedDeltaTime).eulerAngles;
    //    //
    //    //

    //    // https://www.youtube.com/watch?v=0v_H3oOR0aU
    //    // Comment by @maxokaan that mentions how to do it for 3D
    //    Vector3 direction = aimTowards - transform.position;
    //    direction.Normalize();

    //    Vector3 amountToRotate = Vector3.Cross(direction, transform.up) * Vector3.Angle(transform.up, direction);
    //    var easedHeatRotation = Quaternion.Lerp(
    //        transform.rotation,
    //        Quaternion.Euler(amountToRotate),
    //        HeatSeekingRotation * Time.fixedDeltaTime
    //    );

    //    var spinRotationX = 360 * 3 * Time.fixedDeltaTime * transform.rotation.eulerAngles.x;
    //    var finalRotation = Quaternion.Euler(spinRotationX, easedHeatRotation.y, easedHeatRotation.z);

    //    //rb.angularVelocity = -amountToRotate * rotateSpeed;

    //    // Applying movement & rotation
    //    //rb.Move(
    //    //    transform.position + Speed * Time.fixedDeltaTime * transform.up,
    //    //    finalRotation
    //    //);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Tank") && !other.gameObject.Equals(null))
    //    {
    //        print("Hit a tank! KABOOM!");
    //        print(other.transform);
    //        print(other.transform.parent);
    //        print(other.transform.parent.gameObject);
    //        Destroy(other.transform.parent.gameObject); // destroying the player/enemy
    //    }

    //    Destroy(gameObject);
    //}
}
