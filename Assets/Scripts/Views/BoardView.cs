using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(RectTransform))]
public class BoardView : MonoBehaviour
{
    private BoardModel boardModel;
    public BoardModel BoardModel
    {
        get
        {
            return boardModel;
        }
    }

    public RectTransform RectTransform
    {
        get
        {
            return this.GetComponent<RectTransform>();
        }
    }

    public Vector2 BoardPosition
    {
        get
        {
            return RectTransform.localPosition;
        }
    }


    private TextMeshPro numberText
    {
        get
        {
            return this.GetComponentInChildren<TextMeshPro>();
        }
    }

    void Start()
    {
        //TODO: Refactor!
        this.name = "BoardView" + boardModel.BoardNumber;
        numberText.text = boardModel.BoardNumber + "";

        if (boardModel.Index % 2 == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }

        GetComponent<SpriteRenderer>().sortingOrder = 0;
        GetComponent<SpriteRenderer>().sortingLayerName = "Board";

    }

    public void SetBoardModel(BoardModel board)
    {
        this.boardModel = board;
    }
}
