



using UnityEngine;




/// <summary>
/// A Simple Keyboard Input Mover
/// (Maps to WASD for XY)
/// </summary>
public class SimpleMover : MonoBehaviour
{



    [Tooltip("How quickly this moves")]
    [SerializeField] private float _speed = 0.5f;




    /// <summary>
    /// Checks each key and applies requisite translation
    /// </summary>
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            this.transform.Translate(0, Time.deltaTime * _speed, 0);
        }
        if (Input.GetKey("s"))
        {
            this.transform.Translate(0, -Time.deltaTime * _speed, 0);
        }
        if (Input.GetKey("a"))
        {
            this.transform.Translate(-Time.deltaTime * _speed, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            this.transform.Translate(Time.deltaTime * _speed, 0, 0);
        }
    }




}