using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml.Schema;
using Newtonsoft.Json.Utilities;

public class FoodDocument {
    private Consumable _consumable;
    private WorldObject _worldObject;

    public FoodDocument()  : this(InstantiateFood()) {}
    private static Consumable InstantiateFood() {
        return new GameObject("Consumable").AddComponent<Consumable>();
    }

    public FoodDocument(Consumable consumable) {
        _consumable = consumable;
        _worldObject = _consumable.GetComponent<WorldObject>();
        _consumable.Document = this;
    }
//
//    public int FoodIndex {
//        get {
//            var worldState = Object.FindObjectOfType<WorldState>();
//            return worldState.FoodsModels.IndexOf(t => t.Model.name == _worldObject.Model.name);
//        }
//        set {
//            var worldState = Object.FindObjectOfType<WorldState>();
//            Object.Destroy();
//            var model = GameObject.Instantiate(worldState.FoodsModels[value]) as Transform;
//
//        }
//    }

    public int strength { get { return _consumable.strength; } set { _consumable.strength = value; } }
    public int timeScale { get { return _consumable.timeScale; } set { _consumable.timeScale = value; } }

    public Vector3 position {
        get { return _consumable.transform.position; }
        set { _consumable.transform.position = new Vector3(value.x, value.y, 0); }
    }
}

public class Consumable : MonoBehaviour
{
    public string Name = "Consumable";
    public int strength = 100;
    public int timeScale = 600;
    public int _unitsOfInsulin = 0;
    public FoodDocument Document;

    public Vector3 position {
        get {  return transform.position; }
        set { transform.position = new Vector3(value.x, value.y, 0); }
    }
    private DiabetesSimulator _simulator;

    void Awake() {
        //This has to be in awake because of how WorldBuilder sets the layer of 
        // tool items to be HUD immidiately after instantiating them
        gameObject.layer = WorldBuilder.ConsumableLayer;
    }

	void Start ()
	{
        if (Document == null) {
            Document = new FoodDocument(this);
        }
	    _simulator = GameObject.FindObjectOfType<DiabetesSimulator>();
        var collider = GetComponent<Collider2D>();
        if (!collider) collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
	}
	
    public void Eat(PlayerInput playerInput)
    {
        _simulator.addFood(strength, timeScale, Name);
        _simulator.addInsulin(_unitsOfInsulin);
        Destroy(gameObject);
    }
}
