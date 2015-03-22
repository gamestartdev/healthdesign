using UnityEngine;
using System.Collections;

public class AddNpc : MonoBehaviour {

    public Transform _hero;
    public GameObject _npc_templete;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        Rect rt = new Rect(0, 0, Screen.width / 7, Screen.height / 7);
        rt.x = Screen.width / 2 - rt.width / 2;
        rt.y = Screen.height - rt.height;
        if (GUI.Button(rt, "Add NPC"))
        {
            GameObject new_npc = (GameObject)GameObject.Instantiate(_npc_templete);
            new_npc.transform.position = _hero.position + new Vector3(Random.Range(1, 10), 30, Random.Range(1, 10));
        }
    }
}
