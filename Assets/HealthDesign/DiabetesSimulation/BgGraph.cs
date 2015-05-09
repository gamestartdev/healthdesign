/***********************************************************************
/*      Copyright Niugnep Software, LLC 2013 All Rights Reserved.      *
/*                                                                     *
/*     THIS WORK CONTAINS TRADE SECRET AND PROPRIETARY INFORMATION     *
/*     WHICH IS THE PROPERTY OF NIUGNEP SOFTWARE, LLC OR ITS           *
/*             LICENSORS AND IS SUBJECT TO LICENSE TERMS.              *
/**********************************************************************/

using UnityEngine;

public class BgGraph : MonoBehaviour {
    public DiabetesSimulator diabetesSimulator;
    private NGraph mGraph;
    // The live updating plot for the X axis
    private NGraphDataSeriesXyLiveTransient mPlotX;
    // The live updating plot for the Z axis
    private NGraphDataSeriesXyLiveTransient mPlotZ;
    public PlayerInput playerInput;

    private void Awake() {
        diabetesSimulator = FindObjectOfType<DiabetesSimulator>();
        playerInput = FindObjectOfType<PlayerInput>();
        // Setup the graph
        mGraph = GetComponent<NGraph>();
        mGraph.setRanges(0, 10, 0, 400);
        mGraph.AxesDrawAt = new Vector2(-10, -10);

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

        var effectVal = 0f;
        foreach (var affector in diabetesSimulator.Affectors) {
            effectVal += affector.LastAlteration;
        }
        mPlotZ.UpdateValue = effectVal * 100;
    }
}