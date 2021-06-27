using UnityEngine;

public class Rotate : MonoBehaviour
{
    #region --Fields / Properties--
    
    /// <summary>
    /// The point the game object rotates around.
    /// </summary>
    [SerializeField]
    private Transform _pointOfRotation;
    
    /// <summary>
    /// Distance from the point of rotation.
    /// Polar coordinate "r".
    /// </summary>
    [SerializeField]
    private float _distance;
    
    /// <summary>
    /// Optional maximum velocity.
    /// </summary>
    [SerializeField]
    private float _maxVelocity;

    /// <summary>
    /// Key when held down to apply positive acceleration.
    /// </summary>
    [SerializeField]
    private KeyCode _accelerateKey = KeyCode.Mouse0;
    
    /// <summary>
    /// Key when held down to apply negative acceleration.
    /// </summary>
    [SerializeField]
    private KeyCode _decelerateKey = KeyCode.Mouse1;

    /// <summary>
    /// How much to increment/decrement acceleration based on player input.
    /// </summary>
    private float _accelerationIncrement = .0001f;
    
    /// <summary>
    /// Angle of rotation in degrees.
    /// Polar coordinate "theta".
    /// </summary>
    private float _angle;

    /// <summary>
    /// Speed of rotation.
    /// </summary>
    private float _angularVelocity;

    /// <summary>
    /// How much the rotation is speeding up or slowing down.
    /// </summary>
    private float _angularAcceleration;

    /// <summary>
    /// Cached Transform component.
    /// </summary>
    private Transform _transform;
    
    #endregion

    #region --Unity Specific Methods--

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        ApplyAcceleration(_accelerationIncrement);
        Move();
    }

    #endregion

    #region --Custom Methods--

    /// <summary>
    /// Initialization method to handle setting any initial values or caching of components.
    /// </summary>
    private void Init()
    {
        _transform = transform;
        _angle *= Mathf.Deg2Rad;
        Vector3 _newPos = ConvertPolarToCartesian(_distance, _angle);
        _transform.position = _newPos;
    }
    
    /// <summary>
    /// Check player input to handle acceleration.
    /// </summary>
    private void ApplyAcceleration(float _amount)
    {
        if (Input.GetKey(_accelerateKey))
        {
            _angularAcceleration += _amount;
        }
        else if (Input.GetKey(_decelerateKey))
        {
            _angularAcceleration -= _amount;
        }
    }

    /// <summary>
    /// Handles movement and rotation.
    /// </summary>
    private void Move()
    {
        //Motion of game object
        _angle += _angularVelocity;
        _angularVelocity += _angularAcceleration;

        if (_maxVelocity > 0)
        {
            _angularVelocity = Mathf.Clamp(_angularVelocity, 0, _maxVelocity);
        }

        //Get updated position.
        Vector3 _newPos = ConvertPolarToCartesian(_distance, _angle);
        _transform.position = _newPos;
        
        //Rotate around the point of rotation.
        transform.RotateAround(_pointOfRotation.position, Vector3.up, _angularVelocity);
        
        _angularAcceleration = 0.0f;
    }

    /// <summary>
    /// Converts polar coordinates to Cartesian x and y coordinates.
    /// </summary>
    private Vector3 ConvertPolarToCartesian(float _r, float _angle)
    {
        float _x = _r * Mathf.Cos(_angle);
        float _y = _r * Mathf.Sin(_angle);
        Vector3 _position = new Vector3(_x, _y, 0);
        
        return _position;
    }
    
    #endregion
    
}