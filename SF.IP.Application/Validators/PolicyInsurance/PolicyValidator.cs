using FluentValidation;
using FluentValidation.Results;
using SF.IP.Application.Models.InsurancePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Validators.PolicyInsurance
{
    public class PolicyValidator : AbstractValidator<InsurancePolicyDTO>
    {
        public PolicyValidator()
        {
            var utcNow = DateTime.UtcNow;

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Common.Constants.INVALID_FIRSTNAME)
                                        .MinimumLength(3).WithMessage(Common.Constants.INVALID_FIRSTNAME);
            
            RuleFor(x => x.LastName).NotEmpty().WithMessage(Common.Constants.INVALID_LASTNAME)
                               .MinimumLength(3).WithMessage(Common.Constants.INVALID_LASTNAME);

            //https://regex101.com/r/iW31BV/2/
            RuleFor(x => x.LicenseNumber).NotEmpty().Matches(Common.Constants.LICENSE_REGEX).WithErrorCode(Common.Constants.INVALID_USA_ADDRESS);

            RuleFor(x => x.EffectiveDate).Custom((effectiveDate, context) =>
            {  
                if (effectiveDate == DateTime.MinValue || (effectiveDate - utcNow).TotalDays < 30)
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.INVALID_EFFECTIVE_DATE));
                }
            });

            RuleFor(x => x.ExpirationDate).Custom((expirationDate, context) =>
            {
                if (expirationDate == DateTime.MinValue || (expirationDate - utcNow).TotalDays < 60)
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.INVALID_EXPIRATION_DATE));
                }
            });

            RuleFor(x => x.VehicleDetail.Year).Custom((registerationYear, context) =>
            {
                if (registerationYear >= 1998)
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.INVALID_VEHICLE_REG_YEAR));
                }
            });
        }

        private ValidationFailure GetValidationFailure(string errorCode)
        {
            var failure = new ValidationFailure("", "");
            failure.ErrorCode = errorCode;
            return failure;
        }
    }
}
