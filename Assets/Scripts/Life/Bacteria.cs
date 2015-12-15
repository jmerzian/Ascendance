using UnityEngine;
using System.Collections.Generic;

public class Bacteria : Organism
{
	public Bacteria():base()
	{
	}

	public Bacteria(Bacteria clone):base(clone)
	{
	}

	public override void Evolve()
	{
		base.Evolve ();
	}
}