using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntRef
{
	public int value;

	public static implicit operator int(IntRef r) { return r.value; }
}
