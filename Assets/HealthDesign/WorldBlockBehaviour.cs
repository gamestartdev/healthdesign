using UnityEngine;

public class WorldBlockBehaviour : MonoBehaviour {
    public WorldBlock WorldBlock;
    void Start() {
        if (WorldBlock == null) {
            WorldBlock = new WorldBlock(this);
        }
    }
}