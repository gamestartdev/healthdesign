using UnityEngine;

public class WorldBlock {
    private readonly GifAnimation _gifAnim;
    private readonly WorldBlockBehaviour _worldBlockBehaviour;

    public WorldBlock() : this(new GameObject("WorldBlock").AddComponent<WorldBlockBehaviour>()) {}
    public WorldBlock(WorldBlockBehaviour blockBehaviour) {
        _worldBlockBehaviour = blockBehaviour;
        _worldBlockBehaviour.WorldBlock = this;
        _worldBlockBehaviour.gameObject.AddComponent<BoxCollider2D>();
        _gifAnim = _worldBlockBehaviour.gameObject.AddComponent<GifAnimation>();
    }

    public string textureUrl {
        get { return _gifAnim.idle; }
        set {
            if(value.Length > 0) _gifAnim.idle = value;
        }
    }

    public string _id { get; set; }

    public Vector3 position {
        get { return _worldBlockBehaviour.transform.position; }
        set { _worldBlockBehaviour.transform.position = new Vector3(value.x, value.y, 0); }
    }
}