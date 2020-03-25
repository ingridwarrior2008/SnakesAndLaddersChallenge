using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeleportModel
{
    public enum TeleportType { Snake, Ladder }

    [SerializeField]
    private int triggerIndex = 0;
    public int TriggerIndex
    {
        get
        {
            return triggerIndex;
        }
    }

    [SerializeField]
    private TeleportType teleportType = TeleportType.Snake;
    public TeleportType GetTeleportType
    {
        get
        {
            return teleportType;
        }
    }

    [SerializeField]
    private int teleportIndex = 0;
    public int TeleportIndex
    {
        get
        {
            return teleportIndex;
        }
    }

    private BoardPoint boardPoint;
    public BoardPoint BoardPoint
    {
        get
        {
            return boardPoint;
        }
    }

    public void SetBoardPoint(BoardView startPoint, BoardView finalPoint)
    {
        this.boardPoint = new BoardPoint(startPoint, finalPoint);
    }
}

public class BoardPoint
{
    public BoardView startPoint;
    public BoardView finalPoint;

    public BoardPoint(BoardView startPoint, BoardView finalPoint)
    {
        this.startPoint = startPoint;
        this.finalPoint = finalPoint;
    }
}