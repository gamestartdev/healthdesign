	public class InsulinType{
		public string insulinTypeName;
		public int onsetDelayInTicksMinimum;
		public int onsetDelayInTicksMaximum;
		public int peakDelayInTicksMinimum;
		public int peakDelayInTicksMaximum;
		public int durationInTicksMinimum;
		public int durationInTicksMaximum;
		public float infusionRateInMilligramsPerKilogramPerMinute;
		
		public InsulinType(string name, int onsetMin, int onsetMax, int peakMin, int peakMax, int durationMin, int durationMax, float infusionRate){
			this.insulinTypeName=name;
			this.onsetDelayInTicksMinimum = onsetMin;
			this.onsetDelayInTicksMaximum = onsetMax;
			this.peakDelayInTicksMinimum = peakMin;
			this.peakDelayInTicksMaximum = peakMax;
			this.durationInTicksMinimum = durationMin;
			this.durationInTicksMaximum = durationMax;
			this.infusionRateInMilligramsPerKilogramPerMinute = infusionRate;
		}

		public InsulinType(string name, int onsetDelay, int peak, int duration, float infusionRate) : this(name,onsetDelay,onsetDelay,peak,peak,duration,duration,infusionRate){}
	}