﻿using System;
using LightBDD.Core.Configuration;
using LightBDD.Core.Notification;

namespace LightBDD.Notification.Configuration
{
    /// <summary>
    /// Configuration class allowing to customize scenario progress notification behavior.
    /// </summary>
    public class ScenarioProgressNotifierConfiguration : IFeatureConfiguration
    {
        /// <summary>
        /// Returns function providing scenario progress notifier, where function parameter is feature fixture class instance.
        /// By default it is initialized with function returning <see cref="NoProgressNotifier.Default"/> instance.
        /// </summary>
        public Func<object, IScenarioProgressNotifier> NotifierProvider { get; private set; } = fixture => NoProgressNotifier.Default;

        /// <summary>
        /// Updates <see cref="NotifierProvider"/> with new value.
        /// </summary>
        /// <param name="notifierProvider">New provider to set.</param>
        /// <returns>Self.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="notifierProvider"/> is null.</exception>
        public ScenarioProgressNotifierConfiguration UpdateNotifierProvider(Func<IScenarioProgressNotifier> notifierProvider)
        {
            if (notifierProvider == null)
                throw new ArgumentNullException(nameof(notifierProvider));
            NotifierProvider = fixture => notifierProvider();
            return this;
        }
        /// <summary>
        /// Updates <see cref="NotifierProvider"/> with new value.
        /// </summary>
        /// <param name="notifierProvider">New provider to set.</param>
        /// <typeparam name="TFixture">Feature fixture type.</typeparam>
        /// <returns>Self.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="notifierProvider"/> is null.</exception>
        public ScenarioProgressNotifierConfiguration UpdateNotifierProvider<TFixture>(Func<TFixture, IScenarioProgressNotifier> notifierProvider)
        {
            if (notifierProvider == null)
                throw new ArgumentNullException(nameof(notifierProvider));
            NotifierProvider = fixture => CreateScenarioProgressNotifier(notifierProvider, fixture);
            return this;
        }

        private static IScenarioProgressNotifier CreateScenarioProgressNotifier<TFixture>(Func<TFixture, IScenarioProgressNotifier> scenarioProgressNotifier, object fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException(nameof(fixture));

            if (!(fixture is TFixture))
                throw new InvalidOperationException($"Unable to create {nameof(IScenarioProgressNotifier)}. Expected fixture of type '{typeof(TFixture)}' while got '{fixture.GetType()}'.");

            return scenarioProgressNotifier((TFixture)fixture);
        }
    }
}