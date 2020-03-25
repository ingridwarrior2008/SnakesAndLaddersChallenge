using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(RectTransform))]
public class UIBoardManager : MonoBehaviour
{
    #region Properties
    private RectTransform rectTransform;
    private GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    private float boardRectSize = 1;

    [SerializeField]
    private int gridSize = 6;
    public int GridSize
    {
        get
        {
            return gridSize;
        }
    }

    public BoardView boardViewPrefab;
    public TeleporterManager teleporterManagerPrefab;

    private TeleporterManager teleporterManager;

    public float CellSize
    {
        get
        {
            return boardRectSize / gridSize;
        }
    }

    private List<BoardView> boardViews = new List<BoardView>();
    public List<BoardView> BoardViews
    {
        get
        {
            return boardViews;
        }
    }
    #endregion

    #region Lifecycle

    void Start()
    {
        LoadBoard();
    }

    private void OnEnable()
    {
        GameController.OnGameStateChangeDelegate += InitTeleports;
    }


    private void OnDisable()
    {
        GameController.OnGameStateChangeDelegate -= InitTeleports;
    }

    #endregion

    #region Private Methods

    private void LoadBoard()
    {
        InitRect();
        InitGrid();
        GenerateBoardSprite();
    }

    private void InitRect()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(boardRectSize, boardRectSize);
    }


    private void InitGrid()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(CellSize, CellSize);
    }

    private void GenerateBoardSprite()
    {
        int numberOfBoards = gridSize * gridSize;
        bool reverseNumberColumns = false;
        int lastNumberOfRow = 0;
        int firstNumberOfRow = 0;

        for (int i = 0; i < numberOfBoards; i++)
        {
            int index = i;
            BoardView boardView = Instantiate<BoardView>(boardViewPrefab);
            boardView.transform.SetParent(transform);

            if (i % gridSize == 0 && i != 0)
            {
                reverseNumberColumns = !reverseNumberColumns;
            }

            if (reverseNumberColumns)
            {
                if (i % gridSize == 0)
                {
                    lastNumberOfRow = i + gridSize;
                    firstNumberOfRow = i;
                }

                index = lastNumberOfRow - (i - firstNumberOfRow);
                if (index == gridSize)
                {
                    index = lastNumberOfRow;
                }
                index -= 1;
            }


            boardView.SetBoardModel(CreateBoardModel(index));
            boardViews.Add(boardView);
        }
    }

    private void InitTeleports(GameController.GameState gameState)
    {
        if (!teleporterManager)
        {
            teleporterManager = Instantiate<TeleporterManager>(teleporterManagerPrefab);
            teleporterManager.SetBoardManager(this);
        }
    }

    private BoardModel CreateBoardModel(int index)
    {
        float cellSize = boardRectSize / gridSize;
        BoardModel board = new BoardModel(index, cellSize);
        return board;
    }

    #endregion
}

