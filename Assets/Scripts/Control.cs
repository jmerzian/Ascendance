using UnityEngine;
using System.Collections.Generic;

public static partial class Control
{
	/*********************************************
	 * Game  Descriptors
	 * These describe how the game will be setup
	 * *******************************************/
	public static int numOfPlanets = 1;
	public static int gridSize = 5;

	/***************************************************
	 * Time functions to speed up/slow down the game, pause etc.
	 * ************************************************/
	public static bool windowOpen;
	public static bool mainWindow;
	public static bool loading;
	public static bool paused;

	public static void Pause()
	{
		Time.timeScale = 0;
		paused = true;
	}
	public static void Resume()
	{
		if(!loading)
		{
			Time.timeScale = 1;
			paused = false;
		}
	}

	/*********************************************************************************88***
	 * Control explored and unexplored planets and which planet is the focus at this moment
	 * ***********************************************************************************/
	//object camera is focusing on
	public static Transform focusObject;
	//camera zoom
	public static float zoom = 1;

	//Known and unknown planets
	public static List<Transform> knownPlanets = new List<Transform>();
	//List of all planents by transform, parent
	public static Dictionary<Transform,Transform> allPlanets = new Dictionary<Transform, Transform>();

	private static void ListPLanets()
	{
		foreach(GameObject planet in GameObject.FindGameObjectsWithTag("Planet"))
		{
			if(!allPlanets.ContainsKey(planet.transform))
			{
				if(planet.transform.parent != null) allPlanets.Add(planet.transform, planet.transform.parent);
			}
		}
	}

	public static List<Transform> ChildrenPlanets(Transform parent)
	{
		List<Transform> childList = new List<Transform>();

		ListPLanets ();

		foreach(KeyValuePair<Transform,Transform> Planets in allPlanets)
		{
			if(Planets.Value == parent) childList.Add(Planets.Key);
		}
		if (childList.Count > 0)
		{
			childList.Sort (delegate(Transform x, Transform y) 
			{
				float xOrbit = x.GetComponent<Planet>().Orbit;
				float yOrbit = y.GetComponent<Planet>().Orbit;
				return xOrbit.CompareTo(yOrbit);
			});
			return childList;
		}
		else return null;
	}

	/*********************************************************************
	 * functions involving species
	 * ****************************************************************/
	//if True change cursor and add bacteria to selected tile
	//contains a list of all species names to prevent duplication
	public static List<string> nameList = new List<string>();

	public static bool addBacteria;
	//public static Bacteria newBacteria;
}

class intVector2
{
	public int x;
	public int y;

	public intVector2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}