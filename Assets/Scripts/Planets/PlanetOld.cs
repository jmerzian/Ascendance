using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetOld : MonoBehaviour
{
	/*********************************************************************************
	 * variables describing the planet
	 * ******************************************************************************/
	public string planetType;
	public GameObject DefaultBiome;

	public Transform Parent;
	public float Radius;
	public float Orbit;
	public int startAngle;
	public float OrbitSpeed;
	public Sprite Unexplored;
	public Sprite Explored;
	public bool explored;

	/***************************************************************************
	 * Values describing the resources and location of said resources
	 * *************************************************************************/
	public PlanetResources resources;
	//make resources addable from the main unity window
	public float[] newResources;

	//Information about biome elevations 5 per biome
	public int[] elevation = new int[60];
	public int elevationRange;
	private int pixelPerElevation = 2;

	//Information about liquid levels
	public int seaLevel;

	/*****************************************************************************
	 * Hold information about the biomes and creatures living on them
	 * **************************************************************************/
	private Dictionary<Transform,Biome> planetBiomes = new Dictionary<Transform,Biome>();
	public Dictionary<Transform,LifeResources> planetLife = new Dictionary<Transform,LifeResources>();

	//interactivity functions
	private bool hasBeenRevealed = false;

	/***********************************************************
	 * Initialize planet to be at x position, and set up biomes
	 * *********************************************************/
	public virtual void Init() 
	{
		/*******************************************************
		 * Adjust planet scale and location
		 * *****************************************************/
		transform.localScale = new Vector3 (Radius, Radius);
		//Place in the proper orbit
		transform.position = new Vector3 (Parent.position.x+Orbit*Mathf.Cos(startAngle), Parent.position.y+Orbit*Mathf.Sin(startAngle), 0);
		/******************************************************
		 * Place Biomes
		 * **************************************************/
		int biomeSpaces = 12;//(int)Mathf.Ceil(Radius*100);
		for(int i = 0; i < biomeSpaces; i++)
		{
			//maths out the angle and positions of the new biomes
			float angle = (2*Mathf.PI*i)/biomeSpaces;
			Vector3 BiomePosition = new Vector3((Mathf.Cos(angle)*Radius*2.6f)+transform.position.x,(Mathf.Sin(angle)*Radius*2.6f)+transform.position.y,0);

			//Create new biome and add it to the ist of biomes on this planet
			GameObject newBiome = Instantiate(DefaultBiome,BiomePosition,transform.rotation) as GameObject;

			newBiome.transform.parent = transform;

			//add to list of biomes for update etc.
			string startBiome = "Lava";
			if(planetType == "Active")
			{
				startBiome = "Lava";
			}
			else if(planetType == "Dwarf")
			{
				startBiome = "Barren";
			}
			else if(planetType == "Inactive")
			{
				startBiome = "Desert";
			}

			planetBiomes.Add(newBiome.transform,BiomeTypes.defaultBiome[startBiome]);
			//adds this biome to the list of things that live on the planet
			planetLife.Add (newBiome.transform,planetBiomes[newBiome.transform].life);

			foreach(string resource in Control.minerals)
			{
				planetBiomes[newBiome.transform].resources.mineralQuantities[resource] = 
					BiomeTypes.biomeResources[startBiome].mineralQuantities[resource];
			}

			//newBiome.transform.GetComponent<SpriteRenderer>().sprite = BiomeTypes.textures[startBiome];

			//position rotate, parent and color
			//TODO figure out how to display the biome...
			newBiome.transform.localScale = new Vector3(1,1,1);
			newBiome.transform.rotation = Quaternion.AngleAxis((angle*180/Mathf.PI)-90, Vector3.forward);

			//Name the object the same as the planet name and tag as biome
			newBiome.transform.tag = "Biome";
			newBiome.transform.name = "Biome" + i.ToString();
		}

		/*******************************************************************
		 * Change elevation of biomes and adjust the sprites correspondingly
		 * *****************************************************************/
		//set the elevations to random heights
		for(int i = 0; i < elevation.Length; i++)
		{
			elevation[i] = Random.Range(-10,10);
		}
		for(int i = 0; i < planetBiomes.Count; i++)
		{
			//get the child
			Transform biome = transform.FindChild("Biome" + i.ToString());

			biome.GetComponent<SpriteRenderer>().sprite = AdjustElevation(i,biome.GetComponent<SpriteRenderer>().sprite.texture);
		}

		//add atmosphere
		resources = new PlanetResources(newResources);
	}

	/************************************************************
	 * Check to see if planet has been explored and manage biomes
	 * *********************************************************/
	public virtual void Update () 
	{		
		//Move planet in orbit around parent body
		transform.RotateAround(Parent.position, Vector3.forward,OrbitSpeed*Time.deltaTime);
		//Vector3 newPosition = (transform.position - Parent.position).normalized * Radius + Parent.position;
		//transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * OrbitCorrection);
		if(explored && !hasBeenRevealed)
		{
			transform.GetComponent<SpriteRenderer>().sprite = Explored;
			transform.GetComponent<Collider2D>().enabled = true;
			foreach(KeyValuePair<Transform, Biome> unknownBiome in planetBiomes)
			{
				unknownBiome.Key.GetComponent<Renderer>().enabled = true;
				unknownBiome.Key.GetComponent<Collider2D>().enabled = true;
			}
		}
	}

	/******************************************************************8
	 * Allow access to information stored within this class
	 * **************************************************************/
	//return information about biomes
	public Dictionary<Transform,Biome> GetPlanetBiomes()
	{
		return planetBiomes;
	}

	/********************************************************************
	 * Utility classes to do things
	 * *************************************************************/
	private Sprite AdjustElevation(int biome, Texture2D texture)
	{ 
		Sprite returnSprite = new Sprite ();
		Texture2D returnTexture = new Texture2D (texture.width, texture.height);
		
		//iterate over each elevation square and pixel
		for(int i = 0 ; i < 5; i++)
		{
			//get the elevation of the current square and the elevation of the adjacent squares
			int thisElevation = elevation[biome + i] * pixelPerElevation;
			int nextIndex = biome + i + 1;
			if(nextIndex == elevation.Length) nextIndex = 0;
			int nextElevation = elevation[nextIndex] * pixelPerElevation;
			
			int difference = nextElevation - thisElevation;
			
			for(int x = 0; x < texture.width/5; x++)
			{
				for(int y = 0; y < texture.height; y++)
				{
					//Get real positions of objects
					int X = i*texture.width/5 + x;
					float scale = 0;
					
					//Piecewise function to get rounded terrain
					if(x > texture.width/10)scale = Mathf.Sqrt((float)(x)/(float)(texture.width/5));
					else scale = (float)(x)/(float)(texture.width/5)*(float)(x)/(float)(texture.width/5);
					int Y = (int)(y + thisElevation + scale*difference);
					Color pixel = texture.GetPixel(X,y);
					
					//Add color to indicate being below the waterline
					//TODO make this look better
					if(thisElevation/pixelPerElevation < seaLevel || nextElevation/pixelPerElevation < seaLevel)
					{
						if(Y < y+seaLevel)
						{
							pixel.b = 1;//BiomeTypes.Water.GetPixel(X,Y);
						}
					}
					returnTexture.SetPixel(X,Y,pixel);
				}
			}
		}
		
		returnTexture.Apply ();
		
		returnSprite = Sprite.Create (returnTexture,new Rect(0,0,texture.width,texture.height), 
		                              new Vector2 (0.5f,0.5f));
		
		return returnSprite;
	}
}
