using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Constants
{
    public static class AbsenceRuleConstants
    {
        public const int IncentiveThreshold = 2;

        public const int NoAbsences = 0;
        public const int FirstDeductionThreshold = 2;
        public const int SecondDeductionThreshold = 5;
        public const int ThirdDeductionThreshold = 10;
        public const int FourthDeductionThreshold = 15;

        public static readonly int[] AllowedThresholds =
        [
            NoAbsences,
            FirstDeductionThreshold,
            SecondDeductionThreshold,
            ThirdDeductionThreshold,
            FourthDeductionThreshold
        ];
    }
}
