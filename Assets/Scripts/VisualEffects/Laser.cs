using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	public float beamWidth = 100;

	public Vector2 start;
	public Vector2 stop;
	public float timeToLive = 1f;

	private Transform trans;
	private SpriteRenderer spriteRenderer;
	private AudioSource aud;


	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		aud = GetComponent<AudioSource> ();
		trans.position = start;

		Vector2 diffVector = stop - start;

		float beamLength = diffVector.magnitude / spriteRenderer.sprite.bounds.size.y;
		trans.localScale = new Vector2 (beamWidth, beamLength);

		//trans.LookAt (stop);
		transform.up = diffVector;


	}
	
	// Update is called once per frame
	void Update () {
		timeToLive -= Time.deltaTime;

		if (timeToLive <= 0)
			spriteRenderer.enabled = false;

		if (timeToLive <= 0 && !aud.isPlaying)
			GameObject.Destroy (this.gameObject);
	}
}
