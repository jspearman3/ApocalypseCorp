using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHandler : MonoBehaviour {
	public static PowerHandler getInstance() {
		return GameObject.Find ("PowerHandler").GetComponent<PowerHandler> ();
	}

	private MapData data;
	// Use this for initialization
	void Awake () {
		data = TiledMap.getInstance ().mapData;
	}
	
	public List<PowerableStructure> powerableCache;
	public List<PowerCarrier> powerCarrierCache;
	public bool dirtyCache = true;

	private void buildPowerMap(MapCoords currentPoint, int currentPower, Dictionary<MapCoords, PowerableStructure> currentMap, Dictionary<MapCoords, PowerCarrier> carrierMap) {
		if (currentPower <= 0)
			return;
		
		if (data == null)
			data = TiledMap.getInstance ().mapData;
		
		Structure structure = data.getStructure (currentPoint);

		if (structure == null)
			return;

		if (structure is PowerCarrier) {
			MapCoords[] adjacencies = new MapCoords[] {
				currentPoint,
				currentPoint.add (1, 0),
				currentPoint.add (-1, 0),
				currentPoint.add (0, 1), 
				currentPoint.add (0, -1)
			};

			foreach (MapCoords m in adjacencies) {
				Structure adjacent = data.getStructure (m);

				if (adjacent != null) {
					PowerCarrier carrier;
					if (adjacent is PowerCarrier && !carrierMap.TryGetValue(m,out carrier)) {
						carrierMap.Add (m, adjacent as PowerCarrier);
						buildPowerMap (m, currentPower - 1, currentMap, carrierMap);
					}

					PowerableStructure p;
					if (adjacent is PowerableStructure && !currentMap.TryGetValue (m, out p)) {
						currentMap.Add (m, adjacent as PowerableStructure);
					}
				}
			}		
		}
	}

	private void UpdatePowerStructureCaches (MapCoords startPoint, int startPower) {
		Dictionary<MapCoords, PowerableStructure> structuresToPower = new Dictionary<MapCoords, PowerableStructure>();
		Dictionary<MapCoords, PowerCarrier> structuresToCarry = new Dictionary<MapCoords, PowerCarrier>();
		buildPowerMap (startPoint, startPower, structuresToPower, structuresToCarry);

		List<PowerableStructure> structures = new List<PowerableStructure> ();
		foreach (PowerableStructure p in structuresToPower.Values)
			structures.Add (p);
		powerableCache = structures;

		List<PowerCarrier> carriers = new List<PowerCarrier> ();
		foreach (PowerCarrier p in structuresToCarry.Values)
			carriers.Add (p);
		powerCarrierCache = carriers;
	}

	public int StartSurge(MapCoords startPoint, int startPower) {
		if (dirtyCache) {
			UpdatePowerStructureCaches (startPoint, startPower);
			dirtyCache = false;
		} 
		int pollution = 0;
		foreach (PowerableStructure p in powerableCache)
			pollution += p.Power ();

		foreach (PowerCarrier p in powerCarrierCache) {
			if (p is Conduit) {
				Conduit c = p as Conduit;
				c.startSurge ();
			}
		}

		return pollution;
	}
}
