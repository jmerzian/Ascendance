using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/***************************************
 * TODO finish assembling the grid and determining the organism based upon that
 * *********************************/
public class Organism
{
	//Phenotype
	public Mesh mesh;
	public Texture2D UV;
	public string name;
	public string genus;
	public string species;

	//Genotype
	public DNAGrid genotype = new DNAGrid();

	//The resources stored by this organism
	public List<string> resourceList = new List<string>();
	public Dictionary<string,float> In = new Dictionary<string, float>();
	public Dictionary<string,float> Out = new Dictionary<string, float>();
	public Dictionary<string,float> Stored = new Dictionary<string, float> ();
	public Dictionary<string,float> Toxic = new Dictionary<string, float> ();

	//Where this organism can live
	public Vector2 size = new Vector2();
	public Vector2 tempRange = new Vector2();
	public bool aquatic;
	public bool terrestrial;

	//Descriptions of the 
	private Vector2[] dir = new Vector2[]
	{
		new Vector2 (-1, 0),
		new Vector2 (0, 1),
		new Vector2 (1, 0),
		new Vector2 (0, -1)
	};

	public Organism()
	{
		foreach(string resource in Control.gasses)
		{
			In.Add(resource,0);
			Out.Add(resource,0);
			Stored.Add(resource,0);
			Toxic.Add(resource,0);
		}
		foreach(string resource in Control.minerals)
		{
			In.Add(resource,0);
			Out.Add(resource,0);
			Stored.Add(resource,0);
			Toxic.Add(resource,0);
		}
		foreach(string resource in Control.organics)
		{
			In.Add(resource,0);
			Out.Add(resource,0);
			Stored.Add(resource,0);
			Toxic.Add(resource,0);
		}
		foreach(string resource in Control.energies)
		{
			In.Add(resource,0);
			Out.Add(resource,0);
			Stored.Add(resource,0);
			Toxic.Add(resource,0);
		}

		resourceList = new List<string> (Stored.Keys);
	}

	public Organism(Organism clone)
	{
		Mesh cloneMesh = new Mesh();
		cloneMesh.vertices = new Vector3[clone.mesh.vertices.Length];
		cloneMesh.uv = new Vector2[clone.mesh.uv.Length];
		cloneMesh.triangles = new int[clone.mesh.triangles.Length];

		TextureHelper.Copy (ref UV, clone.UV);

		name = clone.name;
		genus = clone.genus;
		species = clone.species;

		In = new Dictionary<string, float> (clone.In);
		Out = new Dictionary<string, float> (clone.Out);
		Stored = new Dictionary<string, float> (clone.Stored);
		Toxic = new Dictionary<string, float> (clone.Toxic);

		size = new Vector2 (clone.size.x, clone.size.y);
		tempRange = new Vector2 (clone.tempRange.x, clone.tempRange.y);

		aquatic = clone.aquatic;
		terrestrial = clone.terrestrial;
	}

	public virtual void AddComponent(string name, int x, int y)
	{
		genotype.components [x, y] = LoadComponents.components [name];
		Update ();
	}

	public virtual void Update()
	{
		//reset all values
		for(int x = 0; x < Control.gridSize; x++)
		{
			for(int y = 0; y < Control.gridSize; y++)
			{
				object component = genotype.components[x,y];
				switch(component.ToString())
				{
				case "Null":
					break;
					//Consumer checks for adjacent producers and sends resources to them
				case "Consumer":
					addConsumer(x,y,component);
					break;
				case "Producer":
					//should already be in a resource chain
					break;
				case "Brain":
					Brain newBrain = (Brain)component;
					break;
				}
			}
		}
	}

	private void addConsumer(int x, int y, object component)
	{
		Consumer newConsumer = (Consumer)component;
		Producer[] adjacentProducers = new Producer[4];

		genotype.resourceChains.Add (new ResourceNode(newConsumer), new List<ResourceNode>());
		
		bool exteriorOrifice = false;
		//Make sure there is an orifice to intake materials
		for(int i = 0; i < 4; i++)
		{
			switch(newConsumer.attachments[i].ToString())
			{
			case "Weapon":
				addWeapon();
				break;
			case "Protection":
				addDefense();
				break;
				//check that there is a connection through to the next cell
				//if there is then update the amount of resources being pulled into this cell
			case "Connector":
				//assign the object
				//updateChain(genotype.resourceChains[newConsumer]);

				//check each adjacent tile for another object
				if((int)(x + dir[i].x) < Control.gridSize && (int)(y + dir[i].y) < Control.gridSize)
				{
					object adjacentObject = genotype.components[(int)(x + dir[i].x),(int)(y + dir[i].y)];
					if(adjacentObject.ToString() == "Producer")
					{
						genotype.resourceChains[newConsumer].Add(new ResourceNode(adjacentObject));
						updateChain(adjacentObject,x+(int)dir[i].x,y+(int)dir[i].y,1);
					}
					else if(adjacentObject.ToString() == "Null")
					{
						exteriorOrifice = true;
					}
				}
				else exteriorOrifice = true;

				Connector newConnector = (Connector)newConsumer.attachments[i];
				if(exteriorOrifice) newConnector.exterior = true;

				break;
			}
		}
	}

	//the head of the resource chain, x and y coordinates on the grid,
	private void updateChain(object parent, int x, int y, int indexOf)
	{
		List<ResourceNode> chain = genotype.resourceChains[parent];

		foreach(Vector2 direction in dir)
		{

		}
	}

	private void addWeapon()
	{
	}

	private void addDefense()
	{
	}

	//TODO evolve the creature
	//edit the mesh and texture according to the changes to the DNA grid
	public virtual void Evolve()
	{
		genotype.Evolve ();
	}
}