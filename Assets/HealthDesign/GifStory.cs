using System;
using UnityEngine;
using System.IO;
using Gif2Textures;
using System.Collections;


public class GifStory : MonoBehaviour
{

    public string url = "http://piskel-imgstore-b.appspot.com/img/ef558485-d037-11e4-bba4-5bfbb68c2d8f.gif";
    Gif _gif;
    SpriteRenderer _spriteRenderer;

    IEnumerator Start()
    {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _gif = new Gif(url);
        StartCoroutine(_gif.Download());

        while (true)
        {
            yield return new WaitForSeconds(NextFrame().Delay);
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
    }

    private Frame NextFrame()
    {
        var frame = _gif.GetNextFrame();
        _spriteRenderer.sprite = frame.Sprite;
        return frame;
    }
}
