﻿using UnityEngine;
using System.Collections;

public class MainScreensScript : MonoBehaviour {
	enum GameState {Title, Game, Win, Lose};
	GameState gameState = GameState.Title;

	public Texture2D winScreen;
	public Texture2D loseScreen;
	public Texture2D titleScreen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetGameState(int x){
		if(x == 0){
			GameState gameState = GameState.Title;
		} else if(x == 1){
			GameState gameState = GameState.Game;
		} else if(x == 2){
			GameState gameState = GameState.Win;
		} else if(x == 3){
			GameState gameState = GameState.Lose;
		}
	}

	void OnGUI(){
		if (gameState == GameState.Title){
			GUI.Label(new Rect(0,0, Screen.width, Screen.height), titleScreen);
		} else if (gameState == GameState.Win){
			GUI.Label(new Rect(0,0, Screen.width, Screen.height), winScreen);
		} else if (gameState == GameState.Lose){
			GUI.Label(new Rect(0,0, Screen.width, Screen.height), loseScreen);
		}
	}
}