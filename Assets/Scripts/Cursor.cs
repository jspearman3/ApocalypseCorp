using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour {
	public Selector selector;
	public Image structImage;
	public Sprite transparent;
	public MoneyHandler moneyHandler;
	public PollutionHandler pollutionHandler;
	public Text conduitMoneyText;
	public Text minerMoneyText;
	public Text heatLaserMoneyText;
	public GameManager game;

	public Structure structureToPlace;

	private EventSystem eventSystem;
	private PowerHandler powerHandler;
	private Transform trans;
	void Start() {
		powerHandler = PowerHandler.getInstance ();
		trans = GetComponent<Transform> ();
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		conduitMoneyText.text = "$" + (new Conduit ()).price;
		minerMoneyText.text = "$" + (new Miner ()).price;
		heatLaserMoneyText.text = "$" + (new HeatLaser ()).price;
	}

	void Update() {
		if (game.gameover)
			return;

		if (structureToPlace == null) {
			structImage.sprite = transparent;
		} else {
			structImage.sprite = structureToPlace.sprite;
		}

		trans.position = Input.mousePosition;
		if (Input.GetMouseButtonDown (0))
			MouseRaycast ();
		if (Input.GetMouseButtonDown (1)) {
			structureToPlace = null;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
			AddConduitToPlace();

		if (Input.GetKeyDown(KeyCode.Alpha2))
			AddMinerToPlace();

		if (Input.GetKeyDown(KeyCode.Alpha3))
			AddHeatLaserToPlace();

	}

	void MouseRaycast() {
		if (eventSystem.IsPointerOverGameObject ())
			return;

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if (hit == null || hit.collider == null)
			return;

		if (hit.collider.gameObject.name == "WorldMap") {
			selector.Deselect ();
			TiledMap map = hit.collider.gameObject.GetComponent<TiledMap> ();
			handleTileClick (map.getMapCoordsFromWorldCoords (hit.centroid), map);
		}
	}

	void handleTileClick(MapCoords coords, TiledMap map) {
		Structure s = map.mapData.getStructure (coords);

		if (!(s is EmptyTile))
			selector.Select (coords);

		if (s is PowerPlant) {
			PowerPlant p = s as PowerPlant;
			//show current power grid
			int newPollution = powerHandler.StartSurge (coords, p.powerRange);
			pollutionHandler.pollute (newPollution);
		} else if (s is EmptyTile && structureToPlace != null) {
			if (moneyHandler.buyStructure (structureToPlace, coords))
				selector.Select (coords);
		}


		if (Input.GetKey (KeyCode.LeftShift)) {
			if (structureToPlace is Conduit) {
				structureToPlace = new Conduit ();
			} else if (structureToPlace is Miner) {
				structureToPlace = new Miner ();
			} else if (structureToPlace is HeatLaser) {
				structureToPlace = new HeatLaser ();
			} else {
				Debug.LogWarning ("COULD NOT RECOGNIZE TYPE " + structureToPlace);
				structureToPlace = null;
			}
		} else {
			structureToPlace = null;
		}
	}

	public void AddConduitToPlace() {
		structureToPlace = new Conduit();
	}

	public void AddMinerToPlace() {
		structureToPlace = new Miner();
	}

	public void AddHeatLaserToPlace() {
		structureToPlace = new HeatLaser();
	}
}
