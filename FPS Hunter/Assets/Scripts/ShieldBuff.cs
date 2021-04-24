using UnityEngine;

public class ShieldBuff : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponentInParent<Player>() != null)
        {
            
        }
    }
}
