using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : PowerableStructure, Purchasable, Destructable {
	public static Sprite Sprite = Resources.Load<Sprite>("Textures/Structures/Miner");
	public override Sprite sprite { get { return Miner.Sprite;}}

	public int price { get { return 20;}}
	public override int polutionGenerated { get { return 5;}}

	private float hp = 100f;
	public float maxHealth { get { return 100f; } }
	public float health { get { return hp;} set {hp = value;}}

	public int moneyGenerated = 1;

	public override Dictionary<string,string> details { 
		get { 
			return new Dictionary<string, string>()
			{
				{ "Location", this.coords.ToString()},
				{ "Health", this.health.ToString("###")},
				{ "Income", this.moneyGenerated.ToString()},
				{ "Pollution", (this.polutionGenerated * PollutionHandler.multiplier).ToString("##.###")}
			};
		}
	}

	protected override void OnPowered ()
	{
		MoneyHandler.getInstance ().addMoney (moneyGenerated);
	}
}
