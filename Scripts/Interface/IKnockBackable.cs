using UnityEngine;

public interface IKnockBackable
{
    void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce);
}
