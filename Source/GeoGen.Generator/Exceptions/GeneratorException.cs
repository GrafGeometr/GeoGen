﻿using GeoGen.Core;
using System;

namespace GeoGen.Generator
{
    /// <summary>
    /// Represents a type of a <see cref="GeoGenException"/> that is thrown
    /// when something incorrect happens in the generator module.
    /// </summary>
    public class GeneratorException : GeoGenException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorException"/> class.
        /// </summary>
        public GeneratorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorException"/> class
        /// with a custom message about what happened.
        /// </summary>
        /// <param name="message">The message about what happened.</param>
        public GeneratorException(string message)
                : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorException"/> class
        /// with a custom message about what happened, and the inner exception that caused this one.
        /// </summary>
        /// <param name="message">The message about what happened.</param>
        /// <param name="innerException">The inner exception that caused this one.</param>
        public GeneratorException(string message, Exception innerException)
                : base(message, innerException)
        {
        }
    }
}