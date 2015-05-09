using System;
using UnityEngine;

public class WorldObject : MonoBehaviour {
    public Transform Model;

    public void Placed() {
        _food = GetComponent<Food>();
        if (_food) {
            _food.enabled = true;
        }
        _gifAnimation = GetComponent<GifAnimation>();
        if (_gifAnimation) {
            _gifAnimation.enabled = true;
        }
    }

    public void Start() {
        _food = GetComponent<Food>();
        _gifAnimation = GetComponent<GifAnimation>();
    }

    public Vector3 Rotation = new Vector3(0, 0.5f, 0);
    private Food _food;
    private GifAnimation _gifAnimation;
    private string _idle = "";
    private string _move = "";
    private string _jump = "";

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
        if (_gifAnimation) {
            UpdateAnim("Idle", ref _idle, _gifAnimation.UpdateIdle);
            UpdateAnim("Move", ref _move, _gifAnimation.UpdateMove);
            UpdateAnim("Jump", ref _jump, _gifAnimation.UpdateJump);
        }
    }

    private void UpdateAnim(string name, ref string value, Action<string> update) {
        GUILayout.Label("Set " + name + ":");
        value = GUILayout.TextArea(value);
        if (GUILayout.Button("Download " + name + "!")) {
            update(value);
        }
    }
}