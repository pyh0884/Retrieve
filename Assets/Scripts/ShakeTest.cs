using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTest : MonoBehaviour
{
	public float vibrationLeft = 2;
	public float vibrationRight = 1;
	public float vibrationTime = 0.3f;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void StartVibration(float leftStrength, float rightStrength, float time = 0.5f)
	{
		StartCoroutine(Vibration(leftStrength, rightStrength, time));
	}

	public IEnumerator Vibration(float left,float right,float time=0.5f)
	{
		try
		{
			XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, left, right);
			yield return new WaitForSeconds(time);
			XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
		}
		finally {
			XInputDotNetPure.GamePad.SetVibration(XInputDotNetPure.PlayerIndex.One, 0, 0);
		}
	}
}
