using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ChangeScene(string a)
    {
        SceneManager.LoadScene(a);
    }
}
