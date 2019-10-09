using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    private void OnEnable()
    {
        Cursor.visible = false;

    }
    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void hide() 
    {
        gameObject.SetActive(false);
      }
}
