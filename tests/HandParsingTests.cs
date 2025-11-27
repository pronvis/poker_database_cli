namespace tests;

using poker_database_cli;
using poker_database_cli.hhparser;

    [TestClass]
    public class PokerStarsHandHistoryParser_Test
    {

        [TestMethod]
        public void ParseSeveralHandsInOneText()
        {
            string testText = """
            PokerStars Hand #92715546927:  Hold'em No Limit ($0.10/$0.25 USD) - 2013/01/21 12:33:21 EET [2013/01/21 5:33:21 ET]
            Table 'Delphinus IV' 6-max Seat #5 is the button
            Seat 1: lepa308 ($13.75 in chips) 
            Seat 2: angrypaca ($25 in chips) 
            Seat 3: limiaoxun ($10.35 in chips) 
            Seat 4: Highway25 ($27.61 in chips) 
            Seat 5: fooxy love ($37.35 in chips) 
            lepa308: posts small blind $0.10
            angrypaca: posts big blind $0.25
            *** HOLE CARDS ***
            Dealt to angrypaca [Jc 2h]
            limiaoxun: folds 
            Highway25: folds 
            fooxy love: raises $0.50 to $0.75
            lepa308: folds 
            angrypaca: folds 
            Uncalled bet ($0.50) returned to fooxy love
            fooxy love collected $0.60 from pot
            *** SUMMARY ***
            Total pot $0.60 | Rake $0 
            Seat 1: lepa308 (small blind) folded before Flop
            Seat 2: angrypaca (big blind) folded before Flop
            Seat 3: limiaoxun folded before Flop (didn't bet)
            Seat 4: Highway25 folded before Flop (didn't bet)
            Seat 5: fooxy love (button) collected ($0.60)



            PokerStars Hand #92715560030:  Hold'em No Limit ($0.10/$0.25 USD) - 2013/01/21 12:33:50 EET [2013/01/21 5:33:50 ET]
            Table 'Delphinus IV' 6-max Seat #1 is the button
            Seat 1: lepa308 ($13.75 in chips) 
            Seat 2: angrypaca ($25 in chips) 
            Seat 3: limiaoxun ($10.35 in chips) 
            Seat 4: Highway25 ($27.61 in chips) 
            Seat 5: fooxy love ($37.70 in chips) 
            angrypaca: posts small blind $0.10
            limiaoxun: posts big blind $0.25
            *** HOLE CARDS ***
            Dealt to angrypaca [4d Qc]
            Highway25: folds 
            fooxy love: folds 
            lepa308: folds 
            angrypaca: folds 
            Uncalled bet ($0.15) returned to limiaoxun
            limiaoxun collected $0.20 from pot
            *** SUMMARY ***
            Total pot $0.20 | Rake $0 
            Seat 1: lepa308 (button) folded before Flop (didn't bet)
            Seat 2: angrypaca (small blind) folded before Flop
            Seat 3: limiaoxun (big blind) collected ($0.20)
            Seat 4: Highway25 folded before Flop (didn't bet)
            Seat 5: fooxy love folded before Flop (didn't bet)



            PokerStars Hand #92715569566:  Hold'em No Limit ($0.10/$0.25 USD) - 2013/01/21 12:34:12 EET [2013/01/21 5:34:12 ET]
            Table 'Delphinus IV' 6-max Seat #2 is the button
            Seat 1: lepa308 ($13.75 in chips) 
            Seat 2: angrypaca ($25 in chips) 
            Seat 3: limiaoxun ($10.45 in chips) 
            Seat 4: Highway25 ($27.61 in chips) 
            Seat 5: fooxy love ($37.70 in chips) 
            limiaoxun: posts small blind $0.10
            Highway25: posts big blind $0.25
            *** HOLE CARDS ***
            Dealt to angrypaca [6s 5h]
            fooxy love: folds 
            lepa308: folds 
            angrypaca: folds 
            limiaoxun: raises $0.37 to $0.62
            Highway25: folds 
            Uncalled bet ($0.37) returned to limiaoxun
            limiaoxun collected $0.50 from pot
            *** SUMMARY ***
            Total pot $0.50 | Rake $0 
            Seat 1: lepa308 folded before Flop (didn't bet)
            Seat 2: angrypaca (button) folded before Flop (didn't bet)
            Seat 3: limiaoxun (small blind) collected ($0.50)
            Seat 4: Highway25 (big blind) folded before Flop
            Seat 5: fooxy love folded before Flop (didn't bet)



            PokerStars Hand #92951950334:  Hold'em No Limit (€0.10/€0.25 EUR) - 2013/01/25 19:32:30 EET [2013/01/25 12:32:30 ET]
            Table 'Lyka II' 6-max Seat #6 is the button
            Seat 1: LuNa_sTaR1 (€47.72 in chips) 
            Seat 2: angrypaca (€25 in chips) 
            Seat 3: Oscar_4poker (€25 in chips) 
            Seat 4: Luckbox6991 (€25.90 in chips) 
            Seat 5: peter2903 (€27.73 in chips) 
            Seat 6: h3ll1982 (€41.61 in chips) 
            LuNa_sTaR1: posts small blind €0.10
            angrypaca: posts big blind €0.25
            *** HOLE CARDS ***
            Dealt to angrypaca [4h 4c]
            Oscar_4poker: folds 
            Luckbox6991: folds 
            peter2903: folds 
            h3ll1982: folds 
            LuNa_sTaR1: raises €0.50 to €0.75
            angrypaca: calls €0.50
            *** FLOP *** [3s Jc Jh]
            LuNa_sTaR1: bets €1
            angrypaca: folds 
            Uncalled bet (€1) returned to LuNa_sTaR1
            LuNa_sTaR1 collected €1.43 from pot
            LuNa_sTaR1: doesn't show hand 
            *** SUMMARY ***
            Total pot €1.50 | Rake €0.07 
            Board [3s Jc Jh]
            Seat 1: LuNa_sTaR1 (small blind) collected (€1.43)
            Seat 2: angrypaca (big blind) folded on the Flop
            Seat 3: Oscar_4poker folded before Flop (didn't bet)
            Seat 4: Luckbox6991 folded before Flop (didn't bet)
            Seat 5: peter2903 folded before Flop (didn't bet)
            Seat 6: h3ll1982 (button) folded before Flop (didn't bet)



            """;

            var hands = PokerStarsHandHistoryParser.parse(testText.Split('\n')).ToList();
            Assert.AreEqual(4, hands.Count(), "parsed hands count should be 3");

            var firstHandDealtCards = hands.First().DealtCardsInfo;        
            Assert.AreEqual("angrypaca", firstHandDealtCards.NickName, "first hand dealt to");
            Assert.AreEqual("Jc 2h", firstHandDealtCards.Cards, "first hand dealt cards");

            {
                var lastHandStackSizes = hands[2].PlayersWithStack;  
                Assert.AreEqual(new PlayerWithStack("lepa308", 1375, Currency.Dollar), lastHandStackSizes[0], "first player stack size");
                Assert.AreEqual(new PlayerWithStack("angrypaca", 2500, Currency.Dollar), lastHandStackSizes[1], "2nd player stack size");
                Assert.AreEqual(new PlayerWithStack("limiaoxun", 1045, Currency.Dollar), lastHandStackSizes[2], "3rd player stack size");
                Assert.AreEqual(new PlayerWithStack("Highway25", 2761, Currency.Dollar), lastHandStackSizes[3], "4th player stack size");
                Assert.AreEqual(new PlayerWithStack("fooxy love", 3770, Currency.Dollar), lastHandStackSizes[4], "5th player stack size");
            }
            {
                var lastHandStackSizes = hands[3].PlayersWithStack;  
                Assert.AreEqual(new PlayerWithStack("LuNa_sTaR1", 4772, Currency.Euro), lastHandStackSizes[0], "LuNa_sTaR1 stack size");
                Assert.AreEqual(new PlayerWithStack("Luckbox6991", 2590, Currency.Euro), lastHandStackSizes[3], "Luckbox6991 stack size");
            }
        }
        
    }