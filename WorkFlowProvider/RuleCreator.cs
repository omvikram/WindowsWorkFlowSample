using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using BusinessObjects;
using System;
using System.Text;

namespace WorkFlowProvider
{
	public class RuleCreator
	{

		public static void CreateDynamicRule(string fileNameWithPath, TPromotionRule promotionRuleObj)
		{
			Rule minTktFareRule = null;
			Rule validityStartDateRule = null;
			Rule validityEndDateRule = null;
			Rule validDayRule = null;
			Rule validRouteRule = null;
			Rule validBulkBookingRule = null;
			Rule validAdvanceBookingRule = null;
			Rule validLastMinuteBookingRule = null;
			Rule discountRule = null;
			Rule offerCodeRule = null;
			Rule maxDiscountLimitRule = null;
			
			RuleSet ruleSet = new RuleSet(promotionRuleObj.OfferCode +  "RuleSet");
			
			if(promotionRuleObj.MinTicketFare != decimal.Zero)
			{
				minTktFareRule = CreateMinTicketFareRule(promotionRuleObj);
				ruleSet.Rules.Add(minTktFareRule);
			}

			if (promotionRuleObj.ValidityStartDate != null)
			{
				validityStartDateRule = CreateValidityStartDateRule(promotionRuleObj);
				ruleSet.Rules.Add(validityStartDateRule);
			}

			if (promotionRuleObj.ValidityEndDate != null)
			{
				validityEndDateRule = CreateValidityEndDateRule(promotionRuleObj);
				ruleSet.Rules.Add(validityEndDateRule);
			}

			if(promotionRuleObj.DaysCSV != null)
			{
			   validDayRule = CreateValidDayRule(promotionRuleObj);
			   ruleSet.Rules.Add(validDayRule);
			}

			if (promotionRuleObj.RouteIDCSV != null)
			{
				validRouteRule = CreateValidRouteRule(promotionRuleObj);
				ruleSet.Rules.Add(validRouteRule);
			}

			if (promotionRuleObj.MinNumOfSeats != null)
			{
				validBulkBookingRule = CreateValidBulkBookingRule(promotionRuleObj);
				ruleSet.Rules.Add(validBulkBookingRule);
			}

			if (promotionRuleObj.MinNumOfAdvanceBookingDays != null)
			{
				validAdvanceBookingRule = CreateValidAdvanceBookingRule(promotionRuleObj);
				ruleSet.Rules.Add(validAdvanceBookingRule);
			}

			if (promotionRuleObj.LastMinuteCountBeforeDept != null)
			{
				validLastMinuteBookingRule = CreateValidLastMinBookingRule(promotionRuleObj);
				ruleSet.Rules.Add(validLastMinuteBookingRule);
			}

			if (promotionRuleObj.DiscountUnit != decimal.Zero)
			{
				discountRule = CreateDiscountUnitRule(promotionRuleObj);
			}
			else if(promotionRuleObj.DiscountPercent != decimal.Zero)
			{
				discountRule = CreateDiscountPercentRule(promotionRuleObj);
			}

			ruleSet.Rules.Add(discountRule);

			offerCodeRule = CreateOfferCodeRule(promotionRuleObj);
			ruleSet.Rules.Add(offerCodeRule);

			if(promotionRuleObj.MaxDiscountAmount != decimal.Zero)
			{
				maxDiscountLimitRule = CreateMaxDiscountLimitRule(promotionRuleObj);
				ruleSet.Rules.Add(maxDiscountLimitRule);
			}

			using (XmlWriter xmlWriter = XmlWriter.Create(fileNameWithPath))
			{
				WorkflowMarkupSerializer markupSerializer = new WorkflowMarkupSerializer();
				markupSerializer.Serialize(xmlWriter, ruleSet);
			}
		}

