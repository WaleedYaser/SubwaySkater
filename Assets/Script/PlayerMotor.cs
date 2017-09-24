using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMotor : MonoBehaviour {

    private const float LANE_DISTANCE = 2.0f;
    private const float TURN_SPEED = 0.5f;

	public bool isRunning = false;

    // Animation
    private Animator _anim;

    // Movement
    private CharacterController _contorller;
    private float _jumpForce = 4.0f;
    private float _gravity = 12.0f;
    private float _verticalVelocity;
    private int _desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right

	// Speed Modifier
	private float _originalSpeed = 7.0f;
	private float _speed;
	private float _speedIncreaseLastTick;
	private float _speedIncreaseTime = 2.5f;
	private float _speedIncreaseAmount = 0.1f;


    private void Start()
    {
        _contorller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
		_speed = _originalSpeed;
    }

    private void Update()
    {
		if (!isRunning)
			return;

		// speed modifier
		if ((Time.time - _speedIncreaseLastTick) > _speedIncreaseTime) {
			_speedIncreaseLastTick = Time.time;
			_speed += _speedIncreaseAmount;
			GameManager.Instance.UpdateModifier (_speed - _originalSpeed);
		}

        // Gather input on which lane we should be
		if(MobileInput.Instance.SwipeLeft)
        {
            _MoveLane(false);
        }
		if (MobileInput.Instance.SwipeRight)
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

        bool isGrounded = _IsGrounded();
        _anim.SetBool("Grounded", isGrounded);

        // Calculate Y
        if (_IsGrounded()) // is grounded
        {
            _verticalVelocity = -0.1f;

			if (MobileInput.Instance.SwipeUp) {
				_verticalVelocity = _jumpForce;
				_anim.SetTrigger ("Jump");
			} else if (MobileInput.Instance.SwipeDown) {
				_StartSliding ();
			}
        }
        else
        {
            _verticalVelocity -= (_gravity * Time.deltaTime);

			if (MobileInput.Instance.SwipeDown)
            {
                _verticalVelocity = -_jumpForce;
            }
        }

        moveVector.y = _verticalVelocity;
        moveVector.z = _speed;

        // Move the pengu
        _contorller.Move(moveVector * Time.deltaTime);

        // Rotate the pengu to where he is going
        Vector3 dir = _contorller.velocity;

        if (dir != Vector3.zero)
        {
            dir.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }
    }

    private void _MoveLane(bool goingRight)
    {
        _desiredLane += (goingRight) ? 1 : -1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);
    }

	private void _StartSliding ()
	{
		_anim.SetBool ("Sliding", true);
		_contorller.height *= 0.5f;
		_contorller.center *= 0.5f;
		Invoke ("_StopSliding", 1f);
	}

	private void _StopSliding ()
	{
		_contorller.height *= 2f;
		_contorller.center *= 2f;
		_anim.SetBool ("Sliding", false);
	}

    private bool _IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                _contorller.bounds.center.x,
                _contorller.bounds.center.y - _contorller.bounds.extents.y + 0.2f,
                _contorller.bounds.center.z),
            Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1f);

        return Physics.Raycast(groundRay, 0.3f);
    }

	public void StartRunning()
	{
		isRunning = true;
		_anim.SetTrigger ("StartRunnig");
	}

	private void _Crash()
	{
		_anim.SetTrigger ("Death");
		isRunning = false;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		switch (hit.gameObject.tag) {
		case "Obstacle":
			_Crash ();
			break;
		}
	}
}

