using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SimpleAnimator))]
[RequireComponent(typeof(CharacterController))]
public class HeroControl : MonoBehaviour 
{ 
    SimpleAnimator _animator;
    CharacterController _char_con;

    float _moveSpeed = 50;
    bool _moving = false;
    Vector3 _moving_dir;

	// Use this for initialization
	void Start () 
    {
        _animator = GetComponent<SimpleAnimator>();
        _char_con = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (_moving)
        {
            Vector3 move = new Vector3(_moving_dir.x, -2, _moving_dir.y) * _moveSpeed * Time.deltaTime;
            _char_con.Move(move);

            if (Mathf.Abs(_moving_dir.x) > Mathf.Abs(_moving_dir.y))
            {
                if (_moving_dir.x < 0)
                    _animator.SetAnimation("walk_left");
                else if (_moving_dir.x > 0)
                    _animator.SetAnimation("walk_right");
            }
            else
            {
                if (_moving_dir.y < 0)
                    _animator.SetAnimation("walk_front");
                else if (_moving_dir.y > 0)
                    _animator.SetAnimation("walk_back");
            }
            
            _animator.GetCurrentAnimation().SetSpeed(_moveSpeed / 40.0f);
              
        }
        else
        {
            Vector3 move = new Vector3(0, -2, 0) * _moveSpeed * Time.deltaTime;
            _char_con.Move(move);

            _animator.SetAnimation("idle");
            _animator.GetCurrentAnimation().SetSpeed(1);
        }
    }

    public void StartMove(float dir_x, float dir_y, float speed )
    {
        _moving = true;
        _moving_dir.x = dir_x;
        _moving_dir.y = dir_y;
        _moving_dir.z = 0;
        _moving_dir.Normalize();
        _moveSpeed = speed;
    }

    public void StopMove()
    {
        _moving = false;
    }

    void OnTouchEnter(object[] pos_v2)
    {
        Vector3 touch_pos = new Vector3((float)pos_v2[0], (float)pos_v2[1], 0);
        Vector3 myPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = touch_pos - myPos;

        StartMove(dir.x, dir.y, _moveSpeed);
    }

    void OnTouchDrag(object[] pos_v2)
    {
        Vector3 touch_pos = new Vector3((float)pos_v2[0], (float)pos_v2[1], 0);
        Vector3 myPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = touch_pos - myPos;

        StartMove(dir.x, dir.y, _moveSpeed);
    }

    void OnTouchExit()
    {
        StopMove();
    }

}
