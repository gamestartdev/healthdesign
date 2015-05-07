	
	public class InsulinShot : IBloodSugarAffector{
		InsulinType typeOfShot;
		double timeStepAdministered;
		long onset;
		long peak;
		long duration;

        private float _lastAlteration;
        public float LastAlteration {
            get {
                return _lastAlteration;
            }
        }

		public InsulinShot(InsulinType type, double startTime, string name="Insulin")
		{
		    this.Name = name;
			this.typeOfShot = type;
			this.timeStepAdministered = startTime;
			this.onset = randomNumberBetween(typeOfShot.onsetDelayInTicksMinimum,typeOfShot.onsetDelayInTicksMaximum);
			this.peak = randomNumberBetween(typeOfShot.peakDelayInTicksMinimum,typeOfShot.peakDelayInTicksMaximum);
			this.duration = randomNumberBetween(typeOfShot.durationInTicksMinimum,typeOfShot.durationInTicksMaximum);
		}
	
		public bool IsExpired(double tick){
			return tick > timeStepAdministered + duration;
		}

	    public string Name { get; private set; }

	    public float GetAlterationForTick(double tick){
			if(tick < timeStepAdministered + onset){
				return 0;
			}else if(tick < timeStepAdministered + peak){
				long range = (peak-onset);
				long progress = (int)(tick-timeStepAdministered + onset);
                _lastAlteration =  -typeOfShot.infusionRateInMilligramsPerKilogramPerMinute * (progress / range);
			    return _lastAlteration;
			}
			else{
				long range = (duration-peak);
				long progress = (int)(tick-timeStepAdministered + peak);
                _lastAlteration  = -typeOfShot.infusionRateInMilligramsPerKilogramPerMinute * ((range - progress) / range);
			    return _lastAlteration;
			}
		;}

		private static int randomNumberBetween(int min,int max){
		    return new System.Random().Next(min,max);
	    }

    }