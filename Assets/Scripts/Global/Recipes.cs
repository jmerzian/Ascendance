using UnityEngine;
using System.Collections.Generic;

//TODO use text file to input recipies
public class Recipe
{
	public string energyType;
	public Dictionary<string,string> inOut = new Dictionary<string, string>();

	public Recipe (string type)
	{
		energyType = type;
	}

	public void AddRecipe (string input, string output)
	{
		inOut.Add (input, output);
	}
	              
	public string getResult(string input)
	{
		if(inOut.ContainsKey(input)) return inOut[input];
		else return null;
	}
}

//Energy types
//"Light","Life"
//"H2O","FE2","U"
//"O2","CO2","H2"
//"Blood","Clorophyll"

static class Recipes
{
	public static Dictionary<string,Recipe> recipes = new Dictionary<string,Recipe>();

	public static void initRecipes()
	{
		//instantiate the Recipe lists
		foreach(string energy in Control.energies)
		{
			if(!recipes.ContainsKey(energy))recipes.Add(energy,new Recipe(energy));
		}
		//TODO read from file the recipes, until then...

		//Recipies to make a light based organism
		//Light
		recipes ["Light"].AddRecipe ("Oxygen", "Clorophyll");
		//Life
		recipes ["Life"].AddRecipe ("Clorophyll","Sugar");
		recipes ["Life"].AddRecipe ("Sugar","Oxygen");
		//Heat
		recipes ["Heat"].AddRecipe ("Oxygen","Water");
		recipes ["Heat"].AddRecipe ("Hydrogen","Water");
	}
}