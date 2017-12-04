using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
	private int index = 0;
	public Sprite[] sprites;
	public Image image;

	public void startTutorial() {
		this.gameObject.SetActive (true);
		index = 0;
		advance ();
	}

	public void advance() {
		if (index >= sprites.Length) {
			this.gameObject.SetActive (false);
			index = 0;
			return;
		}
		image.sprite = sprites [index];
		index++;
	}
}
