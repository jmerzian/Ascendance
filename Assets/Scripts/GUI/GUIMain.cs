using UnityEngine;
using System.Collections.Generic;

public class GUIMain : MonoBehaviour
{
	//textures for the end and in between of resource bars
	public Texture barBase;
	public Texture barBaseEnd;
	
	private int ScreenWidth;
	private int ScreenHeight;
	
	// window controls
	private Rect[] informationWindows = new Rect[]{new Rect(),new Rect(),new Rect(),new Rect(),new Rect(),new Rect(), new Rect()};
	
	//which windows are open
	public static bool[] windowDisplay = new bool[]{false,false,false,false,false,false,false};
	private string[] windowName = new string[]{"Sentients","Animals","Plants","Bacterium"};

	//bool to resume game after closing windows
	private bool resumeOnClose = false;

	public GUISkin editorSkinHolder;
	public static GUISkin editorSkin;

	public GUISkin infoSkinHolder;
	public static GUISkin infoSkin;

	public void Awake()
	{
		infoSkin = infoSkinHolder;
		editorSkin = editorSkinHolder;
	}

	public void Start()
	{
		//get the current screen dimensions so that everything can be placed properly
		ScreenHeight = Screen.height;
		ScreenWidth = Screen.width;

		//Set the default size and position of the pop up windows
		for(int i = 0; i < informationWindows.Length; i++)
		{
			informationWindows[i] = new Rect(0,0,ScreenWidth-180,ScreenHeight-24);
		}
		//set the genetic engineering windows to be a little bit differend
		informationWindows [4] = new Rect (0, 0, ScreenWidth, ScreenHeight);
		informationWindows [5] = new Rect (0, 0, ScreenWidth, ScreenHeight);
		informationWindows [6] = new Rect (0, 0, ScreenWidth, ScreenHeight);
	}

	public void OnGUI()
	{
		//Set the skin
		GUI.skin = GUIMain.infoSkin;

		//Update the current screen dimensions so that everything can be placed properly
		ScreenHeight = Screen.height;
		ScreenWidth = Screen.width;
		
		/**********************************************
		* Resource and planet displays
		* **********************************************/
		//Display the universal resources Faith and Souls
		Color[] resourceColors = new Color[]{Color.yellow,Color.magenta};
		
		for(int i = 0; i < Control.universalResourceNames.Count; i++)
		{
			ResourceBar(Control.universalResources[Control.universalResourceNames[i]], 
			            100, new Rect(0,24*i+8,125,16),resourceColors[i], Control.universalResourceNames[i]);
		}

		//if the biome is to be displayed display all of the biome information in a clear concise beautiful way
		if(GUIDisplay.printBiome)
		{
			if(GUIDisplay.printedBiome != null)BiomeInformation(GUIDisplay.printedBiome);
		}
		//if the planet is to be displayed display all of the planet information in a clear concise beautiful way
		else if(GUIDisplay.printPlanet)
		{
			if(GUIDisplay.printedPlanet != null)PlanetInformation(GUIDisplay.printedPlanet);
		}
		/**********************************************************
		 * Solar system display allowing changing of planet focus
		 * ********************************************************/
		SolarSystemDisplay.DisplaySystem();
		
		/**********************************************************
		* Buttons to open creatue info windows
		* ********************************************************/
		if(GUIDisplay.printBiome || GUIDisplay.printPlanet)
		{
			GUI.BeginGroup(new Rect(ScreenWidth - 120, (Control.gasses.Count+1)*20+(Control.minerals.Count+1)*20,120,250));
			for(int i = 0; i < 4; i++)
			{
				windowDisplay[i] = GUI.Toggle (new Rect (0,20*i, 120, 20), windowDisplay[i], windowName[i]);
				if(windowDisplay[i])
				{
					//informationWindows[i] = GUI.Window(i,informationWindows[i],CreatureWindow.WindowInformation,windowName[i]);
					Control.windowOpen = true;
					for(int j = 0; j < 4; j++)
					{
						if(i != j) windowDisplay[j] = false;
					}
				}
				else Control.windowOpen = false;
			}
			GUI.EndGroup();
		}

	/***********************************************************
	 * Genetic Engineering
	 * ********************************************************/
		GUI.Label(new Rect(Screen.width-120, Screen.height-120,120,30),"Engineering");

		windowDisplay[4] = GUI.Toggle (new Rect(ScreenWidth-120, ScreenHeight-90, 120,30),windowDisplay[4], "Genetic");
		if(windowDisplay[4])
		{
			//Change the GUI skin to the editor
			GUI.skin = GUIMain.editorSkin;
			//informationWindows[4] = GUI.ModalWindow(4,informationWindows[4],GeneticEngineering.windowDisplay,"Genetic Engineering");
			resumeOnClose = true;
		}
		else if(resumeOnClose)
		{
			resumeOnClose = false;
			Control.Resume();
		}

	/***********************************************************
	 * General Useful Information
	 * *********************************************************/
		if (Control.paused) GUI.Label (new Rect (ScreenWidth / 2 - 120, 0, 240, 60), "PAUSED");
	}
	/*****************************************************************
	* Resource bar displays a recource in x number of blocks
	* ***********************************************************/
	
