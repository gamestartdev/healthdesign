using AssemblyCSharp;

public interface IDiabetesPatient
{
	double getInsulinSensitivity();
	double getCarbRatio();
	double getInitialBG();
	int getPersonalInsulinDuration();
}
