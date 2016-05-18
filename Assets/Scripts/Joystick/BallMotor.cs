using UnityEngine;
using System.Collections;

public class BallMotor : MonoBehaviour {

	public float moveSpeed = 20.0f;
	public float drag = 5.0f;
	public float terminalRotationSpeed = 25.0f;
	public Vector3 MoveVector{set;get;}
	public VirtuelJoystickImage joystick;
	private Rigidbody thisRigidbody;

	private void Start ()
	{
		thisRigidbody = gameObject.AddComponent<Rigidbody> ();
		thisRigidbody.maxAngularVelocity = terminalRotationSpeed;
		thisRigidbody.drag = drag;

	}

	private void Update () 
	{
		MoveVector = PoolInput ();
		Move();
	}

	private void Move ()
	{
		thisRigidbody.AddForce((MoveVector * moveSpeed));
	}

	private Vector3 PoolInput()
	{
		Vector3 dir = Vector3.zero;

		//dir.x = Input.GetAxis ("Horizontal");
		//dir.z = Input.GetAxis ("Vertical");

		dir.x = joystick.Horizontal ();
		dir.z = joystick.Vertical ();



		if (dir.magnitude > 1)
			dir.Normalize();
		return dir;
	}

}
