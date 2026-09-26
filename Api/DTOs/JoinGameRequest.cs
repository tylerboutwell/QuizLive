namespace Api.DTOs
{

    public class JoinGameRequest
    {
        public required string GameCode { get; set; }
        public required string PlayerName { get; set; }

        public JoinGameRequest() { }
        public JoinGameRequest(string gameCode, string playerName)
        {
            GameCode = gameCode;
            PlayerName = playerName;
        }
    }
}