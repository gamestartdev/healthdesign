	public class Meal : IBloodSugarAffector {
		public int carbohydratesInGrams;
		public long durationInTicks;
		public double timeStepEaten;
		
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
			return (float)this.carbohydratesInGrams / (float)this.durationInTicks;
		}
	}