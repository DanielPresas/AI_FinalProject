using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour {

    public Vector2Int bounds  = new Vector2Int();
    public Vector2Int foodPos = new Vector2Int();

    private GameObject food = null;

    private void Awake() {
        transform.localScale = new Vector3(bounds.x, bounds.y, 1.0f);
        transform.position = new Vector3 {
            x = bounds.x / 2.0f - 0.5f,
            y = bounds.y / 2.0f - 0.5f,
            z = 0.0f,
        };

        food = new GameObject("Food", typeof(SpriteRenderer));
        food.GetComponent<SpriteRenderer>().sprite = GameAssets.food;
        food.transform.SetParent(gameObject.transform, worldPositionStays: true);
    }

    private void Update() {
        food.transform.position = new Vector3(foodPos.x, foodPos.y, 0.0f);
    }

}
