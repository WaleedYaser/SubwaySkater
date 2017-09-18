using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private const float COIN_SCORE_AMOUNT = 5.0f;

	public static GameManager Instance { set; get; }

	private bool isGameStarted;
	private PlayerMotor motor;

	// UI
	public Text scoreText, coinText, modifierText;
	private float score, lastScore, coinScore, modifierScore;

	private void Awake()
	{
		Instance = this;
		modifierScore = 1;

		scoreText.text = score.ToString ("0");
		coinText.text = coinScore.ToString ("0");
		modifierText.text = "x" + modifierScore.ToString ("0.0");

		motor = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMotor> ();
	}

	private void Update()
	{
		if (MobileInput.Instance.Tap && !isGameStarted) {
			isGameStarted = true;
			motor.StartRunning ();
		}

		if (isGameStarted) {
			score += (Time.deltaTime * modifierScore);

			if (lastScore != (int)score) {
				lastScore = (int) score;
				scoreText.text = score.ToString ("0");
			}
		}
	}

	public void GetCoin()
	{
		coinScore++;
		coinText.text = coinScore.ToString ("0");
		score += COIN_SCORE_AMOUNT;
		scoreText.text = score.ToString("0");

	}
		
	public void UpdateModifier(float modifierAmount)
	{
		modifierScore = 1.0f + modifierAmount;
		modifierText.text = "x" + modifierScore.ToString ("0.0");
	}
}
