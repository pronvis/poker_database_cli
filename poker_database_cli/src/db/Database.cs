using System.Collections.Generic;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace poker_database_cli.db
{
    public interface HHDb
    {
        public void store(Hand hand);
        public long GetHandsCount();
        public long GetPlayersCount();
        public Hand? TryGetHand(long handNumber);
        public bool DeleteHand(long handNumber);

        public ReadOnlySet<long> getPlayerHandsNumber(string nickName);
        public ReadOnlySet<long> getDeletedHandNumbers();
        public void deletePlayerHands(string nickName, List<long> handNumbers);
    }

    public class DescendingComparer : IComparer<long> {
        public int Compare(long x, long y) {
            return -x.CompareTo(y);
        }
    }

    public class InMemoryHHDb : HHDb
    {
        private Dictionary<String, SortedSet<long>> playerHandsIndex;
        private Dictionary<long, Hand> handById;
        private SortedSet<long> removedHands;

        public InMemoryHHDb()
        {
            playerHandsIndex = [];
            handById = [];
            removedHands = new SortedSet<long>(new DescendingComparer());;
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

        public ReadOnlySet<long> getPlayerHandsNumber(string nickName)
        {
            SortedSet<long>? playersHands;
            playerHandsIndex.TryGetValue(nickName, out playersHands);
            if (playersHands == null)
            {
                return [];
            } 

            return playersHands.AsReadOnly();
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

        public bool DeleteHand(long handNumber)
        {
            if(handById.Remove(handNumber))
            {
                removedHands.Add(handNumber);
                return true;
            }

            return false;
        }

        public ReadOnlySet<long> getDeletedHandNumbers()
        {
            return removedHands.AsReadOnly();
        }

        public void deletePlayerHands(string nickName, List<long> handNumbers)
        {
            SortedSet<long>? playersHands;
            playerHandsIndex.TryGetValue(nickName, out playersHands);
            if (playersHands == null)
            {
                return;
            } 

            foreach(var hand in handNumbers)
            {
                playersHands.Remove(hand);
            }
        }
    }
}