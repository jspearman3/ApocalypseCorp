using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTornado : MonoBehaviour {

	private RectTransform rect;
	public float rotationSpeed = 1f;
	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		rect.RotateAround (Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
