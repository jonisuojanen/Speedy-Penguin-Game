using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Banks : MonoBehaviour
{
    public string LoadBankAwake;
    public string LoadBankAwakeStrings;

    public string UnloadBankDestroy;
    public string UnloadBankDestroyStrings;

    private void Awake()
    {
        FMODUnity.RuntimeManager.LoadBank(LoadBankAwake);
        //FMODUnity.RuntimeManager.LoadBank(LoadBankAwakeStrings);
    }

    private void OnDestroy()
    {
        FMODUnity.RuntimeManager.UnloadBank(UnloadBankDestroy);
        //FMODUnity.RuntimeManager.UnloadBank(UnloadBankDestroyStrings);
    }

}
