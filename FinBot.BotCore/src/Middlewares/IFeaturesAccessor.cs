using System.Collections.Generic;
using FinBot.BotCore.Utils;

namespace FinBot.BotCore.Middlewares {
    public interface IFeaturesAccessor<TBase> {

        bool Has<T>() where T : TBase;

        Maybe<T> GetOne<T>() where T : TBase;

        T RequireOne<T>() where T : TBase;

        IEnumerable<T> GetAll<T>() where T : TBase;

        IEnumerable<T> GetAllOfBaseType<T>();

    }
}