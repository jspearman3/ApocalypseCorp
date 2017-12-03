using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Destructable {
	 float maxHealth { get; }
	 float health { get; set; }
}
