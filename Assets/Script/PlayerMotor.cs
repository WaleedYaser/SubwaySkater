using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMotor : MonoBehaviour {

    private const float LANE_DISTANCE = 3.0f;

    // Movement
    private CharacterController _contorller;
    private float _jumpForce = 4.0f;
    private float _gravity = 12.0f;
    private float _verticalVelocity;
    private float _speed = 7.0f;
    private int _desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right

    private void Start()
    {
        _contorller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Gather input on which lane we should be
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _MoveLane(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _MoveLane(true);
        }

        // Calculate where should we be
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        switch (_desiredLane)
        {
            case 0:
                targetPosition += Vector3.left * LANE_DISTANCE;
                break;  
            case 2:
                targetPosition += Vector3.right * LANE_DISTANCE;
                break;
        }

        // Calculate our move delta
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * _speed;
        moveVector.y = -0.1f;
        moveVector.z = _speed;

        // Move the pengu
        _contorller.Move(moveVector * Time.deltaTime);
    }

    private void _MoveLane(bool goingRight)
    {
        _desiredLane += (goingRight) ? 1 : -1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);
    }
}
