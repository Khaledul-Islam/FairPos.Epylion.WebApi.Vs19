﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Operations
{
	public class QualityControl
	{
		public decimal QcId { get; set; }
		public string QC_NO { get; set; }
		public string ARRIVAL_NO { get; set; }
		public string Chln { get; set; }
		public string SupID { get; set; }
		public string ShopID { get; set; }
		public string sBarCode { get; set; }
		public string BarCode { get; set; }
		public string BarcodeExp { get; set; }
		public decimal ArrivalBox { get; set; }
		public decimal ArrivalQty { get; set; }
		public decimal QCQty { get; set; }
		public decimal QCBoxQty { get; set; }
		public decimal DiscPrcnt { get; set; }
		public decimal VATPrcnt { get; set; }
		public decimal ACPU { get; set; }
		public decimal CPU { get; set; }
		public decimal RPU { get; set; }
		public DateTime BuyDT { get; set; }
		public DateTime EXPDT { get; set; }
		public string UserID { get; set; }
		public decimal GIFT_RATIO { get; set; }
		public string GIFT_DESCRIPTION { get; set; }
		public decimal PrdComPer { get; set; }
		public decimal PrdComAmnt { get; set; }
		public decimal AddCost { get; set; }
		public string ReferenceNo { get; set; }
		public decimal TotalPrdComm { get; set; }
		public string DeliveryChlnNo { get; set; }
		public string DebitNoteNo { get; set; }

	}
}
