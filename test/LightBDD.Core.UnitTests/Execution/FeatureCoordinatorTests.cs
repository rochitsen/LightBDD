﻿using System;
using LightBDD.Core.Configuration;
using LightBDD.Core.Execution.Coordination;
using LightBDD.Core.Reporting;
using LightBDD.UnitTests.Helpers.TestableIntegration;
using NUnit.Framework;

namespace LightBDD.Core.UnitTests.Execution
{
    [TestFixture]
    public class FeatureCoordinatorTests
    {
        class TestableFeatureCoordinator : FeatureCoordinator
        {
            public TestableFeatureCoordinator()
                : base(new TestableFeatureRunnerRepository(), new FeatureReportGenerator(), new LightBddConfiguration())
            {
            }

            public TestableFeatureCoordinator InstallSelf()
            {
                Install(this);
                return this;
            }

            public static FeatureCoordinator GetInstalled() => Instance;
        }

        [Test]
        public void It_should_not_be_possible_to_install_multiple_coordinators_at_the_same_time()
        {
            using (var coord1 = new TestableFeatureCoordinator())
            using (var coord2 = new TestableFeatureCoordinator())
            {
                coord1.InstallSelf();
                var ex = Assert.Throws<InvalidOperationException>(() => coord2.InstallSelf());
                Assert.That(ex.Message, Is.EqualTo($"FeatureCoordinator of {typeof(TestableFeatureCoordinator)} type is already installed"));
            }
        }

        [Test]
        public void It_should_be_possible_to_install_new_cooridnator_when_previous_was_disposed()
        {
            using (new TestableFeatureCoordinator().InstallSelf())
            {
            }
            using (var coord = new TestableFeatureCoordinator())
                Assert.DoesNotThrow(() => coord.InstallSelf());
        }

        [Test]
        public void The_coordinator_should_uninstall_self_upon_disposal()
        {
            using (var coord = new TestableFeatureCoordinator().InstallSelf())
                Assert.That(TestableFeatureCoordinator.GetInstalled(), Is.SameAs(coord));
            Assert.That(TestableFeatureCoordinator.GetInstalled(), Is.Null);
        }

        [Test]
        public void The_coordinator_should_uninstall_self_upon_disposal_only_if_it_actively_used_one()
        {
            using (var installedCoordinator = new TestableFeatureCoordinator().InstallSelf())
            {
                using (new TestableFeatureCoordinator()) { }
                Assert.That(TestableFeatureCoordinator.GetInstalled(), Is.SameAs(installedCoordinator));
            }
        }
    }
}