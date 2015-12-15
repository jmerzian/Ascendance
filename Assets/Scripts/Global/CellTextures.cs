using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static class CellTextures
{
	//wierd textures
	public static Texture2D transparent = new Texture2D(320,320);
	public static Texture2D corner = new Texture2D(32,32);
	public static Texture2D edge = new Texture2D(32,32);
	public static Texture2D center = new Texture2D(32,32);
	public static Texture2D interconnect = new Texture2D(32,32);

	public static Dictionary<string,Texture2D> textures = new Dictionary<string,Texture2D>();

	public static void initTextures()
	{
		//creates a transparent sprite for use in various lovations
		transparent.SetPixels (TextureHelper.newImage (320, 320, new Color(0,0,0,0)));
		transparent.Apply ();

		//TODO set to an actual image...
		interconnect.SetPixels (TextureHelper.newImage (32, 32, new Color (0, 0, 0, 1)));
		interconnect.Apply ();

		TextureHelper.Copy(ref edge,(Texture2D)Resources.Load("Textures/Weighting/Edge"));
		TextureHelper.Copy(ref corner,(Texture2D)Resources.Load("Textures/Weighting/Corner"));
		TextureHelper.Copy(ref center,(Texture2D)Resources.Load("Textures/Weighting/Center"));

		foreach(string resource in Control.gasses)
		{
			Texture2D newTexture = (Texture2D)Resources.Load("Textures/Cell/"+resource);
			textures.Add(resource,newTexture);
		}
		foreach(string resource in Control.minerals)
		{
			Texture2D newTexture = (Texture2D)Resources.Load("Textures/Cell/"+resource);
			textures.Add(resource,newTexture);
		}
		foreach(string resource in Control.organics)
		{
			Texture2D newTexture = (Texture2D)Resources.Load("Textures/Cell/"+resource);
			textures.Add(resource,newTexture);
		}
		foreach(string resource in Control.energies)
		{
			Texture2D newTexture = (Texture2D)Resources.Load("Textures/Cell/"+resource);
			textures.Add(resource,newTexture);
		}
	}
}