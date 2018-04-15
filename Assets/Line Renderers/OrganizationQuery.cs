using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizationQuery : MonoBehaviour {

	public float lineWidth = 0.01f;
	public float firstLineStartTime = 57.0f;
	public float secondLineStartTime = 59.0f;
	public float holdTime = 1f;
	public float lerpTime = 1f;
	private LineRenderer lineRenderer;
	private LineRenderer lineRenderer2;
	private GameObject userNode;
	private GameObject kycNode;
	private GameObject company;

	void Start () {
		company = GameObject.Find ("Company Star");
		userNode = GameObject.Find ("Hyperledger/Origin (4)/User Node");
		kycNode = GameObject.Find ("Hyperledger/Origin (1)/KYC Node");
		Invoke("BlockchainToCompany", firstLineStartTime);
		Invoke("UserAuthorizeToCompany", secondLineStartTime);
	}

	void BlockchainToCompany () {
		lineRenderer = this.gameObject.AddComponent<LineRenderer> ();
		lineRenderer.SetWidth (lineWidth, lineWidth);
		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.SetVertexCount (2);
		lineRenderer.SetPosition (0, kycNode.transform.position);
		StartCoroutine (LineDraw (lineRenderer, 1, kycNode.transform.position, company.transform.position, lerpTime));
	}

	void UserAuthorizeToCompany () {
		lineRenderer2 = GameObject.FindGameObjectWithTag ("AuthorizeLine").AddComponent<LineRenderer> ();
		lineRenderer2.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer2.SetWidth (lineWidth, lineWidth);
		lineRenderer2.SetColors (new Color (0,255,0), new Color (0,255,0));
		lineRenderer2.SetVertexCount (2);
		lineRenderer2.SetPosition (0, userNode.transform.position);
		StartCoroutine (LineDraw (lineRenderer2, 1, userNode.transform.position, company.transform.position, lerpTime));
		Invoke("EraseUserAuthorize", 1f);
	}

	void EraseUserAuthorize () {
		StartCoroutine (LineDraw (lineRenderer2, 0, userNode.transform.position, company.transform.position, lerpTime));
	}

	IEnumerator LineDraw(LineRenderer lineRenderer, int targetPosition, Vector3 source, Vector3 target, float time) {
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
}
