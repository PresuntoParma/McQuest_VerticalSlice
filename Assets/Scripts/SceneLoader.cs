using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ChangeScene(string a)
    {
        SceneManager.LoadScene(a);
    }

  public void DeactivatePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
   public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);

    }

}




