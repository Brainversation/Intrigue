using UnityEngine;
using System.Collections;

public class Strobe : MonoBehaviour {

	public float StrobeRate;
	public Light StrobeLight;
	public bool DynamicIntensity;
	private float realStrobe;

	void Start () {
		StartCoroutine("Strobinator");
	}
	
	IEnumerator Strobinator(){
		while(true){
			realStrobe = (1-StrobeRate/100);
			StrobeLight.enabled = !StrobeLight.enabled;
			if(DynamicIntensity)
				StrobeLight.intensity = Random.Range(1,10);
			yield return new WaitForSeconds(realStrobe);
		}
	}
}
