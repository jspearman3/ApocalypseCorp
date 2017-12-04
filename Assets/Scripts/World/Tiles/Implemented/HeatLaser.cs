using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeatLaser : ChargableStructure, Purchasable, Destructable, Updateable {
	public static Sprite Sprite = Resources.Load<Sprite>("Textures/Structures/HeatLaser");
	public static GameObject LaserPrefab = Resources.Load<GameObject>("Prefabs/Laser");
	public override Sprite sprite { get { return HeatLaser.Sprite;}}

	public int price { get { return 500;}}
	private float hp = 100f;
	public float maxHealth { get { return 100f; } }
	public override float maxPower { get { return 100f; } }
	public override float powerGainPerSurge { get { return 2f; } }
	public float shotPowerCost { get { return 20f; } }
	public float health { get { return hp;} set {hp = value;}}

	public float range { get { return 3;}}
	private float damage = .1f;
	public float cooldown { get { return 2;}}
	private float cooldownTimer = 2;

	public override Dictionary<string,string> details { 
		get { 
			return new Dictionary<string, string>()
			{
				{ "Location", this.coords.ToString()},
				{ "Health", this.health.ToString("###")},
				{ "Power", this.power.ToString("##0")}
			};
		}
	}

	public void OnUpdate() {
		IEnumerable<Tornado> ts = GameObject.FindGameObjectsWithTag ("Tornado").Select(g => g.GetComponent<Tornado>());
		if (checkAndUpdateCooldown () && ts.Count() != 0)
			fireAtInRangeTornado (ts);
	}

	private bool checkAndUpdateCooldown() {
		cooldownTimer -= Time.deltaTime;
		if (cooldownTimer <= 0) {
			cooldownTimer = 0;
			return true;
		}
		return false;
	}

	private void fireAtInRangeTornado(IEnumerable<Tornado> ts) {
		if (this.gameObject == null)
			return;
		foreach (Tornado t in ts) {
			float distanceAway = (t.transform.position - this.gameObject.transform.position).magnitude;
			if (distanceAway < range) {
				fireAtTornado (t);
				return;
			}
		}
	}

	private void fireAtTornado(Tornado t) {
		bool success = this.UsePower (shotPowerCost);
		if (success) {
			GameObject laserObj = GameObject.Instantiate (HeatLaser.LaserPrefab, this.gameObject.transform.position, Quaternion.identity);
			Laser laser = laserObj.GetComponent<Laser> ();
			laser.start = this.gameObject.transform.position;
			laser.stop = t.transform.position;
			t.intensity -= damage;
			cooldownTimer = cooldown;
		}
	}

}
