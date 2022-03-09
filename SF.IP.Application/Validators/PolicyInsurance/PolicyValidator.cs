using FluentValidation;
using FluentValidation.Results;
using SF.IP.Application.Interfaces.Database;
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
        private readonly IApplicationDbContext _dbContext;
        public PolicyValidator(IApplicationDbContext dbContext)
        {
            var utcNow = DateTime.UtcNow;
            _dbContext = dbContext;

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Common.Constants.INVALID_FIRSTNAME)
                                        .MinimumLength(3).WithMessage(Common.Constants.INVALID_FIRSTNAME);
            
            RuleFor(x => x.LastName).NotEmpty().WithMessage(Common.Constants.INVALID_LASTNAME)
                               .MinimumLength(3).WithMessage(Common.Constants.INVALID_LASTNAME);

            RuleFor(x => x.Address.Street).NotEmpty().WithMessage(Common.Constants.INVALID_ADDRESS_STREET);
            RuleFor(x => x.Address.Street).MinimumLength(5).WithMessage(Common.Constants.INVALID_ADDRESS_STREET);
            RuleFor(x => x.Address.Street).Matches(Common.Constants.US_STREET_ADDRESS_REGEX).WithMessage(Common.Constants.INVALID_ADDRESS_STREET);

            RuleFor(x => x.Address.ZipCode).NotEmpty().WithMessage(Common.Constants.INVALID_US_ADDRESS);
            RuleFor(x => x.Address.City).NotEmpty().WithMessage(Common.Constants.INVALID_US_ADDRESS);
            RuleFor(x => x.Address.State).NotEmpty().WithMessage(Common.Constants.INVALID_US_ADDRESS);

            RuleFor(x => x.Address).Custom((address, context) =>
            {
                var smallAddress = address with { City = address.City.ToLower() , State = address.State.ToLower(), ZipCode = address.ZipCode.ToLower() };
                var found = _dbContext.USZips.Any(z=>z.City == smallAddress.City && z.ZipCode == smallAddress.ZipCode 
                && (z.StateName == smallAddress.State || z.StateCode == smallAddress.State));
                if (!found)
                {
                    context.AddFailure(GetValidationFailure(Common.Constants.INVALID_US_ADDRESS));
                }
            });

            //https://regex101.com/r/iW31BV/2/
            RuleFor(x => x.LicenseNumber).NotEmpty().WithErrorCode(Common.Constants.LICENSE_REGEX);
            RuleFor(x => x.LicenseNumber).Matches(Common.Constants.LICENSE_REGEX).WithErrorCode(Common.Constants.LICENSE_REGEX);

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
