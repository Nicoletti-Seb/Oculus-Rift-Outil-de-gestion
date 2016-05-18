using UnityEngine;
using System.Collections;

public class AddCube: MonoBehaviour {

	public GameObject emptyGameObjectPrefab;

	void Start () {}

	void Update () {}

	public void addCube(){
		GameObject original = GameObject.Find ("Cube") ;
		GameObject cube = (GameObject) Instantiate(original,new Vector3 (650f, 217.2f, -614f),original.transform.rotation) as GameObject;
	}
}