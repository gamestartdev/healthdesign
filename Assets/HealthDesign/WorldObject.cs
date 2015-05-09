using UnityEngine;

public class WorldObject : MonoBehaviour {
    public Transform Model;
    public void Placed() {
        var food = GetComponent<Food>();
        if (food) food.enabled = true;
    }

    public Vector3 Rotation = new Vector3(0, 0.5f, 0);
    void Update() {
        Model.Rotate(Rotation);
    }

    public void SelectedGUI() {
        GUILayout.Label("WorldObject! "+transform.position);
    }
}