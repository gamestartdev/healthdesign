// (c) Copyright 'ZNE Edu' 2013. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace zneGIF
{
    public class MovieClip : Clip
    {

#region MovieClip Cache System
        static Dictionary<string, MovieClip> s_clip_cache_dic = new Dictionary<string, MovieClip>();

        static internal MovieClip GetClipInCache( string key )
        {
            if (s_clip_cache_dic.ContainsKey(key))
                return s_clip_cache_dic[key];

            return null;
        }

        static internal bool AddClipInCache( string key, MovieClip clip )
        {
            if (!s_clip_cache_dic.ContainsKey(key))
            {
                s_clip_cache_dic.Add(key, clip);
                return true;
            }

            return false;
        }

        static internal bool DelClipInCache( string key )
        {
            if (!s_clip_cache_dic.ContainsKey(key))
            {
                s_clip_cache_dic.Remove(key);
                return true;
            }

            return false;
        }

        static internal bool DelClipInCache( MovieClip clip )
        {
            foreach( KeyValuePair <string, MovieClip> p in s_clip_cache_dic )
            {
                if( p.Value == clip )
                {
                    s_clip_cache_dic.Remove(p.Key);
                    return true;
                }
            }
            return false;
        }

        static public MovieClip Create( string filename, LOAD_METHOD load_method,
                                        FilterMode filter_mode = FilterMode.Bilinear,
                                        TextureWrapMode wrap_mode = TextureWrapMode.Clamp)
        {
            string key = filename + "_@" + filter_mode.ToString() + "_@" + wrap_mode.ToString();

            MovieClip clip = (MovieClip)GetClipInCache( key );
            if( clip == null )
            {
                clip = new MovieClip();

                if( clip.Load( filename, load_method, filter_mode, wrap_mode ) )
                {
                    AddClipInCache(key, clip);
                }
                
            }

            return clip;
        }

        static public MovieClip Create( Stream stream, LOAD_METHOD load_method,
                                          FilterMode filter_mode = FilterMode.Bilinear,
                                          TextureWrapMode wrap_mode = TextureWrapMode.Clamp)
        {
            string hash_code = stream.GetHashCode().ToString();
            string key = hash_code + "_@" + filter_mode.ToString() + "_@" + wrap_mode.ToString();

            MovieClip clip = (MovieClip)GetClipInCache( key );
            if( clip == null )
            {
                clip = new MovieClip();

                if( clip.Load( stream, load_method, filter_mode, wrap_mode ) )
                    AddClipInCache(key, clip);
            }

            return clip;
        }
#endregion


#region Values & Functions

        zneGIF.Frame    _frame      = null;
        Color[]         _background = null;
        float           _frame_time = 0.0f;
        bool            _is_playing = false;
        float           _speed      = 1.0f;

        // Draw image of loder to Texture2D
        void DrawFrame( int index )
        {
            zneGif.Frame src_frame = _loader._frames[index];

            int ix = src_frame._image.desc.image_left;
            int iy = src_frame._image.desc.image_top;
            int iw = src_frame._image.desc.image_width;
            int ih = src_frame._image.desc.image_height;

            Color[] pixels = _frame._texture.GetPixels(ix, iy, iw, ih);

            zneGif.Color[] color_table = _loader._global_color_table;
            if (src_frame._image.desc.local_color_table_flag)
                color_table = src_frame._image.desc.local_color_table;

            int data_length         = src_frame._image.data.Length;
            bool transparent        = src_frame._GCE_data.transparent_color_flag;
            int transparent_index   = src_frame._GCE_data.transparent_color;
            for (int i = 0; i < data_length; i++)
            {
                int idx = src_frame._image.data[i];

                if (transparent && transparent_index == idx)
                    continue;

                zneGif.Color zneColor = color_table[idx];

                pixels[i].r = (float)zneColor.r / 255.0f;
                pixels[i].g = (float)zneColor.g / 255.0f;
                pixels[i].b = (float)zneColor.b / 255.0f;
                pixels[i].a = 1;
            }

            Color[] backup_color = null;
            int disposal_method = src_frame._GCE_data.disposal_method;
            if (disposal_method == 3)
            {
                backup_color = _frame._texture.GetPixels(ix, iy, iw, ih);
            }

            _frame._texture.SetPixels(ix, iy, iw, ih, pixels);
            _frame._texture.Apply();


            if (disposal_method == 2)
            {
                // clear by background
                _frame._texture.SetPixels(ix, iy, iw, ih, _background);
            }
            else if (disposal_method == 3)
            {
                // back to previous image
                _frame._texture.SetPixels(ix, iy, iw, ih, backup_color);
            }

            _frame._comment = src_frame._CME_data.comment;
            _frame._delay_time = (float)src_frame._GCE_data.delay_time * 0.01f;
            _frame._index = index;
            _frame_time = 0;

            if( _loader.IsDone() )
                _frame._is_last_frame = (index == (_loader._frames.Count-1));
            else
                _frame._is_last_frame = false;
            
        }

        // It will override at child
        public override bool CreateNextFrame()
        {
            if( _frame == null )
            {
                // Create Frame
                _width  = _loader._logical_screen_desc.image_width;
                _height = _loader._logical_screen_desc.image_height;
                
                _frame = new Frame();
                _frame._index = -1;
                _frame._delay_time = 0;
                _frame._texture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
                _frame._texture.filterMode = _filter_mode;
                _frame._texture.wrapMode = _wrap_mode;
                _frame._is_last_frame = false;
                _frame._comment = null;
                _frame._is_last_frame = false;

                // Create background pixels
                int     pixel_count = _width * _height;
                int     bg_index    = _loader._logical_screen_desc.background_color;
                bool    transparent = _loader._frames[0]._GCE_data.transparent_color_flag;
                Color bg_pixel      = Color.clear;
                _background         = new Color[pixel_count];
                zneGif.Color[] color_table = _loader._global_color_table;
                
                if (!transparent && color_table != null && bg_index < color_table.Length)
                {
                    bg_pixel.r = (float)color_table[bg_index].r / 255.0f;
                    bg_pixel.g = (float)color_table[bg_index].g / 255.0f;
                    bg_pixel.b = (float)color_table[bg_index].b / 255.0f;
                    bg_pixel.a = 1;
                }

                for (int i = 0; i < pixel_count; i++)
                    _background[i] = bg_pixel;

                _frame._texture.SetPixels( _background );

                // zneGifMovieClip use only 1 texture.
                _frames = new Frame[1];
                _frames[0] = _frame;
                _frame_count = 1;

                // Draw first frame
                DrawFrame( 0 );
            }

            if( _loader.IsDoneWithoutError() )
                OnLoadComplete();

            return true;
        }
    
        internal override void OnUpdate( float delta_time )
        {
            if( _frame == null || !_is_playing )
                return;

            _frame_time += delta_time * _speed;
            if( _frame_time < _frame._delay_time )
                return;

            if (_loader.IsDone())
            {
                int index = _frame._index + 1;

                if (index >= _loader._frames.Count)
                    index = 0;

                DrawFrame(index);
            }
            else if( _loader._frames != null )
            {
                int index = _frame._index + 1;

                if (index < _loader._frames.Count)
                    DrawFrame(index);
            }
        }

        public zneGIF.Frame GetFrame()
        {
            return _frame;
        }

        public override int GetFrameCount()
        {
            if (_loader != null && _loader._frames != null)
                return _loader._frames.Count;

            return 0;
        }

        public override Frame GetFrame(int index)
        {
            return _frame;
        }

        public void SetSpeed( float speed )
        {
            _speed = speed;
        }

        public float GetSpeed()
        {
            return _speed;
        }


        public void Play()
        {
            _is_playing = true;
        }

        public bool IsPlaying()
        {
            return _is_playing;
        }

        public void Stop()
        {
            _is_playing = false;

             if( _frame != null )
             {
                DrawFrame(0);
                return;
             }
        }

        public void Pause()
        {
            _is_playing = false;
        }
#endregion

    }
}