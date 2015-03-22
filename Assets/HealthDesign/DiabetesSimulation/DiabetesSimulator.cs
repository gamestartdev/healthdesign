using UnityEngine;
using System.Collections.Generic;

public class DiabetesSimulator : MonoBehaviour {
	
	public float timeScale = 1f;// time scale multiplier
	private long timeStep;
	public int insulinStrengthScaleFactor = 10;
	public float bloodSugarDangerUpperThreshold = 1000;
	public float bloodSugarDangerLowerThreshold = -200;
	public int insulinTimeScaleFactor = 10;
	public int foodStrengthScaleFactor = 2;
	public int foodTimeScaleFactor = 2;
	
	InsulinType LYSPRO_ASPART_GLULISINE;
	InsulinType REGULAR;
	InsulinType DETEMIR_GLARGINE;

	private List<IBloodSugarAffector> _affectors;
    public List<IBloodSugarAffector> Affectors
    {
        get { return new List<IBloodSugarAffector>(_affectors); }
    }

    private float bloodSugarMilligramsPerDecaliter;
    public float BloodSugar
    {
        get { return bloodSugarMilligramsPerDecaliter; }
    }


    void Start () {
		timeStep = 0;
		bloodSugarMilligramsPerDecaliter = 0;
		_affectors = new List<IBloodSugarAffector>();
		LYSPRO_ASPART_GLULISINE = new InsulinType("Lyspro/Aspart/Glulisine",1*insulinTimeScaleFactor,1*insulinTimeScaleFactor,4*insulinTimeScaleFactor,8*insulinTimeScaleFactor,16*insulinTimeScaleFactor,24*insulinTimeScaleFactor,7*(insulinStrengthScaleFactor/10));
		REGULAR = new InsulinType("Regular Insulin", 2*insulinTimeScaleFactor,4*insulinTimeScaleFactor,8*insulinTimeScaleFactor,16*insulinTimeScaleFactor,24*insulinTimeScaleFactor,32*insulinTimeScaleFactor,6*(insulinStrengthScaleFactor/10));
		DETEMIR_GLARGINE = new InsulinType("Detemir/Glargine",8*insulinTimeScaleFactor,12*insulinTimeScaleFactor,32*insulinTimeScaleFactor,40*insulinTimeScaleFactor,96*insulinTimeScaleFactor,192*insulinTimeScaleFactor,0.8f*(insulinStrengthScaleFactor/10));
	}
	
	void Update () {
		applyBloodSugarUpdates();
		removeOutdatedAffectors();
		advanceTime ();
        analytics();
	}
	
	void OnGUI(){
		showInsulinButtons();
		showFoodButtons();
		showBar();
		showBloodSugarInfo();
	}

    enum BloodSugarWarning
    {
        NEUTRAL,
        LOW,
        HIGH
    }

    private BloodSugarWarning _warning = BloodSugarWarning.NEUTRAL;
    void analytics()
    {
        
    }

	void showBar(){
		GUI.Box(new Rect(Screen.width / 3, Screen.height/4, this.bloodSugarMilligramsPerDecaliter,Screen.height/4),new GUIContent());
	}
	
	void showBloodSugarInfo(){
		GUI.Label(new Rect(Screen.width  / 3, Screen.height / 3 * 2, 100,100),""+this.bloodSugarMilligramsPerDecaliter);

		if(this.bloodSugarMilligramsPerDecaliter > bloodSugarDangerUpperThreshold){
			GUI.Label(new Rect(Screen.width  / 3+120, Screen.height / 3 * 2, 300,100),"Your blood sugar is dangerously high!");
		}else if(this.bloodSugarMilligramsPerDecaliter < bloodSugarDangerLowerThreshold){
			GUI.Label(new Rect(Screen.width  / 3+120, Screen.height / 3 * 2, 300,100),"Your blood sugar is dangerously low!");
		}
	}

	void showFoodButtons(){
		GUI.Label(new Rect(Screen.width / 8 * 6, Screen.height / 20, Screen.width / 8, Screen.height / 8), "EAT SOME FOOD!");
		if(GUI.Button(new Rect(Screen.width/10  * 7,Screen.height/10,Screen.width/6,Screen.height/6),"Hamburger")){
			this.addAffector(new Meal(timeStep,300*foodStrengthScaleFactor,650*foodTimeScaleFactor));
		}
		if(GUI.Button(new Rect(Screen.width/10  * 7,Screen.height/10*4,Screen.width/6,Screen.height/6),"Grilled Kale")){
			this.addAffector(new Meal(timeStep,60*foodStrengthScaleFactor,600*foodTimeScaleFactor));
		}
		if(GUI.Button(new Rect(Screen.width/10  * 7,Screen.height/10*7,Screen.width/6,Screen.height/6),"Baked Potato")){
			this.addAffector(new Meal(timeStep,200*foodStrengthScaleFactor,1000*foodTimeScaleFactor));
		}
	}

    public void addFood(int strength, int time)
    {
        this.addAffector(new Meal(timeStep, strength * foodStrengthScaleFactor, time * foodTimeScaleFactor));
    }
	
	void showInsulinButtons(){
		GUI.Label(new Rect(Screen.width / 8, Screen.height / 20, Screen.width / 8, Screen.height / 8), "TAKE SOME INSULIN!");
		if(GUI.Button(new Rect(Screen.width/10,Screen.height/10,Screen.width/6,Screen.height/6),"Long-term Insulin")){
			this.addAffector(new InsulinShot(DETEMIR_GLARGINE,timeStep));
		}
		if(GUI.Button(new Rect(Screen.width/10,Screen.height/10*4,Screen.width/6,Screen.height/6),"Regular Insulin")){
			this.addAffector(new InsulinShot(REGULAR,timeStep));
		}
		if(GUI.Button(new Rect(Screen.width/10,Screen.height/10*7,Screen.width/6,Screen.height/6),"Instant Insulin")){
			this.addAffector(new InsulinShot(LYSPRO_ASPART_GLULISINE,timeStep));
		}
	}
	
	public void addAffector(IBloodSugarAffector affector){
		this._affectors.Add(affector);
	}
	
	private void applyBloodSugarUpdates(){
		foreach(IBloodSugarAffector affector in this._affectors){
			this.bloodSugarMilligramsPerDecaliter += affector.getAlterationForTick(timeStep);
		}
	}
	
	private void removeOutdatedAffectors(){
		foreach(IBloodSugarAffector affector in new List<IBloodSugarAffector>(this._affectors)){
			if(affector.isExpired(timeStep)){
				this._affectors.Remove(affector);
			}	
		}
	}
	
	private void advanceTime(){
		timeStep += (int)timeScale;
	}

}

