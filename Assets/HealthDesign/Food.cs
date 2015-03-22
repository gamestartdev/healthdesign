using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour
{

    public int strength = 100;
    public int timeScale = 600;
    private DiabetesSimulator _simulator;

	void Start ()
	{
	    _simulator = GameObject.FindObjectOfType<DiabetesSimulator>();
	    var collider = gameObject.AddComponent<BoxCollider2D>();
	    collider.isTrigger = true;
	    gameObject.layer = LayerMask.NameToLayer("Food");
	}
	
    public void Eat(PlayerInput playerInput)
    {
        _simulator.addFood(strength, timeScale);
        Destroy(gameObject);
    }
}
