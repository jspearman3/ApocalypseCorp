using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChargableStructure : PowerableStructure {
	public abstract float maxPower { get;}
	public abstract float powerGainPerSurge { get;}
	private float powerBacking = 0;
	public float power { get { return powerBacking; } set{powerBacking = value;}}

	public override int polutionGenerated { get { return 0;}} //No pollution generated for charging

	protected override void OnPowered ()
	{
		power += powerGainPerSurge;
		if (power > maxPower) {
			power = maxPower;
		}
	}

	protected bool UsePower(float cost) {
		bool success = cost <= power;
		if (success) {
			power -= cost;
		}
		return success;
	}
}
