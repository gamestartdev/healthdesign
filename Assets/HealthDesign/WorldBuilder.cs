using Newtonsoft.Json;
using UnityEngine;

public class WorldBlock {
    private readonly GameObject _gameObject;
    private readonly GifAnimation _gifAnim;
    private Vector3 _position;
    private string _textureUrl;

    public WorldBlock() {
        _gameObject = new GameObject("WorldBlock");
        _gameObject.AddComponent<BoxCollider2D>();
        _gifAnim = _gameObject.AddComponent<GifAnimation>();
    }

    public string textureUrl {
        get { return _textureUrl; }
        set {
            _textureUrl = value;
            _gifAnim.idle = textureUrl;
        }
    }

    public string _id { get; set; }

    public Vector3 position {
        get { return _position; }
        set {
            _position = value;
            _gameObject.transform.position = new Vector3(value.x, value.y, 0);
        }
    }
}

public class WorldBuilder : MonoBehaviour {
    public string textureUrl = "http://piskel-imgstore-b.appspot.com/img/2a8f582e-d065-11e4-a2d8-5bfbb68c2d8f.gif";
    private int nextBlockIndex;

    void Awake() {

        int index = 0;
        while (PlayerPrefs.HasKey("WorldBlock_" + index)) {
            var deserializeObject = JsonConvert.DeserializeObject<WorldBlock>(PlayerPrefs.GetString("WorldBlock_" + index));
            Debug.Log(deserializeObject.position);
            index++;
        }
    }

    void OnGUI() {
        if (GUILayout.Button("Reset DB")) {
            PlayerPrefs.DeleteAll();
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var worldBlock = new WorldBlock(){position = mouseWorldRay.origin, textureUrl =textureUrl};
            var json = JsonConvert.SerializeObject(worldBlock);
            PlayerPrefs.SetString("WorldBlock_" +nextBlockIndex++, json);
            Debug.Log(json);
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