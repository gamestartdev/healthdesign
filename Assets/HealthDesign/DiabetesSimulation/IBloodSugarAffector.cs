using UnityEngine;
using System.Collections;

public interface IBloodSugarAffector {
    float LastAlteration { get; }
	float GetAlterationForTick(double tick);
	bool IsExpired(double tick);
    string Name { get; }
}
