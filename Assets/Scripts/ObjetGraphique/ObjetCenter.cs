using UnityEngine;
using System.Collections;

public class ObjetCenter : MonoBehaviour {

    public float speed = 0.5f;

	// Use this for initialization
	void Start () {
        // Un seul objet center donc pas de création !
        print("start");
	}
	
	// Update is called once per frame
	void Update () {
        print("update");

        CheckKeys();
        //transform.position -= new Vector3(0, 0, 1) * Time.deltaTime; // pour rapprocher l'objet vers soit ! attention 2D :(
	}

    void CheckKeys() {
        if(Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * speed;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * speed;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.down * speed;
        }
    }
}
