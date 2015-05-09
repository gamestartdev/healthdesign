using UnityEngine;

public class WorldBlockBehaviour : MonoBehaviour {

    public GifAnimation GifAnim { get { return GetComponent<GifAnimation>(); } }
    public WorldBlock WorldBlock;
    void Awake() {
        if (WorldBlock == null) {
            WorldBlock = new WorldBlock(this);
        }
        gameObject.layer = 0;
    }
}