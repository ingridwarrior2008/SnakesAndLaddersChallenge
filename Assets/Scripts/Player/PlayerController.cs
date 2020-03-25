using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Player,
    Bot
}

public enum TurnType
{
    Moving,
    Teleporting,
    Wating
}

public class PlayerController : MonoBehaviour, IDice
{
    #region Properties

    public PlayerType playerType = PlayerType.Player;

    [SerializeField]
    private float stoppedDistance = 0.1f;
    [SerializeField]
    private float speed = 10f;

    private BoardView currentPoint;
    private BoardView nextPoint;

    private Vector2 boardManagerPosition;
    private UIBoardManager boardManager;
    private int boardSize;

    private int diceNumber;
    private int numberOfMove;
    private TurnType turnType;

    private TeleporterManager teleporter;

    #endregion

    #region Delegates

    public delegate void PlayerFinishMove(PlayerType playerType, int BoardNumber);
    public static event PlayerFinishMove OnPlayerFinishMoveDelegate;

    #endregion

    #region Lifecycle

    private void Start()
    {
        turnType = TurnType.Wating;
        teleporter = FindObjectOfType<TeleporterManager>();
    }

    public void Update()
    {
        switch (turnType)
        {
            case TurnType.Moving:
                MovePlayer();
                break;
            case TurnType.Teleporting:
                TeleportToPoint();
                break;
            default:
                break;
        }
    }

    public void OnEnable()
    {
        GameController.OnGameStateChangeDelegate += OnGameStateChanged;
    }

    public void OnDisable()
    {
        GameController.OnGameStateChangeDelegate -= OnGameStateChanged;
    }

    #endregion

    #region Public Methods

    public void SetBoardManager(UIBoardManager boardManager)
    {
        this.boardManager = boardManager;
        this.boardManagerPosition = boardManager.transform.position;
        this.boardSize = boardManager.GridSize;
    }

    public void StartRoll(int roll)
    {
        OnRolledDice(roll);
    }

    public void OnRolledDice(int diceNumber)
    {
        this.diceNumber = diceNumber;
        turnType = TurnType.Moving;
    }

    #endregion

    #region Private Methods

    private void OnGameStateChanged(GameController.GameState gameState)
    {
        if (!currentPoint)
        {
            currentPoint = boardManager.BoardViews.FindBoardViewByIndexBoard(1);
            nextPoint = NextBoardPosition();
            transform.position = boardManagerPosition + currentPoint.BoardPosition;
        }
    }

    private void MovePlayer()
    {
        if (numberOfMove != diceNumber)
        {
            MoveToNextBoard();
        }
        else
        {
            OnFinishMove();
        }
    }

    private void OnFinishMove()
    {
        if (!CheckIfPlayerOverlapTeleport())
        {
            numberOfMove = 0;
            diceNumber = 0;
            turnType = TurnType.Wating;
            CallFinishMoveDelegate();
        }
        else
        {
            turnType = TurnType.Teleporting;
        }
    }

    private void MoveToNextBoard()
    {
        if (!nextPoint)
        {
            CallFinishMoveDelegate();
            return;
        }

        Vector2 startPosition = boardManagerPosition + currentPoint.BoardPosition;
        Vector2 nextPosition = boardManagerPosition + nextPoint.BoardPosition;
        Vector3 vectorDirection = nextPosition - startPosition;
        vectorDirection.Normalize();

        if (Vector2.Distance(transform.position, nextPosition) > stoppedDistance)
        {
            transform.position += vectorDirection * speed * Time.deltaTime;
        }
        else
        {
            numberOfMove += 1;
            currentPoint = nextPoint;
            nextPoint = NextBoardPosition();
        }
    }

    private bool CheckIfPlayerOverlapTeleport()
    {
        int boardNumber = currentPoint.BoardModel.BoardNumber;
        return teleporter.Teleports.FindTeleportByTrigger(boardNumber) != null;
    }

    private void TeleportToPoint()
    {
        int index = currentPoint.BoardModel.BoardNumber;
        TeleportModel teleportModel = teleporter.Teleports.FindTeleportByTrigger(index);
        Vector2 playerPosition = transform.position;

        Vector2 startPosition = boardManagerPosition + playerPosition;
        Vector2 nextPosition = boardManagerPosition + teleportModel.BoardPoint.finalPoint.BoardPosition;
        Vector3 vectorDirection = nextPosition - startPosition;

        vectorDirection.Normalize();

        if (Vector2.Distance(transform.position, nextPosition) > stoppedDistance)
        {
            transform.position += vectorDirection * speed * Time.deltaTime;
        }
        else
        {
            currentPoint = teleportModel.BoardPoint.finalPoint;
            nextPoint = NextBoardPosition();
            numberOfMove = 0;
            diceNumber = 0;
            turnType = TurnType.Wating;
            CallFinishMoveDelegate();
        }
    }

    private BoardView NextBoardPosition()
    {
        int nextIndex = currentPoint.BoardModel.BoardNumber + 1;
        return boardManager.BoardViews.FindBoardViewByIndexBoard(nextIndex);
    }

    public void CallFinishMoveDelegate()
    {
        OnPlayerFinishMoveDelegate?.Invoke(playerType, currentPoint.BoardModel.BoardNumber);
    }

    #endregion
}
