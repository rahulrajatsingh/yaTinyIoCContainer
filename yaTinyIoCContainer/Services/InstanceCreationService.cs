using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using yaTinyIoCContainer.Models;

namespace yaTinyIoCContainer
{
    internal class InstanceCreationService
    {
        static InstanceCreationService instance = null;
        static Container m_Container = null;

        static InstanceCreationService()
        {
            instance = new InstanceCreationService();
        }

        private InstanceCreationService()
        { }

        public static InstanceCreationService GetInstance(Container container)
        {
            m_Container = container;
            return instance;
        }

        public object GetNewObject(Type t)
        {
            object obj = null;

            try
            {   
                ConstructorInfo[] consInfo = t.GetConstructors();

                foreach(var ctor in consInfo)
                {
                    ParameterInfo[] parameters = ctor.GetParameters();
                    if(parameters.Count() == 0)
                    {
                        obj = Activator.CreateInstance(t);
                        break;
                    }
                    else
                    {
                        List<object> arguments = new List<object>();
                        foreach(var param in parameters)
                        {
                            Type type = param.ParameterType;
                            arguments.Add(m_Container.Resolve(type));
                        }

                        obj = Activator.CreateInstance(t, arguments.ToArray());
                        break;
                    }
                }
            }
            catch
            {
                // log it maybe
            }

            return obj;
        }
    }
}
