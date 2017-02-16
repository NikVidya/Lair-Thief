using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameScript : MonoBehaviour {
    // Update is called once per frame
    void Update()
    {
        // checks if key Escape is pressed and loads up the main menu if it is
        if (Input.GetKey(KeyCode.Escape))
        {
           SceneManager.LoadScene("Menu");
        }
    }
}
