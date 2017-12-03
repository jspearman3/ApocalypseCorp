using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class MapData {
	
	public Structure[,] tiles;


	public MapData(int width, int height) {
		tiles = new Structure[width, height];
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new EmptyTile();
			}
		}

		int midX = width / 2;
		int midY = height / 2;

		setTile (midX, midY, new PowerPlant ());

		setTile (midX + 1, midY, new Conduit ());
		setTile (midX - 1, midY, new Conduit ());
		setTile (midX, midY + 1, new Conduit ());
		setTile (midX, midY - 1, new Conduit ());
		setTile (midX + 1, midY + 1, new Miner ());
		setTile (midX - 1, midY + 1, new Miner ());
		setTile (midX + 1, midY - 1, new HeatLaser ());
		setTile (midX - 1, midY - 1, new HeatLaser ());
	}

	public Structure getStructure(MapCoords coords) {
		return getStructure (coords.x, coords.y);
	}

	public Structure getStructure(int x, int y)
    {
		if (!validCoords(x, y))
        {
            return null;
        }

        return tiles[x, y];
    }

	private bool validCoords(int x, int y) {
		if (x >= tiles.GetLength(0) || y >= tiles.GetLength(1) || x < 0 || y < 0)
		{
			return false;
		}

		return true;
	}

	public void setTile(int x, int y, Structure tile)
    {
		if (!validCoords(x, y))
        {
			Debug.Log ("invalid coords. x, y: " + x + "," + y );
            return;
        }
		tile.coords = new MapCoords (x, y);
        tiles[x, y] = tile;
    }
}
