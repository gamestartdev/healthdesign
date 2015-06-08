using System;
using UnityEngine;

public class WorldObject : MonoBehaviour {
    public Transform Model;

    public void Placed() {
        _consumable = GetComponent<Consumable>();
        if (_consumable) {
            _consumable.enabled = true;
            _consumable.gameObject.layer = WorldBuilder.ConsumableLayer;
        }
        _gifAnimation = GetComponent<GifAnimation>();
        if (_gifAnimation) {
            _gifAnimation.enabled = true;
        }
    }

    public void Start() {
        _consumable = GetComponent<Consumable>();
        _gifAnimation = GetComponent<GifAnimation>();
    }

    public Vector3 Rotation = new Vector3(0, 0.5f, 0);
    private Consumable _consumable;
    private GifAnimation _gifAnimation;
    private string _idle = "";
    private string _move = "";
    private string _jump = "";

    void Update() {
        if (Model) Model.Rotate(Rotation);
    }

    public void SelectedGUI() {
        if (_consumable) {
            GUILayout.Label("Consumable Strength: " + _consumable.strength);
            _consumable.strength = (int)GUILayout.HorizontalSlider(_consumable.strength, 0, 500);
            GUILayout.Label("Consumable Time Scale: " + _consumable.timeScale);
            _consumable.timeScale = (int)GUILayout.HorizontalSlider(_consumable.timeScale, 0, 1000);
            GUILayout.Label("Units of Insulin: " + _consumable._unitsOfInsulin);
            _consumable._unitsOfInsulin = (int)GUILayout.HorizontalSlider(_consumable._unitsOfInsulin, 0, 30);
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