using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : MonoBehaviour {

	public virtual bool IsPlayerTraversable (){
		return false;
	}
}
