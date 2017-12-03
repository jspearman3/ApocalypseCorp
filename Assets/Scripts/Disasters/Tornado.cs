using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour {
	public float intensity = 1;
	public float spinSpeed = 1;
	public float damageMultiplier = 1;
	public float decayMultiplier = 1;
	public float minIntensity = 1;

	public Vector2 velocity;

	private Transform trans;
	private SpriteRenderer spriteRenderer;
	private TiledMap map;

	// Use this for initialization
	void Start () {
		trans = GetComponent<Transform> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		map = TiledMap.getInstance ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		trans.Rotate (Vector3.forward, spinSpeed * Time.deltaTime);
		trans.localScale = new Vector2 (1, 1) * intensity;
		DealDamage ();
		if (intensity <= minIntensity)
			GameObject.Destroy (this.gameObject);

		Vector2 mapCenter = map.topLeftPoint + new Vector2 (map.gameWidth / 2, -map.gameHeight / 2);
		float distFromCenter = (new Vector2(trans.position.x, trans.position.y) - mapCenter).magnitude;
		if (distFromCenter > map.gameWidth)
			GameObject.Destroy (this.gameObject);
	}

	private void Move() {
		Vector2 velocityChange = velocity * Time.deltaTime;
		trans.position = new Vector2(trans.position.x + velocityChange.x, trans.position.y + velocityChange.y);
	}

	private List<Structure> getAffectedStructures() {
		float damageRadius = spriteRenderer.bounds.extents.x;
		int maxTileXDamageLength = (int) System.Math.Ceiling (damageRadius / map.tileXLength);
		int maxTileYDamageLength = (int) System.Math.Ceiling (damageRadius / map.tileYLength);

		MapCoords location = map.getMapCoordsFromWorldCoords (trans.position);
		List<Structure> structures = new List<Structure> ();

		if (location == null)
			return structures;

		for (int i = -maxTileXDamageLength; i <= maxTileXDamageLength; i++) {
			for (int j = -maxTileYDamageLength; j <= maxTileYDamageLength; j++) {
				Structure s = map.mapData.getStructure (location.add (i, j));
				if (s is Destructable && s.gameObject != null) {
					Vector2 diffVector = s.gameObject.transform.position - trans.position;
					if (diffVector.magnitude < damageRadius) {
						structures.Add (s);
					} else {
						Vector2 furthestPoint = diffVector.normalized * damageRadius;
						if (s.gameObject.GetComponent<StructureObject>().spriteRenderer != null && s.gameObject.GetComponent<StructureObject>().spriteRenderer.bounds.Contains(furthestPoint))
							structures.Add (s);
					}
				}
					
			}
		}
		return structures;
	}

	private void DealDamage() {
		float totalDamageDone = 0;
		foreach (Structure s in getAffectedStructures()) {
			Destructable d = s as Destructable;
			if (d != null && s.gameObject != null) {
				float distanceAway = (s.gameObject.transform.position - trans.position).magnitude;
				float damageDone = (damageMultiplier / distanceAway) * intensity * Time.deltaTime;
				d.health -= damageDone;
				totalDamageDone += damageDone;
			}
		}
		intensity -= decayMultiplier * totalDamageDone * Time.deltaTime;
	}
}
