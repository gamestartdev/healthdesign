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
    public static Camera HudCamera { get { return FindObjectsOfType<Camera>().First(c => c.name == "HUDCam"); } }

    private void Awake() {
        _worldState = GetComponent<WorldState>();
        var worldObjectsParent = new GameObject("WorldTools").transform;
        worldObjectsParent.transform.position = new Vector3(0, -30, 0);

        foreach (var worldTool in WorldObjectPrefabs) {
            var worldObject = Instantiate(worldTool);
            worldObject.transform.parent = worldObjectsParent;
            worldObject.transform.localPosition = new Vector3(_worldObjects.Count, 0, 0);
            worldObject.gameObject.layer = HudLayer;
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
    }

    private static void DestroyWorldObject() {
        var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(mouseWorldRay.origin, mouseWorldRay.direction);
        if (hit && hit.transform.GetComponent<WorldObject>()) {
            Destroy(hit.collider.gameObject);
        }
    }

    private void LeftClick() {
        var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var worldBlock = new WorldBlock {position = mouseWorldRay.origin, textureUrl = textureUrl};
        _worldState.Save(worldBlock);
    }

    private void PlaceRemoveBlock() {
        if (Input.GetMouseButtonDown(0)) {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var worldBlock = new WorldBlock {position = mouseWorldRay.origin, textureUrl = textureUrl};
            _worldState.Save(worldBlock);
        } else if (Input.GetMouseButtonDown(1)) {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseWorldRay.origin, mouseWorldRay.direction);
            if (hit) {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private class HudSelection {
        private static readonly Plane _plane = new Plane(Vector3.back, 0);
        private Vector3 _mainScreenMouseOffset;
        private WorldObject _selectedObject;
        private WorldObject _lastSelectedObject;

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

            _selectedObject.gameObject.layer = 0;
            _selectedObject.Placed();
            _selectedObject = null;
        }

        public void Select() {
            _selectedObject = FindSelection();
            _lastSelectedObject = _selectedObject;
            if (!_selectedObject) return;
            
            _selectedObject.transform.position = MainScreenZeroZ;
            _mainScreenMouseOffset = _selectedObject.transform.position - MainScreenToRay.origin;
        }

        public void SelectedGUI() {
            if (!_lastSelectedObject) return;
            _lastSelectedObject.SelectedGUI();
        }

        private static WorldObject FindSelection() {
            var hud = Physics2D.RaycastAll(HudToRay.origin, HudToRay.direction);
            var main = Physics2D.RaycastAll(MainScreenToRay.origin, HudToRay.direction);
            var orderByDescending = hud.Concat(main).Select(h => h.collider.GetComponent<WorldObject>());
            var firstOrDefault = orderByDescending.FirstOrDefault();
            
            return IsHud(firstOrDefault) ? Instantiate(firstOrDefault) : firstOrDefault ;
        }

        private static bool IsHud(WorldObject worldObject) { return worldObject.gameObject.layer == HudLayer; }

        public void Update() {
            if (!_selectedObject) return;
            _selectedObject.transform.position = MainScreenToRay.origin + _mainScreenMouseOffset;
        }

    }
}