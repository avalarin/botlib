using System;

namespace FinBot.BotCore.Middlewares {
    public class MiddlewareData {

        private readonly ImmutableFeaturesSet<IFeature> _features;

        public MiddlewareData() {
            _features = new ImmutableFeaturesSet<IFeature>();
        }

        public MiddlewareData(ImmutableFeaturesSet<IFeature> newFeatures) {
            _features = newFeatures;
        }

        public IFeaturesAccessor<IFeature> Features => _features;

        public MiddlewareData UpdateFeatures(
            Func<ImmutableFeaturesSet<IFeature>, ImmutableFeaturesSet<IFeature>> updater) {

            var newFeatures = updater(_features);
            return new MiddlewareData(newFeatures);
        }

    }
}