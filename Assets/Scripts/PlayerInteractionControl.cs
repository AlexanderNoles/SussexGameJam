using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionControl : MonoBehaviour
{
	public static GameObject _instance;

	public float velSpeed = 5.0f;
	public float maxRotationSpeed = 50.0f;
	private float currentRotationSpeed;
	private float cachedRotationDirection;

	private float gravity;
	private const float maxGravity = 0.75f;
	private const float minGravity = 0.3f;

	private Rigidbody2D rb;
	private Vector2 velAdditionThisFrame;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		gravity = maxGravity;
		_instance = gameObject;
	}

	private void Update() 
	{
		//Grab the player input
		Vector2 playerInputThisFrame = GetPlayerOrthagonalInput();

		//Apply input to vel addition
		velAdditionThisFrame = playerInputThisFrame.normalized * velSpeed;

		if(velAdditionThisFrame.y > 0.0f)
		{
			gravity = Mathf.MoveTowards(gravity, minGravity, Time.deltaTime);
		}
		else
		{
			gravity = Mathf.MoveTowards(gravity, maxGravity, Time.deltaTime);
		}

		rb.gravityScale = gravity;

		//Rotational Control
		float rotationInputThisFrame = GetRotationInput();

		if(rotationInputThisFrame != 0.0f)
		{
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			//Increase current rotation speed
			currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, maxRotationSpeed, Time.deltaTime * 50.0f);

			//Cache rotational input
			cachedRotationDirection = rotationInputThisFrame;
		}
		else
		{
			rb.constraints = RigidbodyConstraints2D.None;
			//Reduce current rotation speed
			currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, 0.0f, Time.deltaTime * 100.0f);
		}

		if(currentRotationSpeed > 0.0f)
		{
			transform.Rotate(0.0f, 0.0f, cachedRotationDirection * currentRotationSpeed * Time.deltaTime);
		}
	}

	private void FixedUpdate() {
		rb.AddForce(velAdditionThisFrame);	
	}

	private Vector2 GetPlayerOrthagonalInput(){
		return RepairScreenManagement.GetOrthagonalInput();
	}

	private float GetRotationInput()
	{
		return RepairScreenManagement.GetRotationalInput();
	}
}