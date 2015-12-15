using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AddLife
{
	private static Transform planet;

	//Values for starting bacteria life
	private static string[] movementTypes = new string[]{"water","air","organism","soil"};
	public static float[] bacteriaMovementValues;
	private static string[] nutrientTypes = new string[]{"mineral","animal","air"};
	public static float[] bacteriaNutrientValues;
	private static string[] energyTypes = new string[]{"heat","light","radiation"};
	public static float[] bacteriaEnergyValues;

	public static void addBacteria(string name, Biome biome, int quantity, float[] movement, float[] nutreint, float[]energy)
	{
		Dictionary<string,float[]> bacteria = new Dictionary<string, float[]>(){
			{"Movement", new float[]{0,0,0,0}},
			{"Nutrient", new float[]{0,0,0}},
			{"Energy", new float[]{0,0,0}}};

		for(int i = 0; i < bacteriaMovementValues.Length; i++)
		{
			bacteria["Movement"][i] = bacteriaMovementValues[i];
		}
		for(int i = 0; i < bacteriaNutrientValues.Length; i++)
		{
			bacteria["Nutrient"][i] = bacteriaNutrientValues[i];
		}
		for(int i = 0; i < bacteriaEnergyValues.Length; i++)
		{
			bacteria["Energy"][i] = bacteriaEnergyValues[i];
		}
	}
}

