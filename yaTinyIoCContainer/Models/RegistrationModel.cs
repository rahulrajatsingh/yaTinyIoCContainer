using System;

namespace yaTinyIoCContainer.Models
{
    internal enum REG_TYPE
    {
        INSTANCE,
        SINGLETON
    };

    internal class RegistrationModel
    {
        internal Type ObjectType { get; set; }
        internal REG_TYPE RegType { get; set; }
    }
}
