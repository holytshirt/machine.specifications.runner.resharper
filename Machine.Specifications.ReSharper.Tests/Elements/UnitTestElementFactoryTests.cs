﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.Metadata.Reader.Impl;
using JetBrains.ReSharper.UnitTestFramework;
using Machine.Specifications.ReSharperProvider;
using NSubstitute;
using NUnit.Framework;

namespace Machine.Specifications.ReSharper.Tests.Elements
{
    [TestFixture]
    public class UnitTestElementFactoryTests : ReflectionWithSingleProject
    {
        [Test]
        public void CanCreateContext()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var element = factory.GetOrCreateContext(project, new ClrTypeName("MyClass"), BaseTestDataPath, "subject",
                    new[] {"tag1"}, false);

                Assert.That(element, Is.Not.Null);
                Assert.That(element.GetPresentation(), Is.EqualTo("subject, MyClass"));
                Assert.That(element.OwnCategories.Any(x => x.Name == "tag1"), Is.True);
            });
        }

        [Test]
        public void GetsExistingContextFromElementManager()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var element1 = factory.GetOrCreateContext(project, new ClrTypeName("MyClass"), BaseTestDataPath, "subject",
                    new[] { "tag1" }, false);

                ServiceProvider.ElementManager.AddElements(new HashSet<IUnitTestElement> {element1});

                var element2 = factory.GetOrCreateContext(project, new ClrTypeName("MyClass"), BaseTestDataPath, "subject",
                    new[] { "tag1" }, false);

                Assert.That(element1, Is.Not.Null);
                Assert.That(element2, Is.Not.Null);
                Assert.That(element1, Is.SameAs(element2));
            });
        }

        [Test]
        public void CanCreateContextSpec()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var element = factory.GetOrCreateContextSpecification(project, CreateUnitTestElement(), new ClrTypeName("MyClass"), "my_field", false);

                Assert.That(element, Is.Not.Null);
                Assert.That(element.GetPresentation(), Is.EqualTo("my field"));
            });
        }

        [Test]
        public void GetsExistingContextSpecFromElementManager()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var parent = factory.GetOrCreateContext(project, new ClrTypeName("Parent"), BaseTestDataPath, "subject", new string[0], false);
                var element1 = factory.GetOrCreateContextSpecification(project, parent, new ClrTypeName("MyClass"), "my_field", false);

                ServiceProvider.ElementManager.AddElements(new HashSet<IUnitTestElement> {element1});

                var element2 = factory.GetOrCreateContextSpecification(project, parent, new ClrTypeName("MyClass"), "my_field", false);

                Assert.That(element1, Is.Not.Null);
                Assert.That(element2, Is.Not.Null);
                Assert.That(element1, Is.SameAs(element2));
            });
        }

        [Test]
        public void CanCreateBehavior()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var element = factory.GetOrCreateBehavior(project, CreateUnitTestElement(), new ClrTypeName("MyClass"), "my_field", false);

                Assert.That(element, Is.Not.Null);
                Assert.That(element.GetPresentation(), Is.EqualTo("behaves like my field"));
            });
        }

        [Test]
        public void GetsExistingBehaviorFromElementManager()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var parent = factory.GetOrCreateContext(project, new ClrTypeName("Parent"), BaseTestDataPath, "subject", new string[0], false);
                var element1 = factory.GetOrCreateBehavior(project, parent, new ClrTypeName("MyClass"), "my_field", false);

                ServiceProvider.ElementManager.AddElements(new HashSet<IUnitTestElement> { element1 });

                var element2 = factory.GetOrCreateBehavior(project, parent, new ClrTypeName("MyClass"), "my_field", false);

                Assert.That(element1, Is.Not.Null);
                Assert.That(element2, Is.Not.Null);
                Assert.That(element1, Is.SameAs(element2));
            });
        }

        [Test]
        public void CanCreateBehaviorSpec()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var parent = factory.GetOrCreateContext(project, new ClrTypeName("Parent"), BaseTestDataPath, "subject", new string[0], false);
                var element = factory.GetOrCreateBehaviorSpecification(project, parent, new ClrTypeName("MyClass"), "my_field", false);

                Assert.That(element, Is.Not.Null);
                Assert.That(element.GetPresentation(), Is.EqualTo("my field"));
            });
        }

        [Test]
        public void GetsExistingBehaviorSpecFromElementManager()
        {
            With(project =>
            {
                var factory = new UnitTestElementFactory(ServiceProvider, TargetFrameworkId.Default);

                var context = factory.GetOrCreateContext(project, new ClrTypeName("Parent"), BaseTestDataPath, "subject", new string[0], false);
                var behavior = factory.GetOrCreateBehavior(project, context, new ClrTypeName("MyClass"), "my_field", false);
                var element1 = factory.GetOrCreateBehaviorSpecification(project, behavior, new ClrTypeName("MyClass"), "my_field", false);

                ServiceProvider.ElementManager.AddElements(new HashSet<IUnitTestElement> { element1 });

                var element2 = factory.GetOrCreateBehaviorSpecification(project, behavior, new ClrTypeName("MyClass"), "my_field", false);

                Assert.That(element1, Is.Not.Null);
                Assert.That(element2, Is.Not.Null);
                Assert.That(element1, Is.SameAs(element2));
            });
        }
    }
}
