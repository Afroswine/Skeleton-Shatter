using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementNew : MonoBehaviour
{
    private CharacterController _controller;

    [Header("Movement")]
    [SerializeField]
    private float _walkSpeed = 6f;
    [SerializeField]
    private float _sprintSpeed = 12f;
    [SerializeField, Tooltip("How high the player can jump (in Unity meters)")]
    private float _jumpHeight = 1f;
    [Header("Gravity")]
    [SerializeField]
    private float _gravity = 20f; // make negative
    [SerializeField, Tooltip("Maximum fall speed")]
    private float _maxFall = 35f; // make negative
    [SerializeField, Tooltip("The transform used to check for ground collisions")]
    private Transform _groundCheck;
    [SerializeField, Tooltip("Slow the player's descent when this close to the ground")]
    private float _groundDistance;
    [SerializeField, Tooltip("The layers to check for ground collisions")]
    private LayerMask _groundMask;

    private Vector3 _velocity;          // primarily for gravity/jumping
    private Vector3 _groundMovement;    // primarily for grounded movement
    private bool _isGrounded;
    private float _currentSpeed;
    

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        // Ensure the following numbers are negative
        _gravity = -Mathf.Abs(_gravity);
        _maxFall = -Mathf.Abs(_gravity);

        Stats.Gravity.PlayerGrav = _gravity;
        Stats.Gravity.PlayerMaxFall = _maxFall;
    }

    private void Update()
    {
        Gravity();
        DetectInput();

        // apply transforms
        _controller.Move(_groundMovement * _currentSpeed * Time.deltaTime);
        _controller.Move(_velocity * Time.deltaTime);
    }

    // apply gravity to the player
    private void Gravity()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if(_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        else if( !_isGrounded && _velocity.y <= _maxFall)
        {
            _velocity.y = _maxFall;
        }

        _velocity.y += _gravity * Time.deltaTime;
    }

    // detect players movement inputs
    private void DetectInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        _groundMovement = transform.right * x + transform.forward * z;

        // walking & sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed = _sprintSpeed;
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }

        // jumping
        if(Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

    }
}
