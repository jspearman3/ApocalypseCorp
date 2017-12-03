using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {
	public Image selectedImage;
	public Text detailsText;
	public Sprite transparent;
	public Button destroyButton;
	public Text destroyButtonText;
	public TiledMap map;
	private MapCoords selected;
	public GameObject highlighter;
	public GameManager game;

	void Awake() {
		float xScaleFactor = (map.gameWidth / map.numXTiles) / highlighter.GetComponent<SpriteRenderer> ().bounds.size.x;
		float yScaleFactor = (map.gameHeight / map.numYTiles) / highlighter.GetComponent<SpriteRenderer> ().bounds.size.y;

		highlighter.transform.localScale = new Vector2 (xScaleFactor, yScaleFactor);
		highlighter.GetComponent<SpriteRenderer> ().enabled = false;
	}

	void Update() {
		if (game.gameover)
			return;

		if (selected != null)
			Select (selected);

		if (Input.GetMouseButtonDown (1)) {
			highlighter.GetComponent<SpriteRenderer>().enabled = false;
			destroyButton.enabled = false;
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			destroySelected ();
		}
	}

	public string formatDetails(Dictionary<string,string> details) {
		string baseString = "";
		foreach (string s in details.Keys) {
			string v = details[s];
			baseString += s + ":  " + v + "\n";
		}
		return baseString;
	}

	public void Select(MapCoords coords) {
		Structure s = map.mapData.getStructure (coords);
		if (s == null)
			return;
		highlighter.transform.position = map.getWorldCoordsFromMapCoords (coords);
		highlighter.GetComponent<SpriteRenderer>().enabled = true;
		destroyButton.enabled = true;
		destroyButton.image.enabled = true;
		destroyButton.GetComponentInChildren<Text> ().enabled = true;
		selected = coords;
		selectedImage.sprite = s.sprite;
		detailsText.enabled = true;
		detailsText.text = formatDetails(s.details);
		destroyButtonText.text = "(D) Destroy";
		if (s is PowerPlant) {
			destroyButtonText.text = "(D) Lose Game";
		}
	}

	public void Deselect() {
		highlighter.GetComponent<SpriteRenderer>().enabled = false;
		destroyButton.enabled = false;
		destroyButton.image.enabled = false;
		destroyButton.GetComponentInChildren<Text> ().enabled = false;
		selected = null;
		selectedImage.sprite = transparent;
		detailsText.enabled = false;
	}

	public void destroySelected() {
		if (selected == null)
			return;
		map.removeStructure (selected);
		Deselect ();
	}
}
