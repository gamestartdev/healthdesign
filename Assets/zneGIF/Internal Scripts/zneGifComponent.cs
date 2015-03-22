// (c) Copyright 'ZNE Edu' 2013. All rights reserved.

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;


public class zneGifComponent : MonoBehaviour
{
    public enum LoopType
    {
        Once,
        Loop,
        Pingpong,
        ReverseOnce,
        ReverseLoop,
        ReversePingpong,
    }

    public string _error;

    // Name
    public string _name = "";

    // Loading param
    public Texture2D _gifInStreamingAssets;
    public string _filename;
    public zneGIF.LOAD_METHOD _loadMethod = zneGIF.LOAD_METHOD.ONE_FRAME_AT_FIRST;
    public FilterMode _textureFilterMode = FilterMode.Bilinear;
    public TextureWrapMode _textureWrapMode = TextureWrapMode.Clamp;

    // Target param
    public GameObject[] _targets;
    public bool _autoChangeTexture = true;
    public bool _sendMessageOnFrame = false;

    // Play param
    public bool _playOnAwake = true;
    public LoopType _loopType = LoopType.Loop;
    public float _speed = 1.0f;

    // Remove (GIF clip) cash on destroy 
    public bool _removeCashOnDestroy = false;


    internal zneGIF.Clip _clip = null;
    internal zneGIF.Frame _frame = null;
    internal Texture2D _texture = null;
    internal float _frame_time = 0.0f;
    internal int _last_set_frame_index = -1;
    internal bool _is_playing = false;
    internal bool _pingpong_way_normal = true; // true=normal, false=reverse

    internal virtual void SetError(string error_message)
    {
        _error = error_message;
        if( _error != null && _error != "" )
            Debug.LogError("[zneGifComponent]\nError=" + _error);
    }

    public virtual string GetError()
    {
        return _error;
    }

    internal virtual void GifComponent_Awake()
    {
        LoadGifClip();

        if (_playOnAwake)
            Play();
    }

    internal virtual void GifComponent_Start()
    {
#if UNITY_EDITOR
        if ( (_targets == null || _targets.Length == 0) && !Application.isPlaying)
        {
            if (gameObject.GetComponent<Renderer>() != null ||
                gameObject.GetComponent<GUITexture>() != null)
                _targets = new GameObject[] { gameObject };
            else
                SetError("No have target!");
        }
#endif
        SetTextureToTargets(_gifInStreamingAssets);

        _pingpong_way_normal = (_loopType == LoopType.Pingpong);
    }


    internal virtual void GifComponent_Update()
    {
#if UNITY_EDITOR
        OnEditor_Update();
#else
        if (_clip != null && !_clip.HasError() )
        {
            Update_Frame();
        }
#endif

    }

    internal virtual void Update_Frame()
    {
        _clip.Update();

        if (!_is_playing || _clip.GetFrameCount() <= 0 )
            return;

        _frame_time += Time.deltaTime * _speed;

        switch (_loopType)
        {
            case LoopType.Once:
                Update_Once();
                break;

            case LoopType.Loop:
                Update_Loop();
                break;

            case LoopType.Pingpong:
                Update_PingPong();
                break;

            case LoopType.ReverseOnce:
                Update_ReverseOnce();
                break;

            case LoopType.ReverseLoop:
                Update_ReverseLoop();
                break;

            case LoopType.ReversePingpong:
                Update_ReversePingPong();
                break;
        }
    }

    internal virtual void Update_Once()
    {
    }

    internal virtual void Update_ReverseOnce()
    {
    }

    internal virtual void Update_Loop()
    {
    }

    internal virtual void Update_ReverseLoop()
    {
    }

    internal virtual void Update_PingPong()
    {
    }

    internal virtual void Update_ReversePingPong()
    {
    }

    internal string GetFullFilePath()
    {
#if UNITY_WEBPLAYER
        if (_filename != null && _filename.Length > 0)
            return Application.dataPath + "/StreamingAssets/" + _filename;
#else
        if (_filename != null && _filename.Length > 0)
            return Application.streamingAssetsPath + "/" + _filename;
#endif

        return null;
    }

    // Load Gif Clip by Loading param
    internal virtual bool LoadGifClip()
    {
        string filepath = GetFullFilePath();
        return LoadGifClipByFilename(filepath);
    }

    // Load Gif Clip from filename    
    public virtual bool LoadGifClipByFilename(string filename)
    {
        return false;
    }

    // Load Gif Clip by stream
    public virtual bool LoadGifClipByStream(Stream stream)
    {
        return false;
    }

    // Load Gif Clip by Url
    public virtual bool LoadGifClipByUrl(string url)
    {
        return false;
    }

