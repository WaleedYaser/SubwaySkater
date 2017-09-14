using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
	private const float DEADZONE = 100.0f;

	public static MobileInput Instance { set; get; }

	private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
	private Vector2 swipeDelta, startTouch;

	public bool Tap { get { return tap; } }

	public bool SwipeLeft { get { return swipeLeft; } }

	public bool SwipeRight { get { return swipeRight; } }

	public bool SwipeUp { get { return swipeUp; } }

	public bool SwipeDown { get { return swipeDown; } }


	private void Awake ()
	{
		Instance = this;
	}

	private void Update ()
	{
		// Reset all the booleans
		tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

		// Check for inputs
		#region Standalone Inputs
		if (Input.GetMouseButtonDown (0)) {
			tap = true;
			startTouch = (Vector2) Input.mousePosition;
		} else if (Input.GetMouseButtonUp (0)) {
			startTouch = swipeDelta = Vector2.zero;
		}
		#endregion

		#region Mobile Inputs
		if (Input.touches.Length != 0) {
			if (Input.touches [0].phase == TouchPhase.Began) {
				tap = true;
				startTouch = Input.touches[0].position;
			} else if (Input.touches [0].phase == TouchPhase.Ended || Input.touches [0].phase == TouchPhase.Canceled) {
				startTouch = swipeDelta = Vector2.zero;
			}
		}
		#endregion

		// Calculate distance
		swipeDelta = Vector2.zero;
		if (startTouch != Vector2.zero) {

			// Check standalone
			if (Input.GetMouseButton(0)) {
				swipeDelta = (Vector2) Input.mousePosition - startTouch;
			}
		
			// Check mobile
			if (Input.touches.Length != 0) {
				swipeDelta = Input.touches [0].position - startTouch;
			}

			// Check if byond deadzone
			if (swipeDelta.magnitude > DEADZONE) {
				// Complete swipe
				float x = swipeDelta.x;
				float y = swipeDelta.y;

				if (Mathf.Abs (x) > Mathf.Abs (y)) {
					// Horizontal
					if (x < 0) {
						swipeLeft = true;
					} else {
						swipeRight = true;
					}
				} else {
					// Vertical
					if (y < 0) {
						swipeDown = true;
					} else {
						swipeUp = true;
					}
				}

				startTouch = swipeDelta = Vector2.zero;
			}
		}

	}
}
