using UnityEngine;
using System;
using System.Collections.Generic;

public static class MeshHelper
{
	//creates {subdivisions} random subdivisions with a variability of intensity 
	public static Mesh fractalSubdivision(Mesh mesh,int subdivisions, Vector3 intensity)
	{
		Mesh newMesh = new Mesh ();

		Vector3[] Vertice = mesh.vertices;
		Vector2[] UV = mesh.uv;
		int[] triangles = mesh.GetTriangles(0);

		Vector3[] newVertice = new Vector3[Vertice.Length+triangles.Length];
		Vector2[] newUV = new Vector2[newVertice.Length];
		int[] newtriangles = new int[triangles.Length*3];

		Array.Copy (Vertice, newVertice, Vertice.Length);
		Array.Copy (UV, newUV, UV.Length);

		for(int i = 0; i < triangles.Length/3; i++)
		{
			//get the initial node positions and th noise that is being added
			Vector3 noise = new Vector3(UnityEngine.Random.Range(-intensity.x,intensity.x),UnityEngine.Random.Range(-intensity.y,intensity.y)
			                            ,UnityEngine.Random.Range(-intensity.z,intensity.z));
			Vector3[] nodes = new Vector3[]{Vertice[triangles[i]],Vertice[triangles[i+1]],Vertice[triangles[i+2]]};
			Vector3[] UVnodes = new Vector3[]{UV[triangles[i]],UV[triangles[i+1]],UV[triangles[i+2]]};

			//Get the center point
			Vector3 center = (nodes[0] + nodes[1] + nodes[2])/3;
			/*center.x = (Mathf.Abs(nodes[0].x-nodes[1].x)/2) + Mathf.Abs((nodes[1].x-nodes[2].x)/2) + Mathf.Abs((nodes[2].x-nodes[0].x)/2);
			center.y = (Mathf.Abs(nodes[0].y-nodes[1].y)/2) + Mathf.Abs((nodes[1].y-nodes[2].y)/2) + Mathf.Abs((nodes[2].y-nodes[0].y)/2);
			center.z = (Mathf.Abs(nodes[0].z-nodes[1].z)/2) + Mathf.Abs((nodes[1].z-nodes[2].z)/2) + Mathf.Abs((nodes[2].z-nodes[0].z)/2);*/
			Vector3 UVcenter = (UVnodes[0]+UVnodes[1]+UVnodes[2])/2;

			newVertice[Vertice.Length+i] = new Vector3(center.x+noise.x,center.y+noise.y,center.z+noise.z);
			newUV[Vertice.Length+i] = UVcenter;

			newtriangles[i*9] = triangles[i];
			newtriangles[i*9 + 1] = Vertice.Length+i;
			newtriangles[i*9 + 2] = triangles[i+1];
			newtriangles[i*9 + 3] = triangles[i+1];
			newtriangles[i*9 + 4] = Vertice.Length+i;
			newtriangles[i*9 + 5] = triangles[i+2];
			newtriangles[i*9 + 6] = triangles[i+2];
			newtriangles[i*9 + 7] = Vertice.Length+i;
			newtriangles[i*9 + 8] = triangles[i];
			Debug.Log(i + " : ");
			for(int j = 0; j < 9; j++) 
				Debug.Log(newtriangles[i*9+j]);

		}

		newMesh.vertices = newVertice;
		newMesh.uv = newUV;
		newMesh.triangles = newtriangles;

		return newMesh;
	}

	/*****************************
	 * TODO finish making it work with multiple submeshes
	 * *******************************/
	public static Mesh FlatShading(Mesh baseMesh, int subMeshes)
	{
		Mesh unsharedVertexMesh = new Mesh();

		for(int n = 0; n < subMeshes; n++)
		{
			int[] sourceIndices = baseMesh.GetTriangles(n);
			Vector3[] sourceVerts = baseMesh.vertices;
			Vector2[] sourceUVs = baseMesh.uv;
			
			int[] newIndices = new int[sourceIndices.Length];
			Vector3[] newVertices = new Vector3[sourceIndices.Length];
			Vector2[] newUVs = new Vector2[sourceIndices.Length];

			// Create a unique vertex for every index in the original Mesh:
			for(int i = 0; i < sourceIndices.Length; i++)
			{
				newIndices[i] = i;
				newVertices[i] = sourceVerts[sourceIndices[i]];
				newUVs[i] = sourceUVs[sourceIndices[i]];
			}
			unsharedVertexMesh.vertices = newVertices;
			unsharedVertexMesh.uv = newUVs;
			unsharedVertexMesh.SetTriangles (newIndices,n);
		}
		
		unsharedVertexMesh.RecalculateNormals ();
		
		return unsharedVertexMesh;
	}
}