namespace DungeonExplorer
{
    public interface IDamageable
    {
        int Health { get; set; }
        void TakeDamage(int amount);
        void Heal(int amount);
    }
}
