using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class LoadComponents
{
	//Name and Class of textures to be loaded
	public static Dictionary<string,object> components = new Dictionary<string,object> ();

	public static Dictionary<string,object> attachments = new Dictionary<string,object> ();

	public static void Init()
	{
		/**************************************************
		 * Bacteria components
		 * ******************************************/
		Consumer AirIntake = new Consumer ();
		foreach(string resource in Control.gasses) AirIntake.In[resource] = 1;
		components.Add ("AirIntake",AirIntake);

		Consumer MineralIntake = new Consumer ();
		foreach(string resource in Control.minerals) MineralIntake.In[resource] = 1;
		components.Add ("MineralIntake",MineralIntake);

		Consumer OrganismIntake = new Consumer ();
		foreach(string resource in Control.organics) OrganismIntake.In[resource] = 1;
		components.Add ("OrganismIntake",OrganismIntake);

		Producer Clorophyll = new Producer ();
		Clorophyll.Out ["Clorophyll"] = 1;
		Clorophyll.In ["Clorophyll"] = 1;
		components.Add ("Clorophll",Clorophyll);

		Producer Fructose = new Producer ();
		Fructose.Out ["Fructose"] = 1;
		Fructose.In ["Clorophyll"] = 1;
		components.Add ("Fructose",Fructose);

		Brain SingleNeuron = new Brain ();
		SingleNeuron.brainPower = 0.1f;
		components.Add ("SingleNueron",SingleNeuron);
		/*********************************************
		 * Bacteria attachments
		 * ********************************************/
		Connector ResourceTransport = new Connector ();
		attachments.Add ("ResourceTransport", ResourceTransport);

		Weapon Acid = new Weapon (); //Phagocytosis
		attachments.Add ("Acid", Acid);

		Weapon Infect = new Weapon (); //Infection
		attachments.Add ("Infect", Infect);

		Weapon Pop = new Weapon (); //Pointy stick
		attachments.Add ("Pop", Pop);

		/*********************************************************
		 * Set the textures
		 * ******************************************************/
		foreach(string component in components.Values)
		{
			Component baseClass = (Component)components[component];
			baseClass.texture = (Texture2D)Resources.Load("Textures/Cell/"+component);
		}

		foreach(string component in attachments.Keys)
		{
			Component baseClass = (Component)attachments[component];
			baseClass.texture = (Texture2D)Resources.Load("Textures/Cell/"+component);
		}
	}
}