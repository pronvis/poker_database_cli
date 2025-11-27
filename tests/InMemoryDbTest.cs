namespace tests;

using poker_database_cli;
using poker_database_cli.hhparser;
using poker_database_cli.db;

    [TestClass]
    public class InMemoryDb_Test
    {
        [TestMethod]
        public void ProperlyStorePlayerHandsIndex()
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
            Seat 3: limiaoxun_the_new ($10.45 in chips) 
            Seat 4: Highway25 ($27.61 in chips) 
            Seat 5: fooxy love ($37.70 in chips) 
            limiaoxun_the_new: posts small blind $0.10
            Highway25: posts big blind $0.25
            *** HOLE CARDS ***
            Dealt to angrypaca [6s 5h]
            fooxy love: folds 
            lepa308: folds 
            angrypaca: folds 
            limiaoxun_the_new: raises $0.37 to $0.62
            Highway25: folds 
            Uncalled bet ($0.37) returned to limiaoxun_the_new
            limiaoxun_the_new collected $0.50 from pot
            *** SUMMARY ***
            Total pot $0.50 | Rake $0 
            Seat 1: lepa308 folded before Flop (didn't bet)
            Seat 2: angrypaca (button) folded before Flop (didn't bet)
            Seat 3: limiaoxun_the_new (small blind) collected ($0.50)
            Seat 4: Highway25 (big blind) folded before Flop
            Seat 5: fooxy love folded before Flop (didn't bet)



            """;

            var hands = PokerStarsHandHistoryParser.parse(testText.Split('\n'));
            var db = new InMemoryHHDb();
            foreach(var hand in hands)
            {
                db.store(hand);
            }

            {
                var lepasGames = db.getPlayerHandsNumber("lepa308");
                var expected = new SortedSet<long>([92715569566, 92715560030, 92715546927], new DescendingComparer());
                CollectionAssert.AreEqual(lepasGames, expected);
            }

            {
                var limiaoxunGames = db.getPlayerHandsNumber("limiaoxun");
                var expected = new SortedSet<long>([92715560030, 92715546927], new DescendingComparer());
                CollectionAssert.AreEqual(limiaoxunGames, expected);
            }

            {
                var limiaoxunTheNewGames = db.getPlayerHandsNumber("limiaoxun_the_new");
                var expected = new SortedSet<long>([92715569566], new DescendingComparer());
                CollectionAssert.AreEqual(limiaoxunTheNewGames, expected);

            }
        }
        
    }