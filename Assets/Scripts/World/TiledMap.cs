using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
public class TiledMap : MonoBehaviour {
	public static TiledMap getInstance() {
		return GameObject.Find ("WorldMap").GetComponent<TiledMap> ();
	}

	public static float TILE_LENGTH = 1.0f;

	public float gameWidth = 100;
	public float gameHeight = 100;

	public int numXTiles = 11;
	public int numYTiles = 11;

	public float tileXLength;
	public float tileYLength;

	public MapData mapData;
	public GameObject structurePrefab;

	private Transform trans;
	public Vector2 topLeftPoint;

	public PowerHandler powerHandler;
	public GameManager game;
	private SpriteRenderer backgroundRenderer;

	// Use this for initialization
	void Start () {
		tileXLength = ((float) gameWidth) / numXTiles;
		tileYLength = ((float) gameHeight) / numYTiles;

		backgroundRenderer = GetComponent<SpriteRenderer> ();
		trans = GetComponent<Transform> ();
		topLeftPoint = trans.position;

		BoxCollider2D coll = gameObject.AddComponent<BoxCollider2D> ();
		coll.size = new Vector2 (1f, 1f);
		coll.offset = new Vector2 (0.5f, -0.5f);
		coll.isTrigger = true;

		mapData = new MapData (numXTiles, numYTiles);
		BuildGameWorld ();
	}

	private void BuildGameWorld() {
		float bgWidth = backgroundRenderer.sprite.bounds.size.x;
		float bgHeight = backgroundRenderer.sprite.bounds.size.y;

		trans.localScale = new Vector2 (gameWidth / bgWidth, gameHeight / bgHeight);
		BuildStructures ();
	}

	private void BuildStructures() {
		for (int i = 0; i < mapData.tiles.GetLength(0); i++) {
			for (int j = 0; j < mapData.tiles.GetLength(1); j++) {
				Structure s = mapData.tiles [i, j];
				InstantiateStructureObject (new MapCoords (i, j), s);
			}
		}
			
	}

	private void InstantiateStructureObject(MapCoords coords, Structure s) {
		float xScaleFactor = tileXLength / structurePrefab.GetComponent<SpriteRenderer> ().sprite.bounds.size.x;
		float yScaleFactor = tileYLength / structurePrefab.GetComponent<SpriteRenderer> ().sprite.bounds.size.y;

		if (!(s is EmptyTile)) {
			Vector2 worldCoords = getWorldCoordsFromMapCoords (coords);
			GameObject sObj = GameObject.Instantiate (structurePrefab, worldCoords, Quaternion.identity);
			sObj.GetComponent<StructureObject> ().backingInfo = s;
			sObj.GetComponent<SpriteRenderer> ().sprite = s.sprite;
			sObj.transform.localScale = new Vector2 (xScaleFactor, yScaleFactor);
			s.gameObject = sObj;
		} else {
			if (s.gameObject != null) {
				GameObject.Destroy (s.gameObject);
			}
			s.gameObject = null;
		}
	}

	public MapCoords getMapCoordsFromWorldCoords(Vector2 worldCoords) {
		Vector2 diff = worldCoords - topLeftPoint;
		int mapX = (int) ((diff.x / gameWidth) * numXTiles);
		int mapY = (int) ((-diff.y / gameHeight) * numYTiles);
		return new MapCoords (mapX, mapY);
	}

	public Vector2 getWorldCoordsFromMapCoords(MapCoords coords) {
		float worldRelX = (coords.x + 0.5f) * tileXLength;
		float worldRelY = (coords.y + 0.5f) * -tileYLength;
		return new Vector2 (worldRelX, worldRelY) + topLeftPoint;
	}

	public Structure getTileFromWorldCoords(Vector2 worldCoords) {
		return mapData.getStructure (getMapCoordsFromWorldCoords(worldCoords));
	}

	public void updateStructureTile(Structure s, MapCoords coords) {

		Structure toAdd = s;
		if (s == null)
			toAdd = new EmptyTile();

		Structure toRemove = mapData.getStructure (coords);
		if (!(toRemove is EmptyTile)) {
			if (toRemove is PowerPlant)
				game.LoseGame ();
			GameObject.Destroy (toRemove.gameObject);
			toRemove.gameObject = null;
		}

		if (toRemove is PowerCarrier || s is PowerCarrier || toRemove is PowerableStructure || s is PowerableStructure)
			powerHandler.dirtyCache = true;

		mapData.setTile (coords.x, coords.y, toAdd);
		InstantiateStructureObject (coords, toAdd);
	}

	public void removeStructure(MapCoords coords) {
		updateStructureTile (new EmptyTile (), coords);
	}

	public void removeStructure(StructureObject obj) {
		if (obj == null)
			return;
		removeStructure (obj.coords);
	}
}
