using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCryptoLetters : MonoBehaviour {

	public float startTime = 80f;
	string[] names = new string[] {"3f39fb04", "df39fb04", "Af39fb04","A139fb04","AM39fb04","AM69fb04","AMO9fb04","AMOe9fb04","AMOSfb04","AMOScb04","AMOS b04","AMOS d04","AMOS Y04","AMOS Ye4","AMOS YE4","AMOS YE6","AMOS YEE"};

	void Start () {
		Invoke ("Later", startTime);
	}

	void Later() {
		Text text = GetComponent<Text> ();
		StartCoroutine(Cycle (text));
	}

	IEnumerator Cycle(Text t) {
		for (int i = 0; i < names.Length; i++) {
			t.text = names [i];
			yield return new WaitForSecondsRealtime (Random.value * 0.2f);
		}
	}
}
