using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyHandler : MonoBehaviour {
	public static MoneyHandler getInstance() {
		return GameObject.Find ("Money").GetComponent<MoneyHandler> ();
	}

	public TiledMap map;
	public Text moneyText;

	private int money { get; set; }
	// Use this for initialization
	void Start () {
		moneyText = GetComponent<Text> ();
		money = 100;
	}
	
	// Update is called once per frame
	void Update () {
		moneyText.text = "Cash: $" + money;
	}

	public void addMoney (int income) {
		money += income;
	}

	public bool payMoney (int cost) {
		bool success = cost <= money;

		if (success)
			money -= cost;
		return success;
	}

	public bool buyStructure (Structure s, MapCoords coords) {
		if (!(map.mapData.getStructure (coords) is EmptyTile))
			return false;

		if (!(s is Purchasable)) {
			Debug.Log (s.ToString () + " is not purchasable!");
			return false;
		}
		Purchasable p = s as Purchasable;
		bool success = payMoney (p.price);

		if (success)
			map.updateStructureTile (s, coords);
		Debug.Log (s.ToString () + " purchased: " + success);
		return success;
	}
}
