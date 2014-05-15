using System;

namespace BusinessObjects
{
	public class TPromotion
	{
		public InputObject InputObj { get; set; }
		public OutputObject OutputObj {get; set;}

		public bool IsElementExistInArray(string element, string elementCSV)
		{
			bool isExist = false;

			if(!String.IsNullOrEmpty(element) && !String.IsNullOrEmpty(elementCSV))
			{
				string[] elementArr = elementCSV.Split(',');
				 
				 if(elementArr != null && elementArr.Length > 0)
				 {
					 for(int i = 0; i < elementArr.Length; i++)
					 {
						if(element.Equals(elementArr[i]))
						{
							isExist = true;
							break;
						}
					 }
				 }
			}

			return isExist;
		}

		public static bool IsElementExistInArray1(string element, string elementCSV)
		{
			bool isExist = false;

			if (!String.IsNullOrEmpty(element) && !String.IsNullOrEmpty(elementCSV))
			{
				string[] elementArr = elementCSV.Split(',');

				if (elementArr != null && elementArr.Length > 0)
				{
					for (int i = 0; i < elementArr.Length; i++)
					{
						if (element.Equals(elementArr[i]))
						{
							isExist = true;
							break;
						}
					}
				}
			}

			return isExist;
		}
	}

	public class InputObject
	{
		public int? NumOfSeatsForBooking { get; set; }
		public int? NumOfAdvanceBookingDays { get; set; }
		public int? MinuteCountBeforeDept { get; set; }

		public string OfferCode { get; set; }
		public decimal OrderValue { get; set; }
		public DateTime DOJ { get; set; }
		public string RouteId { get; set; }
		public string DayOfWeek { get; set; }
	}

	public class OutputObject
	{
		public decimal Discount { get; set; }
		public string Message { get; set; }
	}
}