	private void ResourceBar(float resourceCurrent, float resourceMax, Rect position, Color color)
	{
		int i;
		
		//calculate number of sections to fit into required size
		int numOfSections = (int)Mathf.Ceil(position.width / 16);
		float numPerSection = resourceMax / numOfSections;
		int displayedSections = (int)Mathf.Ceil(resourceCurrent / numPerSection);
		
		GUI.color = color;
		
		for(i = 0; i < displayedSections; i++)
		{
			GUI.DrawTexture(new Rect(position.x + 16*i + 1, position.y, 16, 16),barBase);
		}
		GUI.DrawTexture(new Rect(position.x + 16*i + 1, position.y, 16, 16),barBaseEnd);
		
		GUI.color = Color.white;
	}
	
	private void ResourceBar(float resourceCurrent, float resourceMax, Rect position, Color color, string name)
	{
		ResourceBar (resourceCurrent, resourceMax, position, color);
		GUI.Label (new Rect(position.x,position.y-4,position.width,32), name + " : " + resourceCurrent);
	}
	
	/********************************************************************************
	 * General planet and biome information
	 * TODO print out resource quantities in resource blocks
	 * *****************************************************************************/
	private void PlanetInformation(Planet selectedPlanet)
	{
		GUI.BeginGroup(new Rect(ScreenWidth-120,0,120,(Control.gasses.Count+1)*20));
		if(selectedPlanet.resources.hasAtmosphere())
		{
			GUI.Box (new Rect(0,0,120,120),selectedPlanet.gameObject.name);

			//iterate over all gasses
			int i = 0;
			foreach(string gas in Control.gasses)
			{
				GUI.Label(new Rect(0,16*i+16,120,20),gas + ": " + selectedPlanet.resources.gasQuantities[gas].ToString("F2")+"M");
				i++;
			}
		}
		else GUI.Box (new Rect(0,0,120,120),selectedPlanet.gameObject.name + "\nHas no atmosphere.");
		GUI.EndGroup ();
	}

	private void BiomeInformation(Biome selectedBiome)
	{
		PlanetInformation (GUIDisplay.printedPlanet);
		//water, ammonia, calcium, Iron, Sulfur,Uranium
		GUI.BeginGroup (new Rect (ScreenWidth - 120, (Control.gasses.Count+1)*20, 120, (Control.minerals.Count+1)*20));
		GUI.Box (new Rect (0, 0, 120, 120), selectedBiome.type);

		//iterate over all minerals
		int i = 0;
		foreach(string mineral in Control.minerals)
		{
			GUI.Label(new Rect(0,16*i+16,120,20),mineral + ": " + selectedBiome.resources.mineralQuantities[mineral].ToString("F2")+"M");
			i++;
		}
		GUI.EndGroup ();
	}
}

public static class GUIDisplay
{
	//whether or not to display the biome or planet information
	public static bool printBiome;
	public static bool printPlanet;
	
	//which biome or planet to print
	public static Biome printedBiome;
	public static Planet printedPlanet;
}