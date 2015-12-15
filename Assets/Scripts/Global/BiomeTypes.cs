using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BiomeTypes
{
	//Default Biomes
	public static Dictionary<string,Biome> defaultBiome = new Dictionary<string,Biome>();

	//Biome textures
	public static Dictionary<string,Texture2D> textures = new Dictionary<string,Texture2D>();
	//Special Textures
	public static Texture2D Gradient;
	public static Texture2D Edge;
	public static Texture2D Atmo;
	public static Texture2D Core;

	public static Material Water;

	public static Dictionary<string,Texture2D> gasses = new Dictionary<string,Texture2D>();

	//Resources for each Biome
	//"Water","Iron","Uranium","Silicon"
	public static Dictionary<string,BiomeResources> biomeResources = new Dictionary<string,BiomeResources>(){
		{"Barren",new BiomeResources(new float[4]{0.5f,0.5f,0.5f,0.5f})},
		{"Lava",new BiomeResources(new float[4]{0,3,2,1})},
		{"Bog",new BiomeResources(new float[4]{2,2,1,1})},
		{"Desert",new BiomeResources(new float[4]{0,1,1,3})}
		};

	public static void init()
	{
		Water = (Material)Resources.Load ("Textures/Biomes/Water");

		Gradient = (Texture2D)Resources.Load ("Textures/Biomes/Gradient");
		Atmo = (Texture2D)Resources.Load ("Textures/Biomes/Atmosphere");
		Core = (Texture2D)Resources.Load ("Textures/Biomes/Core");

		foreach(string biomeType in Control.biomeType)
		{
			Texture2D newTexture = (Texture2D)UnityEngine.Resources.Load("Textures/Biomes/"+biomeType);
			if(newTexture == null) Debug.Log(biomeType + " was unable to load");
			textures.Add(biomeType,newTexture);

			defaultBiome.Add(biomeType,new Biome(biomeType,biomeResources[biomeType]));
		}
	}
}

