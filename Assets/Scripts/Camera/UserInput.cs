using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInput : MonoBehaviour
{
	//maximum viewport scalings
	public float sizeMin;
	public float sizeMax;
	
	//Camera Focus
	//Moved focus object to the main control so that 
	private float planetPos;
	public float camSpeed;
	
	//Object Selection Variables
	private bool click = false;
	private float delay =  0.25f;
	private float doubleClickTime;
	private Transform SelectedObject = null;
	private Transform ClickedObject = null;
	private Vector3[] ClickedVertices = new Vector3[3];
	public Transform PlanetSelector;
	public Transform BiomeSelector;

	// Update is called once per frame
	void Update ()
	{
	/*******************************************
	 * Check to see if an object is being selected
	 * *******************************************/
	//this is how long in seconds to allow for a double click
		if(!Control.windowOpen)
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				if (!click) 
				{ // first click no previous clicks
					click = true;
					doubleClickTime = Time.time; // save the current time
					ClickedObject = ObjectSelection();
					if(ClickedObject != null)
					{
						SelectedObject = ClickedObject;
					}
				} 
				else 
				{
					click = false; // found a double click, now reset
					if(ClickedObject != null && SelectedObject != null)
					{
						if(ClickedObject == ObjectSelection())
						{
							if(SelectedObject.tag == "Planet") Control.focusObject = ClickedObject;
						}
						else
						{
							click = true;
							SelectedObject = ClickedObject;
						}
					}
				}
			}
			if (click) 
			{
				// if the time now is delay seconds more than when the first click started. 
				if ((Time.time - doubleClickTime) > delay) 
				{
					//basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
					click = false;
				}
			}
			//deselect all selected objects
			if(Input.GetMouseButtonDown(1))
			{
				SelectedObject = null;
				GUIDisplay.printedPlanet = null;
				GUIDisplay.printedBiome = null;
				for(int i = 0; i < 3; i++)ClickedVertices[i] = new Vector3(0,0,0);
			}
			/*****************************************************************
		 * Zoom the camera in and out and move around a selected planet
		 * ****************************************************************/
			if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
			{
				Control.zoom -= Control.zoom/10;
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
			{
				Control.zoom += Control.zoom/10;
			}
			if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				planetPos -= camSpeed;
			}
			if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				planetPos += camSpeed;
			}
			
			Camera.main.orthographicSize = Mathf.Clamp(Control.zoom, sizeMin, sizeMax );
			
			/****************************************************************8
		 * Other Inputs
		 * ***************************************************************/
			if(Input.GetKeyDown(KeyCode.Space))
			{
				if(Control.paused)Control.Resume();
				else Control.Pause();
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Control.mainWindow = true;
		}
		/***************************************************************
		 * If an object has been selected position the selector and/or the camera on the selected object
		 * ***********************************************************/
		
		//Select and deselect planet
		if(SelectedObject != null && SelectedObject.tag == "Planet")
		{
			PlanetSelector.transform.GetComponent<Renderer>().enabled = true;
			GUIDisplay.printedPlanet = SelectedObject.GetComponent<Planet>();
			PlanetSelector.position = SelectedObject.position;
			PlanetSelector.localScale = SelectedObject.lossyScale;
			//Vector3 vertice = (ClickedVertices[0] + ClickedVertices[1] + ClickedVertices[2])/3;
			//Debug.Log("center Point " + vertice);

			Vector3 vertice = ClickedVertices[1];
			int biomeIndex = Mathf.RoundToInt((Mathf.Atan2(-vertice.y,vertice.x)*GUIDisplay.printedPlanet.numOfElevations)/(10*Mathf.PI) + Mathf.PI);
			Debug.Log(biomeIndex + " " + Mathf.Atan2(-vertice.y,vertice.x) + " " + Mathf.Atan2(vertice.y,vertice.x) + " " + vertice.z + " " + vertice.y + " " + vertice.x + " ");
			if(GUIDisplay.printedPlanet.planetBiomes.ContainsKey(biomeIndex))
			{
				GUIDisplay.printedBiome = GUIDisplay.printedPlanet.planetBiomes[biomeIndex];
				if(Control.addBacteria)
				{
					//GUIDisplay.printedBiome.life.bacteria.Add(Control.newBacteria.genus + " " + Control.newBacteria.species, Control.newBacteria);
					Control.addBacteria = false;
				}
			}
		}
		else
		{
			PlanetSelector.transform.GetComponent<Renderer>().enabled = false;
			GUIDisplay.printedPlanet = null;
		}

		if(GUIDisplay.printedPlanet != null) GUIDisplay.printPlanet = true;
		if(GUIDisplay.printedBiome != null) GUIDisplay.printBiome = true;
		
		/*****************************************************************
		 * Position and rotate the Camera around the desired planet
		 * *************************************************************/
		if(Control.focusObject != null)
		{
			float PlanetRadius = Control.focusObject.GetComponent<Planet>().Radius;
			Vector2 PlanetPosition = new Vector2((Mathf.Sin(planetPos)*PlanetRadius), (Mathf.Cos(planetPos)*PlanetRadius));
			transform.position = new Vector3(Control.focusObject.position.x + PlanetPosition.x,Control.focusObject.position.y + PlanetPosition.y,-10);
			
			transform.rotation = Quaternion.AngleAxis(-planetPos*180/Mathf.PI, Vector3.forward);
		}
	}
	
	private Transform ObjectSelection()
	{
		RaycastHit hit;
		if (!Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit))
			return null;
		
		MeshCollider meshCollider = hit.collider as MeshCollider;
		Transform hitTransform = hit.collider.transform;
		if (meshCollider == null || meshCollider.sharedMesh == null)
			return hitTransform;
		
		Mesh mesh = meshCollider.sharedMesh;
		Vector3[] vertices = mesh.vertices;
		int[] triangles = mesh.triangles;
		ClickedVertices [0] = hitTransform.TransformPoint(vertices[triangles[hit.triangleIndex * 3 + 0]]) - hitTransform.position;
		ClickedVertices [1] = hitTransform.TransformPoint(vertices[triangles[hit.triangleIndex * 3 + 1]]) - hitTransform.position;
		ClickedVertices [2] = hitTransform.TransformPoint(vertices[triangles[hit.triangleIndex * 3 + 2]]) - hitTransform.position;

		//Debug.Log(ClickedVertices[0].ToString() + ClickedVertices[1].ToString() + ClickedVertices[2].ToString());
		return hitTransform;
	}
}

