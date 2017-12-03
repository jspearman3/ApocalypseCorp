using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureObject : MonoBehaviour {
	public Structure backingInfo;
	public SpriteRenderer spriteRenderer;

	public GameObject healthBar;
	public Transform healthRemainingBar;
	public GameObject powerBar;
	public Transform powerRemainingBar;

	public MapCoords coords {
		get {
			return TiledMap.getInstance ().getMapCoordsFromWorldCoords (transform.position);
		}
	}

	private TiledMap map;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		map = TiledMap.getInstance ();
	}

	void Update() {
		if (backingInfo is Destructable) {

			Destructable d = backingInfo as Destructable;
			updateHealthbar (d);
			float damageFraction = d.health / d.maxHealth;

			if (d.health <= 0) {
				map.removeStructure (this);
			}

			spriteRenderer.color = new Color (1, damageFraction, damageFraction);
		}

		if (backingInfo is Updateable) {
			Updateable u = backingInfo as Updateable;
			u.OnUpdate ();
		}

		if (backingInfo is ChargableStructure) {
			updatePowerbar (backingInfo as ChargableStructure);
		}
	}

	private void updateHealthbar(Destructable d) {
		healthRemainingBar.localScale = new Vector2 (d.health / d.maxHealth, healthRemainingBar.localScale.y);
		if (d.health < d.maxHealth) {
			healthBar.SetActive (true);
		} else {
			healthBar.SetActive (false);
		}
	}

	private void updatePowerbar(ChargableStructure c) {
		powerRemainingBar.localScale = new Vector2 (c.power / c.maxPower, healthRemainingBar.localScale.y);
		if (c.power < c.maxPower) {
			powerBar.SetActive (true);
		} else {
			powerBar.SetActive (false);
		}
	}
}
