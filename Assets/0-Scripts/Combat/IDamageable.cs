/// <summary>
/// Anything that can take damage implements this.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Apply damage to this object.
    /// </summary>
    /// <param name="amount">Damage amount (positive).</param>
    void TakeDamage(float amount);
}