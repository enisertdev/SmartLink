﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Domain.Entities;

namespace SmartLink.Application.Services
{
    public interface ILinkService
    {
        Task<string> GetAiSummary(string url);
        Task<string> CreateSummaryTitle(string url);
    }
}
