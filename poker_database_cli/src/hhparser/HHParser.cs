using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

namespace poker_database_cli.hhparser
{
    public enum Currency
    {
        Dollar,
        Euro, 
        Unknown
    }

    public static class PokerStarsHandHistoryParser
    {

        private static Currency parseCurrency(char ch)
        {
            return ch switch
            {
                '$' => Currency.Dollar,
                '€' => Currency.Euro,
                _ => Currency.Unknown,
            };
        }

        public static IEnumerable<Hand> parse(IEnumerable<String> handsHistoryLines)
        {
            List<String> oneHandLines = new(64);

            foreach(string line in handsHistoryLines)
            {
                if (line.Length == 0 && oneHandLines.Count != 0)
                {
                    Hand? parsedHand = parseHand(oneHandLines);
                    if (parsedHand != null) {
                        oneHandLines = new(64);
                        yield return parsedHand.Value;
                        continue;
                    } else {
                        oneHandLines = new(64);
                        continue;
                    }
                }

                if (line.Length == 0)
                {
                    continue;
                }

                oneHandLines.Add(line);
            }
        }

        private static Hand? parseHand(List<String> handHistory)
        {   
            long? handNumber = getHandNumber(handHistory.First());
            if (handNumber == null) {
                return null;
            }
            var hand = new Hand(handNumber.Value);

            var firstSeatsSection = true;
            foreach(var line in handHistory.Skip(2))
            {
                if(line.StartsWith("Seat") && firstSeatsSection)
                {
                    var seatInfo = getSeatInfo(line);
                    if (seatInfo == null)
                    {
                        return null;
                    } else
                    {
                        hand.addPlayerWithStack(seatInfo.Value);
                    }
                    continue;
                }

                firstSeatsSection = false;

                if(line.StartsWith("Dealt to "))
                {
                    var dealtCardsInfo = getDealToInfo(line);
                    if (dealtCardsInfo == null)
                    {
                        return null;
                    } else
                    {
                        hand.DealtCardsInfo = dealtCardsInfo.Value;
                        return hand;
                    }
                }
            }

            return hand;
        }

        private static long? getHandNumber(string str)
        {
            try
            {
                StringBuilder result = new();
                foreach(var elem in str.SkipWhile(ch => !Char.IsNumber(ch)).TakeWhile(ch => Char.IsNumber(ch)))
                {
                    result.Append(elem);
                }
                return Convert.ToInt64(result.ToString());
            } catch (Exception)
            {
                Console.Error.WriteLine("Fail to get hand number from line: {0}", str);
                return null;
            }
        }

        private static PlayerWithStack? getSeatInfo(string str)
        {
            try
            {
                var rawString = String.Concat(str.SkipWhile(ch => ch != ':').Skip(2));
                var lastOpenBracketIndex = rawString.LastIndexOf('(');
                char currencySymbol = rawString[lastOpenBracketIndex + 1];
                var nickName = rawString[..(lastOpenBracketIndex - 1)];
                var stackSize = Convert.ToDecimal(string.Concat(rawString[(lastOpenBracketIndex+2)..].TakeWhile(ch => ch != ' '))) * 100;
                return new PlayerWithStack(nickName, Convert.ToInt32(stackSize), parseCurrency(currencySymbol));
            } catch (Exception)
            {
                Console.Error.WriteLine("Fail to get seat info from line: {0}", str);
                return null;
            }
        }

        private static PlayerWithCards? getDealToInfo(string str)
        {
            try
            {
                var nameAndCards = String.Concat(str.Skip(9)).Split('[');
                var nickName = nameAndCards[0].Trim();
                var cards = String.Concat(nameAndCards[1].TakeWhile(ch => ch != ']'));
                return new PlayerWithCards(nickName, cards);
            } catch (Exception)
            {
                Console.Error.WriteLine("Fail to get \'dealt to\' info from line: {0}", str);
                return null;
            }
        }
    }
}