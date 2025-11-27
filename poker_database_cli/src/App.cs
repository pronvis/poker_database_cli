using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using poker_database_cli.db;
using poker_database_cli.hhparser;
using poker_database_cli.cli;
using System.Threading;
using System.Dynamic;

namespace poker_database_cli
{
    public class App
    {
        private static CancellationTokenSource startPendingProcessThread()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var thread = new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true;
                while (!token.IsCancellationRequested)
                {
                    Console.Write('.'); 
                    Thread.Sleep(300);
                }
            });
            thread.Start();
            return tokenSource;
        }

        public static void Main(string[] HandHistoryPath)
        {
            if (HandHistoryPath.Length != 1) {
                Console.Error.WriteLine("Wrong arguments length. It should contain only path to hand history folder.");
                return;
            }

            IEnumerable<Hand> parsedHands;
            var hhParseFunc = PokerStarsHandHistoryParser.parse;

            string handHistoryPath = Path.GetFullPath(HandHistoryPath[0]);
            if(Directory.Exists(handHistoryPath))
            {
                List<String> filePaths = getFilesInDir(handHistoryPath, new(256));
                parsedHands = filePaths.SelectMany(path => hhParseFunc(File.ReadLines(path)));
            } else if (File.Exists(handHistoryPath))
            {
                parsedHands = hhParseFunc(File.ReadLines(handHistoryPath));
            } else
            {
                Console.Error.WriteLine("Path {0} does not exists.", handHistoryPath);
                return;
            }

            HHDb db = new InMemoryHHDb();
            var cli = new CLI(db);
            Console.WriteLine("Start parsing directory: '{0}'", handHistoryPath);
            var tokenSource = startPendingProcessThread();
            foreach(Hand hand in parsedHands)
            {
                db.store(hand);
            }
            tokenSource.Cancel();
            Console.WriteLine("\r\nParsing finished!\r\nTotal Hands count: {0}\r\nTotal Players count: {1}", db.GetHandsCount(), db.GetPlayersCount());

            while(!cli.isFinished())
            {
                Console.Write("> ");
                var str = Console.ReadLine();
                if (str != null) {
                    cli.processCommand(str);
                }
            }
        }

        private static List<String> getFilesInDir(string path, List<String> result)
        {

            foreach(string filePath in Directory.GetFiles(path))
            {
                result.Add(filePath);
            }

            foreach(string dir in Directory.GetDirectories(path))
            {
                getFilesInDir(dir, result);
            }

            return result;
        }
    }
}