using UnityEngine;
using System.Collections;

public class MainScreensScriptScaling : MonoBehaviour {
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
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), titleScreen, ScaleMode.StretchToFill);
		} else if (gameState == GameState.Win){
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), winScreen, ScaleMode.StretchToFill);
		} else if (gameState == GameState.Lose){
			GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), loseScreen, ScaleMode.StretchToFill);
		}
	}
}
