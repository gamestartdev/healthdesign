using System;
using UnityEngine;
using System.IO;
using Gif2Textures;
using System.Collections;
using UnityEngine.UI;

public class GifStory : MonoBehaviour
{
	public string message = "You Win!";
    public string url = "http://piskel-imgstore-b.appspot.com/img/ef558485-d037-11e4-bba4-5bfbb68c2d8f.gif";
    Gif _gif;
    SpriteRenderer _spriteRenderer;
	public Text text;
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

	public void Bump(PlayerInput player) {
		player.text.enabled = true;
		player.text.text = message;
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1.5f);

	}

    private Frame NextFrame()
    {
        var frame = _gif.GetNextFrame();
        _spriteRenderer.sprite = frame.Sprite;
        return frame;
    }
}
