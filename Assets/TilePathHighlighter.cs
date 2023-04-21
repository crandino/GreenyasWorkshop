using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TilePathHighlighter
{
    private static WaitForSeconds interval = new WaitForSeconds(0.5f);

    private static IEnumerator StartHighlight(Connection[] connection)
    {
        for (int i = 0; i < connection.Length; i++)
        {
            connection[i]
            yield return interval;
        }
    }
}
