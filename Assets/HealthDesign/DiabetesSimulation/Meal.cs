using System;
using AssemblyCSharp;
public class Meal : IBloodSugarAffector {
		public double carbohydratesInGrams;
		public double absorptionFactor;
		public double timeStepEaten;
		
		public Meal(double timeConsumed, double carbsContainedInGrams, double carbTypeAbsorptionFactor, string name="Meal")
		{
		    this.Name = name;
			this.carbohydratesInGrams = carbsContainedInGrams;
			this.absorptionFactor = carbTypeAbsorptionFactor;
			this.timeStepEaten = timeConsumed;
		}
		public bool IsExpired(double tick){
			return tick > timeStepEaten + 300;
		}

	    public string Name { get; private set; }

	    public float GetAlterationForTick(double tick, IDiabetesPatient patient){
			return Convert.ToSingle (LibGlucoDyn.deltaBGC(tick-this.timeStepEaten, patient.getInsulinSensitivity(), patient.getCarbRatio(), Convert.ToInt32(this.carbohydratesInGrams), Convert.ToInt32(this.absorptionFactor)));
		}
	}