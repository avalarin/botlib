using System;
using Microsoft.Extensions.DependencyInjection;

namespace FinBot.BotCore.Utils {
    public static class ServiceProviderExtensions {
        public static T GetInstance<T>(this IServiceProvider serviceProvider) {
            return ActivatorUtilities.GetServiceOrCreateInstance<T>(serviceProvider);
        }

        public static object GetInstance(this IServiceProvider serviceProvider, Type type) {
            return ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, type);
        }
    }
}