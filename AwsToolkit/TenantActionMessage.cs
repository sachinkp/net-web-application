using System;
using System.Collections.Generic;
using System.Text;

namespace AwsToolkit
{
    /// <summary>
    /// Class TenantActionMessage.
    /// </summary>
    public class TenantActionMessage
    {
        public string TenantName { get; set; }

        public Guid TenantId { get; set; }
    }
}
