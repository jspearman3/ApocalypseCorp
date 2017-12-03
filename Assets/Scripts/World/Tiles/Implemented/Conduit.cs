using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduit : Structure, PowerCarrier, Purchasable, Destructable, Updateable {
	public static Sprite Sprite = Resources.Load<Sprite>("Textures/Structures/Conduit");
	public override Sprite sprite { get { return Conduit.Sprite;}}
	public override Dictionary<string,string> details { 
		get { 
			return new Dictionary<string, string>()
			{
				{ "Location", this.coords.ToString()},
				{ "Health", this.health.ToString("###")}
			};
		}
	}

	public int price { get { return 10;}}

	private float hp = 100f;
	public float maxHealth { get { return 100f; } }
	public float health { get { return hp;} set {hp = value;}}

	public float surgeIntensity = 0;
	public float maxSurgeIntensity = 1;
	public float surgeDecayMultiplier = 3;

	public void startSurge() {
		surgeIntensity = maxSurgeIntensity;
	}

	public void OnUpdate() {
		if (this.gameObject == null)
			return;
		SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer> ();

		if (sr == null)
			return;

		surgeIntensity -= surgeDecayMultiplier * Time.deltaTime;

		if (surgeIntensity <= 0)
			surgeIntensity = 0;

		float blueVal = (1 - surgeIntensity) * sr.color.b;
		sr.color = new Color (sr.color.r, sr.color.b, blueVal);

	}

}
