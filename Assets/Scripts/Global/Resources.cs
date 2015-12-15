using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiomeResources
{
	public Dictionary<string,float> mineralQuantities = new Dictionary<string,float>();

	public BiomeResources()
	{
		foreach(string mineral in Control.minerals)
		{
			mineralQuantities.Add(mineral,0);
		}
	}

	public BiomeResources(float[] newMinerals) : this()
	{
		int iterations = 0;
		if(newMinerals.Length < Control.minerals.Count)iterations = newMinerals.Length;
		else iterations = Control.minerals.Count;
		
		for(int i = 0; i < iterations; i ++)
		{
			mineralQuantities[Control.minerals[i]] = newMinerals[i];
		}
	}
	
	public BiomeResources(Dictionary<string,float> newMinerals) : this()
	{
		foreach(KeyValuePair<string,float> mineral in newMinerals)
		{
			if(Control.minerals.Contains(mineral.Key)) mineralQuantities[mineral.Key] = mineral.Value;
			else Debug.Log("mineral doesn't exist... ");
		}
	}

	public BiomeResources(BiomeResources copy) : this()
	{
		mineralQuantities = new Dictionary<string,float> (copy.mineralQuantities);
	}
}

public class PlanetResources
{
	public Dictionary<string,float> gasQuantities = new Dictionary<string,float>();

	//calculated from other variables
	private bool atmosphere;
	private float gasWeight;
	private float pressure;

	public PlanetResources()
	{
		foreach(string gas in Control.gasses)
		{
			//if a gas hasn't been initialized initiliaze it as zero
			if(!gasQuantities.ContainsKey(gas))
			{
				gasQuantities.Add(gas,0);
			}
		}
	}

	public PlanetResources(float[] newGasses) : this()
	{
		int iterations = 0;
		if(newGasses.Length < Control.gasses.Count)iterations = newGasses.Length;
		else iterations = Control.gasses.Count;

		for(int i = 0; i < iterations; i ++)
		{
			gasQuantities[Control.gasses[i]] = newGasses[i];
		}
	}

	public PlanetResources(Dictionary<string,float> newGasses) : this()
	{
		if(newGasses.Count < Control.gasses.Count)
		{
			foreach(KeyValuePair<string,float> gas in newGasses)
			{
				if(Control.gasses.Contains(gas.Key)) gasQuantities[gas.Key] = gas.Value;
				else Debug.Log("Gas doesn't exist... ");
			}
		}
	}
	//if anything in the gasses has
	public bool hasAtmosphere()
	{
		//getPressure (1);

		if(gasWeight > 0) atmosphere = true;
		else atmosphere = false;

		return atmosphere;
	}

	/*public float getPressure(float radius)
	{
		gasWeight = 0;
		foreach(string gas in Control.gasses)
		{
			gasWeight += Control.gasWeights[gas]*gasQuantities[gas];
		}
		pressure = gasWeight / (200 * 3.1415f * radius);
		return pressure;
	}*/
}

public class EnergyResources
{
	public Dictionary<string,float> energyQuantities = new Dictionary<string,float>();

	public EnergyResources()
	{
		foreach(string energy in Control.energies)
		{
			//if a gas hasn't been initialized initiliaze it as zero
			if(!energyQuantities.ContainsKey(energy))
			{
				energyQuantities.Add(energy,0);
			}
		}
	}
}

public class LifeResources
{
	//public Dictionary<string,Bacteria> bacteria = new Dictionary<string,Bacteria>();

	public LifeResources()
	{
	}

	public LifeResources(LifeResources copy)
	{
		//bacteria = copy.bacteria;
	}
}

public class DeadResources : LifeResources
{
}