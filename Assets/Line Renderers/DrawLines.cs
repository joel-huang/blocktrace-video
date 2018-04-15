using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour {

	private LineRenderer lineRenderer;


	void Start () {
		GameObject[] stars = GameObject.FindGameObjectsWithTag ("Star");
		lineRenderer = this.gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetWidth(0.02F, 0.02F);
		lineRenderer.material = new Material (Shader.Find("Mobile/Particles/Additive"));
		lineRenderer.SetVertexCount(stars.Length);
		Debug.Log (stars.Length);

		for (int i = 0; i < stars.Length-1; i++) {
			try {
				lineRenderer.SetPosition (2*i, stars[i].transform.position);
				lineRenderer.SetPosition (2*i+1, stars[i+1].transform.position);
			} catch (UnityException e) {
				Debug.Log (e.StackTrace);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		float theta = Time.timeSinceLevelLoad / 10f;
		float distance = 0.001f*Mathf.Sin(theta);
		lineRenderer.SetWidth (distance, distance);

		
	}
}
