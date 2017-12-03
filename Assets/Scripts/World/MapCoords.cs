using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCoords {

	public int x;
	public int y;

	public MapCoords(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public MapCoords add(int x=0, int y=0) {
		return new MapCoords (this.x + x, this.y + y);
	}

	public override bool Equals (object obj)
	{
		if (!(obj is MapCoords))
			return false;
		MapCoords other = obj as MapCoords;
		return other.x == this.x && other.y == this.y;
	}

	override
	public string ToString() {
		return "[" + x + ", " + y + "]";
	}

	public override int GetHashCode ()
	{
		return x * 1000 + y;
	}

}
