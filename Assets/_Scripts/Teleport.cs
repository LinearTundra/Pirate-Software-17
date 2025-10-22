using UnityEngine;

public class Teleport : MonoBehaviour {

    private Collider2D[] portals;
    private bool canTeleport = true;

    private void Start() {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        portals = GetComponentsInChildren<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.tag != gameObject.tag && gameObject.tag != "Purple") return;

        if (!canTeleport) return;

        // scale.x == +ve  =>  portal facing left
        // scale.x == -ve  =>  portal facing right


        Debug.Log($"Portal1 {other.IsTouching(portals[0])}, Portal2 {other.IsTouching(portals[1])}");
        Debug.Log($"Portal1 {portals[0].transform.position}, Portal2 {portals[1].transform.position}");

        if (other.IsTouching(portals[0])) other.transform.position = portals[1].transform.position;
        else other.transform.position = portals[0].transform.position;
        
        canTeleport = false;

        coolDown();
    }

    private async void coolDown() {
        await Awaitable.WaitForSecondsAsync(Time.fixedDeltaTime*2);
        canTeleport = true;
    }

}
