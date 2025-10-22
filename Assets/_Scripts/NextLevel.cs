using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NextLevel : MonoBehaviour {
    
    private Collider2D col;

    private void Start() {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D() {
        if (col.IsTouching(PlayerController.player1) && col.IsTouching(PlayerController.player2)) GameManager.nextScene();
    }

    private void OnTriggerStay2D() {
        if (col.IsTouching(PlayerController.player1) && col.IsTouching(PlayerController.player2)) GameManager.nextScene();
    }


}
