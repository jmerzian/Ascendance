using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Biome
{
	//basic information about biome
	public string type;
	public Texture2D texture;
	public bool selected;
	public BiomeResources resources = new BiomeResources();
	public LifeResources life = new LifeResources();
	public DeadResources dead = new DeadResources();
	public int underwater;

	public Biome(string newType)
	{
		type = newType;
		texture = BiomeTypes.textures [newType];

		foreach(string mineral in Control.minerals)
		{
			resources.mineralQuantities[mineral] = 10;
		}
	}

	public Biome(string newType,BiomeResources resources)
	{
		type = newType;
		texture = BiomeTypes.textures [newType];

		resources = new BiomeResources (resources);
	}

	public Biome(Biome copy)
	{
		type = copy.type;
		//TODO may have to texturehelper copy this...
		texture = copy.texture;

		selected = copy.selected;

		resources = new BiomeResources (copy.resources);
		//life = new LifeResources (copy.life);
	}

	/********************************************************
	 * Update the biome
	 * -
	 * ***************************************************/
	public IEnumerator Update()
	{
		/*foreach(Bacteria bacteria in life.bacteria.Values)
		{
			foreach(string resource in Control.minerals)
			{
				resources.mineralQuantities[resource] += (bacteria.resourceOut[resource] - bacteria.resourceIn[resource]) * bacteria.quantity/1000000;
				if(resources.mineralQuantities[resource] < 0) resources.mineralQuantities[resource] = 0;
			}
			yield return null;
		}*/
		yield return null;
	}
}