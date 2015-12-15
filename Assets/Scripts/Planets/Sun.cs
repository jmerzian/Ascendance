using UnityEngine;
using System.Collections.Generic;

public class Sun: MonoBehaviour
{
	public float Radius;

	// Use this for initialization
	public void Start ()
	{
		//Adjust self to the proper size
		transform.localScale = new Vector3 (Radius, Radius,Radius);
		//Place in the proper orbit
		transform.position = new Vector3 (0,0,0);
	}

	// Update is called once per frame
	public void Update ()
	{
		//Sun does nothing... just sits there
	}
}

