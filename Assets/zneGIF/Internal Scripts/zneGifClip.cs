// (c) Copyright 'ZNE Edu' 2013. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace zneGIF
{
    public enum LOAD_METHOD
    {
        NO_FRAME_AT_FIRST,     // No frame, When call Clip.Create. after next loading.
        ONE_FRAME_AT_FIRST,    // Load first frame, When call Clip.Create. after next loading.
        ALL_FRAME_AT_FIRST,    // Load all frame at once, When call Clip.Create. 
    }

    public class Clip
    {

        public enum STATUS
        {
            VOID,                   // Not started Load yet.
            OPENNING_FILE,          // Step1. File open or download.
            CREATING_STREAM_BY_WWW,   // Step2. Create stream.
            STREAM_CREATED,
            LOADING_GIF,            // Step3. Load GIF form stream.
            CREATING_FRAME,         // Step4. Create frame.
            DONE,                   // Done.
        }

        public   string          _name           = "";
        public   string          _filename       = null;
        internal string          _error          = null;
        internal zneGif.Loader   _loader         = null;
        internal LOAD_METHOD     _load_method    = LOAD_METHOD.ONE_FRAME_AT_FIRST;
        internal FilterMode      _filter_mode    = FilterMode.Bilinear;
        internal TextureWrapMode _wrap_mode      = TextureWrapMode.Clamp;
        internal WWW             _www            = null;
        internal Stream          _stream         = null;
        internal STATUS          _status         = STATUS.VOID;
        internal Frame[]         _frames         = null;
        internal int             _frame_count    = 0;
        internal int             _width          = 0;
        internal int             _height         = 0;
        internal int             _last_update_tick = 0;
        internal bool            _is_single_frame = false;


        internal virtual void OnLoadComplete()
        {
            _status = STATUS.DONE;
            
            // Check if single frame GIF 
            _is_single_frame = (_loader._frames.Count == 1);

            Debug.Log(Path.GetFileName(_name) + " Load Completed (" + GetWidth() + "x" + GetHeight() + ", " + GetFrameCount() + " frames)");
        }

        internal virtual void OnLoadFail( string error )
        {
            SetError( error );
            _status = STATUS.DONE;
            
            Debug.Log(Path.GetFileName(_name) + " Load Failed!\nError: " + _error );
        }

        // Load by filename
        public virtual bool Load( string filename , 
                                    LOAD_METHOD load_method, 
                                    FilterMode filter_mode = FilterMode.Bilinear,
                                    TextureWrapMode wrap_mode = TextureWrapMode.Clamp )
        {
            _loader     = new zneGif.Loader();
            _error      = null;
            _frames     = null;
            _frame_count= 0;
            _filename   = filename;
            _load_method= load_method;
            _filter_mode= filter_mode;
            _wrap_mode  = wrap_mode;
            _status     = STATUS.OPENNING_FILE;


#if UNITY_WEBPLAYER
            // If load method not NO_FRAME_AT_FIRST then sometimes application is stoped in WebPlayer
            // So, We force change it to NO_FRAME_AT_FIRST. in WebPlayer.
            _load_method = LOAD_METHOD.NO_FRAME_AT_FIRST;
#endif

            bool use_force_www = _load_method == LOAD_METHOD.NO_FRAME_AT_FIRST;

            if( !CreateStream( use_force_www ) )
            {
                OnLoadFail(_error);
                return false;
            }
            
            if( _load_method != LOAD_METHOD.NO_FRAME_AT_FIRST )
            {
                while( _stream == null  )   
                {
                    if( !ChecktoWWWIsGood() )
                    {
                        OnLoadFail( _error );
                        return false;
                    }
                }

                if( !LoadGIF( _stream ) )
                {
                    OnLoadFail( _error );
                    return false;
                }
            }

            return true;
        }

        // Load by stream
        public virtual bool Load( Stream stream, 
                                    LOAD_METHOD load_method, 
                                    FilterMode filter_mode = FilterMode.Bilinear,
                                    TextureWrapMode wrap_mode = TextureWrapMode.Clamp )
        {
            _loader     = new zneGif.Loader();
            _error      = null;
            _frames     = null;
            _frame_count= 0;
            _filename   = null;
            _load_method= load_method;
            _filter_mode= filter_mode;
            _wrap_mode  = wrap_mode;
            _status     = STATUS.STREAM_CREATED;

            if( stream == null )
            {
                OnLoadFail( "Load by stream failed, Stream was null!" );
                return false;
            }
            _stream = stream;

           if( !LoadGIF( _stream ) )
            {
                OnLoadFail( _error );
                return false;
            }

            return true;
        }

        // Load Next Data
        // return true - No error keep going
        // return false - Has error, load failed
        public virtual bool LoadNextData()
        {
            if ( !IsLoading() )
            {
                SetError( "LoadNextData failed. Invalid call" ); 
                return false;
            }

            // Create stream
            if( _status < STATUS.STREAM_CREATED )
            {
                if( ChecktoWWWIsGood() && _stream != null )
                {
                    LoadGIF( _stream );
                }

                if( HasError() )
                {
                    OnLoadFail( _error );
                    return false;
                }
            }

            // Load GIF And Create Frame
            else if( _status < STATUS.DONE )
            {
                // Load GIF partial data
                if( !_loader.IsDone() && !_loader.AsyncNextLoad() )
                {
                    OnLoadFail( _loader.GetLastError().ToString() );
                    return false;
                }

                // Create zneGIF frame
                if( _loader._frames != null && _loader._frames.Count > 0 )
                {
                    _status = STATUS.CREATING_FRAME;
                    CreateNextFrame();
                }
            }

            return true;
        }

        // It will override at child
        public virtual bool CreateNextFrame()
        {
            OnLoadComplete();
            return true;
        }

        // return true = success
        // return false = fail, has error
        internal virtual bool CreateStream( bool use_force_www )
        {
            _stream = null;

            if( _filename == null || _filename == null )
            {
                SetError( "Can't create stream. Bacause _filename is none" );
                return false;
            }

            string lower_filename = _filename.ToLower();
            
            // If not url, make prefix
            if( use_force_www && 
                !lower_filename.Contains("file://") &&
                !lower_filename.Contains("http://") &&
                !lower_filename.Contains("https://") )
            {
                _filename = "file://" + _filename;
                lower_filename = "file://" + lower_filename;
            }


            // Check It is url? or filepath
            if( lower_filename.Contains("file://") ||
                lower_filename.Contains("http://") ||
                lower_filename.Contains("https://"))
            {
                // Load with WWW
                _www = new WWW(_filename);
                _status = STATUS.CREATING_STREAM_BY_WWW;
            }
            else
            {
                // Load with IO.File
                if( File.Exists(_filename) )
                {
                    _stream = File.OpenRead(_filename);
                }
                else
                {
                    SetError("[" + Path.GetFileName(_filename) + "] Not exists file." );
                    return false;
                }

                if( _stream == null )
                {
                    SetError("[" + Path.GetFileName(_filename) + "] File open failed!" );
                    return false;
                }

                _status = STATUS.STREAM_CREATED;
            }

            return true;
        }

        // retrun true = No error keep going.
        // return false = Has error. Something is wrong.
        internal virtual bool ChecktoWWWIsGood()
        {
            if( _status != STATUS.CREATING_STREAM_BY_WWW )
            {
                SetError("[" + Path.GetFileName(_filename) + "] Create stream failed, Invalid loading step.");
                return false;
            }

            if( _www == null )
            {
                SetError("[" + Path.GetFileName(_filename) + "] Create stream failed, WWW object is null");
                return false;
            }

            if( _www.isDone )
            {
                if (_www.error != null)
                {
                    SetError("[" + Path.GetFileName(_filename) + "] Create stream failed, www.error=" + _www.error);
                    return false;
                }

                _stream = new MemoryStream(_www.bytes);
                _www.Dispose();
                _www = null;

                if( _stream == null )
                {
                    SetError("[" + Path.GetFileName(_filename) + "] new MemoryStream Failed!" );
                    return false;
                }

                _status = STATUS.STREAM_CREATED;
            }

            return true;
        }

        // return true = Load success
        // return false = Load failed. has error
        internal virtual bool LoadGIF( Stream stream )
        {
            if( stream == null )
            {
                SetError( "LoadGIF failed. stream was null!" );
                return false;
            }

            
            _status = STATUS.LOADING_GIF;

            switch (_load_method)
            {
                // NO_FRAME_AT_FIRST
                case LOAD_METHOD.NO_FRAME_AT_FIRST:
                    if( !_loader.AsyncLoad(stream) )
                    {
                        SetError( _loader.GetLastError().ToString() );
                        return false;
                    }
                    break;

                // ONE_FRAME_AT_FIRST
                case LOAD_METHOD.ONE_FRAME_AT_FIRST:
                    if( _loader.AsyncLoad(stream) )
                    {
                        while( _frame_count == 0 )
                        {
                            if( !LoadNextData() )
                            {
                                SetError( _loader.GetLastError().ToString() );
                                return false;
                            }
                        }
                    }
                    break;

                // ALL_FRAME_AT_FIRST
                case LOAD_METHOD.ALL_FRAME_AT_FIRST:
                    if( _loader.Load( stream ) )
                    {
                        while( !IsDone() )
                        {
                            if( !LoadNextData() )
                            {
                                SetError( _loader.GetLastError().ToString() );
                                return false;
                            }
                        }
                    }
                    break;
            }

            return true;
        }

        public virtual void Update()
        {
            if( _last_update_tick == Time.frameCount )
                return;

            _last_update_tick = Time.frameCount;

            if( IsLoading() )
            {
                if( !LoadNextData() )
                {
                    OnLoadFail( _error );
                    return;
                }
            }

            OnUpdate( Time.deltaTime );
        }

        internal virtual void OnUpdate( float delta_time )
        {
        }
       
        public virtual STATUS GetStatus()
        {
            return _status;
        }

        public virtual bool IsVoid()
        {
            return (_status == STATUS.VOID);
        }

        public virtual bool IsLoading()
        {
            return ( _status > STATUS.VOID && _status < STATUS.DONE );
        }

        public virtual bool IsDone()
        {
            return _status == STATUS.DONE;
        }

        public virtual bool IsDoneWithoutError()
        {
            return (_status == STATUS.DONE && _error == null);
        }

        public virtual bool HasError()
        {
            return (_error != null);
        }

        internal virtual void SetError( string error )
        {
            _error = error;
        }

        public virtual string GetError()
        {
            return _error;
        }

        public virtual int GetWidth()
        {
            return _width;
        }

        public virtual int GetHeight()
        {
            return _height;
        }

        public virtual int GetFrameCount()
        {
            return _frame_count;
        }

        public virtual Frame GetFrame(int index)
        {
            if (_frames != null && index >= 0 && index < _frames.Length)
                return _frames[index];

            return null;
        }

    }

}
