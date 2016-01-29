using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yaTinyIoCContainer.Models;

namespace yaTinyIoCContainer
{
    internal class SingletonCreationService
    {
        static SingletonCreationService instance = null;
        static Dictionary<string, object> objectPool = new Dictionary<string, object>();
        static Container m_Registry = null;

        static SingletonCreationService()
        {
            instance = new SingletonCreationService();
        }

        private SingletonCreationService()
        { }

        public static SingletonCreationService GetInstance(Container container)
        {
            m_Registry = container;
            return instance;
        }

        public object GetSingleton(Type t)
        {
            object obj = null;

            try
            {
                if (objectPool.ContainsKey(t.Name) == false)
                {
                    obj = InstanceCreationService.GetInstance(m_Registry).GetNewObject(t);
                    objectPool.Add(t.Name, obj);
                }
                else
                {
                    obj = objectPool[t.Name];
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
