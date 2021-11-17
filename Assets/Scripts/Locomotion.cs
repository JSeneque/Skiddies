using UnityEngine;

public class Locomotion : MonoBehaviour
{
    // the collection of wheels
    [SerializeField] private WheelCollider[] _wheelColliders;
    // torque
    [SerializeField] private float torque = 250.0f;

    // Update is called once per frame
    void Update()
    {
        // get simple up /down inputs
        float acceleration = Input.GetAxis("Vertical");
        Move(acceleration);
    }

    private void Move(float acceleration)
    {
        // ensure the values are clamped
        acceleration = Mathf.Clamp(acceleration, -1f, 1f);
        // calculate the thrust torque
        float thrustTorque = acceleration * torque;

        // apply thrust torque to each wheel
        foreach (var wheel in _wheelColliders)
        {
            wheel.motorTorque = thrustTorque;
        }
    }
}
