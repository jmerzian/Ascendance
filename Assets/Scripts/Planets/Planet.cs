using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/****************************************
 * TODO LIST:
 * Derpy at end of circle...
 * adjust size and color of atmosphere and water according to available resources
 * Implement flat shading by maing it so that none of the triangles share vertexs
 * *************************************/
public class Planet : MonoBehaviour
{
	/*********************************************************************************
	 * variables describing the planet
	 * ******************************************************************************/
	public string planetType;

	public float Radius;
	public float Orbit;
	public int startAngle;
	public float OrbitSpeed;
	public float rotationSpeed;

	public bool explored;

	/**************************************************************************
	 * Describing the planet mesh
	 * ************************************************************************/
	private Mesh mesh;
	private Vector3[] Vertice;
	private Vector2[] UV;
	private int[] triangles;
	private int[] faceTriangles;

	private Mesh waterMesh;
	private Vector3[] waterVertice;
	private Vector2[] waterUV;
	private int[] waterTriangles;
	private int[] waterFaceTriangles;

	public Shader shader;
	public Color color;
	public Texture defaultTex;
	private Texture2D UVMap;// = new Texture2D(240,240);

	private GameObject atmosphere;
	private GameObject liquid;
	public Sprite back;
	public Sprite atmo;

	/***************************************************************************
	 * Values describing the resources and location of said resources
	 * *************************************************************************/
	public PlanetResources resources;
	//make resources addable from the main unity window
	public float[] newResources;

	//Information about biome elevations 5 per biome
	public int numOfElevations;
	public float heightPerElevation;
	public int[] elevation;
	public Dictionary<int,List<Vector3>> elevationNodes = new Dictionary<int, List<Vector3>> ();
	public int elevationRange;
	public float noiseRange;

	//Information about liquid levels
	public int seaLevel;

	/*****************************************************************************
	 * Hold information about the biomes and creatures living on them
	 * **************************************************************************/
	//that int points to a biome...
	public Dictionary<int,Biome> planetBiomes = new Dictionary<int,Biome>();
	public Dictionary<int,LifeResources> planetLife = new Dictionary<int,LifeResources>();

	//...and to a UV coordinate
	private Dictionary<int,Vector2> biomeMap = new Dictionary<int, Vector2> ();

	//interactivity functions
	private bool hasBeenRevealed = false;

