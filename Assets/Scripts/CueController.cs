﻿using UnityEngine;
using System.Collections;

public class CueController : MonoBehaviour {
	public class StrikeMessage {
		public float StrikeSpeed { get; private set; }
		public float ForceMultiplier { get; private set; }
		public GameObject CueBall { get; private set; }

		public StrikeMessage(float strikeSpeed, float forceMultiplier, GameObject cueBall) {
			StrikeSpeed = strikeSpeed;
			ForceMultiplier = forceMultiplier;
			CueBall = cueBall;
		}
	}

	private StrikeMessage strike;
	private Vector3 cueBallOffset;

	void Start () {
	
	}

	public void Reset(GameObject cueBall) {
		transform.position = cueBall.transform.position + cueBallOffset;
		GetComponent<Collider>().enabled = true;
		foreach (var renderer in GetComponentsInChildren<Renderer>())
			renderer.enabled = true;
	}

	void FixedUpdate () {
		if (strike != null)
			transform.Translate(strike.StrikeSpeed * Vector3.up * Time.deltaTime);
	}
	
	void OnCollisionEnter(Collision collision) {
		if (strike == null || collision.gameObject != strike.CueBall)
			return;

		var lastStrike = strike;
		strike = null;
		lastStrike.CueBall.GetComponent<Rigidbody>().AddForceAtPosition(collision.contacts[0].normal * lastStrike.ForceMultiplier, collision.contacts[0].point);
		GetComponent<Collider>().enabled = false;
		foreach (var renderer in GetComponentsInChildren<Renderer>())
			renderer.enabled = false;
	}

	void Strike(StrikeMessage strike) {
		cueBallOffset = transform.position - strike.CueBall.transform.position;
		this.strike = strike;
	}
}
