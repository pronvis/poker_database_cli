using System.Linq;

namespace poker_database_cli.cli
{
    public enum CommandType
    {
        GetPlayersGeneralInfo,
        GetPlayerLastInfo,
        DeleteHandFromDb,
        GetDeletedHandNumbers,
    }

    public class CommandsParser
    {

        private static Dictionary<string, Func<IEnumerator<string>, Command?>> commandsList = new Dictionary<string, Func<IEnumerator<string>, Command?>>
        {
            {"GetPlayersGeneralInfo", GetPlayersGeneralInfo.parse},
            {"GetPlayerLastInfo", GetPlayerLastInfo.parse},
            {"DeleteHandFromDb", DeleteHandFromDb.parse},
            {"GetDeletedHandNumbers", GetDeletedHandNumbers.parse},
        };

        public static IEnumerable<Command> parse(string str)
        {
            var splittedStr = str.Trim().Split(' ').Where(s => s.Length > 0);
            if (!splittedStr.Any())
            {
                yield break;
            }

            IEnumerator<string> wordIterator = splittedStr.GetEnumerator();
            while(wordIterator.MoveNext())
            {
                var commandName = wordIterator.Current;
                if(!commandsList.Keys.Contains(commandName))
                {
                    Console.Error.WriteLine("Unknown command: '{0}'", commandName);
                    continue;
                } else
                {
                    Func<IEnumerator<string>, Command?> commandParser = commandsList[commandName];
                    var parsedCommand = commandParser(wordIterator);
                    if(parsedCommand == null)
                    {
                        Console.Error.WriteLine("Can't parse arguments for command: '{0}'", commandName);
                        continue;
                    }
                    yield return parsedCommand;
                }   
            }
        }
    }
    
    public abstract class Command
    {
        public abstract CommandType GetCommandType();
        public abstract List<String> GetArguments();
    }

    public class GetPlayersGeneralInfo : Command
    {
        private GetPlayersGeneralInfo()
        {}


        public override List<String> GetArguments()
        {
            return [];
        }

        public static GetPlayersGeneralInfo? parse(IEnumerator<string> iter)
        {
            return new GetPlayersGeneralInfo();
        }

        public override CommandType GetCommandType()
        {
            return CommandType.GetPlayersGeneralInfo;
        }
    }

        public class GetPlayerLastInfo : Command
    {
        private GetPlayerLastInfo(string nickName)
        {
            NickName = nickName;
        }

        private string NickName { get; }

        public override List<String> GetArguments()
        {
            return [NickName];
        }

        public static GetPlayerLastInfo? parse(IEnumerator<string> iter)
        {
            iter.MoveNext();
            var nameArgument = iter.Current;
            if (nameArgument == null)
            {
                return null;
            }
            
            if (nameArgument == "-p" || nameArgument == "--PlayerName") {
                iter.MoveNext();
                var nickName = iter.Current;
                if (nickName == null)
                {
                    return null;
                }

                return new GetPlayerLastInfo(nickName);
            } else
            {
                return null;
            }
        }

        public override CommandType GetCommandType()
        {
            return CommandType.GetPlayerLastInfo;
        }
    }

    public class DeleteHandFromDb : Command
    {
        private DeleteHandFromDb(long handNumber)
        {
            HandNumber = handNumber;
        }

        private long HandNumber { get; }

        public override List<String> GetArguments()
        {
            return [HandNumber.ToString()];
        }

        public static DeleteHandFromDb? parse(IEnumerator<string> iter)
        {
            iter.MoveNext();
            var nameArgument = iter.Current;
            if (nameArgument == null)
            {
                return null;
            }
            
            if (nameArgument == "-n" || nameArgument == "--HandNumber") {
                iter.MoveNext();
                var handNumberStr = iter.Current;
                if (handNumberStr == null)
                {
                    return null;
                }

                try
                {
                    long handNumber = Int64.Parse(handNumberStr);
                    return new DeleteHandFromDb(handNumber);
                } catch (Exception)
                {
                    Console.Error.WriteLine("Wrong 'hand number' argument: {0}. It should be integer value.", handNumberStr);
                    return null;
                }
            } else
            {
                return null;
            }
        }

        public override CommandType GetCommandType()
        {
            return CommandType.DeleteHandFromDb;
        }
    }

    public class GetDeletedHandNumbers : Command
    {
        private GetDeletedHandNumbers(){}

        public override List<String> GetArguments()
        {
            return [];
        }

        public static GetDeletedHandNumbers? parse(IEnumerator<string> iter)
        {
            return new GetDeletedHandNumbers();
        }

        public override CommandType GetCommandType()
        {
            return CommandType.GetDeletedHandNumbers;
        }
    }
}
