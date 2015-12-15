using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour 
{
	//Useful objects
	public Transform Sun;

	//Gameflow Variables
	public float turnTime;
	private float lastTurnTime;

	//Loading variables
	public Texture2D loadingMenu;
	Rect loadingWindow = new Rect (0, 0, Screen.width, Screen.height);
	bool go;
	bool loaded;
	bool turnFinished;
	private int loadingProgress;

	private string[] loadingMessage = new string[]
	{
		"Imaging Microbes",
		"Devising Devious Concoctions",
		"Gathering critters from home",
		"Organizing Otherworldly Blueprints",
		"Perusing Geological Blueprints",
		"Architecturalizing landscapes"
	};

	private string[] updatingMessage = new string[]
	{
		"",
		"OMs are NOMing",
		"Compensating for lack of food"
	};

	void Start ()
	{
		loaded = false;
		go = false;
		StartCoroutine(LoadScene ());
	}

	//Control the game flow from here
	/*************************************
	 * Current Flow:
	 * run for turnTime secs
	 * pause the game
	 * evolve species
	 * show even window
	 * *********************************/
	void Update () 
	{			
		if((Time.time - lastTurnTime) > turnTime)
		{
			Control.Pause();
			StartCoroutine(endTurn());
		}
	}

	void OnGUI()
	{
		//Set the skin
		GUI.skin = GUIMain.infoSkin;
		if(!Control.loading)GUI.Label(new Rect((Screen.width/2)-60,0,120,20),"Turn Timer: " + (turnTime - (Time.time - lastTurnTime)).ToString("F2"));
		else GUI.Label(new Rect((Screen.width/2)-60,0,120,20),updatingMessage[loadingProgress]);

		//if loading siplay the window
		if(!go)
		{
			GUI.skin = GUIMain.editorSkin;
			loadingWindow = GUI.Window(0,loadingWindow,loadingDisplay,"Loading");
			Control.windowOpen = true;
		}
	}

	private IEnumerator endTurn()
	{
		loadingProgress = 1;
		Control.loading = true;
		yield return StartCoroutine (updatePlanets ());
		loadingProgress = 2;
		yield return StartCoroutine (evolveCreatures());
		lastTurnTime = Time.time;
		loaded = true;
		Control.loading = false;
		loadingProgress = 0;
	}

	
	//update biomes here
	private IEnumerator updatePlanets()
	{
		foreach(Transform planet in SolarSystemDisplay.planets)
		{
			yield return StartCoroutine(SolarSystemDisplay.planetClass[planet].turnUpdate());
		}
	}
	
	//TODO evolve the creature here
	private IEnumerator evolveCreatures()
	{
		yield return null;
	}

	
	public void loadingDisplay(int ID)
	{
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),loadingMenu);
		if(loaded)
		{
			if(GUI.Button(new Rect(Screen.width/2-120,Screen.height-240,240,40),"GO"))
			{
				Control.windowOpen = false;
				go = true;
				//Show GE window
				//GUIMain.windowDisplay [4] = true;
			}
		}
		else
		{
			GUI.Label(new Rect(Screen.width/2-240,Screen.height-240,480,40),loadingMessage[loadingProgress]);
		}
	}

	private IEnumerator LoadScene()
	{
		/*****************************
		 * Load things
		 * ******************************/
		//TODO delete this... it's solely for testing purposes
		//load cell textures
		Control.Pause ();
		CellTextures.initTextures();
		loadingProgress = 0;
		yield return null;
		//initialize the windows
		Recipes.initRecipes();
		loadingProgress = 1;
		yield return null;
		//load default cells
		loadingProgress = 2;
		yield return null;
		//initialize the Bacteria GE grid
		loadingProgress = 3;
		yield return null;

		//Load the biome types
		BiomeTypes.init ();
		loadingProgress = 4;
		yield return null;

		//Create a new solar system
		SolarSystemDisplay.Init (Sun);
		loadingProgress = 5;
		loaded = true;
	}
}
