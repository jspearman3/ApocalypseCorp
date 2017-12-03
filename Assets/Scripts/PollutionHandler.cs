using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutionHandler : MonoBehaviour {

	public Text pollutionText;
	public static float multiplier = 0.0001f;

	public float pollution { get; set; }
	// Use this for initialization
	void Start () {
		pollutionText = GetComponent<Text> ();
		pollution = 0;
	}

	public string getPollutionString() {
		return pollution.ToString ("#0.000");
	}

	// Update is called once per frame
	void Update () {
		pollutionText.text = "Pollution: " + getPollutionString();
	}

	public void pollute (int amount) {
		pollution += amount * multiplier;
	}
}
