using System;
using AssemblyCSharp;	
	public class BasalInsulinShot : IBloodSugarAffector {
		double timeStepAdministered;
		double unitsInsulinPerMinute;
		int durationInMinutes;
		
		public BasalInsulinShot(double startTime, double insulinPerMinute, int minutesOfActivity)
		{
		    this.Name = "Insulin (Basal)";
			this.timeStepAdministered = startTime;
			this.unitsInsulinPerMinute = insulinPerMinute;
			this.durationInMinutes = minutesOfActivity;
		}
	
		public bool IsExpired(double tick){
			return tick > timeStepAdministered + this.durationInMinutes;
		}

	    public string Name { get; private set; }

	    public float GetAlterationForTick(double tick, IDiabetesPatient patient){
			if(tick >= this.timeStepAdministered && !this.IsExpired (tick)){
				return Convert.ToSingle(LibGlucoDyn.deltatempBGI(tick, this.unitsInsulinPerMinute, patient.getInsulinSensitivity(), patient.getPersonalInsulinDuration(), Convert.ToInt64(timeStepAdministered), Convert.ToInt64(this.timeStepAdministered + this.durationInMinutes)));
			}
			return 0;
		}
}