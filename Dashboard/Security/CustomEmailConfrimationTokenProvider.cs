using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Security
{
    public class CustomEmailConfrimationTokenProvider<TUser> 
        : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomEmailConfrimationTokenProvider(IDataProtectionProvider dataProtectionProvider, 
            IOptions<CustomEmailConfrimationTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {

        }
    }
}
