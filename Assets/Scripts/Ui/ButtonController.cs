using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void on_Click(string level_name)
    {
        SceneManager.LoadScene(level_name);
    }
    public void on_quit()
    {
        Application.Quit();
    }
    public void RandomLevel()
    {
        int x = Random.Range(2,6);
        Debug.Log(x);
        SceneManager.LoadScene(x);
    }
}