	/***********************************************************
	 * Initialize planet to be at x position, and set up biomes
	 * *********************************************************/
	public void Init() 
	{
		/******************************************
		 * Initialize the meshes
		 * *****************************************/
		Mesh newMesh = new Mesh ();
		Vertice = new Vector3[numOfElevations * 8 + 1];
		UV = new Vector2[numOfElevations * 8 + 1];
		triangles = new int[numOfElevations * 36];

		Mesh waterMesh = new Mesh ();
		waterVertice = new Vector3[numOfElevations * 8 +1];
		waterUV = new Vector2[numOfElevations * 8 + 1];
		waterTriangles = new int[numOfElevations * 36];		

		/****************************************
		 * Create the main mesh
		 * ************************************/
		elevation = new int[numOfElevations];
		int numOfBiomes = numOfElevations / 5;

		//Create the vertice locations
		for (int i = 0; i < numOfElevations*2; i++)
		{
			if(i%2!=1)elevationNodes.Add(Mathf.FloorToInt(i/2),new List<Vector3>());
			//set each of the starting elevations to a random value
			elevation[Mathf.FloorToInt(i/2)] = Random.Range(-elevationRange,elevationRange);
			float angle = i*2*Mathf.PI/(numOfElevations);
			for(int j = 0; j < 6; j++)
			{
				//set vector position
				float radius = Radius - (j*j*Radius/80 + elevation[Mathf.FloorToInt(i/2)]*Radius*(heightPerElevation/50));
				Vector3 noise;
				if(j != 5) noise = new Vector3(Random.Range(-Radius/noiseRange,Radius/noiseRange),
				                               Random.Range(-Radius/noiseRange,Radius/noiseRange),
				                               Random.Range(-Radius/noiseRange,Radius/noiseRange));
				else noise =  new Vector3(Random.Range(-Radius/20,Radius/20),Random.Range(-Radius/20,Radius/20),0);
				Vector3 newNode = new Vector3((radius)*Mathf.Cos(angle),(radius)*Mathf.Sin(angle),j*Radius/10);
				newNode += noise;

				Vertice[j*numOfElevations + i] = newNode;

				UV[j*numOfElevations + i] = new Vector2((float)(i)/(float)(4*numOfElevations),(float)j/5);

				//Debug.Log(UV[j*numOfElevations + i]);
	
				elevationNodes[Mathf.FloorToInt(i/2)].Add(newNode);
			}
			//set the UV map for the  wierd odd one...
		}
		//Is this the center?
		Vertice[8*numOfElevations]= new Vector3(0,0,2*Radius/5);

		//TODO if this works fix it so that it works with the last part... because that is currently a little bit wierd...

		for(int i = 0; i < numOfElevations*2; i++)
		{
			for(int j = 0 ; j < 5; j++)
			{
				//SET THE TRIANGLES
				//triangle 1
				triangles[6*(i+j*numOfElevations)] = i + j*numOfElevations; 
				triangles[6*(i+j*numOfElevations)+1] = (i+1)+j*numOfElevations; 
				triangles[6*(i+j*numOfElevations)+2] = i+(j+1)*numOfElevations;
				//Debug.Log("Triangle " + ((6*i+j*numOfElevations)/3).ToString() + " " + new Vector3(triangles[6*i+j*numOfElevations],triangles[6*i+1+j*numOfElevations],triangles[6*i+2+j*numOfElevations]));
				//triangle two)
				if((i+1) != numOfElevations*2)
				{
					triangles[6*(i+j*numOfElevations)+5] = (i+1)+j*numOfElevations; 
					triangles[6*(i+j*numOfElevations)+4] = i+numOfElevations+j*numOfElevations; 
					triangles[6*(i+j*numOfElevations)+3] = (i+1)+(j+1)*numOfElevations;
				}
				else
				{
					/*triangles[6*(i+j*numOfElevations)+3] = i + j*numOfElevations; 
					triangles[6*(i+j*numOfElevations)+4] = j*numOfElevations; 
					triangles[6*(i+j*numOfElevations)+5] = (i+1) +j*numOfElevations;*/
				}
				//Debug.Log("Triangle " + ((6*i+ 3 +j*numOfElevations)/3).ToString() + " " + new Vector3(triangles[6*i+3+j*numOfElevations],triangles[6*i+4+j*numOfElevations],triangles[6*i+5+j*numOfElevations]));
			}
		}
		//Create the inside face
		faceTriangles = new int[(numOfElevations)*6];
		for(int i = 0; i <numOfElevations*2; i++)
		{
			//Set the inside face
			faceTriangles[3*i] = numOfElevations*8;
			faceTriangles[3*i+1] = numOfElevations*6 + (i-1);
			faceTriangles[3*i+2] = numOfElevations*6 + (i-0);
		}

		//assign the mesh the new values
		newMesh.vertices = Vertice;
		newMesh.uv = UV;
		newMesh.triangles = triangles;
		/************************************************************************
		 * Add biomes, resources and textures
		 * *********************************************************************/
		resources = new PlanetResources(newResources);

		//Create the biomes
		//Set the textures
		UVMap = new Texture2D (numOfElevations * 120, 240);
		Texture2D[] UVTextures = new Texture2D[numOfElevations];
		//init texture array
		for(int x = 0; x < numOfElevations; x++)
		{
			UVTextures[x] = new Texture2D(120,240);
		}

		//TODO select the used texture based on biome type...
		for(int i = 0; i < numOfBiomes; i++)
		{
			if(planetType == "Active") 
			{
				string biomeType = Control.biomeType[Random.Range(0,Control.biomeType.Count)];
				planetBiomes.Add(i,new Biome(biomeType));
				planetLife.Add(i,planetBiomes[i].life);
				foreach(string resource in Control.minerals)
				{
					float resourceQuantity = Random.Range(0,10);// * BiomeTypes.defaultBiome[biomeType].resources.mineralQuantities[resource];
					planetBiomes[i].resources.mineralQuantities[resource] = resourceQuantity;
				}
				for(int j = 0; j < 5; j++)
				{
					if(elevation[i*5+j] < seaLevel) planetBiomes[i].underwater += 1;
				}

				TextureHelper.Copy(ref UVTextures[i*5],BiomeTypes.textures[biomeType]);
			}
		}
		//Blend the biomes together
		for(int i = 0; i < numOfBiomes; i++)
		{
			for(int j = 0; j < 5; j++)
			{ 
				int nextIndex = i+1;
				if(nextIndex > numOfBiomes-1) nextIndex = 0;

				if(j == 2)
				{
					//do nothing this texture has already been set...
				}
				if(j == 4 && i != numOfBiomes-1)
				{
					UVTextures[i*5 + j] = TextureHelper.CombineTextures(UVTextures[i*5],UVTextures[(nextIndex)*5],BiomeTypes.Gradient);
				}
				else
				{
					UVTextures[i*5 + j] = UVTextures[i*5];
				}
			}
		}
		//combine the UVtexture array into a map
		for(int x = 0; x < numOfElevations; x++)
		{
			for(int xx = 0; xx < 120; xx++)
			{
				for(int yy = 0; yy < 240; yy++)
				{
					UVMap.SetPixel((x*120) + xx, yy, UVTextures[x].GetPixel(xx,yy));
				}
			}
		}
		UVMap.Apply ();

		/********************************************
		 * TODO Add the water and atmosphere
		 * ****************************************/
		/****************************
		 * Water
		 * *****************************/
		liquid = new GameObject("Water");
		liquid.AddComponent<MeshFilter> ();
		liquid.AddComponent<MeshRenderer> ();

		for(int i = 0; i < numOfElevations*2; i++)
		{
			float angle = i*2*Mathf.PI/numOfElevations;
			for(int j = 0; j < 6; j++)
			{
				//set vector position
				float radius = Radius - j*j*Radius/80 + seaLevel*Radius*((heightPerElevation)/50);
				waterVertice[j*numOfElevations + i] = new Vector3((radius)*Mathf.Cos(angle),(radius)*Mathf.Sin(angle),j*(Radius)/10.5f);
				waterUV[j*numOfElevations + i] = new Vector2((float)i/(float)numOfElevations,(float)j/2);
			}
		}
		waterVertice[8*numOfElevations]= new Vector3(0,0,1.9f*Radius/5f);
		for(int i = 0; i < numOfElevations*2; i++)
		{
			for(int j = 0 ; j < 5; j++)
			{
				//SET THE waterTriangles
				//triangle 1
				waterTriangles[6*(i+j*numOfElevations)] = i + j*numOfElevations; 
				waterTriangles[6*(i+j*numOfElevations)+1] = (i+1)+j*numOfElevations; 
				waterTriangles[6*(i+j*numOfElevations)+2] = i+(j+1)*numOfElevations;
				//Debug.Log("Triangle " + ((6*i+j*numOfElevations)/3).ToString() + " " + new Vector3(waterTriangles[6*i+j*numOfElevations],waterTriangles[6*i+1+j*numOfElevations],waterTriangles[6*i+2+j*numOfElevations]));
				//triangle two)
				if((i+1) != numOfElevations)
				{
					waterTriangles[6*(i+j*numOfElevations)+5] = (i+1)+j*numOfElevations; 
					waterTriangles[6*(i+j*numOfElevations)+4] = i+numOfElevations+j*numOfElevations; 
					waterTriangles[6*(i+j*numOfElevations)+3] = (i+1)+(j+1)*numOfElevations;
				}
				else
				{
					waterTriangles[6*(i+j*numOfElevations)+3] = i + j*numOfElevations; 
					waterTriangles[6*(i+j*numOfElevations)+4] = j*numOfElevations; 
					waterTriangles[6*(i+j*numOfElevations)+5] = (i+1) +j*numOfElevations;
				}
				//Debug.Log("Triangle " + ((6*i+ 3 +j*numOfElevations)/3).ToString() + " " + new Vector3(waterTriangles[6*i+3+j*numOfElevations],waterTriangles[6*i+4+j*numOfElevations],waterTriangles[6*i+5+j*numOfElevations]));
			}
		}
		//Create the inside face
		waterFaceTriangles = new int[6*(numOfElevations)];
		for(int i = 0; i <numOfElevations*2; i++)
		{
			//Set the inside face
			waterFaceTriangles[3*i] = numOfElevations*8;
			waterFaceTriangles[3*i+1] = numOfElevations*6 + (i-1);
			waterFaceTriangles[3*i+2] = numOfElevations*6 + (i);
		}
		//assign the mesh the new values
		waterMesh.vertices = waterVertice;
		waterMesh.triangles = waterTriangles;
		waterMesh.RecalculateNormals ();
		//assign the meshcollider
		waterMesh.subMeshCount = 2;
		waterMesh.SetIndices(waterTriangles,MeshTopology.Triangles,0);
		waterMesh.SetIndices(faceTriangles,MeshTopology.Triangles,1);
		
		liquid.GetComponent<MeshFilter> ().mesh = waterMesh;
		liquid.GetComponent<Renderer>().materials = new Material[]{new Material(BiomeTypes.Water),new Material(BiomeTypes.Water)};
		Color waterColor = new Color (Random.Range (0, 1f), Random.Range (0, 1f), Random.Range (0, 1f), 1);
		liquid.GetComponent<Renderer>().materials[0].color = waterColor;
		liquid.GetComponent<Renderer>().materials[1].color = new Color(waterColor.r/2,waterColor.g/2,waterColor.b/2);

		liquid.transform.rotation = Quaternion.Euler (0, 180, 0);
		liquid.transform.position = transform.position;
		liquid.transform.parent = transform;
		
		
		/**************
		 * Atmosphere
		 * ****************/
		atmosphere = new GameObject ("Atmosphere");
		atmosphere.transform.rotation = Quaternion.Euler (0, 180, 0);
		atmosphere.transform.parent = transform;
		Texture2D atmoTexture = new Texture2D (1024, 1024);
		TextureHelper.Copy (ref atmoTexture, BiomeTypes.Atmo);

		SpriteRenderer atmo = atmosphere.AddComponent<SpriteRenderer> ();
		atmo.color = new Color (Random.Range (0, 1f), Random.Range (0, 1f), Random.Range (0, 1f));
		atmo.sprite = Sprite.Create(atmoTexture, new Rect (0,0,1024,1024), new Vector2 (0.5f,0.5f));

		atmosphere.transform.localScale = new Vector3 (Radius/3, Radius/3, Radius/3);
		atmosphere.transform.position = transform.position;

		/*********************************************************
		 * Assign all of the changes and clean up
		 * *******************************************************/
		//assign the meshcollider
		Material renderMaterial = new Material (shader);
		renderMaterial.mainTexture = UVMap;

		Material faceMaterial = new Material (shader);
		faceMaterial.mainTexture = BiomeTypes.Core;

		//Create the submeshes
		newMesh.subMeshCount = 2;
		newMesh.SetIndices(triangles,MeshTopology.Triangles,0);
		newMesh.SetIndices(faceTriangles,MeshTopology.Triangles,1);
		//Subdivide the mesh to add some texture
		newMesh = MeshHelper.FlatShading(newMesh,1);
		gameObject.GetComponent<MeshFilter> ().mesh = newMesh;
		transform.GetComponent<Renderer>().materials = new Material[]{renderMaterial};//,faceMaterial};
		
		gameObject.AddComponent <MeshCollider> ();
		//set up a shere collider so that the planet itself can be selected
		SphereCollider collider = gameObject.AddComponent<SphereCollider> ();
		collider.radius = Radius/2;

		//Change the object's position
		transform.position = new Vector3 (transform.parent.position.x+Orbit*Mathf.Cos(startAngle), transform.parent.position.y+Orbit*Mathf.Sin(startAngle), 0);
	}
	
	/************************************************************
	 * Check to see if planet has been explored and manage biomes
	 * *********************************************************/
	public virtual void FixedUpdate () 
	{
		//Move planet in orbit around parent body
		transform.RotateAround(transform.parent.position, Vector3.forward,OrbitSpeed*Time.deltaTime);
		//Rotate the planet
		transform.RotateAround(transform.position, Vector3.forward,rotationSpeed*Time.deltaTime);

		if(explored && !hasBeenRevealed)
		{
			transform.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			transform.GetComponent<Renderer>().enabled = false;
		}
	}

	public IEnumerator turnUpdate()
	{
		foreach(Biome biome in planetBiomes.Values)
		{
			yield return StartCoroutine(biome.Update());
			/*foreach(Bacteria bacteria in biome.life.bacteria.Values)
			{
				foreach(string resource in Control.gasses)
				{
					resources.gasQuantities[resource] += (bacteria.resourceOut[resource] - bacteria.resourceIn[resource]) * bacteria.quantity/1000000;
					if(resources.gasQuantities[resource] < 0) resources.gasQuantities[resource] = 0;
				}
			}*/
		}
	}
}
