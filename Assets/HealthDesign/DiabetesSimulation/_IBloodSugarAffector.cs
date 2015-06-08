using UnityEngine;
using System.Collections;

public interface _IBloodSugarAffector {
	float GetAlterationForTick(double tick, IDiabetesPatient patient);
	bool IsExpired(double tick);
	bool HasBegun(double tick);
    string Name { get; }
}
