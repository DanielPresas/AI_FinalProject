using UnityEngine;

public class FoodObject : MonoBehaviour {
    public void UpdatePosition(Vector2Int foodPosition) {
        transform.position = new Vector3(foodPosition.x, foodPosition.y, 0.0f);
    }
}
