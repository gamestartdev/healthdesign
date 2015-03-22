using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class PrintStreamingAssetsPath : MonoBehaviour {


	// Use this for initialization
	void Start () {

        GetComponent<GUIText>().text = "Path=" + Application.dataPath;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
