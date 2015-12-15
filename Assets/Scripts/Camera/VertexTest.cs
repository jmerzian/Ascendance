using UnityEngine;
using System.Collections;

public class VertexTest : MonoBehaviour
{
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			if (!Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit))
				return;
			
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
				return;
			
			Mesh mesh = meshCollider.sharedMesh;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
			Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
			Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
			Transform hitTransform = hit.collider.transform;
			p0 = hitTransform.TransformPoint(p0);
			p1 = hitTransform.TransformPoint(p1);
			p2 = hitTransform.TransformPoint(p2);
			Debug.DrawLine(p0, p1, Color.green, 10);
			Debug.DrawLine(p1, p2, Color.red, 10);
			Debug.DrawLine(p2, p0, Color.blue, 10);
			Debug.Log (hit.triangleIndex);
		}
	}
}

