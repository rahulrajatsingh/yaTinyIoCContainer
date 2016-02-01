using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using yaTinyIoCContainer.Attributes;
using yaTinyIoCContainer.Core;
using yaTinyIoCContainer.Models;
using yaTinyIoCContainer.Services;

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

        private object Resolve(Type t)
        {
            object obj = null;

            if (instanceRegistry.ContainsKey(t) == true)
            {
                RegistrationModel model = instanceRegistry[t];

                if (model != null)
                {
                    Type typeToCreate = model.ObjectType;

                    ConstructorInfo[] consInfo = typeToCreate.GetConstructors();

                    var dependentCtor = consInfo.FirstOrDefault(item => item.CustomAttributes.FirstOrDefault(att => att.AttributeType == typeof(TinyDependencyAttribute)) != null);

                    if(dependentCtor == null)
                    {
                        // use the default constructor to create
                        obj = CreateInstance(model);
                    }
                    else
                    {
                        // We found a constructor with dependency attribute
                        ParameterInfo[] parameters = dependentCtor.GetParameters();
                        if (parameters.Count() == 0)
                        {
                            // Futile dependency attribute, use the default constructor only
                            obj = CreateInstance(model);
                        }
                        else
                        {
                            // valid dependency attribute, lets create the dependencies first and pass them in constructor
                            List<object> arguments = new List<object>();
                            foreach (var param in parameters)
                            {
                                Type type = param.ParameterType;
                                arguments.Add(this.Resolve(type));
                            }

                            obj = CreateInstance(model, arguments.ToArray());
                        }
                    }
                }
            }

            return obj;
        }

        private object CreateInstance(RegistrationModel model, object[] arguments = null)
        {
            object returnedObj = null;
            Type typeToCreate = model.ObjectType;

            if (model.RegType == REG_TYPE.INSTANCE)
            {
                returnedObj = InstanceCreationService.GetInstance().GetNewObject(typeToCreate, arguments);
            }
            else if (model.RegType == REG_TYPE.SINGLETON)
            {
                returnedObj = SingletonCreationService.GetInstance().GetSingleton(typeToCreate, arguments);
            }

            return returnedObj;
        }
    }
}
