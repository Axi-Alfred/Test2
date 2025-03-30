using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToWheel : MonoBehaviour
{
    // Called when user clicks the "Return to Wheel" button
    public void GoToWheel()
    {
        SceneManager.LoadScene("Wheel");
    }
}
