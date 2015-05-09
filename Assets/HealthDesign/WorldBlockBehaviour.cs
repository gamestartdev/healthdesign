using UnityEngine;

public class WorldBlockBehaviour : MonoBehaviour {

    public  GifAnimation GifAnim;
    public WorldBlock WorldBlock;
    void Start() {
        if (WorldBlock == null) {
            WorldBlock = new WorldBlock(this);
        }
    }
}