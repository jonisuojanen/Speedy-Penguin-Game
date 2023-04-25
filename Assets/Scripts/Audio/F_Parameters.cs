using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Parameters : MonoBehaviour
{

public void Paused()
{
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Paused", 1);
}

public void NotPaused()
{
    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Paused", 0);
}

}
