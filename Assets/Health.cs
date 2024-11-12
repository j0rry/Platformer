using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;

    private void Update() {
        if(health <= 0){
            Destroy(this.gameObject);
        }
    }
}
