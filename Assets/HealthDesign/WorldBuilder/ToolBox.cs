using UnityEngine;

public class ToolBox : MonoBehaviour {

    private static Ray ScreenPointToRay { get { return Camera.main.ScreenPointToRay(Input.mousePosition); } }
	
    // Use this for initialization
	void Start () {
	    
	}
	


	// Update is called once per frame
	void Update () {
	    RaycastHit hitInfo;
	    var ray = ScreenPointToRay;
	    if (Physics.Raycast(ray, out hitInfo)) {
	        
	    } else {
	        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
	    }
	}

}
