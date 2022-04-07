using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private SnakeObject _snakeObject = null;
    [SerializeField] private FoodObject _foodObject   = null;

    [Space]
    [SerializeField] private GameObject _level        = null;
    [SerializeField] private Vector2Int _bounds = new Vector2Int();

    [Space]
    [SerializeField] private float _moveDelay   = 0.5f;
    [SerializeField] private bool _humanPlayer  = true;

    private bool _started = false;

    public static int score = 0;
    public static int highScore = 0;
    public static int generation = 1;

    private static GameManager get;
    private static Simulation sim;

    private void Awake() {
        if(get != null) {
            gameObject.SetActive(false);
            return;
        }
        get = this;

        Random.InitState((int)System.DateTime.UtcNow.ToFileTimeUtc());

        _level.transform.localScale = new Vector3(_bounds.x, _bounds.y, 1.0f);
        _level.transform.position = new Vector3 {
            x = _bounds.x / 2.0f - 0.5f,
            y = _bounds.y / 2.0f - 0.5f,
            z = 0.0f,
        };

        if(_humanPlayer) {
            sim = new Simulation {
                bounds = _bounds,
                moveDelay = _moveDelay,
                getAction = GetHumanInput,
            };
        }
    }

    private void Update() {
        if(!_started) return;

        var gameOver = !sim.Update();
        if(!gameOver) {
            score = sim.score;
            UIManager.UpdateScore();
        }
        else {
            if(score > highScore) { highScore = score; }
            sim.Reset();
            _snakeObject.ResetLife();

            generation += 1;
            UIManager.ResetToNewGeneration();
            return;
        }

        _foodObject.UpdatePosition(sim.foodPosition);
        _snakeObject.UpdatePosition(sim.snake);
    }

    public static void StartGame() {
        get._started = true;
        get._foodObject.gameObject.SetActive(true);
        get._snakeObject.gameObject.SetActive(true);

        sim.Reset();
        get._foodObject.UpdatePosition(sim.foodPosition);
        get._snakeObject.UpdatePosition(sim.snake);

        UIManager.ResetToNewGeneration();
    }

    private static Snake.Direction GetHumanInput(Snake.Direction currentDir) {
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            if(currentDir != Snake.Direction.Down)  { return Snake.Direction.Up;    }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            if(currentDir != Snake.Direction.Up)    { return Snake.Direction.Down;  }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if(currentDir != Snake.Direction.Left)  { return Snake.Direction.Right; }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if(currentDir != Snake.Direction.Right) { return Snake.Direction.Left;  }
        }
        return currentDir;
    }
}
