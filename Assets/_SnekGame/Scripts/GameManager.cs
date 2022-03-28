using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public LevelGrid levelGrid;
    public Snake snake;

    private void Awake() {
        Random.InitState((int)System.DateTime.UtcNow.ToFileTimeUtc());
        SpawnFood();
    }

    private void Update() {
        if(snake.headPos == levelGrid.foodPos) {
            SpawnFood();
            snake.AddToBody();
        }

        if(snake.headPos.x <= 0 || snake.headPos.y <= 0 || snake.headPos.x >= levelGrid.bounds.x || snake.headPos.y >= levelGrid.bounds.y) {
            snake.ResetLife();
        }
    }

    private void SpawnFood() {
        do {
            levelGrid.foodPos = new Vector2Int {
                x = Random.Range(0, levelGrid.bounds.x),
                y = Random.Range(0, levelGrid.bounds.y),
            };
        } while(snake.snakeBodyPositions.Any(p => p == levelGrid.foodPos));
    }

}
