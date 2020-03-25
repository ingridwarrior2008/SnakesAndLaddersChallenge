using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Properties
    public enum GameState { Start, Pause, Playing };

    [SerializeField]
    private GameObject gameModeMenu;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private Text playerTurnText;

    [SerializeField]
    private Text diceText;

    [SerializeField]
    private Text winText;

    [SerializeField]
    private Button rollButton;

    [SerializeField]
    private Text pauseAndPlayText;

    [SerializeField]
    private PlayerController playerControllerPrefab;

    [SerializeField]
    private PlayerController botControllerPrefab;

    private PlayerController player;
    private UIBoardManager boardManager;
    private PlayerController bot;

    private PlayerType playerPlaying = PlayerType.Player;

    private GameState gameState = GameState.Start;
    public GameState GetGameState
    {
        get
        {
            return gameState;
        }
    }

    private int numberOfBoards
    {
        get
        {
            return boardManager.GridSize * boardManager.GridSize;
        }
    }

    #endregion

    public delegate void GameStateChange(GameState gameState);
    public static event GameStateChange OnGameStateChangeDelegate;

    #region Lifecycle

    void Start()
    {
        diceText.text = GameConstants.GameDiceText + 0;
        boardManager = FindObjectOfType<UIBoardManager>();
    }

    void Update()
    {
        playerTurnText.text = GameConstants.GameTurnText + playerPlaying.ToString();
    }

    public void OnEnable()
    {
        PlayerController.OnPlayerFinishMoveDelegate += OnPlayerFinishMoveDelegate;
    }

    public void OnDisable()
    {
        PlayerController.OnPlayerFinishMoveDelegate -= OnPlayerFinishMoveDelegate;
    }

    #endregion

    public void OnPausePressed()
    {
        switch (gameState)
        {
            case GameState.Start:
                gameModeMenu.SetActive(true);
                OnPlayPressed(); 
                SpawnPlayers();
                break;
            case GameState.Playing:
                Time.timeScale = 0;
                gameState = GameState.Pause;
                pauseAndPlayText.text = GameConstants.GamePlayText;
                break;
            case GameState.Pause:
                Time.timeScale = 1;
                OnPlayPressed();
                break;
        }

        OnGameStateChangeDelegate?.Invoke(gameState);
    }

    #region Public Methods

    public void StartRoll()
    {
        int roll = Random.Range(GameConstants.MinDiceValue, GameConstants.MaxDiceValue);
        diceText.text = GameConstants.GameDiceText + roll;

        IDice diceInterface = playerPlaying == PlayerType.Player ? player : bot;
        diceInterface.StartRoll(roll);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void SpawnPlayers()
    {
        player = Instantiate<PlayerController>(playerControllerPrefab);
        player.SetBoardManager(boardManager);

        bot = Instantiate<PlayerController>(botControllerPrefab);
        bot.SetBoardManager(boardManager);
    }

    #endregion

    #region Private Methods

    private void OnPlayPressed()
    {
        pauseAndPlayText.text = GameConstants.GamePauseText;
        gameState = GameState.Playing;
    }

    private void OnPlayerFinishMoveDelegate(PlayerType playerType, int BoardNumber)
    {
        rollButton.interactable = playerType == PlayerType.Player ? false : true;
        playerPlaying = playerType == PlayerType.Player ? PlayerType.Bot : PlayerType.Player;

        if (playerType == PlayerType.Player)
        {
            StartRoll();
        }

        if (numberOfBoards == BoardNumber)
        {
            winPanel.SetActive(true);
            winText.text = playerType.ToString() + GameConstants.GameWonMessage;
        }
    }

    #endregion
}
