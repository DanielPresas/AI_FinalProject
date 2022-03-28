using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Snake : MonoBehaviour {

    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    public Vector2Int headPos = new Vector2Int(12, 9);
    public int snakeBodySize => _snakeBody.Count;
    public List<Vector2Int> snakeBodyPositions {
        get {
            var list = new List<Vector2Int> { headPos };
            list.AddRange(_snakeMoves);
            return list;
        }
    }

    [SerializeField] private float _gridMoveDelay = 0.5f;

    private Direction _moveDirection = Direction.Right;

    private List<GameObject> _snakeBody  = new List<GameObject>();
    private List<Vector2Int> _snakeMoves = new List<Vector2Int>();
    private List<float> _snakeDirs       = new List<float>();

    private void Awake() {
        _snakeMoves.Capacity = 100;
        _snakeDirs.Capacity = 100;
        _snakeBody.Capacity = 100;

        ResetLife();
        StartCoroutine(MoveSnake());
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            if(_moveDirection != Direction.Down)  { _moveDirection = Direction.Up;    }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if(_moveDirection != Direction.Up)    { _moveDirection = Direction.Down;  }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(_moveDirection != Direction.Left)  { _moveDirection = Direction.Right; }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(_moveDirection != Direction.Right) { _moveDirection = Direction.Left;  }
        }
    }

    IEnumerator MoveSnake() {
        while(true) {
            var moveDirection = new Vector2Int();
            var viewDirection = 0.0f;
            switch(_moveDirection) {
                case Direction.Right: { moveDirection.x = +1; viewDirection =  90.0f; break; }
                case Direction.Left:  { moveDirection.x = -1; viewDirection = 270.0f; break; }
                case Direction.Up:    { moveDirection.y = +1; viewDirection = 180.0f; break; }
                case Direction.Down:  { moveDirection.y = -1; viewDirection =   0.0f; break; }
            }

            _snakeMoves.Insert(0, headPos);
            _snakeDirs .Insert(0, viewDirection);


            headPos += moveDirection;

            while(_snakeMoves.Count > snakeBodySize) { _snakeMoves.RemoveAt(_snakeMoves.Count - 1); }
            while(_snakeDirs .Count > snakeBodySize) { _snakeDirs .RemoveAt(_snakeDirs .Count - 1); }

            foreach(var pos in _snakeMoves) {
                if(headPos == pos) {
                    ResetLife();
                }
            }

            transform.position = new Vector3(headPos.x, headPos.y, 0.0f);
            transform.eulerAngles = new Vector3(0, 0, viewDirection);

            for(int i = 0; i < snakeBodySize; ++i) {
                var pos = _snakeMoves[i];
                var dir = _snakeDirs[i];
                _snakeBody[i].transform.position    = new Vector3(pos.x, pos.y, 0.0f);
                _snakeBody[i].transform.eulerAngles = new Vector3(0.0f, 0.0f, dir);
            }

            yield return new WaitForSeconds(_gridMoveDelay);
        }
    }

    public void AddToBody() {
        var body = new GameObject($"Body_{snakeBodySize}", typeof(SpriteRenderer));
        body.GetComponent<SpriteRenderer>().sprite = GameAssets.snakeBody;
        body.GetComponent<SpriteRenderer>().sortingOrder = -_snakeBody.Count - 1;
        body.transform.SetParent(gameObject.transform, worldPositionStays: false);
        body.transform.position = snakeBodySize <= 0 ? gameObject.transform.position : _snakeBody[snakeBodySize - 1].transform.position;
        _snakeBody.Add(body);
    }

    public void ResetLife() {
        headPos = new Vector2Int(12, 9);
        _moveDirection = Direction.Right;

        foreach(var body in _snakeBody) { Destroy(body); }
        _snakeBody.Clear();
        AddToBody();
    }

}
