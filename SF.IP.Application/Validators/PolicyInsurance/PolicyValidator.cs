using FluentValidation;
using FluentValidation.Results;
using SF.IP.Application.Common;
using SF.IP.Application.Interfaces.Database;
using SF.IP.Application.Models.InsurancePolicy;
using System;
using System.Linq;


namespace SF.IP.Application.Validators.PolicyInsurance;

public class PolicyValidator : AbstractValidator<InsurancePolicyDTO>
{
    private readonly IApplicationDbContext _dbContext;
    public PolicyValidator(IApplicationDbContext dbContext)
    {
        var utcNow = DateTime.UtcNow;
        _dbContext = dbContext;

        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage(SFConstants.ErrorCodeMessages[SFConstants.INVALID_FIRSTNAME]).WithErrorCode(SFConstants.INVALID_FIRSTNAME);
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage(SFConstants.ErrorCodeMessages[SFConstants.INVALID_LASTNAME]).WithErrorCode(SFConstants.INVALID_LASTNAME);
        RuleFor(x => x.Address.Street).MinimumLength(5).WithMessage(SFConstants.ErrorCodeMessages[SFConstants.INVALID_ADDRESS_STREET]).WithErrorCode(SFConstants.INVALID_ADDRESS_STREET);
        RuleFor(x => x.Address.ZipCode).MinimumLength(3).WithMessage(SFConstants.ErrorCodeMessages[SFConstants.INVALID_US_ADDRESS]).WithErrorCode(SFConstants.INVALID_US_ADDRESS);
        RuleFor(x => x.Address.City).MinimumLength(3).WithMessage(SFConstants.ErrorCodeMessages[SFConstants.INVALID_US_ADDRESS]).WithErrorCode(SFConstants.INVALID_US_ADDRESS);
        RuleFor(x => x.Address.State).MinimumLength(2).WithMessage(SFConstants.ErrorCodeMessages[SFConstants.INVALID_US_ADDRESS]).WithErrorCode(SFConstants.INVALID_US_ADDRESS);
        //https://regex101.com/r/iW31BV/2/
        RuleFor(x => x.LicenseNumber).Matches(SFConstants.LICENSE_REGEX).WithErrorCode(SFConstants.ErrorCodeMessages[SFConstants.INVALID_LICENSE_NUMBER])
            .WithErrorCode(SFConstants.INVALID_LICENSE_NUMBER);

        RuleFor(x => x.Address).Custom((address, context) =>
        {
            var smallAddress = address with { City = address.City.ToLower(), State = address.State.ToLower(), ZipCode = address.ZipCode.ToLower() };
            var found = _dbContext.USZips.Any(z => z.City == smallAddress.City && z.ZipCode == smallAddress.ZipCode
            && (z.StateName == smallAddress.State || z.StateCode == smallAddress.State));
            if (!found)
            {
                context.AddFailure(GetValidationFailure(SFConstants.INVALID_US_ADDRESS));
            }
        });

        RuleFor(x => x.EffectiveDate).Custom((effectiveDate, context) =>
        {
            if (effectiveDate == DateTime.MinValue || (effectiveDate - utcNow).TotalDays < 30)
            {
                context.AddFailure(GetValidationFailure(SFConstants.INVALID_EFFECTIVE_DATE));
            }
        });

        RuleFor(x => x.ExpirationDate).Custom((expirationDate, context) =>
        {
            if (expirationDate == DateTime.MinValue || (expirationDate - utcNow).TotalDays < 60)
            {
                context.AddFailure(GetValidationFailure(SFConstants.INVALID_EXPIRATION_DATE));
            }
        });

        RuleFor(x => x.VehicleDetail.Year).Custom((registerationYear, context) =>
        {
            if (registerationYear >= 1998)
            {
                context.AddFailure(GetValidationFailure(SFConstants.INVALID_VEHICLE_REG_YEAR));
            }
        });
    }

    private ValidationFailure GetValidationFailure(string errorCode)
    {
        var failure = new ValidationFailure("", SFConstants.ErrorCodeMessages[errorCode]);
        failure.ErrorCode = errorCode;
        return failure;
    }
}

