using System;
using UnityEngine;

public class WorldObject : MonoBehaviour {
    public Transform Model;

    public void Placed() {
        _food = GetComponent<Food>();
        _worldBlockBehaviour = GetComponent<WorldBlockBehaviour>();
        if (_food) {
            _food.enabled = true;
        }
    }

    public Vector3 Rotation = new Vector3(0, 0.5f, 0);
    private Food _food;
    private WorldBlockBehaviour _worldBlockBehaviour;
    private string _urlInput;
    void Update() {
        if (Model) Model.Rotate(Rotation);
    }

    public void SelectedGUI() {
        if (_food) {
            GUILayout.Label("Food Strength: " + _food.strength);
            _food.strength = (int)GUILayout.HorizontalSlider(_food.strength, 0, 500);
            GUILayout.Label("Food Time Scale: " + _food.timeScale);
            _food.timeScale = (int)GUILayout.HorizontalSlider(_food.timeScale, 0, 1000);
        }
        if (_worldBlockBehaviour) {
            GUILayout.Label("Image URL: " + _urlInput);
            _urlInput = GUILayout.TextField(_urlInput, 1000);
            if (GUILayout.Button("Download Image!")) {
                _worldBlockBehaviour.WorldBlock.textureUrl = _urlInput;
            }
        }
    }
}