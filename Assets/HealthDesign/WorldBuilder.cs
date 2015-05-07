using UnityEngine;
using System.Collections;
using DustinHorne.Json.Examples;
using Newtonsoft.Json;

public class WorldBuilder : MonoBehaviour
{
    private class WorldBlock {
        
        public string _id;
        public Vector3 position;

        private WorldBlock() { }
        [JsonConstructor]
        public WorldBlock(string _id, Vector3 position)
        {
            Debug.Log("public");
            Debug.Log(this.position);
            Debug.Log(position);

            Debug.Log(this._id);
            Debug.Log(_id);
        }


        public static WorldBlock Create() { return new WorldBlock() {_id = "asdf", position = Vector3.one*2}; }
    }

    public string worldTexture = "http://piskel-imgstore-b.appspot.com/img/2a8f582e-d065-11e4-a2d8-5bfbb68c2d8f.gif";
    private int nextBlockIndex;

    private class Grid
    {
    }

    // Use this for initialization
    private void Start() {
        var json = JsonConvert.SerializeObject(WorldBlock.Create());
        Debug.Log(json);
        WorldBlock deserializeObject = JsonConvert.DeserializeObject<WorldBlock>(json);
        Debug.Log(deserializeObject.position);
    }

//
//    public void Sample() {
//        //This string is the JSON representation of the object
//        string serialized = JsonConvert.SerializeObject(original);
//
//        //Now we can deserialize this string back into an object
//        var newobject = JsonConvert.DeserializeObject<JNSimpleObjectModel>(serialized);
//
//        Debug.Log(newobject.IntList.Count);
//    }


    private Transform CreateWorldBlock(Vector3 pos) {
        Debug.Log(pos);
        var t = new GameObject().transform;
        t.gameObject.AddComponent<BoxCollider2D>();
        t.position = new Vector3(pos.x, pos.y, 0);
        var gifAnim = t.gameObject.AddComponent<GifAnimation>();
        gifAnim.idle = worldTexture;

        return t;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            CreateWorldBlock(mouseWorldRay.origin);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            var mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldRay.origin, mouseWorldRay.direction);
            if (hit)
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}