using UnityEngine;

public class BgGraph : MonoBehaviour {
    public DiabetesSimulator diabetesSimulator;
    private NGraph mGraph;
    // The live updating plot for the X axis
    private NGraphDataSeriesXyLiveTransient mPlotX;
    // The live updating plot for the Z axis
    private NGraphDataSeriesXyLiveTransient mPlotZ;
    public PlayerInput playerInput;

    public int xMin = 0;
    public int xMax = 10;
    public int yMin = 0;
    public int yMax = 10000;
    public Vector2 _axesDrawAt = new Vector2(-10, -10);

    private void Awake() {
        diabetesSimulator = FindObjectOfType<DiabetesSimulator>();
        playerInput = FindObjectOfType<PlayerInput>();
        // Setup the graph
        mGraph = GetComponentInChildren<NGraph>();
        mGraph.setRanges(xMin, xMax, yMin, yMax);
        mGraph.AxesDrawAt = _axesDrawAt;

        // Make the two plots
        mPlotX = mGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("Bg", Color.green);
        mPlotZ = mGraph.addDataSeries<NGraphDataSeriesXyLiveTransient>("Movement", Color.blue);

        // Change thier update rates to be quicker
        mPlotX.UpdateRate = 0.01f;
        mPlotZ.UpdateRate = 0.01f;
    }

    // Update is called once per frame
    private void Update() {
        mPlotX.UpdateValue = diabetesSimulator.BloodSugar;
        //mPlotZ.UpdateValue = effectVal * 100;
    }
}