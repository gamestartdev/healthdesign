	
	public class InsulinShot : IBloodSugarAffector{
		InsulinType typeOfShot;
		double timeStepAdministered;
		long onset;
		long peak;
		long duration;
		
		public InsulinShot(InsulinType type, double startTime){
			this.typeOfShot = type;
			this.timeStepAdministered = startTime;
			this.onset = randomNumberBetween(typeOfShot.onsetDelayInTicksMinimum,typeOfShot.onsetDelayInTicksMaximum);
			this.peak = randomNumberBetween(typeOfShot.peakDelayInTicksMinimum,typeOfShot.peakDelayInTicksMaximum);
			this.duration = randomNumberBetween(typeOfShot.durationInTicksMinimum,typeOfShot.durationInTicksMaximum);
		}
	
		public bool isExpired(double tick){
			return tick > timeStepAdministered + duration;
		}
		
		public float getAlterationForTick(double tick){
			if(tick < timeStepAdministered + onset){
				return 0;
			}else if(tick < timeStepAdministered + peak){
				long range = (peak-onset);
				long progress = (int)(tick-timeStepAdministered + onset);
				return -typeOfShot.infusionRateInMilligramsPerKilogramPerMinute * (progress/range); 
			}
			else{
				long range = (duration-peak);
				long progress = (int)(tick-timeStepAdministered + peak);
				return -typeOfShot.infusionRateInMilligramsPerKilogramPerMinute * ((range-progress)/range);
			}
		}
		private static int randomNumberBetween(int min,int max){
		return new System.Random().Next(min,max);
	}
	}