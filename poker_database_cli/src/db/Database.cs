using System.Collections.Generic;

namespace poker_database_cli.db
{
    public interface HHDb
    {
        public void store(Hand hand);
        public long GetHandsCount();
        public long GetPlayersCount();
        public Hand? TryGetHand(long handNumber);

        public SortedSet<long> getPlayerHandsNumber(string nickName);
    }

    class DescendingComparer : IComparer<long> {
        public int Compare(long x, long y) {
            return -x.CompareTo(y);
        }
    }

    public class InMemoryHHDb : HHDb
    {
        private Dictionary<String, SortedSet<long>> playerHandsIndex;
        private Dictionary<long, Hand> handById;

        public InMemoryHHDb()
        {
            playerHandsIndex = [];
            handById = [];
        }

        public void store(Hand hand)
        {
            handById.Add(hand.HandNumber, hand);

            foreach(var player in hand.PlayersWithStack)
            {
                SortedSet<long>? playersHands;
                playerHandsIndex.TryGetValue(player.NickName, out playersHands);
                if (playersHands == null)
                {
                    playersHands = new SortedSet<long>(new DescendingComparer());
                    playerHandsIndex.Add(player.NickName, playersHands);
                }
                playersHands.Add(hand.HandNumber);
            }
        }

        public SortedSet<long> getPlayerHandsNumber(string nickName)
        {
            SortedSet<long>? playersHands;
            playerHandsIndex.TryGetValue(nickName, out playersHands);
            if (playersHands == null)
            {
                return [];
            } 

            return playersHands;
        }

        public long GetHandsCount()
        {
            return handById.Count;
        }

        public long GetPlayersCount()
        {
            return playerHandsIndex.Count;
        }

        public Hand? TryGetHand(long handNumber)
        {
            Hand result;
            var found = handById.TryGetValue(handNumber, out result);
            if (found)
            {
                return result;
            }
            return null;
        }
    }
}