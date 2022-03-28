using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {

    [SerializeField] private Sprite _snakeHead = null;
    [SerializeField] private Sprite _snakeBody = null;
    [SerializeField] private Sprite _food      = null;

    public static Sprite snakeHead => get._snakeHead;
    public static Sprite snakeBody => get._snakeBody;
    public static Sprite food      => get._food;

    private static GameAssets get = null;

    private void Awake() {
        if(get != null) {
            gameObject.SetActive(false);
            return;
        }

        get = this;
    }
}
