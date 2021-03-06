using System.Collections.Generic;
using System.Linq;
using LightBDD.Core.Results;
using LightBDD.UnitTests.Helpers;

namespace LightBDD.AcceptanceTests.Helpers.Builders
{
    internal class FeatureBuilder
    {
        private readonly List<ScenarioBuilder> _scenarios = new List<ScenarioBuilder>();

        public FeatureBuilder(string feature)
        {
            Name = feature;
        }

        public string Name { get; }

        public ScenarioBuilder NewScenario(ExecutionStatus status)
        {
            var builder = new ScenarioBuilder(status);
            _scenarios.Add(builder);
            return builder;
        }

        public IFeatureResult Build()
        {
            return TestResults.CreateFeatureResult(Name, "descr", "label", _scenarios.Select(s => s.Build()).ToArray());
        }
    }
}