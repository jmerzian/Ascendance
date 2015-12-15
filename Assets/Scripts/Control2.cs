using UnityEngine;
using System.Collections.Generic;

public static partial class Control
{
	/*****************************************************
	 * Types of Resources
	 * TODO use a text file to input these
	 * ***************************************************/
	//Universal resources
	//faith, souls
	public static List<string> universalResourceNames = new List<string>{"Faith","Souls"};
	public static Dictionary<string,Texture2D> universalResourceTextures;
	public static Dictionary<string,float> universalResources = new Dictionary<string,float>(){{"Faith",100},{"Souls",100}};
	
	//Minerals used in the game
	//water, ammonia, calcium, Iron, Sulfur,Uranium
	//weights refer to atomic mass
	public static List<string> minerals = new List<string>{"Water","Iron","Uranium","Silicon"};
	public static Dictionary<string,float> mineralWeights;
	public static Dictionary<string,Texture2D> mineralTextures;
	
	//Gasses used in the game
	//Oxygen, nitrogen, methane, carbon-dioxide, hydrogen
	//weights refer to atomic mass
	public static List<string> gasses = new List<string>{"Oxygen","CO2","Hydrogen"};
	public static Dictionary<string,float> gasWeights;
	public static Dictionary<string,Texture2D> gasTextures;
	
	//organic materials used in the game
	//Oxygen, nitrogen, methane, carbon-dioxide, hydrogen
	//weights refer to atomic mass
	public static List<string> organics = new List<string>{"Blood","Clorophyll","Fructose"};
	public static Dictionary<string,float> organicWeights;
	public static Dictionary<string,Texture2D> organicTextures;
	
	//energy types used in the game
	//Heat, Light, Radiation, Movement, Life
	//weight refers to how useful said resource is
	public static List<string> energies = new List<string>{"Life","Light","Movement","Heat"};
	public static Dictionary<string,float> energyWeights;
	public static Dictionary<string,Texture2D> energiesTextures;
	
	/************************************************************
	 * Different types of planets and biomes
	 * ************************************************************/
	public static List<string> planetType = new List<string>{"Active","Inactive","Gas","Dwarf"};
	public static List<string> biomeType = new List<string>{"Barren","Lava","Bog","Desert"};
}

