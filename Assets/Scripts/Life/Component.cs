using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*************************************
 * TODO create the different types of components
 * *************************************/
public class Component
{
	public string name;
	public Texture2D texture;
	public Attachment[] attachments = new Attachment[4];

	public Dictionary<string,float> Waste =new Dictionary<string, float>();
	public Dictionary<string,float> Toxins =new Dictionary<string, float>();

	public Component()
	{
		foreach(string resource in Control.gasses)Waste.Add(resource,0);
		foreach(string resource in Control.minerals)Waste.Add(resource,0);
		foreach(string resource in Control.organics)Waste.Add(resource,0);
		foreach(string resource in Control.energies)Waste.Add(resource,0);

		Toxins = new Dictionary<string, float> (Waste);
	}

	public Component(Component clone)
	{
		TextureHelper.Copy (ref texture, clone.texture);
		Array.Copy (clone.attachments, attachments, 4);

		Waste = new Dictionary<string, float> (clone.Waste);
		Toxins = new Dictionary<string, float> (clone.Toxins);
	}
}

public class Null:Component
{
}

public class Consumer:Component
{
	public Dictionary<string,float>  In = new Dictionary<string, float>();

	public Consumer():base()
	{
		foreach(string resource in Control.gasses)In.Add(resource,0);
		foreach(string resource in Control.minerals)In.Add(resource,0);
		foreach(string resource in Control.organics)In.Add(resource,0);
		foreach(string resource in Control.energies)In.Add(resource,0);
	}
}
public class Producer:Component
{
	public Dictionary<string,float> In = new Dictionary<string, float>();
	public Dictionary<string,float> Out = new Dictionary<string, float>();

	public Producer():base()
	{
		foreach(string resource in Control.gasses)Out.Add(resource,0);
		foreach(string resource in Control.minerals)Out.Add(resource,0);
		foreach(string resource in Control.organics)Out.Add(resource,0);
		foreach(string resource in Control.energies)Out.Add(resource,0);
	}
}
public class Brain:Component
{
	public float brainPower;
}

/*****************************************
 * TODO create the different types of attachments
 * ****************************************/
public class Attachment
{
	public string name;
	public Texture2D texture;

	public Attachment()
	{
	}
}

public class Weapon:Attachment
{
	public Weapon():base()
	{
	}
}

public class Protection:Attachment
{
	public Color shellColor;
	public Texture2D shellTexture;

	public Protection():base()
	{
	}
}

public class Connector:Attachment
{
	//connects resources T/F
	public bool resources;
	//check if it leads outside the organism
	public bool exterior;

	public Connector():base()
	{
	}
}

public class Sensor:Attachment
{
	public Sensor():base()
	{
	}
}

/**********************************
 * Ideas... might implement
 * ********************************/
public class Immunology:Component
{
}