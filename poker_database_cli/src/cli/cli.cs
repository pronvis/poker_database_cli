using System.Reflection.Metadata;
using System.Text;
using poker_database_cli.db;
using poker_database_cli.hhparser;
using System.Threading;

namespace poker_database_cli.cli
{

    public class CLI
    {
        private HHDb db;
        private bool finished;

        public CLI(HHDb db)
        {
            this.db = db;
            finished = false;
        }

        public bool isFinished()
        {
            return finished;
        }

        public void processCommand(string command)
        {
            var commands = CommandsParser.parse(command);
            bool afterFirstCommand = false;
            foreach(var parsedCommand in commands)
            {
                if (afterFirstCommand)
                {
                    Console.WriteLine("----------------");
                }

                switch (parsedCommand.GetCommandType())
                {
                    case CommandType.GetPlayersGeneralInfo: {
                        processGetPlayerGeneralInfoCommand();
                        break;
                    }

                    case CommandType.GetPlayerLastInfo: {
                        processGetPlayerLastInfoCommand(parsedCommand.GetArguments()[0]);
                        break;
                    }

                    default:
                    {
                        Console.WriteLine("xxx");
                        break;
                    }
                };
                afterFirstCommand = true;
            }
        }

        private void processGetPlayerGeneralInfoCommand()
        {
            Console.WriteLine("Total Hands count: {0}\r\nTotal Players count: {1}", db.GetHandsCount(), db.GetPlayersCount());
        }

        private void processGetPlayerLastInfoCommand(string nickName)
        {
            var playersHands = db.getPlayerHandsNumber(nickName);
            int i = 0;
            Console.WriteLine("Player '{0}' played {1} hands", nickName, playersHands.Count);
            Console.WriteLine("His last 10 played hands:");
            foreach (long handNumber in playersHands)
            {
                if (i == 10)
                {
                    return;
                }

                var hand = db.TryGetHand(handNumber);
                if(hand == null)
                {
                    continue;
                } else
                {
                    var handV = hand.Value;
                    string dealtCards;
                    if (handV.DealtCardsInfo.NickName == nickName)
                    {
                        dealtCards = handV.DealtCardsInfo.Cards;
                    } else
                    {
                        dealtCards = "Unknown cards";
                    }

                    string? stackSizeInfo = null;
                    foreach(var playerWithStack in handV.PlayersWithStack)
                    {
                        if (playerWithStack.NickName == nickName)
                        {
                            stackSizeInfo = StackInfoIntoString(playerWithStack);
                            break;
                        }
                    }

                    if(stackSizeInfo == null)
                    {
                        stackSizeInfo = "Unknown stack";
                    }

                    Console.WriteLine("{0} | {1} | {2}", handV.HandNumber, dealtCards, stackSizeInfo);
                    i++;
                }
            }
        }

        private static string StackInfoIntoString(PlayerWithStack stackInfo)
        {
            char currencySymbol = stackInfo.Currency switch
            {
                Currency.Dollar => '$',
                Currency.Euro => '€',
                _ => '?',
            };

            var bld = new StringBuilder();
            bld.Append(currencySymbol);
            bld.Append(stackInfo.StackInCents);
            bld.Insert(bld.Length-2, '.');
            return bld.ToString();
        }
    }
}