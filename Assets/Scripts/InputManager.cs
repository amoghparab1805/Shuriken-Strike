using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public InputData inputData;

    // Update is called once per frame
    void Update()
    {
        writeIntoData();
    }

    void writeIntoData() {
        inputData.isPressed = Input.GetMouseButtonDown(0);
        inputData.isHeld = Input.GetMouseButton(0);
        inputData.isReleased = Input.GetMouseButtonUp(0);
    }
}
