namespace FinBot.BotCore.MongoDB {
    public interface IMongoDBConfiguration {
        string ConnectionString { get; }
        string Database { get; }
        string ContextDataCollection { get; }
        string ParamsCollection { get;  }
        string UsersCollection { get;  }
    }
}
