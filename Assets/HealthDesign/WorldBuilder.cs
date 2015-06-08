using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (WorldState))]
public class WorldBuilder : MonoBehaviour {
    private readonly HudSelection _hudSelection = new HudSelection();
    private readonly List<WorldObject> _worldObjects = new List<WorldObject>();
    private WorldState _worldState;
    public string textureUrl = "http://piskel-imgstore-b.appspot.com/img/2a8f582e-d065-11e4-a2d8-5bfbb68c2d8f.gif";
    public WorldObject[] WorldObjectPrefabs;
    public static int HudLayer { get { return LayerMask.NameToLayer("HUD"); } }
    public static int ConsumableLayer { get { return LayerMask.NameToLayer("Consumable"); } } 

    public static Camera HudCamera { get { return FindObjectsOfType<Camera>().First(c => c.name == "HUDCam"); } }

    private void Awake() {
        _worldState = GetComponent<WorldState>();
        var worldObjectsParent = new GameObject("WorldTools").transform;
        worldObjectsParent.transform.position = new Vector3(0, -30, 0);

        foreach (var worldTool in WorldObjectPrefabs) {
            var worldObject = Instantiate(worldTool);
            worldObject.transform.parent = worldObjectsParent;
            worldObject.transform.localPosition = new Vector3(_worldObjects.Count * 2, 0, 0);
            worldObject.gameObject.layer = HudLayer;
            Debug.Log(worldObject);
            Debug.Log(worldObject.gameObject.layer);

            _worldObjects.Add(worldObject);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _hudSelection.Select();
        } else if (Input.GetMouseButtonUp(0)) {
            _hudSelection.Deselect();
        } else if (Input.GetMouseButton(0)) {
            _hudSelection.Update();
        }

        if (Input.GetMouseButtonDown(1)) {
            DestroyWorldObject();
        }
    }

    void OnGUI() {
        _hudSelection.SelectedGUI();
        if(GUILayout.Button("Clear")) PlayerPrefs.DeleteAll();
    }

    private static void DestroyWorldObject() {
        var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(mouseWorldRay.origin, mouseWorldRay.direction);
        if (hit && hit.transform.GetComponent<WorldObject>()) {
            Destroy(hit.collider.gameObject);
        }
    }


    private class HudSelection {
        private static readonly Plane _plane = new Plane(Vector3.back, 0);
        private Vector3 _mainScreenMouseOffset;
        private WorldObject _selectedObject;
        private bool _editWindowOpen;
        private bool _moved;

        private static Vector3 MainScreenZeroZ
        {
            get
            {
                float dist;
                if (_plane.Raycast(MainScreenToRay, out dist)) {
                    return MainScreenToRay.GetPoint(dist);
                }
                return Vector3.zero;
            }
        }
        private static Ray HudToRay { get { return HudCamera.ScreenPointToRay(Input.mousePosition); } }
        private static Ray MainScreenToRay { get { return Camera.main.ScreenPointToRay(Input.mousePosition); } }


        public void Deselect() {
            if (!_selectedObject) return;

            if (_moved) {
                _selectedObject.Placed();
            }
            else {
                _editWindowOpen = true;
            }
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) { CloseEditWindow(); }
            if (_editWindowOpen) return;
            if (!_selectedObject) return;
            var newPosition = MainScreenToRay.origin + _mainScreenMouseOffset;
            if ((_selectedObject.transform.position - newPosition).sqrMagnitude > 0.1f) {
                _moved = true;
            }
            _selectedObject.transform.position = newPosition;
        }

        public void Select() {
            if (_editWindowOpen) return;

            _selectedObject = FindSelection();
            if (!_selectedObject) return;
            
            _selectedObject.transform.position = MainScreenZeroZ;
            _mainScreenMouseOffset = _selectedObject.transform.position - MainScreenToRay.origin;
            _moved = false;
        }

        public void SelectedGUI() {
            if (!_editWindowOpen) return;
            var clientRect = new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f);
            GUI.ModalWindow(0, clientRect, ModalEditWindow, _selectedObject.name);
        }

        private void ModalEditWindow(int id) {
            _selectedObject.SelectedGUI();
            if (GUILayout.Button("Finished Editing!")) {
                CloseEditWindow();
            }
        }

        private void CloseEditWindow() {
            _selectedObject = null;
            _editWindowOpen = false;
            _moved = false;
            Deselect();
        }

        private static WorldObject FindSelection() {
            var hud = Physics2D.RaycastAll(HudToRay.origin, HudToRay.direction);
            var main = Physics2D.RaycastAll(MainScreenToRay.origin, HudToRay.direction);
            var orderByDescending = hud.Concat(main).Select(h => h.collider.GetComponent<WorldObject>());
            var firstOrDefault = orderByDescending.FirstOrDefault();
            return IsHud(firstOrDefault) ? Instantiate(firstOrDefault) : firstOrDefault ;
        }

        private static bool IsHud(WorldObject worldObject) { return worldObject && worldObject.gameObject.layer == HudLayer; }

    }
}