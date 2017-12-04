using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surger : Structure, Purchasable, Destructable, Updateable, PowerCarrier {
	public static Sprite Sprite = Resources.Load<Sprite>("Textures/Structures/Surger");
	public override Sprite sprite { get { return Surger.Sprite;}}
	public int polutionGenerated { get { return 10;}}
	public int price { get { return 1000;}}
	private float hp = 100f;
	public float maxHealth { get { return 100f; } }
	public float health { get { return hp;} set {hp = value;}}

	public float cooldown { get { return 2;}}
	private float cooldownTimer = 2;

	public override Dictionary<string,string> details { 
		get { 
			return new Dictionary<string, string>()
			{
				{ "Location", this.coords.ToString()},
				{ "Health", this.health.ToString("###")},
				{ "Cooldown", this.cooldown.ToString("#.#s")},
				{ "Pollution", (this.polutionGenerated * PollutionHandler.multiplier).ToString("##.###")}
			};
		}
	}

	public void OnUpdate() {
		if (checkAndUpdateCooldown ()) {
			PowerHandler.getInstance ().StartSurge (this.coords, 100);
			PollutionHandler.getInstance ().pollute (polutionGenerated);
		}
	}

	private bool checkAndUpdateCooldown() {
		cooldownTimer -= Time.deltaTime;
		if (cooldownTimer <= 0) {
			cooldownTimer = cooldown;
			return true;
		}
		return false;
	}
		
}
