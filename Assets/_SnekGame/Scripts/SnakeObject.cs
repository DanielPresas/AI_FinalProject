using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake {
    public enum Direction {
        Up    = 0,
        Down  = 1,
        Left  = 2,
        Right = 3,
    }

    public Direction moveDirection = Direction.Right;
    public Vector2Int headPosition = new Vector2Int(12, 9);
    public float viewAngle = 90.0f;

    public int bodySize = 1;
    public List<Vector2Int> moves = new List<Vector2Int>();
    public List<float> directions = new List<float>();

    public List<Vector2Int> bodyPositions {
        get {
            var list = new List<Vector2Int> { headPosition };
            list.AddRange(moves);
            return list;
        }
    }

    private Vector2Int _cachedHeadPosition = new Vector2Int(0, 0);

    public Snake() {
        moves.Capacity      = 100;
        directions.Capacity = 100;
    }

    public void ResetLife() {
        moves.Clear();
        directions.Clear();

        headPosition = new Vector2Int(12, 9);
        moveDirection = Direction.Right;

        bodySize = 1;
        moves.Add(new Vector2Int(11, 9));
        directions.Add(90.0f);
    }

    public Snake Clone() {
        var clone = new Snake {
            headPosition = headPosition,
            moveDirection = moveDirection,
            viewAngle = viewAngle,
            bodySize = bodySize,
            moves = moves.ToList(),
            directions = directions.ToList(),
        };

        return clone;
    }

    public void Move() {
        var moveDir = new Vector2Int();
        switch(moveDirection) {
            case Direction.Right: { moveDir.x = +1; viewAngle =  90.0f; break; }
            case Direction.Left:  { moveDir.x = -1; viewAngle = 270.0f; break; }
            case Direction.Up:    { moveDir.y = +1; viewAngle = 180.0f; break; }
            case Direction.Down:  { moveDir.y = -1; viewAngle =   0.0f; break; }
        }
        moves.Insert(0, headPosition);
        directions.Insert(0, viewAngle);

        headPosition += moveDir;

        while(moves.Count      > bodySize) { moves.RemoveAt(moves.Count - 1); }
        while(directions.Count > bodySize) { directions.RemoveAt(directions.Count - 1); }
    }

    public void SimulateMove() {
        _cachedHeadPosition = headPosition;

        var moveDir = new Vector2Int();
        switch(moveDirection) {
            case Direction.Right: { moveDir.x = +1; break; }
            case Direction.Left:  { moveDir.x = -1; break; }
            case Direction.Up:    { moveDir.y = +1; break; }
            case Direction.Down:  { moveDir.y = -1; break; }
        }
        headPosition += moveDir;
    }

    public void UndoSimulatedMove() {
        headPosition = _cachedHeadPosition;
    }

    public void AddToBody() => bodySize += 1;

    public bool CheckSelfCollision() {
        foreach(var pos in moves) {
            if(headPosition == pos) {
                return true;
            }
        }
        return false;
    }

    public bool CheckWallCollision(Vector2Int bounds) {
        return headPosition.x < 0 ||
            headPosition.y < 0 ||
            headPosition.x >= bounds.x ||
            headPosition.y >= bounds.y;
    }

}

public class SnakeObject : MonoBehaviour {
    public List<GameObject> body  = new List<GameObject>();

    private void Awake() {
        body.Capacity = 100;
        ResetLife();
    }

    public void ResetLife() {
        foreach(var body in body) { Destroy(body); }
        body.Clear();
    }

    public void UpdatePosition(Snake snake) {
        transform.position = new Vector3(snake.headPosition.x, snake.headPosition.y, 0.0f);
        transform.eulerAngles = new Vector3(0, 0, snake.viewAngle);

        while(snake.moves.Count > body.Count) { AddToBody(); }
        for(int i = 0; i < body.Count; ++i) {
            var pos = snake.moves[i];
            var dir = snake.directions[i];
            body[i].transform.position    = new Vector3(pos.x, pos.y, 0.0f);
            body[i].transform.eulerAngles = new Vector3(0.0f, 0.0f, dir);
        }
    }

    public void AddToBody() {
        var b = new GameObject($"Body_{body.Count}", typeof(SpriteRenderer));
        b.GetComponent<SpriteRenderer>().sprite = GameAssets.snakeBody;
        b.GetComponent<SpriteRenderer>().sortingOrder = -body.Count - 1;
        b.transform.SetParent(gameObject.transform, worldPositionStays: false);
        b.transform.position = body.Count <= 0 ? gameObject.transform.position : body[body.Count - 1].transform.position;
        body.Add(b);
    }

}
