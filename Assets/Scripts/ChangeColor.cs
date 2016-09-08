using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {

	bool drag = false;
	Material originalMaterial;
	Renderer rend;
	GameObject cube;
	Color color;

	// Use this for initialization
	void Start () {
		cube = transform.Find("Cube").gameObject;
		rend = cube.GetComponent<Renderer>();
		rend.enabled = true;
		originalMaterial = rend.sharedMaterial;
	}

	void Update()
	{
		color = rend.material.color;
		color.b = 255.0f;
		if (drag) {
			rend.sharedMaterial.color = color;
		} else {
			rend.sharedMaterial = originalMaterial;
		}
	}

	public void setDrag(){
		drag = true;
	}

	public void StopDrag()
	{
		drag = false;
	}
}
