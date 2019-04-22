using Dependency_Injection___Containers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DepedencyInjectionContainersTests
{
    public class ContainerTestBase
    {
        protected Container container;

        [SetUp]
        public void BeforeEach()
        {
            container = new Container();
        }

        [TearDown]
        public void AfterEach()
        {
            container = null;
        }
    }

    [TestFixture]
    public class ContainerTest: ContainerTestBase
    {
        [Test]
        public void test_register_resolve_container_components()
        {
            container.Register<ITestInterface, TestClass>();

            var resolvedObject = container.Resolve<ITestInterface>();

            Assert.IsInstanceOf(typeof(TestClass), resolvedObject);
        }

        [Test]
        public void test_register_type_method()
        {
            container.RegisterType<TestDependency>();

            var resolvedObject = container.Resolve<TestDependency>();

            Assert.NotNull(resolvedObject);
        }

        [Test]
        public void test_register_class_with_dependencies()
        {
            container.RegisterType<TestClassWithDependency>();
            container.RegisterType<TestDependency>();

            var resolvedObject = container.Resolve<TestClassWithDependency>();

            Assert.NotNull(resolvedObject);
        }
    }

    [TestFixture]
    public class ContainerResolveTest : ContainerTestBase
    {
        [SetUp]
        public void RegisterClasses()
        {
            container = new Container();
            container.RegisterType<TestDependency>();
            container.RegisterType<TestClassWithDependency>();
        }

        [Test]
        public void Creates_An_Instance_With_No_Params()
        {
            var subject = container.Resolve(typeof(TestDependency));
            Assert.IsInstanceOf(typeof(TestDependency), subject);
            Assert.NotNull(subject);
        }

        [Test]
        public void Creates_An_Instance_With_Params()
        {
            var subject = container.Resolve(typeof(TestClassWithDependency));
            Assert.IsInstanceOf(typeof(TestClassWithDependency), subject);
            Assert.NotNull(subject);
        }

        [Test]
        public void It_Allows_Generic_Initialization()
        {
            var subject = container.Resolve<TestDependency>();
            Assert.IsInstanceOf(typeof(TestDependency), subject);
            Assert.NotNull(subject);
        }
    }

    public class SingletonContainerRegisterTest : ContainerTestBase
    {
        [Test]
        public void test_register_singleton()
        {
            var dependency = new TestDependency();
            container.RegisterAsSingleton(dependency);
            var subject = container.Resolve<TestDependency>();
            Assert.AreSame(dependency, subject);
        }
    }

    public class UnregisteredThingsTestsInContainer : ContainerTestBase
    {
        [Test]
        public void throws_when_resolving_not_registered_type()
        {
            var exc = Assert.Throws<TypeNotRegisteredException>(
                () => container.Resolve<TestClass>());

            Assert.AreEqual(typeof(TestClass), exc.Type);
        }

        [Test]
        public void throws_when_resolving_not_registered_interface()
        {
            var exc = Assert.Throws<TypeNotRegisteredException>(
                () => container.Resolve<ITestInterface>());

            Assert.AreEqual(typeof(ITestInterface), exc.Type);
        }

        [Test]
        public void throws_when_resolving_type_with_unregistered_dependencies()
        {
            container.RegisterType<TestClassWithDependency>();

            var exc = Assert.Throws<TypeNotRegisteredException>(
                () => container.Resolve<TestClassWithDependency>()
            );
            Assert.AreEqual(typeof(TestDependency), exc.Type);
        }
    }

    interface ITestInterface
    {

    }

    class TestClass : ITestInterface
    {
    }

    class TestClassWithDependency : ITestInterface
    {
        public TestDependency dependency;

        public TestClassWithDependency(TestDependency dependency)
        {
            this.dependency = dependency;
        }
    }

    class TestDependency
    {

    }
}