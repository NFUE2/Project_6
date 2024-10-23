
public interface IDamagable
{
    void TakeDamage(float damage);
}

public interface IPunDamagable
{
    void Damage(float damage);
    void DamageRPC(float damage);
}