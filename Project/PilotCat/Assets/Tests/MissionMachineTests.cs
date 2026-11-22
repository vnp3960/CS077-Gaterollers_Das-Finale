using NUnit.Framework;
using PilotCat.Core;

public class MissionMachineTests
{
    MissionMachine Flying()
    {
        var m = new MissionMachine();
        m.TryChange(MissionState.Flying);
        return m;
    }

    // Normal: a new mission starts at Briefing and can take off
    [Test]
    public void NewMission_StartsInBriefing_ThenFlies()
    {
        var m = new MissionMachine();
        Assert.AreEqual(MissionState.Briefing, m.State);
        Assert.IsTrue(m.TryChange(MissionState.Flying));
        Assert.AreEqual(MissionState.Flying, m.State);
    }

    // Boundary: 49 Cat Nip stays Flying, 50 opens extraction
    [Test]
    public void CheckHold_49StaysFlying_50OpensExtraction()
    {
        var m = Flying();
        Assert.IsFalse(m.CheckHold(49, 50));
        Assert.AreEqual(MissionState.Flying, m.State);
        Assert.IsTrue(m.CheckHold(50, 50));
        Assert.AreEqual(MissionState.ExtractionReady, m.State);
    }

    // Failure: can't skip straight from Flying to a delivery
    [Test]
    public void FlyingToMarket_IsRefused_StateUnchanged()
    {
        var m = Flying();
        Assert.IsFalse(m.TryChange(MissionState.MarketDelivery));
        Assert.AreEqual(MissionState.Flying, m.State);
    }

    // Failure: once shot down, you can't still win
    [Test]
    public void AfterShotDown_DeliveryIsRefused()
    {
        var m = Flying();
        m.TryChange(MissionState.ShotDown);
        Assert.IsFalse(m.TryChange(MissionState.OrphanageDrop));
        Assert.AreEqual(MissionState.ShotDown, m.State);
        Assert.IsTrue(m.IsOver);
    }

    // Normal: both endings are reachable from ExtractionReady
    [Test]
    public void ExtractionReady_CanReachEitherEnding()
    {
        var a = Flying(); a.CheckHold(50, 50);
        var b = Flying(); b.CheckHold(50, 50);
        Assert.IsTrue(a.TryChange(MissionState.MarketDelivery));
        Assert.IsTrue(b.TryChange(MissionState.OrphanageDrop));
    }

    // Normal: market doubles the score, orphanage doesn't
    [Test]
    public void FinalScore_MarketDoubles_OrphanageDoesNot()
    {
        Assert.AreEqual(200, MissionMachine.FinalScore(MissionState.MarketDelivery, 50, 2));
        Assert.AreEqual(100, MissionMachine.FinalScore(MissionState.OrphanageDrop, 50, 2));
    }
}