using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Cloud.Analytics;

public class DiabetesSimulator : MonoBehaviour, IDiabetesPatient {

 
	public float timeScale = 1f;// time scale multiplier
	private long timeStep;
    public float HighSugar = 10000;
    public float LowSugar = 100;

	//TODO: Look into (ask Nate?) whether these ranges of values can be enforced by the inspector somehow
	public int insulinDurationInHours = 1;	//1-6 in GlucoDyn, by 1
	public double carbohydrateNeutralizationRatioInGramsToUnitsInsulin = 10.0d; //1-100 in GlucoDyn, by 0.5
	public double insulinSensitivityFactorInMgPerDlToUnitsInsulin = 1.0d;	//1-100 in GlucoDyn, by 0.5
	public double initialBloodGlucoseLevelInMilligramsPerDecaliter = 10.0d; //1-300 in GlucoDyn, by 10

    private List<IBloodSugarAffector> _affectors = new List<IBloodSugarAffector>();
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
        const string projectId = "d1f9f021-f08d-4c81-9979-ecbad58b42b8";
        UnityAnalytics.StartSDK(projectId);

		timeStep = 0;
		bloodSugarMilligramsPerDecaliter = (HighSugar - LowSugar)/2;

	}
	
	void FixedUpdate () {
		applyBloodSugarUpdates();
		removeOutdatedAffectors();
		advanceTime ();
        analytics();
	}
	
	void OnGUI(){
		showInsulinButtons();
		//showFoodButtons();+
		showBar();
		showBloodSugarInfo();
	    showAffectorNames();
	}

    private void showAffectorNames()
    {
        GUILayout.Label("Current Blood Sugar Affects:");

        foreach (var affector in _affectors)
        {
            GUILayout.Button(affector.Name);
        }
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
        BloodSugarWarning currentWarning = BloodSugarWarning.NEUTRAL;
        if(this.bloodSugarMilligramsPerDecaliter > HighSugar)
            currentWarning = BloodSugarWarning.HIGH;
        if(this.bloodSugarMilligramsPerDecaliter < LowSugar)
            currentWarning = BloodSugarWarning.LOW;
        if (currentWarning != _warning)
        {
            UnityAnalytics.CustomEvent("bloodSugarState", new Dictionary<string, object>
            {
                { "warning", currentWarning },
                { "bloodSugarMilligramsPerDecaliter", this.bloodSugarMilligramsPerDecaliter }
            });
            Debug.Log("Changing state: "+ currentWarning);
            _warning = currentWarning;
            StopAllCoroutines();
            if (_warning == BloodSugarWarning.HIGH || _warning == BloodSugarWarning.LOW)
            {
                StartCoroutine(ReloadLevel());
            }

        }
        if (transform.position.y < -100)
        {
            Application.LoadLevel(Application.loadedLevelName);
            UnityAnalytics.CustomEvent("gameOver", new Dictionary<string, object>
            {
                { "reason", "Fell off ledge"},
            });
        }
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(2f);
        Application.LoadLevel(Application.loadedLevelName);
    }

	void showBar(){
		GUI.Box(new Rect(0, Screen.height/6*4.5f, this.bloodSugarMilligramsPerDecaliter*(Screen.width/HighSugar),Screen.height/12),new GUIContent());
	}
	
	void showBloodSugarInfo(){
        GUI.Label(new Rect(20, Screen.height / 6 * 4.5f, 100, 100), "" + this.bloodSugarMilligramsPerDecaliter);

		if(this.bloodSugarMilligramsPerDecaliter > HighSugar){
            GUI.Label(new Rect(120, Screen.height / 6 * 4.5f, 300, 100), "Your blood sugar is dangerously high!");
		}else if(this.bloodSugarMilligramsPerDecaliter < LowSugar){
            GUI.Label(new Rect(120, Screen.height / 6 * 4.5f, 300, 100), "Your blood sugar is dangerously low!");
		}
	}

	void showFoodButtons(){
		GUI.Label(new Rect(Screen.width / 8 * 6, Screen.height / 20, Screen.width / 8, Screen.height / 8), "EAT SOME FOOD!");
		if(GUI.Button(new Rect(Screen.width/10  * 7,Screen.height/10,Screen.width/6,Screen.height/6),"Hamburger")){
			this.addAffector(new Meal(timeStep,300,90));
		}
		if(GUI.Button(new Rect(Screen.width/10  * 7,Screen.height/10*4,Screen.width/6,Screen.height/6),"Grilled Kale")){
			this.addAffector(new Meal(timeStep,60,240));
		}
		if(GUI.Button(new Rect(Screen.width/10  * 7,Screen.height/10*7,Screen.width/6,Screen.height/6),"Baked Potato")){
			this.addAffector(new Meal(timeStep,200,240));
		}
	}

    public void addFood(int strength, int time, string name)
    {
        this.addAffector(new Meal(timeStep, strength, time, name));
    }
	
	void showInsulinButtons(){

		if(GUI.Button(new Rect(0,Screen.height/6*5,Screen.width/3,Screen.height/6),"Basal Insulin\r\n.002 units/tick\r\n2400 ticks")){
			double insulinPerMinute = .002;
			int minutesOfActivity=2400;
            this.addAffector(new BasalInsulinShot(timeStep, insulinPerMinute, minutesOfActivity));
		}
		if(GUI.Button(new Rect(Screen.width/3,Screen.height/6*5,Screen.width/3,Screen.height/6),"Bolus Insulin\r\n5 units")){
			int unitsOfInsulin = 5;
			this.addAffector(new BolusInsulinShot(timeStep,unitsOfInsulin));
		}
		if(GUI.Button(new Rect(Screen.width/3*2,Screen.height/6*5,Screen.width/3,Screen.height/6),"Bolus Insulin\r\n10 units")){
			int unitsOfInsulin = 10;
			this.addAffector(new BolusInsulinShot(timeStep,unitsOfInsulin));
		}
	}
	
	public void addAffector(IBloodSugarAffector affector){
		this._affectors.Add(affector);
	}
	
	private void applyBloodSugarUpdates(){
		foreach(IBloodSugarAffector affector in this._affectors){
			this.bloodSugarMilligramsPerDecaliter += affector.GetAlterationForTick(timeStep, this);
		}
	}
	
	private void removeOutdatedAffectors(){
		foreach(IBloodSugarAffector affector in new List<IBloodSugarAffector>(this._affectors)){
			if(affector.IsExpired(timeStep)){
				this._affectors.Remove(affector);
			}	
		}
	}
	
	private void advanceTime(){
		timeStep += (int)timeScale;
	}

	public double getInsulinSensitivity(){
		return this.insulinSensitivityFactorInMgPerDlToUnitsInsulin;
	}
	public double getCarbRatio(){
		return this.carbohydrateNeutralizationRatioInGramsToUnitsInsulin;
	}
	public double getInitialBG(){
		return this.initialBloodGlucoseLevelInMilligramsPerDecaliter;
	}
	public int getPersonalInsulinDuration(){
		return this.insulinDurationInHours;
	}

}

