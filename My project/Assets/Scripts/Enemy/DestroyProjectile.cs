using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{

    public float deleteAfter = 5f;

    private void Start()
    {
        Destroy(gameObject, deleteAfter);
    }
}
