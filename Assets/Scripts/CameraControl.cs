using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	private Camera cam;
	public float minSize;

	public TiledMap map;

	private float targetSize = 3.5f;

	public float scrollSensitivity = .5f;
	public float scrollLerpVal = 100f;
	public float lateralSpeed = 1;
	public float screenEdgeMovementCutoff = 30;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
		UnityEngine.Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Update is called once per frame
	void Update () {
		HandleScroll ();
		HandleLateral ();
	}
	void HandleLateral() {
		Debug.Log (Input.mousePosition);
	}

	void HandleScroll() {
		targetSize -= Input.mouseScrollDelta.y * scrollSensitivity;
		if (targetSize <= minSize) {
			targetSize = minSize;
		}
		cam.orthographicSize = Mathf.Lerp (cam.orthographicSize, targetSize, scrollLerpVal);
	}
}
