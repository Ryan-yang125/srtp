using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public GameObject WinDialog;
    private void OnTriggerEnter2D(Collider2D collision)
    {
            WinDialog.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
            WinDialog.SetActive(false);
        
    }
}
