using UnityEngine;

public class Locomotion : MonoBehaviour
{
    // the collection of wheels
    [SerializeField] private WheelCollider[] _wheelColliders;
    // torque
    [SerializeField] private float _torque = 250.0f;
    // maximum steering angle
    [SerializeField] private float _maxSteeringAngle = 30.0f;
    // maximum braking torque
    [SerializeField] private float _maxBrakingTorque = 550.0f;
    [SerializeField] private GameObject _skidPrefab;
    [SerializeField] private ParticleSystem _wheelSmokePrefab;
    [SerializeField] private float _skidThreshold = 0.4f;
    [SerializeField] private AudioClip _skidSoundEffect;

    private Transform[] _skidTrails = new Transform[4];
    private ParticleSystem[] _wheelSmokes = new ParticleSystem[4];
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Car missing audio source");
        }

        // have the wheel smoke ready
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            // create a particle but stop it play right away
            _wheelSmokes[i] = Instantiate(_wheelSmokePrefab);
            _wheelSmokes[i].Stop();
        }
    }


    // Update is called once per frame
    void Update()
    {
        // get simple up /down inputs
        float acceleration = Input.GetAxis("Vertical");
        // steering inputs
        float steering = Input.GetAxis("Horizontal");
        // brake input
        float braking = Input.GetAxis("Brakes");

        Move(acceleration, steering, braking);
        SkidCheck();
    }

    private void Move(float acceleration, float steering, float braking)
    {
        Quaternion quaternion;
        Vector3 position;

        // ensure the values are clamped
        acceleration = Mathf.Clamp(acceleration, -1f, 1f);
        steering = Mathf.Clamp(steering , - 1f, 1f) * _maxSteeringAngle;
        braking = Mathf.Clamp(braking, 0, 1f) * _maxBrakingTorque;
        // calculate the thrust torque
        float thrustTorque = acceleration * _torque;

        // apply thrust torque to each wheel
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].motorTorque = thrustTorque;

            // apply steering to the front wheels and braking to the rear
            if (i < 2)
            {
                _wheelColliders[i].steerAngle = steering;
            }
            else
            {
                _wheelColliders[i].brakeTorque = braking;
            }

            // get the position and rotation of the wheel collider
            _wheelColliders[i].GetWorldPose(out position, out quaternion);
            // reposition the game object with the mesh of the wheel
            _wheelColliders[i].transform.GetChild(0).transform.position = position;
            // apply the rotation to the game object
            _wheelColliders[i].transform.GetChild(0).transform.rotation = quaternion;
        }
    }

    private void SkidCheck()
    {
        int skidCount = 0;
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            // get the point on the ground where the wheel collider is touching
            WheelHit wheelHit;
            _wheelColliders[i].GetGroundHit(out wheelHit);

            // check if there is any forward or sideway slipping
            if (Mathf.Abs(wheelHit.forwardSlip) >= _skidThreshold ||
                Mathf.Abs(wheelHit.sidewaysSlip) >= _skidThreshold)
            {
                skidCount++;
                // check the sound isn't playing
                if(!_audioSource.isPlaying)
                {
                    // play the skidding sound effect
                    _audioSource.PlayOneShot(_skidSoundEffect);
                }
                // position the wheel smoke half way up the wheel
                _wheelSmokes[i].transform.position = _wheelColliders[i].transform.position - _wheelColliders[i].transform.up * _wheelColliders[i].radius;
                // do one burst
                _wheelSmokes[i].Emit(1);
            }
        }

        // turn off the skidding sound if there is no wheels skidding
        if (skidCount == 0 && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
