using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyButton : MonoBehaviour
{
   private bool keyPressed = false;

    private void Update()
    {
        if(!keyPressed && Input.anyKeyDown)
        {
            keyPressed = true;
            SceneManager.LoadScene("GamePlay Orca");
        }
    }
}
