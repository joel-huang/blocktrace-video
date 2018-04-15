using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadToBlockchain : MonoBehaviour {

	public float lineWidth = 0.01f;
	public float startTime = 32.0f;
	public float holdTime = 1f;
	public float lerpTime = 2.5f;
	public float lerpTime2 = 0.25f;
	public float renewTime = 85.0f;
	public float teardownTime = 98f;
	private LineRenderer lineRenderer;
	private GameObject[] stars;
	private GameObject earth;
	private GameObject userNode;
	private GameObject kycNode;

	void Start () {
		earth = GameObject.Find ("Earth");
		userNode = GameObject.Find ("Hyperledger/Origin (4)/User Node");
		kycNode = GameObject.Find ("Hyperledger/Origin (1)/KYC Node");
		stars = GameObject.FindGameObjectsWithTag ("Star");
		Invoke("EarthToUser", startTime);
		Invoke ("EraseEarthToUser", startTime + holdTime);
		Invoke("UserToKYC", startTime + lerpTime + holdTime + 0.2f);
		Invoke ("StartRenew", renewTime);
		Invoke ("Teardown", teardownTime);
	}

	void EarthToUser () {
		lineRenderer = this.gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetWidth(lineWidth, lineWidth);
		lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition (0, earth.transform.position);
		StartCoroutine (LineDraw (1, earth.transform.position, userNode.transform.position, lerpTime));
	}

	void EraseEarthToUser() {
		StartCoroutine (LineDraw (0, earth.transform.position, userNode.transform.position, lerpTime));
	}

	void UserToKYC() {
		StartCoroutine (LineDraw (1, userNode.transform.position, kycNode.transform.position, lerpTime2));
		Invoke ("DrawBlockchain", 1f);
	}

	void Teardown() {
		for (int i = 0; i < stars.Length; i++) {
			StartCoroutine(Teardown(stars[i]));
			StartCoroutine (Teardown2 (stars [i]));
		}
		StartCoroutine (Teardown3 ());
	}

	IEnumerator LineDraw(int targetPosition, Vector3 source, Vector3 target, float time) {
		float t = 0;
		lineRenderer.SetPosition(targetPosition, source);
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			lineRenderer.SetPosition(targetPosition, newpos);
			yield return null;
		}
		lineRenderer.SetPosition(targetPosition, target);
	}

	IEnumerator LineDraw2(int targetPosition, Vector3 source, Vector3 target, float time) {
		float t = 0;
		lineRenderer.SetPosition(targetPosition, source);
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			lineRenderer.SetPosition(targetPosition, newpos);
			yield return null;
		}
		lineRenderer.SetPosition(targetPosition, target);
	}

	void DrawBlockchain() {
		for (int i = 0; i < stars.Length; i++) {
			StartCoroutine (DrawConnection (stars[i]));
		}
	}

	void StartRenew() {
		Debug.Log ("started renew");
		for (int i = 0; i < stars.Length; i++) {
			StartCoroutine (Renew(stars[i]));
		}
		Debug.Log ("started greenlines");
		for (int i = 0; i < stars.Length; i++) {
			StartCoroutine (Greenlines(stars[i]));
		}
	}

	IEnumerator DrawConnection(GameObject star) {
		LineRenderer l = star.AddComponent<LineRenderer> ();
		l.SetWidth(lineWidth, lineWidth);
		l.material = new Material (Shader.Find("Particles/Additive"));
		l.SetVertexCount(2);
		float time = 0.7f;
		float t = 0;
		l.SetPosition(0, kycNode.transform.position);
		Vector3 source = kycNode.transform.position;
		Vector3 target = star.transform.position;
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			l.SetPosition(1, newpos);
			yield return null;
		}
		l.SetPosition(1, target);
	}

	IEnumerator Renew(GameObject star) {
		LineRenderer l = star.GetComponent<LineRenderer> ();
		float time = 1f;
		float t = 0;
		// clear white lines
		// first move the source to endpoint
		Vector3 source = kycNode.transform.position;
		Vector3 target = star.transform.position;
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			l.SetPosition(0, newpos);
			yield return null;
		}
		l.SetPosition(0, target);
	}

	IEnumerator Greenlines(GameObject star) {
		GameObject slave = new GameObject ();
		slave.transform.SetParent (star.transform);
		LineRenderer g = slave.AddComponent<LineRenderer> ();
		g.SetPosition(0, kycNode.transform.position);
		g.SetPosition (1, kycNode.transform.position);
		g.SetWidth(lineWidth, lineWidth);
		g.SetColors (new Color (0,255,0), new Color (0,255,0));
		g.material = new Material (Shader.Find("Particles/Additive"));
		g.SetVertexCount(2);
		float time = 0.7f;
		float t = 0;
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(kycNode.transform.position, star.transform.position, t / time);
			g.SetPosition(1, newpos);
			yield return null;
		}
		g.SetPosition(1, star.transform.position);
		StartCoroutine (FadeWhite (g));
	}

	IEnumerator FadeWhite(LineRenderer line) {
		float time = 2f;
		float t = 0;
		Color color;
		Color green = new Color (0f, 1f, 0f, 1f);
		Color white = new Color (1f,1f,1f,1f);
		for (; t < time; t += Time.deltaTime) {
			color = Color.Lerp (green, white, t/time);
			Debug.Log (color);
			line.startColor = color;
			line.endColor = color;
			yield return null;
		}
	}

	IEnumerator Teardown(GameObject star) {
		LineRenderer l = star.transform.Find ("New Game Object").GetComponent<LineRenderer>();
		float time = 1f;
		float t = 0;
		// clear white lines
		// first move the source to endpoint
		Vector3 source = kycNode.transform.position;
		Vector3 target = star.transform.position;
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			l.SetPosition(0, newpos);
			yield return null;
		}
		l.SetPosition(0, target);
	}

	IEnumerator Teardown2(GameObject star) {
		LineRenderer l = star.GetComponent<LineRenderer> ();
		float time = 1f;
		float t = 0;
		// clear white lines
		// first move the source to endpoint
		Vector3 source = kycNode.transform.position;
		Vector3 target = star.transform.position;
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			l.SetPosition(0, newpos);
			yield return null;
		}
		l.SetPosition(0, target);
	}

	IEnumerator Teardown3() {

		float time = 1f;
		float t = 0;
		// clear white lines
		// first move the source to endpoint
		Vector3 source = userNode.transform.position;
		Vector3 target = kycNode.transform.position;
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(source, target, t / time);
			lineRenderer.SetPosition(0, newpos);
			yield return null;
		}
		lineRenderer.SetPosition(0, target);

		GameObject company = GameObject.Find ("Company Star");
		LineRenderer coyLine = GameObject.FindGameObjectWithTag ("Problem").GetComponent<LineRenderer> ();
		t = 0;
		Vector3 sauce = kycNode.transform.position;
		Vector3 taget = company.transform.position;
		Vector3 newpos2;
		for (; t < time; t += Time.deltaTime)
		{
			newpos2 = Vector3.Lerp(sauce, taget, t / time);
			coyLine.SetPosition(0, newpos2);
			yield return null;
		}
		coyLine.SetPosition(0, taget);

		ParticleSystem p = company.GetComponentInChildren<ParticleSystem> ();
		p.Stop();
		Destroy (p);
	}
}
