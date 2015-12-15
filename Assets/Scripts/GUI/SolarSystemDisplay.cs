using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SolarSystemDisplay
{
	public static List<Transform> planets = new List<Transform>();
	public static Dictionary<Transform,Planet> planetClass = new Dictionary<Transform, Planet>();

	public static void Init(Transform sun)
	{
		for(int i = 0 ; i < Control.numOfPlanets; i++)
		{
			Transform newPlanet = CreatePlanet(sun,i+1);
			for(int j = 0; j < 5; j++)
			{
				if(Random.Range(0,10) < newPlanet.GetComponent<Planet>().Radius/2) 
				{
					Transform newMoon = CreatePlanet(newPlanet,j+1);
				}
			}
		}
	}

	private static Transform CreatePlanet(Transform parent,int orbit)
	{
		//set up the game object
		GameObject newPlanet = new GameObject();
		newPlanet.AddComponent<MeshFilter> ();
		newPlanet.AddComponent<MeshRenderer> ();
		newPlanet.transform.rotation = Quaternion.Euler(new Vector3 (0,180,0));
		newPlanet.tag = "Planet";

		//Set up the planet class
		Planet planetInfo = newPlanet.AddComponent<Planet>();
		planetInfo.planetType = "Active";
		planetInfo.Radius = Random.Range(2,10f);
		planetInfo.Orbit = Random.Range(60*orbit,(float)120*orbit);
		planetInfo.startAngle = Random.Range(0,360);
		planetInfo.OrbitSpeed = Random.Range(-2,2f);
		planetInfo.rotationSpeed = Random.Range (-20, 20f);
		planetInfo.explored = true;
		planetInfo.shader = Shader.Find("Diffuse");
		planetInfo.newResources = new float[10];
		for(int i = 0; i < 10; i++)
		{
			planetInfo.newResources[i] = Random.Range(0,10f);
		}
		planetInfo.numOfElevations = 5*Random.Range(4,12);
		planetInfo.heightPerElevation = Random.Range(0.2f,0.5f);
		planetInfo.noiseRange = Random.Range (20, 100);
		planetInfo.elevationRange = Random.Range(5,10);
		planetInfo.seaLevel = Random.Range(-10,10);

		newPlanet.transform.parent = parent;

		//if it's a moon adjust the orbit and radius values
		if(parent.tag == "Planet")
		{
			planetInfo.Orbit /= 10;
			planetInfo.Radius /= 4;
			planetInfo.seaLevel /= 4;
		}

		planets.Add (newPlanet.transform);
		planetClass.Add (newPlanet.transform, planetInfo);

		planetInfo.Init ();
		return newPlanet.transform;
	}
	public static void DisplaySystem()
	{
		//Set the skin
		GUI.skin = GUIMain.infoSkin;

		//TODO import textures...
		GUI.Box (new Rect (0, Screen.height - 125, 125, 125), "SUN");
		//int j = 0;
		for(int i = 0; i < planets.Count; i++)
		{
			//TODO differentiate between different types of planets, IE moons asteroids etc.
			/*
			if(planets.Contains(planets[i].parent))
			{
				j++;
				if(GUI.Button (new Rect (125 + 32*j, Screen.height - 96,32,32), "Moon"))
				{
					Control.focusObject = planets[i];
					Control.zoom = 4;
				}
			}
			else
			{*/
				if(GUI.Button (new Rect (125 + 64*i, Screen.height - 64,64,64), "Planet"))
		        {
		       		Control.focusObject = planets[i];
					Control.zoom = 4;
				}
			//}
		}
	}
}