﻿using System.Collections.Generic;
using GeoGen.Core.Constructions.Parameters;

namespace GeoGen.Core.Constructions.Arguments
{
    /// <summary>
    /// Represents a set of <see cref="ConstructionArgument"/> that is passable as a <see cref="ConstructionParameter"/>.
    /// The type of passed arguments might be a <see cref="ObjectConstructionArgument"/>, or another set of arguments.
    /// It's size is not supposed to be, since it's either a <see cref="ObjectConstructionArgument"/>, or a set
    /// within a set (which doesn't make sense in our context). 
    /// </summary>
    public class SetConstructionArgument : ConstructionArgument
    {
        #region Public properties

        /// <summary>
        /// Gets the hash set containing the passed arguments. 
        /// </summary>
        public HashSet<ConstructionArgument> PassedArguments { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new set containing arguments passed to a construction.
        /// </summary>
        /// <param name="passedArguments">The passed arguments.</param>
        public SetConstructionArgument(HashSet<ConstructionArgument> passedArguments)
        {
            PassedArguments = passedArguments;
        }

        #endregion
    }
}