using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yaTinyIoCContainer.Core
{
    public interface IContainer
    {
        void RegisterInstanceType<I, C>()
            where I : class
            where C : class;

        void RegisterSingletonType<I, C>()
            where I : class
            where C : class;

        T Resolve<T>();
    }
}
