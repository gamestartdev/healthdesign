using UnityEngine;
using System.Collections;

public interface IBloodSugarAffector {
	float getAlterationForTick(double tick);
	bool isExpired(double tick);
}
