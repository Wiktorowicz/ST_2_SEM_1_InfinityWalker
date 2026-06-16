using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    private Animator animator;
    private PlayerController playerController;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Obstacle"))
            return;

        if (transform.position.y > 1.2f) {
            animator.SetTrigger("DeadAirTrigger");
        }
        else {
            animator.SetTrigger("DeadGroundTrigger");
        }

        GameManager.Instance.EndGame();
    }
}