using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Possibly call the DNAGrid in the game
public class DNAGrid
{
	public object[,] components = new object[Control.gridSize,Control.gridSize];
	public Dictionary<object, string> componentType = new Dictionary<object, string>();
	public Dictionary<object,List<ResourceNode>> resourceChains = new Dictionary<object,List<ResourceNode>>();

	public DNAGrid()
	{
		for(int x = 0; x < Control.gridSize; x++)
		{
			for(int y = 0; y < Control.gridSize; y++)
			{
				components[x,y] = new Null();
				componentType.Add(components[x,y],"Null");
			}
		}
	}

	//make random changes to the grid here
	//select based on pressures
	public void Evolve()
	{
	}
}

public class ResourceNode
{
	public object node;
	public List<ResourceNode> children = new List<ResourceNode>();

	public ResourceNode(object newParent)
	{
		node = newParent;
	}
}