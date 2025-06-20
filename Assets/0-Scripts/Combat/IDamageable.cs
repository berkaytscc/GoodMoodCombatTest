public interface IDamageable
{
    /// <summary>
    /// Apply damage to this object.
    /// </summary>
    /// <param name="amount">Damage amount (positive).</param>
    public bool TryTakeDamage(float amount);
}