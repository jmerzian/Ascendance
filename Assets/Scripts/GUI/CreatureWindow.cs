using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CreatureWindow
{
	private static Planet planet;
	private static Biome biome;

	//private static Bacteria displayedBacteria;

	private static Vector2[] scroll = new Vector2[10];
	private static int[] scrollLength = new int[10];

	public static void WindowInformation(int ID)
	{
		//Set the skin
		GUI.skin = GUIMain.infoSkin;

		planet = GUIDisplay.printedPlanet;
		biome = GUIDisplay.printedBiome;
		//use ID to switch what list is loaded
		switch(ID)
		{
			//sentient
		case 0:
			break;
			//animal
		case 1:
			break;
			//plant
		case 2:
			break;
			//Bacteria
		case 3:
			if(biome != null)
			{
				/*if(biome.life.bacteria.Count > 0)
				{
					scroll[0] = GUI.BeginScrollView(new Rect(0,40,240,Screen.height-80), scroll[0], new Rect(0, 0, 240,biome.life.bacteria.Count));
					int i = 0;
					foreach(KeyValuePair<string,Bacteria> bacteria in biome.life.bacteria)
					{
						GUI.DrawTexture(new Rect(0,30*i,30,30),bacteria.Value.sprite, ScaleMode.ScaleToFit);
						if(GUI.Button(new Rect(40,30*i,200,24),bacteria.Key)) displayedBacteria = bacteria.Value;
						i++;
					}
					GUI.EndScrollView();

					if(displayedBacteria != null)displayBacteria();
				}*/
				//else GUI.Label(new Rect(0,40,240,Screen.height-80), "This biome is devoid of life");
			}
			else if(planet != null)
			{
				if(planet.planetLife.Count > 0)
				{
					scroll[0] = GUI.BeginScrollView(new Rect(0,40,240,Screen.height-80), scroll[0], new Rect(0, 0, 240,planet.planetLife.Count ));
					int i = 0;
					foreach(KeyValuePair<int,LifeResources> life in planet.planetLife)
					{
						/*foreach(KeyValuePair<string,Bacteria> bacteria in life.Value.bacteria)
						{
							GUI.DrawTexture(new Rect(0,30*i,30,30),bacteria.Value.sprite, ScaleMode.ScaleToFit);
							if(GUI.Button(new Rect(40,30*i,200,24),bacteria.Key)) displayedBacteria = bacteria.Value;
							i++;
							//if(GUI.Button(new Rect(40,30*i,200,24),"Go to Biome")) GUIDisplay.printedBiome = planet.GetPlanetBiomes()[life.Key];
							i++;
						}*/
					}
					GUI.EndScrollView();
					
					//if(displayedBacteria != null)displayBacteria();
				}
				else GUI.Label(new Rect(0,40,240,Screen.height-80), "This Planet is devoid of life");
			}
			else
			{
				
			}
			break;
		}
		//allow the window to be resized
		GUI.DragWindow ();
	}

	private static void displayBacteria()
	{
		GUI.BeginGroup (new Rect(240,40,320,320));
		//Portrait
		//GUI.DrawTexture(new Rect(0,0,320,320),displayedBacteria.sprite);
		//GUI.Label (new Rect (0, 0, 120, 40),"x" + displayedBacteria.quantity.ToString());
		GUI.EndGroup ();

		GUI.BeginGroup (new Rect (480, 40, 240, Screen.height - 40));
		int i = 0;
		scroll[1] = GUI.BeginScrollView(new Rect(0,0,120,(Screen.width - 560)/2),scroll[1],new Rect(0,0,120,scrollLength[1]*40));
		GUI.Label (new Rect (0, 40*i, 120, 40), "Resources In");
		i++;
		/*foreach(KeyValuePair<string, float> resource in displayedBacteria.resourceIn)
		{
			if(resource.Value > 0)
			{
				GUI.Label(new Rect(0,40*i,120,40),resource.Key + ": " + resource.Value.ToString("F4"));
				i++;
			}
		}*/
		scrollLength [1] = i;
		GUI.EndScrollView();

		i = 0;
		scroll[2] = GUI.BeginScrollView(new Rect(0,(Screen.width - 560)/2,120,(Screen.width - 560)/2), scroll[2],new Rect(0,0,120,scrollLength[2]*40));
		GUI.Label (new Rect (0, 40*i, 120, 40), "Energy In");
		i++;
		/*foreach(KeyValuePair<string, float> resource in displayedBacteria.energyIn)
		{
			if(resource.Value > 0)
			{
				GUI.Label(new Rect(0,40*i,120,40),resource.Key + ": " + resource.Value.ToString("F4"));
				i++;
			}
		}
		scrollLength [2] = i;
		GUI.EndScrollView();

		i = 0;
		scroll[3] = GUI.BeginScrollView(new Rect(120,0,120,(Screen.width - 560)/2),scroll[3],new Rect(0,0,120,scrollLength[3]*40));
		GUI.Label (new Rect (0, 40*i, 120, 40), "Resources Out");
		i++;
		foreach(KeyValuePair<string, float> resource in displayedBacteria.resourceOut)
		{
			if(resource.Value > 0)
			{
				GUI.Label(new Rect(0,40*i,120,40),resource.Key + ": " + resource.Value.ToString("F4"));
				i++;
			}
		}
		scrollLength [3] = i;
		GUI.EndScrollView();

		i = 0;
		scroll[4] = GUI.BeginScrollView(new Rect(120,(Screen.width - 560)/2,120,(Screen.width - 560)/2),scroll[4],new Rect(0,0,120,scrollLength[4]*40));
		GUI.Label (new Rect (0, 40*i, 120, 40), "Energy Out");
		i++;
		foreach(KeyValuePair<string, float> resource in displayedBacteria.energyOut)
		{
			if(resource.Value > 0)
			{
				GUI.Label(new Rect(0,40*i,120,40),resource.Key + ": " + resource.Value.ToString("F4"));
				i++;
			}
		}
		scrollLength [4] = i;*/
		GUI.EndScrollView();
		GUI.EndGroup ();
	}
}