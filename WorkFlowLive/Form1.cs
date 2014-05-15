using System;
using System.Windows.Forms;
using BusinessObjects;
using WorkflowController;

namespace WorkFlowLive
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			RuleController ruleCtrlObj = null;
			TPromotionRule promotionRuleObj = null;

			bool isDiscountPercent = false;
			bool isDiscountAmount = false;

			string offerCode = string.Empty;
			string offerCodeDesc = string.Empty;

			if (!String.IsNullOrEmpty(textBox4.Text))
			{
				offerCode = textBox4.Text;
			}
			else
			{
				label9.Text = "Please enter the offer code";
				label9.ForeColor = System.Drawing.Color.Red;
				return;
			}

			if (!String.IsNullOrEmpty(textBox5.Text))
			{
				offerCodeDesc = textBox5.Text;
			}
			else
			{
				label9.Text = "Please enter the offer code description";
				label9.ForeColor = System.Drawing.Color.Red;
				return;
			}

			if (!String.IsNullOrEmpty(textBox6.Text))
			{
				isDiscountPercent = true;
			}
			if (!String.IsNullOrEmpty(textBox7.Text))
			{
				isDiscountAmount = true;
			}

			if (isDiscountPercent || isDiscountAmount)
			{
				decimal discount = decimal.Zero;

				if (isDiscountPercent && Convert.ToDecimal(textBox6.Text) > 0)
				{
					discount = Convert.ToDecimal(textBox6.Text);
				}
				else if (isDiscountAmount && Convert.ToDecimal(textBox7.Text) > 0)
				{
					discount = Convert.ToDecimal(textBox7.Text);
				}

				promotionRuleObj = new TPromotionRule();
				promotionRuleObj.OfferCode = offerCode;
				promotionRuleObj.OfferDesciption = offerCodeDesc;
				if (isDiscountAmount) promotionRuleObj.DiscountUnit = discount;
				else if (isDiscountPercent) promotionRuleObj.DiscountPercent = discount;
			}
			else
			{
				label9.Text = "Please enter vaild discount";
				label9.ForeColor = System.Drawing.Color.Red;
				return;
			}

			if (!String.IsNullOrEmpty(textBox8.Text))
			{
				promotionRuleObj.MinTicketFare = Convert.ToDecimal(textBox8.Text);
			}
			else
			{
				promotionRuleObj.MinTicketFare = decimal.Zero;
			}

			if (!String.IsNullOrEmpty(textBox9.Text))
			{
				promotionRuleObj.MaxDiscountAmount = Convert.ToDecimal(textBox9.Text);
			}
			else
			{
				promotionRuleObj.MaxDiscountAmount = decimal.Zero;
			}

			if(!String.IsNullOrEmpty(textBox11.Text))
			{
				promotionRuleObj.ValidityStartDate = DateTime.Parse(textBox11.Text);
			}
			
			if(!String.IsNullOrEmpty(textBox12.Text))
			{
				promotionRuleObj.ValidityEndDate = DateTime.Parse(textBox12.Text);
			}

			if(!String.IsNullOrEmpty(textBox14.Text))
			{
				promotionRuleObj.DaysCSV = textBox14.Text;
			}

			if(!String.IsNullOrEmpty(textBox15.Text))
			{
				promotionRuleObj.RouteIDCSV = textBox15.Text;
			}

			if (!String.IsNullOrEmpty(textBox17.Text))
			{
				promotionRuleObj.MinNumOfSeats = Convert.ToInt32(textBox17.Text);
			}

			if (!String.IsNullOrEmpty(textBox18.Text))
			{
				promotionRuleObj.MinNumOfAdvanceBookingDays = Convert.ToInt32(textBox18.Text);
			}

			if (!String.IsNullOrEmpty(textBox19.Text))
			{
				promotionRuleObj.LastMinuteCountBeforeDept = Convert.ToInt32(textBox19.Text);
			}

			try
			{
				ruleCtrlObj = new RuleController();
				ruleCtrlObj.CreateDynamicRule(promotionRuleObj);
				label9.Text = "Nice!! The rule has been created.";
				label9.ForeColor = System.Drawing.Color.Green;
			}
			catch (Exception ex)
			{
				label9.Text = "Sorry!! The expected rule can't be created right now";
				label9.ForeColor = System.Drawing.Color.Red;
				return;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			TPromotion promObj = new TPromotion();
			promObj.InputObj = new InputObject();

			if (!String.IsNullOrEmpty(textBox1.Text))
			{
				promObj.InputObj.OfferCode = textBox1.Text;
			}
			else
			{
				label2.Text = "Please enter the offer code";
				label2.ForeColor = System.Drawing.Color.Red;
				return;
			}


			if (!String.IsNullOrEmpty(textBox2.Text) && Convert.ToDecimal(textBox2.Text) > 0)
			{
				promObj.InputObj.OrderValue = Convert.ToDecimal(textBox2.Text);
			}
			else
			{
				label2.Text = "Please enter vaild amount";
				label2.ForeColor = System.Drawing.Color.Red;
				return;
			}

			if(!String.IsNullOrEmpty(textBox13.Text))
			{
				promObj.InputObj.DOJ = DateTime.Parse(textBox13.Text);
				promObj.InputObj.DayOfWeek = promObj.InputObj.DOJ.DayOfWeek.ToString();
			}

			if(!String.IsNullOrEmpty(textBox16.Text))
			{
				promObj.InputObj.RouteId = textBox16.Text;
			}

			if (!String.IsNullOrEmpty(textBox10.Text))
			{
				promObj.InputObj.NumOfSeatsForBooking = Convert.ToInt32(textBox10.Text);
			}

			if (!String.IsNullOrEmpty(textBox20.Text))
			{
				promObj.InputObj.NumOfAdvanceBookingDays = Convert.ToInt32(textBox20.Text);
			}

			if (!String.IsNullOrEmpty(textBox21.Text))
			{
				promObj.InputObj.MinuteCountBeforeDept = Convert.ToInt32(textBox21.Text);
			}

			RuleController ruleCtrlObj = new RuleController();

			TPromotion promOutputObj = null;
			if (!String.IsNullOrEmpty(textBox3.Text))
			{
				promOutputObj = ruleCtrlObj.ApplyRule(promObj, textBox3.Text);
			}
			else
			{
				promOutputObj = ruleCtrlObj.ApplyRule(promObj);
			}

			if (promOutputObj != null && promOutputObj.OutputObj != null && promObj.OutputObj.Discount != 0)
			{
				decimal finalFare = (decimal)(promObj.InputObj.OrderValue - promOutputObj.OutputObj.Discount);
				label2.Text = "Congratulations!!! You have got an offer of Rs. " + promOutputObj.OutputObj.Discount.ToString()
								+ "\r\n Final Fare : Rs. " + finalFare;
				label2.ForeColor = System.Drawing.Color.Green;
			}
			else
			{
				label2.Text = "Oops!!! You are not eligible for this offer.";
				label2.ForeColor = System.Drawing.Color.Red;
			} 
		}
	}
}