    public virtual bool IsLoading()
    {
        return _clip != null && _clip.IsLoading();
    }

    public virtual bool IsFileOpening()
    {
        return (_clip != null && _clip.GetStatus() == zneGIF.Clip.STATUS.CREATING_STREAM_BY_WWW);
    }

    // 0~1.0
    public virtual float GetFileOpeningProgress()
    {
        if (_clip != null)
        {
            if (_clip.GetStatus() == zneGIF.Clip.STATUS.CREATING_STREAM_BY_WWW)
            {
                return _clip._www.progress;
            }
            else if( _clip.GetStatus() >= zneGIF.Clip.STATUS.STREAM_CREATED )
            {
                return 100;
            }
        }
        return 0;
    }

    public virtual void SetLoopType(LoopType loop_tye)
    {
        _loopType = loop_tye;
        _pingpong_way_normal = (_loopType == LoopType.Pingpong);
    }

    public virtual LoopType GetLoopType()
    {
        return _loopType;
    }

    public virtual void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public virtual float GetSpeed()
    {
        return _speed;
    }

    public virtual void Play()
    {
        _is_playing = true;
    }

    public virtual bool IsPlaying()
    {
        return _is_playing;
    }

    public virtual void Stop()
    {
        _is_playing = false;
        _frame = null;

        _pingpong_way_normal = (_loopType == LoopType.Pingpong);
    }

    public virtual void Pause()
    {
        _is_playing = false;
    }

    internal virtual void SetFrame(zneGIF.Frame frame)
    {
        _frame = frame;
        _frame_time = 0;
        _texture = (_frame != null) ? _frame._texture : null;

        if (_frame != null && _last_set_frame_index != _frame._index)
        {
            Debug.Log("SetFrame");

            _last_set_frame_index = _frame._index;

            // Auto change texture to target
            if (_autoChangeTexture)
                SetTextureToTargets(_texture);

            // Send message to target
            if (_sendMessageOnFrame)
                SendMssageToTargets();
        }
    }

    internal virtual void SetTextureToTargets( Texture2D texture)
    {
        if (_targets != null)
        {
            foreach (GameObject target in _targets)
                SetTextureTo(target, texture);
        }
    }

    internal virtual void SendMssageToTargets()
    {
        if (_targets != null && _frame != null )
        {
            foreach (GameObject target in _targets)
                target.SendMessage("OnGIF_SetFrame", _frame);
        }
    }


    internal virtual void SetTextureTo(GameObject target, Texture2D texture)
    {
        if (target != null)
        {
            //  Set to GUI Texture
            if (target.GetComponent<GUITexture>() != null)
            {
                target.GetComponent<GUITexture>().texture = texture;
            }

            // Set to main texture in material
            else
            {
#if UNITY_EDITOR
                if (target.GetComponent<Renderer>() != null)
                {
                    if (Application.isPlaying)
                        target.GetComponent<Renderer>().material.mainTexture = texture;
                    else
                        target.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
                }
#else
                target.renderer.material.mainTexture = texture;
#endif
            }
        }
    }




    #region Codes for editor
#if UNITY_EDITOR

    internal virtual void OnEditor_Update()
    {
        // Save GIF asset filename
        string cur_filename = GetGifFilenameInStreamingAsset(_gifInStreamingAssets);
        if (_filename != cur_filename)
        {
            // GIF asset changed!
            _filename = cur_filename;

            if (_filename != null && _filename != "")
                SetError(null);
        }

        if (_clip != null && !_clip.HasError() && Application.isPlaying)
        {
            Update_Frame();
        }
        else
        {
            SetTextureToTargets(_gifInStreamingAssets);
        }
    }

    internal virtual string GetGifFilenameInStreamingAsset(UnityEngine.Object stream_assets_obj)
    {
        if (stream_assets_obj == null)
            return "";

        string gif_path = AssetDatabase.GetAssetPath(stream_assets_obj);

        // Is GIF file?
        string ext = Path.GetExtension(gif_path).ToLower();
        if (ext != ".gif")
        {
            Debug.Log(gif_path);
            SetError("This image is not GIF!");
            return "";
        }

        // Is in AssetPath?
        string temp = gif_path.ToLower();
        temp = temp.Replace("streamingassets/", "streamingassets#");

        if (!temp.Contains("#"))
        {
            SetError("This GIF file is not in StreamingAssets!");
            return "";
        }

        // get pathname after StreamingAssets
        int index = temp.LastIndexOf("#");
        if (index != -1)
        {
            return gif_path.Substring(index + 1);
        }

        return null;
    }

#endif
    #endregion

}