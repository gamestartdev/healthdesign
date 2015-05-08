using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(WorldState))]
public class WorldBuilder : MonoBehaviour {
    public string textureUrl = "http://piskel-imgstore-b.appspot.com/img/2a8f582e-d065-11e4-a2d8-5bfbb68c2d8f.gif";
    public WorldObject[] WorldObjectPrefabs;
    
    private List<WorldObject> _worldObjects;
    private WorldState _worldState;

    void Awake() {
        _worldState = GetComponent<WorldState>();
        Transform worldObjectsParent = new GameObject("WorldTools").transform;
        foreach (WorldObject worldTool in WorldObjectPrefabs) {
            var worldObject = Instantiate(worldTool);
            worldObject.transform.position = new Vector3(_worldObjects.Count, 0, 0);
            worldObject.transform.parent = worldObjectsParent;
            worldObject.gameObject.layer = LayerMask.NameToLayer("HUD");
            _worldObjects.Add(worldObject);
        }
        GetComponents<WorldTool>();
    }

    private void OnGUI() {
        
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var worldBlock = new WorldBlock(){position = mouseWorldRay.origin, textureUrl =textureUrl};
            _worldState.Save(worldBlock);
        }
        else if (Input.GetMouseButtonDown(1)) {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseWorldRay.origin, mouseWorldRay.direction);
            if (hit) {
                Destroy(hit.collider.gameObject);
            }
        }
    }

}