using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpinWheel : MonoBehaviour
{
    [Header("Wheel Setup")]
    public Transform wheelTransform;       // Reference to your wheel object
    public string[] sceneNames;            // Must match the number of wedges

    [Header("Spin Settings")]
    public float minStartSpeed = 800f;     // Minimum initial speed (degrees/sec)
    public float maxStartSpeed = 1200f;    // Maximum initial speed (degrees/sec)
    public float deceleration = 300f;      // How quickly the wheel slows down (degrees/sec^2)

    [Header("Scene Load Delay")]
    public float sceneLoadDelay = 2f;      // Delay before loading the next scene

    private bool isSpinning = false;

    public void SpinTheWheel()
    {
        if (!isSpinning)
        {
            float randomSpeed = Random.Range(minStartSpeed, maxStartSpeed);
            StartCoroutine(SpinWithFriction(randomSpeed));
        }
    }

    private IEnumerator SpinWithFriction(float currentSpeed)
    {
        isSpinning = true;

        // Spin until the current speed drops to zero or below.
        while (currentSpeed > 0f)
        {
            // Rotate the wheel by currentSpeed * Time.deltaTime degrees
            wheelTransform.Rotate(0f, 0f, currentSpeed * Time.deltaTime);

            // Reduce the speed
            currentSpeed -= deceleration * Time.deltaTime;
            yield return null;
        }

        // Wheel has stopped; get the final angle
        float finalAngle = wheelTransform.eulerAngles.z;
        int selectedIndex = DetermineSliceIndex(finalAngle);

        // Optional: Wait before loading the next scene so the player sees the result
        yield return new WaitForSeconds(sceneLoadDelay);

        // Load the scene corresponding to the wedge landed on
        SceneManager.LoadScene(sceneNames[selectedIndex]);

        isSpinning = false;
    }

    private int DetermineSliceIndex(float wheelZRotation)
    {
        // Determine the angle for each wedge
        float sliceSize = 360f / sceneNames.Length;
        // Convert the wheel's rotation into a 0-360 degree value
        float adjustedRotation = wheelZRotation % 360f;
        // Determine the index of the wedge (without snapping)
        int index = Mathf.FloorToInt(adjustedRotation / sliceSize);
        Debug.Log("Wheel landed on wedge index: " + index);
        return index;
    }
}
