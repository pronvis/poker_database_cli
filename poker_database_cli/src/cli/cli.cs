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

                    case CommandType.DeleteHandFromDb: {
                        long handNumber = Int64.Parse(parsedCommand.GetArguments()[0]); //it is safe here, cause we already parsed it in 'DeleteHandFromDb' class
                        processDeleteHandCommand(handNumber);
                        break;
                    }

                    case CommandType.GetDeletedHandNumbers: {
                        processGetDeletedHandNumbersCommand();
                        break;
                    }

                    default:
                    {
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
            var deletedHands = db.getDeletedHandNumbers();
            List<long> handsToDeleteFromPlayerIndex = new(32);
            int i = 0;
            Console.WriteLine("Player '{0}' played {1} hands", nickName, playersHands.Count);
            Console.WriteLine("His last 10 played hands:");
            foreach (long handNumber in playersHands)
            {
                if (i == 10)
                {
                    break;
                }

                if(deletedHands.Contains(handNumber))
                {
                    handsToDeleteFromPlayerIndex.Add(handNumber);
                    continue;
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

                    stackSizeInfo ??= "Unknown stack";

                    Console.WriteLine("{0} | {1} | {2}", handV.HandNumber, dealtCards, stackSizeInfo);
                    i++;
                }
            }

            if(handsToDeleteFromPlayerIndex.Count != 0)
            {
                db.deletePlayerHands(nickName, handsToDeleteFromPlayerIndex);
            }
        }

        private void processDeleteHandCommand(long handNumber)
        {
            if(db.DeleteHand(handNumber))
            {
                Console.WriteLine("Hand #{0} has been deleted", handNumber);
            } else
            {
                Console.WriteLine("Hand #{0} has not been deleted, cause it doesnt exists", handNumber);
            }
        }

        private void processGetDeletedHandNumbersCommand()
        {
            var deletedHands = db.getDeletedHandNumbers();
            var deletedHandsStr = String.Join(", ", deletedHands.Select(h => h.ToString()));
            Console.WriteLine("Hands that has been deleted: [{0}]", deletedHandsStr);
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