using UnityEngine;
using UnityEngine.InputSystem;

public class Rotator : MonoBehaviour {
    public float turnAngle = 20f; // obrót w lewo/prawo
    public float tiltAngle = 15f; // przechylenie
    public float smoothSpeed = 10f;

    void Update() {
        float horizontal = 0f;

        if (Keyboard.current != null) {
            if (Keyboard.current.aKey.isPressed)
                horizontal = -1f;

            if (Keyboard.current.dKey.isPressed)
                horizontal = 1f;
        }

        Quaternion targetRotation = Quaternion.Euler(
            0f,                          // X
            horizontal * turnAngle,      // Y - skrêt
            -horizontal * tiltAngle      // Z - przechylenie
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }
}