using UnityEngine;
using System.IO;
using Gif2Textures;
using System.Collections;

public class GifDemoTest : MonoBehaviour
{
	public string url = "http://piskel-imgstore-b.appspot.com/img/90ae3d68-cb82-11e4-8653-058e997efba9.gif";
	public bool m_FasterButMoreMemoryUsage = true;
	
	float m_Timer = 0.0f;
	float m_CurrentDelay = 0;
	GifFrames m_GifFrames = null;


	IEnumerator Start() {
		WWW www = new WWW(url);
		yield return www;

		Debug.Log(www.text);

		MemoryStream ms = new MemoryStream(www.bytes);
		
		m_GifFrames = new GifFrames();
		if (m_GifFrames.Load(ms, m_FasterButMoreMemoryUsage))
			SetNextFrame();
		else
			m_GifFrames = null;
	}
	
	void Update()
	{
		if (m_GifFrames != null)
		{
			m_Timer += Time.deltaTime;
			
			if (m_Timer >= m_CurrentDelay)
			{
				m_Timer -= m_CurrentDelay;
				SetNextFrame();
			}
		}
	}
	
	void SetNextFrame()
	{
		Texture2D tex;
		m_GifFrames.GetNextFrame(out tex, out m_CurrentDelay);
		if (GetComponent<Renderer>() != null && GetComponent<Renderer>().material != null)
			GetComponent<Renderer>().material.mainTexture = tex;
		if (GetComponent<GUITexture>() != null)
			GetComponent<GUITexture>().texture = tex;
	}
}
