	public class Meal : IBloodSugarAffector {
		public int carbohydratesInGrams;
		public long durationInTicks;
		public double timeStepEaten;
		
		public Meal(double time, int carbs, long duration){
			this.carbohydratesInGrams = carbs;
			this.durationInTicks = duration;
			this.timeStepEaten = time;
		}
		public bool isExpired(double tick){
			return tick > timeStepEaten + durationInTicks;
		}
		public float getAlterationForTick(double tick){
			return (float)this.carbohydratesInGrams / (float)this.durationInTicks;
		}
	}