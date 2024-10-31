using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {

	void Update ()
	{

		if(UnityEngine.Input.GetKeyDown(KeyCode.Z) || UnityEngine.Input.GetKeyDown(KeyCode.X) || UnityEngine.Input.GetKeyDown(KeyCode.C))
		   Destroy(transform.gameObject);
	
	}
}
