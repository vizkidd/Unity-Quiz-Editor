using UnityEngine;
using System.Collections;

public class RuntimeDebug : MonoBehaviour {

	public void Log(string val)
    {
        Debug.Log(val);
    }

    public static void StaticLog(string val)
    {
        Debug.Log(val);
    }
}
