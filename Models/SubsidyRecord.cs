using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Models
{
    public class SubsidyRecord
    {
        public string? FISCAL_YEAR { get; set; }
        public string? QUARTER { get; set; }
        public string? SOL_ID { get; set; }
        public string? LOAN_TYPE { get; set; }
        public string? CLIENT_CODE { get; set; }
        public string? LOAN_ACCOUNT { get; set; }
        public string? NAME_OF_BORROWER { get; set; }
        public decimal? SUBDIY_INTEREST_AMOUNT { get; set; }
        public string? SUBSIDY_CATEGORY { get; set; }
        public string? REMARKS { get; set; }
        public string? STATUS { get; set; }

    }
}
