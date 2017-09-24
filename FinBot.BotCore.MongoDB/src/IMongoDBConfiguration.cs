namespace FinBot.BotCore.MongoDB {
    public interface IMongoDBConfiguration {
        string ConnectionString { get; }
        string Database { get; }
    }
}
