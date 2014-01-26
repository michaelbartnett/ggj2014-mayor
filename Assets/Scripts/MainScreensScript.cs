using UnityEngine;
using System.Collections;

public enum GameState { Title, Game, Win, Lose };

public class MainScreensScript : MonoBehaviour {
	private GameState gameState = GameState.Title;

	public Texture2D winScreen;
	public Texture2D loseScreen;
	public Texture2D titleScreen;

    public static MainScreensScript Instance { get; private set; }

	// Use this for initialization
	void Awake () {
        Instance = this;
	}

    void OnDestroy()
    {
        Instance = null;
    }
	
    public void SetGameState(GameState state)
    {
        this.gameState = state;
        //int x = (int)state;
        //if(x == 0){
        //    GameState gameState = GameState.Title;
        //} else if(x == 1){
        //    GameState gameState = GameState.Game;
        //} else if(x == 2){
        //    GameState gameState = GameState.Win;
        //} else if(x == 3){
        //    GameState gameState = GameState.Lose;
        //}
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
