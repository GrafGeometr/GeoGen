﻿using GeoGen.Core;
using GeoGen.ProblemGenerator;
using GeoGen.TheoremProver;
using GeoGen.TheoremRanker;
using GeoGen.TheoremSimplifier;
using GeoGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoGen.ProblemAnalyzer
{
    /// <summary>
    /// The default implementation of <see cref="IGeneratedProblemAnalyzer"/> that combines the three GeoGen algorithms: 
    /// <see cref="ITheoremProver"/>,  <see cref="ITheoremRanker"/> and <see cref="ITheoremSimplifier"/>. The algorithm is 
    /// the following:
    /// <list type="number">
    /// <item>Theorems are attempted to be proved. Proved ones are automatically not interesting.</item>
    /// <item>Unproved theorems are attempted to be simplified. Those where it is possible are automatically not interesting.</item>
    /// <item>The remaining theorems are ranked and sorted by the ranking ascending. These are the final interesting theorems.</item>
    /// </list>
    /// </summary>
    public class GeneratedProblemAnalyzer : IGeneratedProblemAnalyzer
    {
        #region Dependencies

        /// <summary>
        /// The prover of theorems.
        /// </summary>
        private readonly ITheoremProver _prover;

        /// <summary>
        /// The ranker of theorems.
        /// </summary>
        private readonly ITheoremRanker _ranker;

        /// <summary>
        /// The simplifier of theorems.
        /// </summary>
        private readonly ITheoremSimplifier _simplifier;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedProblemAnalyzer"/> class.
        /// </summary>
        /// <param name="prover">The prover of theorems.</param>
        /// <param name="ranker">The ranker of theorems.</param>
        /// <param name="simplifier">The simplifier of theorems.</param>
        public GeneratedProblemAnalyzer(ITheoremProver prover, ITheoremRanker ranker, ITheoremSimplifier simplifier)
        {
            _prover = prover ?? throw new ArgumentNullException(nameof(prover));
            _ranker = ranker ?? throw new ArgumentNullException(nameof(ranker));
            _simplifier = simplifier ?? throw new ArgumentNullException(nameof(simplifier));
        }

        #endregion

        #region IGeneratedProblemAnalyzer implementation

        /// <inheritdoc/>
        public GeneratedProblemAnalyzerOutputWithoutProofs AnalyzeWithoutProofConstruction(ProblemGeneratorOutput generatorOutput)
            // Delegate the call to the general method
            => (GeneratedProblemAnalyzerOutputWithoutProofs)Analyze(generatorOutput, constructProofs: false);

        /// <inheritdoc/>
        public GeneratedProblemAnalyzerOutputWithProofs AnalyzeWithProofConstruction(ProblemGeneratorOutput generatorOutput)
            // Delegate the call to the general method
            => (GeneratedProblemAnalyzerOutputWithProofs)Analyze(generatorOutput, constructProofs: true);

        /// <summary>
        /// Performs the analysis of a given generator output.
        /// </summary>
        /// <param name="output">The generator output to be analyzed.</param>
        /// <param name="constructProofs">Indicates whether we should construct proofs or not, which might affect the result.</param>
        /// <returns>The result depending on whether we're constructing proofs or not.</returns>
        private GeneratedProblemAnalyzerOutputBase Analyze(ProblemGeneratorOutput output, bool constructProofs)
        {
            // Call the prover
            var proverOutput = constructProofs
                // If we should construct proofs, do so
                ? (object)_prover.ProveTheoremsAndConstructProofs(output.OldTheorems, output.NewTheorems, output.ContextualPicture)
                // If we shouldn't construct proofs, don't do it
                : _prover.ProveTheorems(output.OldTheorems, output.NewTheorems, output.ContextualPicture);

            // Find the proved theorems 
            var provedTheorems = constructProofs
                // If we have constructed proofs, there is a dictionary
                ? (IReadOnlyCollection<Theorem>)((IReadOnlyDictionary<Theorem, TheoremProof>)proverOutput).Keys
                // Otherwise there is a collection directly
                : (IReadOnlyCollection<Theorem>)proverOutput;

            // Get the unproven theorems by taking all the new theorems
            var interestingTheorems = output.NewTheorems.AllObjects
                // Excluding those that are proven
                .Where(theorem => !provedTheorems.Contains(theorem))
                // Enumerate
                .ToArray();

            // Simplify the interesting theorems
            var simplifiedTheorems = interestingTheorems
                // Attempt to simplify each
                .Select(theorem => (theorem, simplification: _simplifier.Simplify(theorem, output.Configuration)))
                // Take those where it worked out
                .Where(pair => pair.simplification != null)
                // Enumerate to a dictionary
                .ToDictionary(pair => pair.theorem, pair => pair.simplification.Value);

            // Interesting theorems can now be reseted
            interestingTheorems = interestingTheorems
                // By excluding simplifiable ones
                .Where(theorem => !simplifiedTheorems.ContainsKey(theorem))
                // Enumerate
                .ToArray();

            // Prepare the map of all theorems
            var allTheorems = new TheoremMap(output.OldTheorems.AllObjects.Concat(output.NewTheorems.AllObjects));

            // Rank the interesting theorems
            var theoremRankings = interestingTheorems
                // Enumerate rankings to a dictionary
                .ToDictionary(theorem => theorem, theorem => _ranker.Rank(theorem, output.Configuration, allTheorems));

            // Sort the interesting theorems by the ranking
            interestingTheorems = interestingTheorems
                 // By rankings ASC (that's why -)
                 .OrderBy(theorem => -theoremRankings[theorem].TotalRanking)
                 // Enumerate
                 .ToArray();

            // Now we can finally return the result
            return constructProofs
                // If we're constructing proofs, then we have a proof dictionary
                ? new GeneratedProblemAnalyzerOutputWithProofs(simplifiedTheorems, theoremRankings, interestingTheorems, (IReadOnlyDictionary<Theorem, TheoremProof>)proverOutput)
                // If we're not constructing proofs, then we have just a proved theorem collection
                : (GeneratedProblemAnalyzerOutputBase)new GeneratedProblemAnalyzerOutputWithoutProofs(simplifiedTheorems, theoremRankings, interestingTheorems, (IReadOnlyCollection<Theorem>)proverOutput);
        }

        #endregion
    }
}