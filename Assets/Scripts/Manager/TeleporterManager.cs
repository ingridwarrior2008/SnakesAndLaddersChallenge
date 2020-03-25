using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private List<TeleportModel> teleports = new List<TeleportModel>();
    public List<TeleportModel> Teleports
    {
        get
        {
            return teleports;
        }
    }

    
    public Material ladderColor;
    public Material snakeColor;

    private TeleportLineRender teleportLineRender;
    #endregion

    #region Lifecycle
    void Start()
    {
        LoadTeleportLines();
    }

    #endregion

    #region Public Methods

    public void SetBoardManager(UIBoardManager boardManager)
    {
        teleportLineRender = new TeleportLineRender(boardManager, ladderColor, snakeColor);
    }

    #endregion

    #region Private Methods

    private void LoadTeleportLines()
    {
        foreach (TeleportModel teleport in teleports)
        {
            teleportLineRender.CreateTeleportLines(teleport);
        }
    }

    #endregion
}
