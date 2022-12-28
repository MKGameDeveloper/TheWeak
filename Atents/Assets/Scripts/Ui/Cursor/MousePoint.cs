using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint : MonoBehaviour
{

    public Texture2D cursor;

    public Vector2 ClickPos = Vector2.zero;

    public void Start()
    {
        StartCoroutine("CursorChange");
    }

    IEnumerator CursorChange()
    {
        yield return new WaitForEndOfFrame();

        Cursor.SetCursor(cursor, ClickPos, CursorMode.Auto);
    }
}
