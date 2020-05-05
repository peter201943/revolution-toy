



using UnityEngine;




/// <summary>
/// A simple script for following a game object
/// </summary>
public class SmoothCamera : MonoBehaviour
{



    [Space]
    [Header("Camera Movement Properties")]
    [Space]
    
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
    /// Moves the camera to the target after every frame
    /// Combines target, offset, and lerps at the given speed
    /// </summary>
    private void FixedUpdate()
    {
        // Error Handling
        if (!_target)
        {
            return;
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