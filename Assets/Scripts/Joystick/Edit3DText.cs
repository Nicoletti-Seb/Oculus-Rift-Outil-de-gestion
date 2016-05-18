using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Edit3DText : MonoBehaviour {

	
	// Update is called once per frame
	public void EditText (string ob) {
		
		InputField title;
		title = GameObject.Find ("InputField Title").GetComponent<InputField> ();

		InputField username;
		username = GameObject.Find ("InputField").GetComponent<InputField> ();

		if (ob.Equals("Cube")){
			TextMesh textObject;
			textObject = GameObject.Find ("TitleCube").GetComponent<TextMesh>();
			textObject.text = username.text;
		}
		else if(ob.Equals("Cylinder")){
			TextMesh textObject;
			textObject = GameObject.Find ("TitleCylinder").GetComponent<TextMesh>();
			textObject.text = username.text;
		}
		else if(ob.Equals("Sphere")){
			TextMesh textObject;
			textObject = GameObject.Find ("TitleSphere").GetComponent<TextMesh>();
			textObject.text = username.text;
		}
		else if(ob.Equals("Text3D")){
			TextMesh textObject;
			textObject = GameObject.Find ("Text3D").GetComponent<TextMesh>();
			textObject.text = title.text;
		}

	}
}
