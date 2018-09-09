using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float orbitSpeed = 1.0f;
	public GameObject focusObject;
	public Camera gameCamera;
	private Vector3 focusObjectPosition;

	Vector3 lastMovement;
	Vector3 deltaMovement;
	Vector3 originPosition;

	void Start () {
		focusObjectPosition = focusObject.transform.position;
		gameCamera.transform.LookAt (focusObject.transform);
	}

	// Update is called once per frame
	void Update () {
		deltaMovement = Input.mousePosition - lastMovement;

		if (Input.GetMouseButton (1)) {
			transform.RotateAround (focusObjectPosition, Vector3.up, deltaMovement.x * orbitSpeed * Time.deltaTime);
			transform.RotateAround (focusObjectPosition, transform.TransformDirection (Vector3.left), deltaMovement.y * orbitSpeed * Time.deltaTime);
		}

		transform.RotateAround (focusObjectPosition, Vector3.up, 0 * Time.deltaTime);
		lastMovement = Input.mousePosition;
	}
}
