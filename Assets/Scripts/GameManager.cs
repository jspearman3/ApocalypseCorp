using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public bool gameover = false;
	public GameObject loseScreen;
	public Text loseText;
	public PollutionHandler pollutionHandler;
	public void LoseGame() {
		gameover = true;
		loseScreen.SetActive (true);
		loseText.text = pollutionHandler.getPollutionString (); 
	}

	public void RestartGame() {
		SceneManager.LoadScene ("Main");
	}

	public void BackToMainMenu() {
		SceneManager.LoadScene ("TitleScreen");
	}
}
