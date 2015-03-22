using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HeroControl))]
public class NpcAI : MonoBehaviour {

    HeroControl _hero_con;
    Vector2 _random_pos;
    float _random_speed;
    float _delay_time = 0;

	// Use this for initialization
	void Start () {
	    _hero_con = GetComponent<HeroControl>();
	}
	
	// Update is called once per frame
	void Update () {
        
        _delay_time -= Time.deltaTime;

        if (_delay_time < 0)
            RandomAction();
        	
	}

    void RandomAction()
    {
        _hero_con.StopMove();

        if (Random.Range(0, 10) < 3)
        {
            // Stop motion
            _delay_time = 2.0f;
        }
        else
        {
            float x = Random.Range(-1000, 1000);
            float y = Random.Range(-1000, 1000);
            float speed = Random.Range(20, 100);
            _delay_time = Random.Range(2, 8);

            _hero_con.StartMove(x, y, speed);
        }
    }
}
