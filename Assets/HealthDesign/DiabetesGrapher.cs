using System.Collections.Generic;
using UnityEngine;

public class DiabetesGrapher : MonoBehaviour
{

    private DiabetesSimulator _simulator;
        
    [Range(10, 200)]
	public int resolution = 10;
	public float scaleX = 5;
	private int currentResolution;
	private ParticleSystem.Particle[] points;
    private Stack<float> yValues;
	
	private void CreatePoints () {
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		float increment = 1f / (resolution - 1) * scaleX;
		for (int i = 0; i < resolution; i++) {
			float x = i * increment;
			points[i].position = new Vector3(x, 0f, 0f);
			points[i].color = new Color(x, 0f, 0f);
			points[i].size = 0.1f;
		}
	}

    void Start()
    {
        _simulator = GameObject.FindObjectOfType<DiabetesSimulator>();
    }
	
	void Update () {
		if (currentResolution != resolution || points == null) {
			CreatePoints();
		}
		for (int i = 0; i < resolution; i++) {
			Vector3 p = points[i].position;

			p.y = PositionY(p.x);

			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
		}
		GetComponent<ParticleSystem>().SetParticles(points, points.Length);
	}

    private float PositionY(float x)
    {
        float ratio = ((_simulator.BloodSugar - _simulator.LowSugar) /
                   _simulator.HighSugar);

        return ratio;
    }
	
	private static float Linear (float x) {
		return x;
	}
	
	private static float Exponential (float x) {
		return x * x;
	}

	private static float Parabola (float x){
		x = 2f * x - 1f;
		return x * x;
	}

	private static float Sine (float x){
		return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
	}
}