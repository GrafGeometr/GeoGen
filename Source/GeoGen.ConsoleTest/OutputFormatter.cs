﻿using GeoGen.Analyzer;
using GeoGen.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GeoGen.ConsoleTest
{
    public class OutputFormatter
    {
        private Dictionary<ConfigurationObject, char> _objectToName = new Dictionary<ConfigurationObject, char>();

        private Configuration _configuration;

        public OutputFormatter(Configuration configuration)
        {
            _configuration = configuration;
            Register(configuration.ObjectsMap.AllObjects);
        }

        public string FormatConfiguration()
        {
            var looseObjects = _configuration.LooseObjectsHolder.LooseObjects;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{_configuration.LooseObjectsHolder.Layout}: {string.Join(", ",_configuration.LooseObjectsHolder.LooseObjects.Select(ObjectToStringById))}\n");
            
            foreach (var constructedObject in _configuration.ConstructedObjects)
            {
                stringBuilder.Append($"{ObjectToStringById(constructedObject)} = {ConstructedObjectToString(constructedObject)}\n");
            }

            return stringBuilder.ToString().Trim();
        }

        public void Register(IEnumerable<ConfigurationObject> objects)
        {
            foreach (var configurationObject in objects)
            {
                if (_objectToName.ContainsKey(configurationObject))
                    continue;

                var newLetter = (char) ('A' + _objectToName.Count);

                _objectToName.Add(configurationObject, newLetter);
            }
        }

        public string FormatTheorems(IEnumerable<AnalyzedTheorem> theorems)
        {
            var stringBuilder = new StringBuilder();

            foreach (var theorem in theorems)
            {
                stringBuilder.Append(ConvertTheoremToString(theorem)).Append("\n");
            }

            return stringBuilder.ToString().Trim();
        }

        public string ConvertTheoremToString(AnalyzedTheorem theorem)
        {
            var containersNumbersString = theorem.NumberOfTrueContainersAfterSecondTest != null
                ? $"{theorem.NumberOfTrueContainersAfterFirstTest},{theorem.NumberOfTrueContainersAfterSecondTest}"
                : $"{theorem.NumberOfTrueContainersAfterFirstTest}";

            if (theorem.Type == TheoremType.EqualLineSegments || theorem.Type == TheoremType.EqualAngles)
            {
                var firstTwo = theorem.InvolvedObjects.Take(2).Select(TheoremObjectToString).ToList();
                firstTwo.Sort();

                var secondTwo = theorem.InvolvedObjects.Skip(2).Select(TheoremObjectToString).ToList();
                secondTwo.Sort();

                var firstPart = $"[{ string.Join(", ", firstTwo)}]";
                var secondPart = $"[{string.Join(", ", secondTwo)}]";

                return $"[{containersNumbersString}] {theorem.Type}: {(firstPart.CompareTo(secondPart) < 0 ? firstPart : secondPart)} {(firstPart.CompareTo(secondPart) < 0 ? secondPart : firstPart)}";
            }

            var list = theorem.InvolvedObjects.Select(TheoremObjectToString).ToList();

            list.Sort();

            return $"[{containersNumbersString}] {theorem.Type}: {string.Join(", ", list)}";
        }

        private string ConstructedObjectToString(ConstructedConfigurationObject constructedObject)
        {
            var result = new StringBuilder();

            result.Append(ConstructionName(constructedObject.Construction))
                    .Append("(")
                    .Append(string.Join(", ", constructedObject.PassedArguments.Select(ArgumentToString)))
                    .Append(")");

            return result.ToString();
        }

        private string ConstructionName(Construction construction)
        {
            if (construction is PredefinedConstruction predefinedConstruction)
                return Regex.Match(predefinedConstruction.Type.ToString(), "(.*)From.*").Groups[1].Value;

            return construction.Name;
        }

        private string ArgumentToString(ConstructionArgument argument)
        {
            if (argument is ObjectConstructionArgument objectArgument)
            {
                return ObjectToStringById(objectArgument.PassedObject);
            }

            var setArgument = (SetConstructionArgument) argument;

            return $"{{{string.Join(", ", setArgument.PassedArguments.Select(ArgumentToString))}}}";
        }

        private string TheoremObjectToString(TheoremObject theoremObject)
        {
            if (theoremObject.Signature == TheoremObjectSignature.SingleObject)
            {
                return ObjectToStringById(theoremObject.InternalObjects.First());
            }

            var isLine = theoremObject.Signature == TheoremObjectSignature.LineGivenByPoints;

            var list = theoremObject.InternalObjects.Select(ObjectToStringById).ToList();

            list.Sort();

            return $"{(isLine ? "[" : "(")}{string.Join(", ", list)}{(isLine ? "]" : ")")}";
        }

        private string ObjectToStringById(ConfigurationObject configurationObject)
        {
            return _objectToName[configurationObject].ToString();
        }
    }
}