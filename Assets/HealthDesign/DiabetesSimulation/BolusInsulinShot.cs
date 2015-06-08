using System;
using AssemblyCSharp;	
public class BolusInsulinShot : IBloodSugarAffector{
	double timeStepAdministered;
	double unitsInsulin;
		
	public BolusInsulinShot(double startTime, double insulinUnits)
	{
		this.Name = "Insulin (Bolus)";
		this.timeStepAdministered = startTime;
		this.unitsInsulin = insulinUnits;
	}
	
	public bool IsExpired(double tick){
		return tick > timeStepAdministered + this.getDurationInSeconds();
	}

	public string Name { get; private set; }

	public float GetAlterationForTick(double tick, IDiabetesPatient patient){
		if(tick >= this.timeStepAdministered && !this.IsExpired (tick)){
			return Convert.ToSingle(LibGlucoDyn.deltaBGI(tick-this.timeStepAdministered, this.unitsInsulin, patient.getInsulinSensitivity(), patient.getPersonalInsulinDuration()));
		}
		return 0;
	}

	public int getDurationInSeconds(){//TODO refactor to property and fix your terrible naming convention compliance
		return 300;
	}
}