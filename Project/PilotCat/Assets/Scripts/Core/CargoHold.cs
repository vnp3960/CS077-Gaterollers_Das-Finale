using System.Collections.Generic;

namespace PilotCat.Core
{
    public enum ItemType { CatNip, FishSnack }

    // Duke's cargo: a count and a capacity for each item type.
    // Plain C# (not a MonoBehaviour), so tests can run it without a scene.
    public class CargoHold
    {
        private readonly Dictionary<ItemType, int> counts = new Dictionary<ItemType, int>();
        private readonly Dictionary<ItemType, int> capacities = new Dictionary<ItemType, int>();

        public CargoHold(int catNipCapacity = 50, int fishCapacity = 3)
        {
            capacities[ItemType.CatNip] = catNipCapacity;
            capacities[ItemType.FishSnack] = fishCapacity;
            Clear();
        }

        // Inspect
        public int Count(ItemType item) => counts[item];
        public int Capacity(ItemType item) => capacities[item];
        public bool IsFull(ItemType item) => counts[item] >= capacities[item];

        // Add: refused (returns false, nothing changes) if the amount
        // isn't positive or it would go over capacity
        public bool TryAdd(ItemType item, int amount = 1)
        {
            if (amount <= 0) return false;
            if (counts[item] + amount > capacities[item]) return false;
            counts[item] += amount;
            return true;
        }

        // Remove: refused if the amount isn't positive or there aren't enough
        public bool TryRemove(ItemType item, int amount = 1)
        {
            if (amount <= 0) return false;
            if (counts[item] < amount) return false;
            counts[item] -= amount;
            return true;
        }

        // Empty the hold (used on restart)
        public void Clear()
        {
            counts[ItemType.CatNip] = 0;
            counts[ItemType.FishSnack] = 0;
        }
    }
}