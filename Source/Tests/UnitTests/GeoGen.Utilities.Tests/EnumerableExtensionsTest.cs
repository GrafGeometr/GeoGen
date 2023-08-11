﻿using FluentAssertions;
using NUnit.Framework;

namespace GeoGen.Utilities.Tests
{
    /// <summary>
    /// The test class for <see cref="EnumerableExtensions"/>.
    /// </summary>
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        [Test]
        public void Test_Subsets()
        {
            // Prepare a superset
            var superset = new[] { 1, 2, 3, 4 };

            // Assert counts
            superset.Subsets().Count().Should().Be(16);
            superset.Subsets(0).Count().Should().Be(1);
            superset.Subsets(1).Count().Should().Be(4);
            superset.Subsets(2).Count().Should().Be(6);
            superset.Subsets(3).Count().Should().Be(4);
            superset.Subsets(4).Count().Should().Be(1);

            // Assert sizes
            Enumerable.Range(0, 5).ForEach(size => superset.Subsets(size).ForEach(subset => subset.Length.Should().Be(size)));

            // Assert existence of some subsets
            new[]
            {
                new[] {1, 2, 3, 4},
                new[] {1, 2, 3},
                new[] {2, 3},
                new[] {4},
                new[] {1},
            }
            .ForEach(subset =>
            {
                // Make sure it's listed in all subsets
                superset.Subsets().Any(_subset => _subset.OrderlessEquals(subset)).Should().BeTrue();

                // Make sure it's listed in the subsets with the given count
                superset.Subsets(subset.Length).Any(_subset => _subset.OrderlessEquals(subset)).Should().BeTrue();
            });
        }

        [Test]
        public void Test_Variations()
        {
            // Prepare an array
            var array = new[] { 1, 2, 3, 4 };

            // Assert counts
            array.Variations(1).Count().Should().Be(4);
            array.Variations(2).Count().Should().Be(12);
            array.Variations(3).Count().Should().Be(24);
            array.Variations(4).Count().Should().Be(24);

            // Assert sizes
            Enumerable.Range(1, 4).ForEach(size => array.Variations(size).ForEach(variation => variation.Length.Should().Be(size)));

            // Assert existence of some variations
            new[]
            {
                new[] { 1, 2, 3, 4 },
                new[] { 1, 3, 2 },
                new[] { 3, 2 },
                new[] { 4 },
            }
            .ForEach(variation =>
            {
                // Make sure it's listed in the subsets with the given count
                array.Variations(variation.Length).Any(_variation => _variation.SequenceEqual(variation)).Should().BeTrue();
            });
        }

        [Test]
        public void Test_Combine()
        {
            // Prepare some arrays to be combined
            var arrays = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 1 },
                new[] { 1, 2 },
                new[] { 1, 2, 4 }
            };

            // Test count
            arrays.Combine().Count().Should().Be(3 * 1 * 2 * 3);

            // Test existence of some arrays
            arrays.Combine().Any(array => array.SequenceEqual(new[] { 1, 1, 1, 1 })).Should().BeTrue();
            arrays.Combine().Any(array => array.SequenceEqual(new[] { 2, 1, 2, 2 })).Should().BeTrue();
            arrays.Combine().Any(array => array.SequenceEqual(new[] { 3, 1, 2, 4 })).Should().BeTrue();
        }
    }
}