		public static string CreateDynamicRule(TPromotionRule promotionRuleObj)
		{
			Rule minTktFareRule = null;
			Rule validityStartDateRule = null;
			Rule validityEndDateRule = null;
			Rule validDayRule = null;
			Rule validRouteRule = null;
			Rule validBulkBookingRule = null;
			Rule validAdvanceBookingRule = null;
			Rule validLastMinuteBookingRule = null;
			Rule discountRule = null;
			Rule offerCodeRule = null;
			Rule maxDiscountLimitRule = null;

			StringBuilder strBldr = new StringBuilder();

			RuleSet ruleSet = new RuleSet(promotionRuleObj.OfferCode + "RuleSet");

			if (promotionRuleObj.MinTicketFare != decimal.Zero)
			{
				minTktFareRule = CreateMinTicketFareRule(promotionRuleObj);
				ruleSet.Rules.Add(minTktFareRule);
			}

			if (promotionRuleObj.ValidityStartDate != null)
			{
				validityStartDateRule = CreateValidityStartDateRule(promotionRuleObj);
				ruleSet.Rules.Add(validityStartDateRule);
			}

			if (promotionRuleObj.ValidityEndDate != null)
			{
				validityEndDateRule = CreateValidityEndDateRule(promotionRuleObj);
				ruleSet.Rules.Add(validityEndDateRule);
			}

			if (promotionRuleObj.DaysCSV != null)
			{
				validDayRule = CreateValidDayRule(promotionRuleObj);
				ruleSet.Rules.Add(validDayRule);
			}

			if (promotionRuleObj.RouteIDCSV != null)
			{
				validRouteRule = CreateValidRouteRule(promotionRuleObj);
				ruleSet.Rules.Add(validRouteRule);
			}

			if (promotionRuleObj.MinNumOfSeats != null)
			{
				validBulkBookingRule = CreateValidBulkBookingRule(promotionRuleObj);
				ruleSet.Rules.Add(validBulkBookingRule);
			}

			if (promotionRuleObj.MinNumOfAdvanceBookingDays != null)
			{
				validAdvanceBookingRule = CreateValidAdvanceBookingRule(promotionRuleObj);
				ruleSet.Rules.Add(validAdvanceBookingRule);
			}

			if (promotionRuleObj.LastMinuteCountBeforeDept != null)
			{
				validLastMinuteBookingRule = CreateValidLastMinBookingRule(promotionRuleObj);
				ruleSet.Rules.Add(validLastMinuteBookingRule);
			}

			if (promotionRuleObj.DiscountUnit != decimal.Zero)
			{
				discountRule = CreateDiscountUnitRule(promotionRuleObj);
			}
			else if (promotionRuleObj.DiscountPercent != decimal.Zero)
			{
				discountRule = CreateDiscountPercentRule(promotionRuleObj);
			}

			ruleSet.Rules.Add(discountRule);

			offerCodeRule = CreateOfferCodeRule(promotionRuleObj);
			ruleSet.Rules.Add(offerCodeRule);

			if (promotionRuleObj.MaxDiscountAmount != decimal.Zero)
			{
				maxDiscountLimitRule = CreateMaxDiscountLimitRule(promotionRuleObj);
				ruleSet.Rules.Add(maxDiscountLimitRule);
			}

			using (XmlWriter xmlWriter = XmlWriter.Create(strBldr))
			{
				WorkflowMarkupSerializer markupSerializer = new WorkflowMarkupSerializer();
				markupSerializer.Serialize(xmlWriter, ruleSet);
			}

			return strBldr.ToString();
		}

