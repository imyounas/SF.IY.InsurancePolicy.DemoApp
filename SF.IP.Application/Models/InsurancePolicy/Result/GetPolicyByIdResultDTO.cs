﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Models.InsurancePolicy.Result
{
    public class GetPolicyByIdResultDTO : BaseResultDTO
    {
        public InsurancePolicyDTO InsurancePolicy { get; set; }
    }
}