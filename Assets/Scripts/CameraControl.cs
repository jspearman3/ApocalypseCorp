using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	private Camera cam;
	private Transform trans;
	public float minSize;
	public float maxSize;

	public TiledMap map;

	private float targetSize = 3.5f;

	public float scrollSensitivity = .5f;
	public float scrollLerpVal = 100f;
	public float lateralSpeed = .05f;
	public float screenEdgeMovementCutoff = 20;

	public Vector2 topLeft;
	public Vector2 bottomRight;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
		targetSize = cam.orthographicSize;
		trans = GetComponent<Transform> ();
		UnityEngine.Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Update is called once per frame
	void Update () {
		HandleScroll ();
		HandleLateral ();
		HandleWorldConstraint ();
	}
	void HandleWorldConstraint() {

		if (trans.position.x - topLeft.x < 0) {
			trans.position = new Vector3 (topLeft.x, trans.position.y, trans.position.z);
		}

		if (trans.position.y - topLeft.y < 0) {
			trans.position = new Vector3 (trans.position.x, topLeft.y, trans.position.z);
		}

		if (trans.position.x - bottomRight.x > 0) {
			trans.position = new Vector3 (bottomRight.x, trans.position.y, trans.position.z);
		}

		if (trans.position.y - bottomRight.y > 0) {
			trans.position = new Vector3 (trans.position.x, bottomRight.y, trans.position.z);
		}


	}

	void HandleLateral() {
		Vector3 diffVector = new Vector3 ();
		float adjustedLateralSpeed = lateralSpeed * cam.orthographicSize;
		if (Input.mousePosition.x < screenEdgeMovementCutoff) {
			diffVector += new Vector3 (-adjustedLateralSpeed, 0, 0);
		}
		if (Input.mousePosition.x > Screen.width - screenEdgeMovementCutoff) {
			diffVector += new Vector3 (adjustedLateralSpeed, 0,0);
		}
		if (Input.mousePosition.y < screenEdgeMovementCutoff) {
			diffVector += new Vector3 (0, -adjustedLateralSpeed,0);
		}
		if (Input.mousePosition.y > Screen.height - screenEdgeMovementCutoff) {
			diffVector += new Vector3 (0, adjustedLateralSpeed,0);
		}
		trans.position += diffVector * Time.deltaTime;
	}

	void HandleScroll() {
		targetSize -= Input.mouseScrollDelta.y * scrollSensitivity;
		if (targetSize <= minSize) {
			targetSize = minSize;
		} else if (targetSize >= maxSize) {
			targetSize = maxSize;
		}
		cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, targetSize, scrollLerpVal);
	}
}
