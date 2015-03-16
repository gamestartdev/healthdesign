using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	class Grid {
		
	}

	// Use this for initialization
	void Start () {
		
	}

	Transform CreateWorldBlock(Vector3 pos) {
		Debug.Log(pos);
		var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
		t.position = new Vector3(pos.x, pos.y, 0);
		return t;
    }
    
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (0)) {
			var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.Log(mouseWorldRay);
			
			RaycastHit2D hit = Physics2D.Raycast(mouseWorldRay.origin, mouseWorldRay.direction);
			if(hit) {
				Destroy (hit.collider.gameObject);
			} else {;
				CreateWorldBlock(mouseWorldRay.origin);
			}
		}
	}
}
