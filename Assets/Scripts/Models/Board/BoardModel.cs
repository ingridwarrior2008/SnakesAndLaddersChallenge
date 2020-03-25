using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel
{
    [SerializeField]
    private int index;
    public int Index
    {
        get
        {
            return index;
        }
    }

    [SerializeField]
    public int BoardNumber
    {
        get
        {
            return index + 1;
        }
    }

    [SerializeField]
    private float size;
    public float Size
    {
        get
        {
            return size;
        }
    }

    public BoardModel(int index, float size)
    {
        this.index = index;
        this.size = size;
    }
}
