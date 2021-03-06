using UnityEngine;
using System.Collections;

public interface IBloodSugarAffector {

    float GetAlterationForTick(double tick, IDiabetesPatient patient);
    bool IsExpired(double tick);
    string Name { get; }
}
