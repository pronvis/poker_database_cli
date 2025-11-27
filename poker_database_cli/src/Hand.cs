using poker_database_cli.hhparser;

namespace poker_database_cli
{
    public struct Hand
    {
        public Hand(long handNumber)
        {
            HandNumber = handNumber;
        }

        public long HandNumber { get; }

        private List<PlayerWithStack> _playersWithStackInCents = new List<PlayerWithStack>();
        public List<PlayerWithStack> PlayersWithStack
        {
            get => _playersWithStackInCents;
        }
        
        public void addPlayerWithStack(PlayerWithStack playerInfo)
            {
                _playersWithStackInCents.Add(playerInfo);
            }
        private PlayerWithCards _dealtCardsInfo;

        public PlayerWithCards DealtCardsInfo
            {
                get => _dealtCardsInfo;
                set => _dealtCardsInfo = value;
            }

        public string? getCards(string nickName)
        {
            var dealtToPlayerNickName = _dealtCardsInfo.NickName;
            if (nickName != dealtToPlayerNickName)
            {
                return null;
            } 

            return _dealtCardsInfo.Cards;
        }
    }

    public struct PlayerWithStack
    {
           public PlayerWithStack(string nickName, long stackInCents, Currency currency)
    {
        NickName = nickName;
        StackInCents = stackInCents;
        Currency = currency;
    }

    public string NickName { get; }
    public long StackInCents { get; }
    public Currency Currency { get; }
    }

    public struct PlayerWithCards
    {
           public PlayerWithCards(string nickName, string cards)
    {
        NickName = nickName;
        Cards = cards;
    }

    public string NickName { get; }
    public string Cards { get; }
    }
}