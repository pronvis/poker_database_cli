Test recruting task.

### To run
`dotnet run --project poker_database_cli '{relative or full path to hand_history directory or file}'`

#### Commands:
- `GetPlayersGeneralInfo` return players count and hands count in database. 
- `GetPlayerLastInfo --PlayerName/-p {playerName}` return player hands count & brief info about last 10 hands.
- `DeleteHandFromDb --HandNumber/-n {handNumber}` delete hand from database.
- `GetDeletedHandNumbers` return list of deleted hands.

### To run tests
`dotnet test`

