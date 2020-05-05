



using UnityEngine;




/// <summary>
/// A simple script for following a game object
/// </summary>
public class SmoothCamera : MonoBehaviour
{



    [Space]
    [Header("Camera Movement Properties")]
    [Space]

    [Tooltip("Check to follow the mouse, and not the object")]
    [SerializeField] private bool _followMouse = false;
    
    [Tooltip("What this follows")]
    [SerializeField] private Transform _target;

    [Tooltip("How quickly the camera follows the target")]
    [SerializeField] private float _moveSpeed = 0.5f;

    [Tooltip("How quickly the camera looks at the target")]
    [SerializeField] private float _rotSpeed = 0.5f;

    [Tooltip("How far back the camera is from the target")]
    [SerializeField] private Vector3 _offsetDistance;

    [Tooltip("What angle to look at the target with")]
    [SerializeField] private Quaternion _offsetAngle;




    /// <summary>
    /// <para> If the mouse is selected, a new game object is created </para>
    /// <para> This object represents the mouse in the game space </para>
    /// </summary>
    private void Start()
    {
        // Mouse Case
        if (_followMouse)
        {
            _target = new GameObject().transform;
            _target.position = Input.mousePosition;
        }
    }




    /// <summary>
    /// <para> Moves the camera to the target after every frame </para>
    /// <para> Combines target, offset, and lerps at the given speed </para>
    /// </summary>
    private void FixedUpdate()
    {
        // Error Handling
        if (!_target)
        {
            return;
        }

        // Mouse Case
        if (_followMouse)
        {
            _target.position = Input.mousePosition;
        }

        // Update Position
        Vector3 targetPosition = _target.position + _offsetDistance;
        this.transform.position = Vector3.Lerp(
            this.transform.position,
            targetPosition,
            _moveSpeed);

        // Update Rotation
        this.transform.rotation = Quaternion.Lerp(
            this.transform.rotation,
            Quaternion.Lerp(
                _target.rotation,
                _offsetAngle,
                0.5f),
            Time.time * _rotSpeed);
    }




}