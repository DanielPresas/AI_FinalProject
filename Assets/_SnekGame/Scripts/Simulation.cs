using System.Linq;
using UnityEngine;

public class Simulation {
    public struct State {
        public bool[] danger;
        public Snake.Direction currentDir;
        public Vector2Int relativeFoodPosition;
    }

    public delegate Snake.Direction GetAction(State dir);

    public Snake snake             = new Snake();
    public Vector2Int bounds       = new Vector2Int();
    public Vector2Int foodPosition = new Vector2Int();

    public int score => snake.bodySize - 1;
    public float moveDelay = 0.2f;
    public GetAction getAction = null;

    private float _moveTimer = 0.0f;

    public void Reset() {
        MoveFood();
        snake.ResetLife();
    }

    private State GetState() {
        var state = new State {
            currentDir = snake.moveDirection,
            relativeFoodPosition = foodPosition - snake.headPosition,
            danger = new bool[4] { false, false, false, false },
        };

        var snakeClone = snake.Clone();
        for (int i = 0; i < 4; ++i) {
            snakeClone.moveDirection = (Snake.Direction)i;
            snakeClone.SimulateMove();
            if(snake.CheckSelfCollision() || snake.CheckWallCollision(bounds)) {
                state.danger[i] = true;
            }
            snakeClone.UndoSimulatedMove();
        }

        return state;
    }

    public bool Update() {
        _moveTimer += Time.deltaTime;
        snake.moveDirection = getAction.Invoke(GetState());

        if(_moveTimer >= moveDelay) {
            _moveTimer = 0.0f;
            snake.Move();
        }

        if(snake.headPosition == foodPosition) {
            MoveFood();
            snake.AddToBody();
        }

        if(snake.CheckSelfCollision() || snake.CheckWallCollision(bounds)) {
            return false;
        }

        return true;
    }

    private void MoveFood() {
        do {
            foodPosition = new Vector2Int {
                x = Random.Range(0, bounds.x),
                y = Random.Range(0, bounds.y),
            };
        } while(snake.bodyPositions.Any(p => p == foodPosition));
    }
}
