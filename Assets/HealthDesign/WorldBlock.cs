using UnityEngine;

public class WorldBlock {
    private readonly WorldBlockBehaviour _worldBlockBehaviour;

    public WorldBlock() : this(new GameObject("WorldBlock").AddComponent<WorldBlockBehaviour>()) {}
    public WorldBlock(WorldBlockBehaviour blockBehaviour) {
        _worldBlockBehaviour = blockBehaviour;
        _worldBlockBehaviour.WorldBlock = this;
    }

    public string textureUrl {
        get { return _worldBlockBehaviour.GifAnim.idle; }
        set {
            if (value.Length > 0) {
                _worldBlockBehaviour.GifAnim.UpdateIdle(value);
//                Object.Destroy(_worldBlockBehaviour.GifAnim);
//                var animation = _worldBlockBehaviour.gameObject.AddComponent<GifAnimation>();
//                animation.idle = value;

            } 
        }
    }

    public string _id { get; set; }

    public Vector3 position {
        get { return _worldBlockBehaviour.transform.position; }
        set { _worldBlockBehaviour.transform.position = new Vector3(value.x, value.y, 0); }
    }
}