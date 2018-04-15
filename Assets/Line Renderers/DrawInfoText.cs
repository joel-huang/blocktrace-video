using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawInfoText : MonoBehaviour {

	public Vector3 kneePos = Vector3.one;
	public Vector3 endPos = Vector3.right;
	public float widthMultiplier = 0.01f;
	public float thighMultiplier = 0.1f;
	public float calfMultiplier = 0.1f;
	public float lerpTime = 1f;
	public float waitTime = 25.0f;
	public float holdTime = 3.0f;
	private LineRenderer lr;

	private void Start() {
		Invoke ("StartDrawing", waitTime);
	}

	private void StartDrawing() {
		lr = transform.gameObject.AddComponent<LineRenderer>();
		lr.widthMultiplier = widthMultiplier;
		lr.material = new Material (Shader.Find("Mobile/Particles/Additive"));
		Vector3 target = transform.position + kneePos * thighMultiplier;
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, target);
		StartCoroutine(LineDraw2(0,1));
	}

	private void Next() {
		lr.positionCount++;
		Vector3 target = lr.GetPosition(1) + endPos * calfMultiplier;
		lr.SetPosition (2, target);
		StartCoroutine(LineDraw3(1,2));
	}

	private void StartErase() {
		StartCoroutine (LineDraw (2, 1));
	}

	private void EraseLast() {
		lr.positionCount--;
		StartCoroutine (Erase (1, 0));
	}

	IEnumerator LineDraw(int first, int second) {
		float t = 0;
		float time = lerpTime;
		Vector3 orig = lr.GetPosition(first);
		Vector3 orig2 = lr.GetPosition(second);
		lr.SetPosition(2, orig);
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(orig, orig2, t / time);
			lr.SetPosition(2, newpos);
			yield return null;
		}
		lr.SetPosition(2, orig2);
		EraseLast ();
	}

	IEnumerator Erase(int first, int second) {
		float t = 0;
		float time = lerpTime;
		Vector3 orig = lr.GetPosition(first);
		Vector3 orig2 = lr.GetPosition(second);
		lr.SetPosition(1, orig);
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(orig, orig2, t / time);
			lr.SetPosition(1, newpos);
			yield return null;
		}
		lr.SetPosition (1, orig2);
	}

	IEnumerator LineDraw2(int first, int second) {
		float t = 0;
		float time = lerpTime;
		Vector3 orig = lr.GetPosition(first);
		Vector3 orig2 = lr.GetPosition(second);
		lr.SetPosition(1, orig);
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(orig, orig2, t / time);
			lr.SetPosition(1, newpos);
			yield return null;
		}
		lr.SetPosition(1, orig2);
		Next ();
	}

	IEnumerator LineDraw3(int first, int second) {
		float t = 0;
		float time = lerpTime;
		Vector3 orig = lr.GetPosition(first);
		Vector3 orig2 = lr.GetPosition(second);
		lr.SetPosition(2, orig);
		Vector3 newpos;
		for (; t < time; t += Time.deltaTime)
		{
			newpos = Vector3.Lerp(orig, orig2, t / time);
			lr.SetPosition(2, newpos);
			yield return null;
		}
		lr.SetPosition(2, orig2);
		Invoke ("StartErase", holdTime);
	}
}
