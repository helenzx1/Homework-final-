using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButtonController : MonoBehaviour
{
    public void OnEndButtonClicked()
    {
        SceneManager.LoadScene("S5");
    }
}
