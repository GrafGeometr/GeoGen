﻿using System;
using GeoGen.Core;
using NUnit.Framework;

namespace GeoGen.Generator.Test.ConstructingConfigurations.ObjectToString
{
    [TestFixture]
    public class DefaultObjectToStringProviderTest
    {
        private static DefaultObjectIdResolver _defaultResolver;

        private static DefaultObjectToStringConverter Provider()
        {
            _defaultResolver = new DefaultObjectIdResolver();

            return new DefaultObjectToStringConverter(_defaultResolver);
        }

        [Test]
        public void Test_Default_Resolver_Cant_Be_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultObjectToStringConverter(null));
        }

        [Test]
        public void Test_Passed_Object_Cant_Be_Null()
        {
            Assert.Throws<ArgumentNullException>(() => Provider().ConvertToString(null));
        }

        [Test]
        public void Test_Passed_Object_Must_Have_Id()
        {
            var obj = new LooseConfigurationObject(ConfigurationObjectType.Point);

            Assert.Throws<GeneratorException>(() => Provider().ConvertToString(obj));
        }

        [Test]
        public void Test_Object_To_String_Is_Correct()
        {
            var obj = new LooseConfigurationObject(ConfigurationObjectType.Point) {Id = 42};
            var asString1 = Provider().ConvertToString(obj);

            Assert.AreEqual("42", asString1);
        }

        [Test]
        public void Test_Resolver_Is_Returned()
        {
            var provider = Provider().Resolver;

            Assert.AreSame(provider, _defaultResolver);
        }
    }
}