		private static Rule CreateMinTicketFareRule(TPromotionRule promotionRuleObj)
		{
			Rule minTktFareRule = new Rule("MinTicketFareRule");
			minTktFareRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			minTktFareRule.Priority = 100;

			RuleStatementAction minTktFareThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("VALID"))
																		);
			RuleStatementAction minTktFareElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("INVALID. Fare should be greater than" + promotionRuleObj.MinTicketFare))
																		);

			RuleCondition minTktFareCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"),"OrderValue"),
																			CodeBinaryOperatorType.GreaterThanOrEqual,
																			new CodePrimitiveExpression(promotionRuleObj.MinTicketFare))
																		);
			minTktFareRule.Condition = minTktFareCondition;
			minTktFareRule.ThenActions.Add(minTktFareThenAction);
			minTktFareRule.ElseActions.Add(minTktFareElseAction);
			minTktFareRule.ElseActions.Add(new RuleHaltAction());

			return minTktFareRule;
		}


		private static Rule CreateValidityStartDateRule(TPromotionRule promotionRuleObj)
		{
			Rule validityStartDateRule = new Rule("ValidityStartDateRule");
			validityStartDateRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityStartDateRule.Priority = 90;

			RuleStatementAction validityStartDateThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityStartDateElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("INVALID"))
																		);

			RuleCondition validityStartDateCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"),"DOJ"),
																			CodeBinaryOperatorType.GreaterThanOrEqual,
																			new CodePrimitiveExpression(promotionRuleObj.ValidityStartDate))
																		);
			validityStartDateRule.Condition = validityStartDateCondition;
			validityStartDateRule.ThenActions.Add(validityStartDateThenAction);
			validityStartDateRule.ElseActions.Add(validityStartDateElseAction);
			validityStartDateRule.ElseActions.Add(new RuleHaltAction());

			return validityStartDateRule;
		}

		private static Rule CreateValidityEndDateRule(TPromotionRule promotionRuleObj)
		{
			Rule validityEndDateRule = new Rule("ValidityEndDateRule");
			validityEndDateRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityEndDateRule.Priority = 80;

			RuleStatementAction validityEndDateThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityEndDateElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("INVALID"))
																		);

			RuleCondition validityEndDateCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"),"DOJ"),
																			CodeBinaryOperatorType.LessThan,
																			new CodePrimitiveExpression(promotionRuleObj.ValidityEndDate))
																		);
			validityEndDateRule.Condition = validityEndDateCondition;
			validityEndDateRule.ThenActions.Add(validityEndDateThenAction);
			validityEndDateRule.ElseActions.Add(validityEndDateElseAction);
			validityEndDateRule.ElseActions.Add(new RuleHaltAction());

			return validityEndDateRule;
		}

		private static Rule CreateValidDayRule(TPromotionRule promotionRuleObj)
		{
			Rule validityDayRule = new Rule("ValidityDayRule");
			validityDayRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityDayRule.Priority = 70;

			RuleStatementAction validityDayThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityDayElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("INVALID. This offer is not applicable today."))
																		);

			/*RuleCondition validityDayCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(
																			new CodeMethodInvokeExpression(
																			new CodeThisReferenceExpression(),
																			"IsElementExistInArray",
																			new CodeExpression[] {new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
			                                                                new CodeThisReferenceExpression(), "InputObj"),"DayOfWeek"), 
			                                                                new CodePrimitiveExpression(promotionRuleObj.DaysCSV)}),
																			CodeBinaryOperatorType.BooleanAnd,
																			new CodePrimitiveExpression(true))
																		);*/

			RuleCondition validityDayCondition = new RuleExpressionCondition(
																				new CodeBinaryOperatorExpression(
																					new CodeMethodInvokeExpression(
																							new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(BusinessObjects.TPromotion)), "IsElementExistInArray")
																						,	new CodeExpression[] {	new CodePropertyReferenceExpression(	new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "InputObj"),"DayOfWeek")
																																						, 	new CodePrimitiveExpression(promotionRuleObj.DaysCSV)}
																					)
																					, CodeBinaryOperatorType.BooleanAnd
																					, new CodePrimitiveExpression(true)
																				)
																			);
			validityDayRule.Condition = validityDayCondition;
			validityDayRule.ThenActions.Add(validityDayThenAction);
			validityDayRule.ElseActions.Add(validityDayElseAction);
			validityDayRule.ElseActions.Add(new RuleHaltAction());

			return validityDayRule;
		}

		private static Rule CreateValidRouteRule(TPromotionRule promotionRuleObj)
		{
			Rule validityRouteRule = new Rule("ValidityRouteRule");
			validityRouteRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityRouteRule.Priority = 60;

			RuleStatementAction validityRouteThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityRouteElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("INVALID. This offer is not applicable for this route."))
																		);

			/*RuleCondition validityRouteCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(
																			new CodeMethodInvokeExpression(
																			new CodeThisReferenceExpression(),
																			"IsElementExistInArray",
																			new CodeExpression[] {new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"),"RouteId"), 
																			new CodePrimitiveExpression(promotionRuleObj.RouteIDCSV)}),
																			CodeBinaryOperatorType.BooleanAnd,
																			new CodePrimitiveExpression(true))
																		);*/

			RuleCondition validityRouteCondition = new RuleExpressionCondition(
																				new CodeBinaryOperatorExpression(
																					new CodeMethodInvokeExpression(
																							new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(BusinessObjects.TPromotion)), "IsElementExistInArray")
																						, new CodeExpression[] {	new CodePropertyReferenceExpression(	new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "InputObj"),"RouteId")
																																						, 	new CodePrimitiveExpression(promotionRuleObj.RouteIDCSV)}
																					)
																					, CodeBinaryOperatorType.BooleanAnd
																					, new CodePrimitiveExpression(true)
																				)
																			);

			validityRouteRule.Condition = validityRouteCondition;
			validityRouteRule.ThenActions.Add(validityRouteThenAction);
			validityRouteRule.ElseActions.Add(validityRouteElseAction);
			validityRouteRule.ElseActions.Add(new RuleHaltAction());

			return validityRouteRule;
		}


		private static Rule CreateValidBulkBookingRule(TPromotionRule promotionRuleObj)
		{
			Rule validityBulkBookingRule = new Rule("ValidityBulkBookingRule");
			validityBulkBookingRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityBulkBookingRule.Priority = 55;

			RuleStatementAction validityBulkBookingThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"), "Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityBulkBookingRouteElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"), "Message"),
																			new CodePrimitiveExpression("INVALID. This offer is only applicable for bulk booking."))
																		);

			RuleCondition validityBulkBookingRouteCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"), "NumOfSeatsForBooking"),
																			CodeBinaryOperatorType.GreaterThanOrEqual,
																			new CodePrimitiveExpression(promotionRuleObj.MinNumOfSeats))
																		);

			validityBulkBookingRule.Condition = validityBulkBookingRouteCondition;
			validityBulkBookingRule.ThenActions.Add(validityBulkBookingThenAction);
			validityBulkBookingRule.ElseActions.Add(validityBulkBookingRouteElseAction);
			validityBulkBookingRule.ElseActions.Add(new RuleHaltAction());

			return validityBulkBookingRule;
		}

		private static Rule CreateValidAdvanceBookingRule(TPromotionRule promotionRuleObj)
		{
			Rule validityAdvanceBookingRule = new Rule("ValidityAdvanceBookingRule");
			validityAdvanceBookingRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityAdvanceBookingRule.Priority = 50;

			RuleStatementAction validityAdvanceBookingThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"), "Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityAdvanceBookingElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"), "Message"),
																			new CodePrimitiveExpression("INVALID"))
																		);

			RuleCondition validityAdvanceBookingCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"), "NumOfAdvanceBookingDays"),
																			CodeBinaryOperatorType.GreaterThanOrEqual,
																			new CodePrimitiveExpression(promotionRuleObj.MinNumOfAdvanceBookingDays))
																		);

			validityAdvanceBookingRule.Condition = validityAdvanceBookingCondition;
			validityAdvanceBookingRule.ThenActions.Add(validityAdvanceBookingThenAction);
			validityAdvanceBookingRule.ElseActions.Add(validityAdvanceBookingElseAction);
			validityAdvanceBookingRule.ElseActions.Add(new RuleHaltAction());

			return validityAdvanceBookingRule;
		}

		private static Rule CreateValidLastMinBookingRule(TPromotionRule promotionRuleObj)
		{
			Rule validityLastMinBookingRule = new Rule("ValidityLastMinBookingRule");
			validityLastMinBookingRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			validityLastMinBookingRule.Priority = 45;

			RuleStatementAction validityLastMinBookingThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"), "Message"),
																			new CodePrimitiveExpression("VALID"))
																		);

			RuleStatementAction validityLastMinBookingElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"), "Message"),
																			new CodePrimitiveExpression("INVALID"))
																		);

			RuleCondition validityLastMinBookingCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"), "MinuteCountBeforeDept"),
																			CodeBinaryOperatorType.LessThanOrEqual,
																			new CodePrimitiveExpression(promotionRuleObj.LastMinuteCountBeforeDept))
																		);

			validityLastMinBookingRule.Condition = validityLastMinBookingCondition;
			validityLastMinBookingRule.ThenActions.Add(validityLastMinBookingThenAction);
			validityLastMinBookingRule.ElseActions.Add(validityLastMinBookingElseAction);
			validityLastMinBookingRule.ElseActions.Add(new RuleHaltAction());

			return validityLastMinBookingRule;
		}


		public static Rule CreateDiscountUnitRule(TPromotionRule promotionRuleObj)
		{
			Rule discountRule = new Rule("DiscountUnitRule");
			discountRule.ReevaluationBehavior = RuleReevaluationBehavior.Always;
			discountRule.Priority = 30;
			
			RuleStatementAction discountThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Discount"),
																			new CodePrimitiveExpression(promotionRuleObj.DiscountUnit))
																			);


			RuleStatementAction discountElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Discount"),
																			new CodePrimitiveExpression(0))
																		);

			RuleCondition discountCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			CodeBinaryOperatorType.ValueEquality,
																			new CodePrimitiveExpression("VALID"))
																		);
			discountRule.Condition = discountCondition;
			discountRule.ThenActions.Add(discountThenAction);
			discountRule.ElseActions.Add(discountElseAction);

			return discountRule;
		}

		private static Rule CreateDiscountPercentRule(TPromotionRule promotionRuleObj)
		{
			Rule discountRule = new Rule("DiscountPercentRule");
			discountRule.ReevaluationBehavior = RuleReevaluationBehavior.Always;
			discountRule.Priority = 30;


			RuleStatementAction discountThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Discount"),
																			new CodeBinaryOperatorExpression(
																			new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"),"OrderValue"),
																			CodeBinaryOperatorType.Multiply,
																			new CodePrimitiveExpression(promotionRuleObj.DiscountPercent/100))
																		));


			RuleStatementAction discountElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Discount"),
																			new CodePrimitiveExpression(0))
																		);

			RuleCondition discountCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			CodeBinaryOperatorType.ValueEquality,
																			new CodePrimitiveExpression("VALID"))
																		);
			discountRule.Condition = discountCondition;
			discountRule.ThenActions.Add(discountThenAction);
			discountRule.ElseActions.Add(discountElseAction);

			return discountRule;
		}

		private static Rule CreateMaxDiscountLimitRule(TPromotionRule promotionRuleObj)
		{
			Rule maxDiscountLimitRule = new Rule("MaxDiscountLimitRule");
			maxDiscountLimitRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			maxDiscountLimitRule.Priority = 20;

			RuleStatementAction maxDiscountLimitThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Discount"),
																			new CodePrimitiveExpression(promotionRuleObj.MaxDiscountAmount))
																		);

			RuleCondition maxDiscountLimitCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(
																				new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																				new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																				CodeBinaryOperatorType.ValueEquality,
																				new CodePrimitiveExpression("VALID")),
																			CodeBinaryOperatorType.BooleanAnd,
																				new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																				new CodeThisReferenceExpression(), "OutputObj"),"Discount"),
																				CodeBinaryOperatorType.GreaterThanOrEqual,
																				new CodePrimitiveExpression(promotionRuleObj.MaxDiscountAmount)))
																		);
			maxDiscountLimitRule.Condition = maxDiscountLimitCondition;
			maxDiscountLimitRule.ThenActions.Add(maxDiscountLimitThenAction);

			return maxDiscountLimitRule;
		}

		private static Rule CreateOfferCodeRule(TPromotionRule promotionRuleObj)
		{
			Rule offerCodeRule = new Rule("OfferCodeRule");
			offerCodeRule.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			offerCodeRule.Priority = 10;

			RuleStatementAction offerCodeThenAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("VALID"))
																		);
			RuleStatementAction offerCodeElseAction = new RuleStatementAction(
																			new CodeAssignStatement(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "OutputObj"),"Message"),
																			new CodePrimitiveExpression("INVALID. Invalid offer code."))
																		);

			RuleCondition offerCodeCondition = new RuleExpressionCondition(
																			new CodeBinaryOperatorExpression(new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(
																			new CodeThisReferenceExpression(), "InputObj"),"OfferCode"),
																			CodeBinaryOperatorType.ValueEquality,
																			new CodePrimitiveExpression(promotionRuleObj.OfferCode))
																		);
			offerCodeRule.Condition = offerCodeCondition;
			offerCodeRule.ThenActions.Add(offerCodeThenAction);
			offerCodeRule.ElseActions.Add(offerCodeElseAction);
			offerCodeRule.ElseActions.Add(new RuleHaltAction());

			return offerCodeRule;
		}
	}
}
