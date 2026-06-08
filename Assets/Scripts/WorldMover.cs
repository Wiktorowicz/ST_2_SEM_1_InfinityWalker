using UnityEngine;

public class WorldMover : MonoBehaviour
{
    void Update()
    {
        transform.Translate(
            Vector3.back * GameManager.Instance.WorldSpeed * Time.deltaTime
        );
    }
}