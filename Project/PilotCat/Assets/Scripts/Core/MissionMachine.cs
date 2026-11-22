namespace PilotCat.Core
{
    public enum MissionState { Briefing, Flying, ExtractionReady, MarketDelivery, OrphanageDrop, ShotDown }

    // Tracks where the mission is. Only the changes in IsAllowed can happen;
    // anything else is refused and the state stays the same.
    public class MissionMachine
    {
        public MissionState State { get; private set; } = MissionState.Briefing;

        public bool IsOver =>
            State == MissionState.MarketDelivery ||
            State == MissionState.OrphanageDrop ||
            State == MissionState.ShotDown;

        // Every allowed change
        public static bool IsAllowed(MissionState from, MissionState to)
        {
            switch (from)
            {
                case MissionState.Briefing:
                    return to == MissionState.Flying;
                case MissionState.Flying:
                    return to == MissionState.ExtractionReady || to == MissionState.ShotDown;
                case MissionState.ExtractionReady:
                    return to == MissionState.MarketDelivery
                        || to == MissionState.OrphanageDrop
                        || to == MissionState.ShotDown;
                default:
                    // Any ending: R restarts at Briefing
                    return to == MissionState.Briefing;
            }
        }

        public bool TryChange(MissionState to)
        {
            if (!IsAllowed(State, to)) return false;
            State = to;
            return true;
        }

        // Called every frame: a full hold moves Flying -> ExtractionReady
        public bool CheckHold(int catNip, int goal)
        {
            if (State == MissionState.Flying && catNip >= goal)
                return TryChange(MissionState.ExtractionReady);
            return false;
        }

        // Market delivery doubles the score; the orphanage drop doesn't
        public static int FinalScore(MissionState ending, int catNip, int kills)
        {
            int score = catNip * kills;
            return ending == MissionState.MarketDelivery ? score * 2 : score;
        }
    }
}