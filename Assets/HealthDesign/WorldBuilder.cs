using UnityEngine;
using System.Collections;

public class WorldBuilder : MonoBehaviour
{
    public string worldTexture = "http://piskel-imgstore-b.appspot.com/img/2a8f582e-d065-11e4-a2d8-5bfbb68c2d8f.gif";

    private class Grid
    {
    }

    // Use this for initialization
    private void Start()
    {
    }

    private Transform CreateWorldBlock(Vector3 pos)
    {
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