using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTest : MonoBehaviour
{
	public float vibrationStrength = 2;
	public float vibrationTime = 0.3f;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	IEnumerator Vibration(int mode)
	{
		XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, vibrationStrength, vibrationStrength);
		yield return new WaitForSeconds(vibrationTime);
		XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
	}
}
