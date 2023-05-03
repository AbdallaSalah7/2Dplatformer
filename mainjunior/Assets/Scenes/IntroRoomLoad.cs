using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IntroRoomLoad : MonoBehaviour
{
    string sceneName = "StartRoom";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
