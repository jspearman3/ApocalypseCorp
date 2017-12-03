using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerableStructure : Structure {
	public abstract int polutionGenerated { get; }

	public int Power() {
		OnPowered ();
		return polutionGenerated;
	}

	// return polution generated (can be negative)
	protected abstract void OnPowered ();
}
