using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml.Schema;
using Newtonsoft.Json.Utilities;

public class FoodDocument {
    private Food _food;
    private WorldObject _worldObject;

    public FoodDocument()  : this(InstantiateFood()) {}
    private static Food InstantiateFood() {
        return new GameObject("Food").AddComponent<Food>();
    }

    public FoodDocument(Food food) {
        _food = food;
        _worldObject = _food.GetComponent<WorldObject>();
        _food.Document = this;
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

    public int strength { get { return _food.strength; } set { _food.strength = value; } }
    public int timeScale { get { return _food.timeScale; } set { _food.timeScale = value; } }

    public Vector3 position {
        get { return _food.transform.position; }
        set { _food.transform.position = new Vector3(value.x, value.y, 0); }
    }
}

public class Food : MonoBehaviour
{
    public string Name = "Food";
    public int strength = 100;
    public int timeScale = 600;
    public FoodDocument Document;

    public Vector3 position {
        get {  return transform.position; }
        set { transform.position = new Vector3(value.x, value.y, 0); }
    }
    private DiabetesSimulator _simulator;
	void Start ()
	{
        if (Document == null) {
            Document = new FoodDocument(this);
        }
	    _simulator = GameObject.FindObjectOfType<DiabetesSimulator>();
        var collider = GetComponent<Collider2D>();
        if (!collider) collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
	    gameObject.layer = LayerMask.NameToLayer("Food");
	}
	
    public void Eat(PlayerInput playerInput)
    {
        _simulator.addFood(strength, timeScale, Name);
        Destroy(gameObject);
    }
}
