using System;

namespace FinBot.BotCore.Utils {
    public static class MaybeExtensions {

        public static Maybe<T> Nullable<T>(this T self) {
            return Maybe<T>.OfNullable(self);
        }
        
        public static Maybe<T> NotNull<T>(this T self) {
            return Maybe<T>.Of(self);
        }
        
    }
}