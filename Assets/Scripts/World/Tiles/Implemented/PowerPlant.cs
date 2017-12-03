using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Structure, PowerCarrier, Destructable {
	public static Sprite Sprite = Resources.Load<Sprite>("Textures/Structures/PowerPlant");
	public override Sprite sprite { get { return PowerPlant.Sprite;}}

	public int powerRange { get { return 100;}}

	private float hp = 100f;
	public float maxHealth { get { return 100f; } }
	public float health { get { return hp;} set {hp = value;}}

	public override Dictionary<string,string> details { 
		get { 
			return new Dictionary<string, string>()
			{
				{ "Location", this.coords.ToString()},
				{ "Health", this.health.ToString("###")},
				{ "Range", this.powerRange.ToString()}
			};
		}
	}
}
