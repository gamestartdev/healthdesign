using UnityEngine;
using System.Collections;


public class ScreenTouch : MonoBehaviour {

    private int _touch_id = -1;
    private readonly int MOUSE_TOUCH_ID = -1000;
    private object[] _msg_value_v2 = new object[2];

	// Use this for initialization
	void Start () {
            _touch_id = -1;
	}
	
	// Update is called once per frame
	void Update () {

        if (_touch_id == -1)
        {
#if (!UNITY_IPHONE && !UNITY_ANDROID) || UNITY_EDITOR
            // Use Mouse
            if (Input.GetMouseButtonDown(0))
            {
                _touch_id = MOUSE_TOUCH_ID;
                _msg_value_v2[0] = Input.mousePosition.x;
                _msg_value_v2[1] = Input.mousePosition.y;
                gameObject.SendMessage("OnTouchEnter", _msg_value_v2);
                return;
            }
#else // Use Touch
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    _touch_id = touch.fingerId;
                    
                    _msg_value_v2[0] = touch.position.x;
                    _msg_value_v2[1] = touch.position.y;
                    gameObject.SendMessage("OnTouchEnter", _msg_value_v2);
                    return;
                }
            }
#endif


        }
        // be touching
        else
        {

#if (!UNITY_IPHONE && !UNITY_ANDROID) || UNITY_EDITOR
            // Use Mouse
            if (Input.GetMouseButton(0))
            {
                _msg_value_v2[0] = Input.mousePosition.x;
                _msg_value_v2[1] = Input.mousePosition.y;
                gameObject.SendMessage("OnTouchDrag", _msg_value_v2);
                return;
            }
#else // Use Touch
			for( int i=0; i<Input.touchCount; i++ )
			{
				Touch touch =  Input.GetTouch(i);
				
				if( touch.fingerId == _touch_id )
				{
					
					if( touch.phase == TouchPhase.Stationary )
						return; // don't moved
					
					if( touch.phase == TouchPhase.Moved )
					{
						// Dragging
                        _msg_value_v2[0] = touch.position.x;
                        _msg_value_v2[1] = touch.position.y;
                        gameObject.SendMessage("OnTouchDrag", _msg_value_v2);

						return;
					}
					
					if( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
					{
						break;
					}
				}
			}
#endif

            // Touch Exited
            gameObject.SendMessage("OnTouchExit");
            _touch_id = -1;
        }
	}
}

