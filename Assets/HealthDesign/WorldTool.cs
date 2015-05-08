using UnityEngine;

public class WorldTool : MonoBehaviour {
    public WorldObject WorldObject;




    void OnMouseUp() {
        var instance = GameObject.Instantiate(WorldObject);
    }
}