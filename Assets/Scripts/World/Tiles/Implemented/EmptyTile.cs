using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTile : Structure {
	public static Sprite Sprite = Resources.Load<Sprite>("Textures/transparent");
	public override Sprite sprite { get { return EmptyTile.Sprite;}}
	public override Dictionary<string,string> details { 
		get { 
			return new Dictionary<string, string> ();
		}
	}
}
