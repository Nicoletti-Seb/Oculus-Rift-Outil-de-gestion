using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {

	public Image gambar;
	public Canvas canvas;
	public GameObject ob;

	// Use this for initialization
	void Start () {
		ob = GameObject.Find ("Canvas2");
		canvas = ob.GetComponent<Canvas> ();
		canvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {}

	public void setVisible (){
		GameObject ob;
		ob = GameObject.Find ("Canvas2");
		canvas = ob.GetComponent<Canvas> ();
		if (canvas.enabled==false) {
			canvas.enabled = true;
		}
	}

	public void ChangePic(string nameImage){
		gambar.sprite = Resources.Load<Sprite> ("Images/"+nameImage) as Sprite;
	}
}
