using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour {
	public PollutionHandler pollutionHandler;
	public TiledMap map;
	public GameObject tornadoPrefab;

	public float spawnChanceCooldown = 1;
	public float spawnChanceTimer = 1;

	public float disasterChanceMultiplier = 1;
	public float baseChance = 0.01f;

	public float spawnDistFromMap = 1f;
	public float tornadoVelocityMin = 1f;
	public float tornadoVelocityMax = 1f;
	public float tornadoDirectionDeviationMax = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		spawnChanceTimer -= Time.deltaTime;

		if (spawnChanceTimer <= 0) {
			spawnChanceTimer = spawnChanceCooldown;
			trySpawn ();
		}
	}

	private Vector2 getRandomSpawnPosition() {
		int side = Random.Range (0, 4); //0 and 1 are left and right sides. 2 and 3 are top and bottom sides
		float xVal;
		float yVal;
		if (side <= 1) {
			yVal = Random.Range (0f, -map.gameHeight);
			xVal = 0f;
			if (side == 0) {
				xVal = -spawnDistFromMap;
			} else {
				xVal = map.gameWidth + spawnDistFromMap;
			}
		} else {
			xVal = Random.Range (0f, map.gameWidth);
			yVal = 0f;
			if (side == 2) {
				yVal = spawnDistFromMap;
			} else {
				yVal = -(map.gameHeight + spawnDistFromMap);
			}
		}
		return new Vector2 (xVal, yVal) + map.topLeftPoint; 
	}

	private void trySpawn() {
		float pollutionFactor = pollutionHandler.pollution * disasterChanceMultiplier;
		float odds = baseChance + pollutionFactor;
		Debug.Log ("odds for tornado: " + odds);
		if (Random.Range (0f, 1) < odds) {
			Vector2 spawnPosition = getRandomSpawnPosition();
			Vector2 mapMiddle = map.topLeftPoint + new Vector2 (map.gameWidth / 2, -map.gameHeight / 2);
			Vector2 diff = mapMiddle - spawnPosition;;
			Vector2 velocity = Rotate(diff.normalized * Random.Range (tornadoVelocityMin, tornadoVelocityMax), Random.Range(-tornadoDirectionDeviationMax, tornadoDirectionDeviationMax));
			GameObject tornado = GameObject.Instantiate(tornadoPrefab, spawnPosition, Quaternion.identity);
			tornado.GetComponent<Tornado>().velocity = velocity;
		}
	}

	public static Vector2 Rotate(Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}
