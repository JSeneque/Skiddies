using UnityEngine;

public class Locomotion : MonoBehaviour
{
    // the collection of wheels
    [SerializeField] private WheelCollider[] _wheelColliders;
    // torque
    [SerializeField] private float _torque = 250.0f;
    // maximum steering angle
    [SerializeField] private float _maxSteeringAngle = 30.0f;

    // Update is called once per frame
    void Update()
    {
        // get simple up /down inputs
        float acceleration = Input.GetAxis("Vertical");
        // steering inputs
        float steering = Input.GetAxis("Horizontal");
        Move(acceleration, steering);
    }

    private void Move(float acceleration, float steering)
    {
        Quaternion quaternion;
        Vector3 position;

        // ensure the values are clamped
        acceleration = Mathf.Clamp(acceleration, -1f, 1f);
        steering = Mathf.Clamp(steering , - 1f, 1f) * _maxSteeringAngle;
        // calculate the thrust torque
        float thrustTorque = acceleration * _torque;

        // apply thrust torque to each wheel
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].motorTorque = thrustTorque;

            // apply steering to the front wheels
            if (i < 2)
            {
                _wheelColliders[i].steerAngle = steering;
            }

            // get the position and rotation of the wheel collider
            _wheelColliders[i].GetWorldPose(out position, out quaternion);
            // reposition the game object with the mesh of the wheel
            _wheelColliders[i].transform.GetChild(0).transform.position = position;
            // apply the rotation to the game object
            _wheelColliders[i].transform.GetChild(0).transform.rotation = quaternion;
        }
    }
}
