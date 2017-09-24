namespace BotLib.MongoDB {
    public interface IMongoDBConfiguration {
        string ConnectionString { get; }
        string Database { get; }
    }
}
