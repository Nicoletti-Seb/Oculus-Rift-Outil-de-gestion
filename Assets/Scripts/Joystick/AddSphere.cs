using UnityEngine;
using System.Collections;

public class AddSphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addSphere(){
		GameObject original = GameObject.Find ("Sphere") ;
		GameObject cylinder = (GameObject) Instantiate(original,new Vector3 (650f, 166.2f, -612.7f),original.transform.rotation) as GameObject;
	}
}
