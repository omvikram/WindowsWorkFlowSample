using System;
using System.Reflection;
using System.Workflow.Activities.Rules;
using BusinessObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkFlowProvider;

namespace WorkflowController
{
    [TestClass]
    public class RuleController
    {
        [TestMethod]
        public TPromotion ApplyRule(TPromotion promObj, string ruleName = "")
        {
			RuleSet newRule = null;

			TPromotion promOutputObj = new TPromotion();
			promObj.OutputObj = new OutputObject();

			string path = Assembly.GetExecutingAssembly().Location.Replace(Assembly.GetExecutingAssembly().ManifestModule.Name, String.Empty);

			if(String.IsNullOrEmpty(ruleName))
			{
				ruleName = promObj.InputObj.OfferCode;

				newRule = RulesEvaluator<TPromotion>.LaunchNewRulesDialog(ruleName, path);

				promOutputObj = RulesEvaluator<TPromotion>.ProcessRule(promObj, newRule);
			}
			else
			{
				newRule = RulesEvaluator<TPromotion>.LoadRule(path, ruleName);

				promOutputObj = RulesEvaluator<TPromotion>.ProcessRule(promObj, newRule);
			}
			
			return promOutputObj;
       }

	   public void CreateDynamicRule(TPromotionRule promotionRuleObj)
	   {
		   string path = Assembly.GetExecutingAssembly().Location.Replace(Assembly.GetExecutingAssembly().ManifestModule.Name, String.Empty);
		   string fileNameWithPath = string.Empty;

		   if (promotionRuleObj != null)
		   {
				fileNameWithPath = path + promotionRuleObj.OfferCode + ".rules";
		   }

		   RuleCreator.CreateDynamicRule(fileNameWithPath, promotionRuleObj);
	   }
    }
}
