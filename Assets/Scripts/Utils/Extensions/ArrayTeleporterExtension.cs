using System.Collections;
using System.Collections.Generic;

public static class ArrayTeleporterExtension
{
    public static TeleportModel FindTeleportByTrigger(this List<TeleportModel> list, int index)
    {
        if (list.Count == 0)
        {
            return null;
        }

        return list.Find((TeleportModel obj) => obj.TriggerIndex == index);
    }
}