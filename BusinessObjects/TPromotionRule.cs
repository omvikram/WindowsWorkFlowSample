using System;

namespace BusinessObjects
{
	public class TPromotionRule
	{
		public int? MinNumOfSeats { get; set; }
		public int? MinNumOfAdvanceBookingDays { get; set; }
		public int? LastMinuteCountBeforeDept { get; set; }

		public string OfferCode { get; set; }
		public string OfferDesciption { get; set; }

		public decimal DiscountPercent { get; set; }
		public decimal DiscountUnit { get; set; }
		public decimal MinTicketFare { get; set; }
		public decimal MaxDiscountAmount {get; set; }

		public string RouteIDCSV {get; set;}
		public string DaysCSV {get; set;}

		public DateTime ValidityStartDate { get; set; }
		public DateTime ValidityEndDate { get; set; }
	}
}
