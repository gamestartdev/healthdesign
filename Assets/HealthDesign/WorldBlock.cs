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