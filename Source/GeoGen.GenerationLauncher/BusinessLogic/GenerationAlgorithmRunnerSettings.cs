﻿namespace GeoGen.GenerationLauncher
{
    /// <summary>
    /// The settings for <see cref="GenerationAlgorithmRunner"/>.
    /// </summary>
    public class GenerationAlgorithmRunnerSettings
    {
        #region Public properties

        /// <summary>
        /// Indicates how often we log the number of generated configurations.
        /// If this number is 'n', then after every n-th configuration there will be a message.
        /// </summary>
        public int GenerationProgresLoggingFrequency { get; }

        /// <summary>
        /// Indicates whether we should log the progress.
        /// </summary>
        public bool LogProgress { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerationAlgorithmRunnerSettings"/> class.
        /// </summary>
        /// <param name="generationProgresLoggingFrequency">Indicates how often we log the number of generated configurations. If this number is 'n', then after every n-th configuration there will be a message.</param>
        /// <param name="logProgress">Indicates whether we should log the progress.</param>
        public GenerationAlgorithmRunnerSettings(int generationProgresLoggingFrequency, bool logProgress)
        {
            GenerationProgresLoggingFrequency = generationProgresLoggingFrequency;
            LogProgress = logProgress;
        }

        #endregion
    }
}