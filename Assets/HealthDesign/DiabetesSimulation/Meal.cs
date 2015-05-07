	public class Meal : IBloodSugarAffector {
		public int carbohydratesInGrams;
		public long durationInTicks;
		public double timeStepEaten;

	    private float _lastAlteration;
	    public float LastAlteration  {
	        get {
	            return _lastAlteration;
	        } 
	    }
		public Meal(double time, int carbs, long duration, string name="Meal")
		{
		    this.Name = name;
			this.carbohydratesInGrams = carbs;
			this.durationInTicks = duration;
			this.timeStepEaten = time;
		}
		public bool IsExpired(double tick){
			return tick > timeStepEaten + durationInTicks;
		}

	    public string Name { get; private set; }


	    public float GetAlterationForTick(double tick){
			_lastAlteration =  (float)this.carbohydratesInGrams / (float)this.durationInTicks;
	        return _lastAlteration;
	;    }
	}