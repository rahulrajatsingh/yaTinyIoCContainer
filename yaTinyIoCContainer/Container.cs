using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yaTinyIoCContainer.Core;
using yaTinyIoCContainer.Models;

namespace yaTinyIoCContainer
{
    public class Container : IContainer
    {
        Dictionary<Type, RegistrationModel> instanceRegistry = new Dictionary<Type, RegistrationModel>();
        
        public void RegisterInstanceType<I, C>()
            where I : class
            where C : class
        {
            RegisterType<I, C>(REG_TYPE.INSTANCE);
        }

        public void RegisterSingletonType<I, C>()
            where I : class
            where C : class
        {
            RegisterType<I, C>(REG_TYPE.SINGLETON);
        }

        private void RegisterType<I, C>(REG_TYPE type)
        {
            if (instanceRegistry.ContainsKey(typeof(I)) == true)
            {
                instanceRegistry.Remove(typeof(I));
            }

            instanceRegistry.Add(
                typeof(I),
                    new RegistrationModel
                    {
                        RegType = type,
                        ObjectType = typeof(C)
                    }
                );
        }

        public I Resolve<I>()
        {
            return (I)Resolve(typeof(I));            
        }

        internal object Resolve(Type t)
        {
            object obj = null;

            if (instanceRegistry.ContainsKey(t) == true)
            {
                RegistrationModel model = instanceRegistry[t];

                if (model != null)
                {
                    Type typeToCreate = model.ObjectType;
                    object returnedObj = null;

                    if (model.RegType == REG_TYPE.INSTANCE)
                    {
                        returnedObj = InstanceCreationService.GetInstance(this).GetNewObject(typeToCreate);
                    }
                    else if (model.RegType == REG_TYPE.SINGLETON)
                    {
                        returnedObj = SingletonCreationService.GetInstance(this).GetSingleton(typeToCreate);
                    }

                    obj = returnedObj;
                }
            }

            return obj;
        }
    }
}
