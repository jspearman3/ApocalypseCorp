using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public abstract class Structure {
	public static int tileWidthInPixels = 16;
	public abstract Sprite sprite { get; }
	public abstract Dictionary<string, string> details { get; }
	public MapCoords coords;
	public GameObject gameObject;

	public Structure(MapCoords coords) {
		this.coords = coords;
	}

	public Structure() {}
}
