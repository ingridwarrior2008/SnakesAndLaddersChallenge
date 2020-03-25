using System.Collections;
using System.Collections.Generic;

public static class ArrayBoardViewExtension
{
    public static BoardView FindBoardViewByIndexBoard(this List<BoardView> list, int index)
    {
        if (list.Count == 0)
        {
            return null;
        }

        return list.Find((BoardView obj) => obj.BoardModel.BoardNumber == index);
    }
}