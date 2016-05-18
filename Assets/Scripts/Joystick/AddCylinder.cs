using UnityEngine;
using System.Collections;

public class AddCylinder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addCylinder(){
		GameObject original = GameObject.Find ("Cylinder") ;
		GameObject cylinder = (GameObject) Instantiate(original,new Vector3 (650f, 114.2f, -613.1f),original.transform.rotation) as GameObject;
	}
}
