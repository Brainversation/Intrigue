using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClothesChanger : MonoBehaviour {

	public modelType mType;
	public GameObject[] objectsToChange;
	private List<Color> colors = new List<Color>();
	private Material[] mats;
	public enum modelType{
		Blue, Red, Yellow, Puce,
	}

	// Use this for initialization
	void Start () {
		AddColors();
		
		if(mType == modelType.Blue){
			mats = gameObject.renderer.materials;
			Color newColor = colors[Random.Range(0,colors.Count)];
			int i = 0;
			foreach(Material mat in mats){
				if(i>=5){
					if(i == 6)
						mat.SetColor("_Color",colors[Random.Range(0,colors.Count)]);
					else
						mat.SetColor("_Color", newColor);
				}
				++i;
			}
		}
		if(mType == modelType.Yellow){
			Color newColor = colors[Random.Range(0,colors.Count)];
			foreach(GameObject obj in objectsToChange){
				foreach(Material mat in obj.renderer.materials){
					mat.SetColor("_Color", newColor);
				}
			}
		}
	}

	void AddColors(){
		colors.Add(new Color(36f/255f,25f/255f,178f/255f));
		colors.Add(new Color(53f/255f,47f/255f,133f/255f));
		colors.Add(new Color(16f/255f,8f/255f,115f/255f));
		colors.Add(new Color(87f/255f,77f/255f,216f/255f));
		colors.Add(new Color(121f/255f,114f/255f,216f/255f));
		colors.Add(new Color(217f/255f,0f/255f,91f/255f));
		colors.Add(new Color(163f/255f,41f/255f,92f/255f));
		colors.Add(new Color(141f/255f,0f/255f,59f/255f));
		colors.Add(new Color(236f/255f,59f/255f,134f/255f));
		colors.Add(new Color(236f/255f,106f/255f,161f/255f));
		colors.Add(new Color(59f/255f,130f/255f,61f/255f));
		colors.Add(new Color(53f/255f,140f/255f,121f/255f));
		colors.Add(new Color(135f/255f,59f/255f,43f/255f));


	}
}
