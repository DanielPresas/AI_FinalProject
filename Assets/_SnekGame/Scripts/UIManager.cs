using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _score      = null;
    [SerializeField] private TextMeshProUGUI _highScore  = null;
    [SerializeField] private TextMeshProUGUI _generation = null;

    private static UIManager get = null;

    private void Awake() {
        if(get != null) {
            gameObject.SetActive(false);
            return;
        }

        get = this;
    }

    public void UI_StartButton() {
        GameManager.StartGame();
    }

    public static void UpdateScore() {
        get._score.text = $"Score: {GameManager.score}";
    }

    public static void ResetToNewGeneration() {
        get._score.text = $"Score: {GameManager.score = 0}";
        get._highScore.text = $"High score: {GameManager.highScore}";
        get._generation.text = $"Generation: {GameManager.generation}";
    }
}
