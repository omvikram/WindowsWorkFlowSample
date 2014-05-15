
#region Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Workflow.Activities.Rules;
using System.Workflow.Activities.Rules.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using System.Text;

#endregion

namespace WorkFlowProvider
{
    /// <summary>
    /// Implements a wrapper around the Windows Workflow Foundation Rules Engine
    /// </summary>
    /// <typeparam name="T">A Data Object type to process</typeparam>
    public static class RulesEvaluator<T> where T : new()
    {
        #region Rules Editor Support

        /// <summary>
        /// Launch the Rules Form to create a new rule
        /// </summary>
        /// <param name="existingRule"></param>
        /// <param name="ruleName"></param>
        /// <param name="outputPath"></param>
        /// <param name="overwriteFile"></param>
        public static RuleSet LaunchNewRulesDialog(string ruleName, string outputPath)
        {
            return LaunchRulesDialog(null, ruleName, outputPath);
        }

        /// <summary>
        /// Launch the Rules Editor with an existing rule (for editing), or to create a new rule
        /// </summary>
        /// <param name="ruleSet">An existing rule, or pass NULL to create a new rule</param>
        /// <param name="ruleName">The rule name (for the file name)</param>
        /// <param name="outputPath">The path to save rules to</param>
        /// <returns>A rule (if one is saved/edited)</returns>
        public static RuleSet LaunchRulesDialog(RuleSet ruleSet, string ruleName, string outputPath)
        {
            // You could pass in an existing ruleset object for editing if you wanted to, we're creating a new rule, so it's set to null
            RuleSetDialog ruleSetDialog = new RuleSetDialog(typeof(T), null, ruleSet);

            if (ruleSetDialog.ShowDialog() == DialogResult.OK)
            {
                // grab the ruleset
                ruleSet = ruleSetDialog.RuleSet;
                // We're going to serialize it to disk so it can be reloaded later
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();

                string fileName = String.Format("{0}.rules", ruleName);
                string fullName = Path.Combine(outputPath, fileName);

                if (File.Exists(fullName))
                {
                    File.Delete(fullName); //delete existing rule
                }

                using (XmlWriter rulesWriter = XmlWriter.Create(fullName))
                {
                    serializer.Serialize(rulesWriter, ruleSet);
                    rulesWriter.Close();
                }
            }
            return ruleSet;
        }

        #endregion

        #region Rule Processing

        /// <summary>
        /// Applies a set of rules to a specified data object
        /// </summary>
        /// <param name="employeeToProcess"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static T ProcessRules(T objectToProcess, ReadOnlyCollection<RuleSet> rules)
        {
            RuleValidation validation = new RuleValidation(typeof(T), null);
            RuleExecution execution = new RuleExecution(validation, objectToProcess);

            foreach (RuleSet rule in rules)
            {
                rule.Execute(execution);
            }

            return objectToProcess;
        }

        /// <summary>
        /// Execute a single rule on a single data object
        /// </summary>
        /// <param name="objectToProcess"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static T ProcessRule(T objectToProcess, RuleSet rule)
        {
            RuleValidation validation = new RuleValidation(typeof(T), null);
            RuleExecution execution = new RuleExecution(validation, objectToProcess);
			
			rule.Execute(execution);

            return objectToProcess;
        }

        #endregion

        #region Rules Management

        /// <summary>
        /// Loads a single rule given a path and file name
        /// </summary>
        /// <param name="rulesLocation"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static RuleSet LoadRule(string rulesLocation, string fileName)
        {
            RuleSet ruleSet = null;

            // Deserialize from a .rules file.
            using (XmlTextReader rulesReader = new XmlTextReader(Path.Combine(rulesLocation, fileName)))
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                ruleSet = (RuleSet)serializer.Deserialize(rulesReader);
            }

            return ruleSet;
        }


		/// <summary>
		/// Loads a single rule given the rule xml
		/// </summary>
		/// <param name="ruleXML"></param>
		/// <returns></returns>
		public static RuleSet LoadRule(string ruleXML)
		{
			RuleSet ruleSet = null;

			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(ruleXML));

			// Deserialize from a .rules file.
			using (XmlTextReader rulesReader = new XmlTextReader(ms))
			{
				WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
				ruleSet = (RuleSet)serializer.Deserialize(rulesReader);
			}

			return ruleSet;
		}

        /// <summary>
        /// Loads a set of rules from disk
        /// </summary>
        /// <param name="rulesLocation"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<RuleSet> LoadRules(string rulesLocation)
        {
            RuleSet ruleSet = null;
            List<RuleSet> rules = new List<RuleSet>();

            foreach (string fileName in Directory.GetFiles(rulesLocation, "*.rules"))
            {
                // Deserialize from a .rules file.
                using (XmlTextReader rulesReader = new XmlTextReader(fileName))
                {
                    WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                    ruleSet = (RuleSet)serializer.Deserialize(rulesReader);
                    rules.Add(ruleSet);
                    rulesReader.Close();
                }
            }
            return rules.AsReadOnly();
        }

        #endregion
    }
}