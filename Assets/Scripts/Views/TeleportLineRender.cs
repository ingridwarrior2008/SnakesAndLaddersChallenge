using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeleportLineRender
{
    private Material ladderColor;
    private Material snakeColor;
    private UIBoardManager boardManager;

    public TeleportLineRender(UIBoardManager boardManager, Material ladderColor, Material snakeColor)
    {
        this.boardManager = boardManager;
        this.ladderColor = ladderColor;
        this.snakeColor = snakeColor;
    }

    public void CreateTeleportLines(TeleportModel teleport)
    {
        Vector3 boardManagerPosition = boardManager.transform.position;

        BoardView startBoard = boardManager.BoardViews.FindBoardViewByIndexBoard(teleport.TriggerIndex);
        BoardView finalBoard = boardManager.BoardViews.FindBoardViewByIndexBoard(teleport.TeleportIndex);

        teleport.SetBoardPoint(startBoard, finalBoard);

        Vector2 startPosition = boardManagerPosition + startBoard.RectTransform.localPosition;
        Vector2 finalPosition = boardManagerPosition + finalBoard.RectTransform.localPosition;

        CreateTeleportLines(startPosition, finalPosition, teleport.GetTeleportType);
    }

    private void CreateTeleportLines(Vector2 startPoint, Vector2 finalPoint, TeleportModel.TeleportType teleportType)
    {
        //TODO Refactor
        GameObject lineObject = new GameObject("TeleportFromLine: ");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = teleportType == TeleportModel.TeleportType.Ladder ? ladderColor : snakeColor;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(startPoint.x, startPoint.y, 1.0f));
        lineRenderer.SetPosition(1, new Vector3(finalPoint.x, finalPoint.y, 1.0f));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingOrder = 0;
        lineRenderer.sortingLayerName = "Lines";
    }
}
