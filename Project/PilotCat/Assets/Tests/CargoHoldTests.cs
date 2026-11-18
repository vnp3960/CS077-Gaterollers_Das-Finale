using NUnit.Framework;
using PilotCat.Core;

public class CargoHoldTests
{
    // Normal: adding to an empty hold works
    [Test]
    public void AddCatNip_ToEmptyHold_CountIsOne()
    {
        var hold = new CargoHold();
        Assert.IsTrue(hold.TryAdd(ItemType.CatNip));
        Assert.AreEqual(1, hold.Count(ItemType.CatNip));
    }

    // Boundary: the 50th fits, the 51st is refused
    [Test]
    public void AddCatNip_AtCapacity_50thAccepted_51stRefused()
    {
        var hold = new CargoHold();
        for (int i = 0; i < 49; i++) hold.TryAdd(ItemType.CatNip);

        Assert.IsTrue(hold.TryAdd(ItemType.CatNip), "50th should fit");
        Assert.IsTrue(hold.IsFull(ItemType.CatNip));
        Assert.IsFalse(hold.TryAdd(ItemType.CatNip), "51st should be refused");
        Assert.AreEqual(50, hold.Count(ItemType.CatNip));
    }

    // Failure: eating a fish with none in the hold
    [Test]
    public void EatFish_WithNoFish_IsRefused_NothingChanges()
    {
        var hold = new CargoHold();
        Assert.IsFalse(hold.TryRemove(ItemType.FishSnack));
        Assert.AreEqual(0, hold.Count(ItemType.FishSnack));
    }

    // Failure: a negative amount can't sneak items in
    [Test]
    public void AddNegativeAmount_IsRefused()
    {
        var hold = new CargoHold();
        Assert.IsFalse(hold.TryAdd(ItemType.CatNip, -5));
        Assert.AreEqual(0, hold.Count(ItemType.CatNip));
    }
}