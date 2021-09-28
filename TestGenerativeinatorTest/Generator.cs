using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace TestGenerativeinatorTest
{
    public class GeneratorAttribute : TestMethodAttribute
    {
        public GeneratorAttribute(int times)
        {
            this.Times = times;
        }

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var runTestResults = base.Execute(testMethod);

            if (runTestResults.Any(x => x.Outcome != UnitTestOutcome.Passed))
                return runTestResults;

            var testResults = new List<TestResult>(Times + runTestResults.Length);

            testResults.AddRange(runTestResults);

            for (int i = 0; i < Times; i++)
            {
                var parametersInfo = testMethod.MethodInfo.GetParameters();

                var parameters = GetRandomParameters(parametersInfo);
                var testResult = testMethod.Invoke(parameters);

                var parametersDescriptions = parameters.Select((p, i) => $"{parametersInfo[i].Name}: {p}");

                testResult.LogOutput = $"Parameters: {String.Join(", ", parametersDescriptions)}";

                testResults.Add(testResult);
            }

            return testResults
                .OrderBy(x => x.Outcome == UnitTestOutcome.Passed)
                .ToArray();

        }

        private object[] GetRandomParameters(ParameterInfo[] parametersInfo)
        {
            var parameters = new object[parametersInfo.Length];

            for (int i = 0; i < parametersInfo.Length; i++)
            {
                if (parametersInfo[i].ParameterType == typeof(int))
                {
                    parameters[i] = new Random().Next(0, 100000);
                }
            }

            return parameters;
        }

        public int Times { get; }
    }
}
