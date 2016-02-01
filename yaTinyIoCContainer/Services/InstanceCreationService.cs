using System;

namespace yaTinyIoCContainer.Services
{
    internal class InstanceCreationService
    {
        static InstanceCreationService instance = null;

        static InstanceCreationService()
        {
            instance = new InstanceCreationService();
        }

        private InstanceCreationService()
        { }

        public static InstanceCreationService GetInstance()
        {
            return instance;
        }

        public object GetNewObject(Type t, object[] arguments = null)
        {
            object obj = null;

            try
            {
                obj = Activator.CreateInstance(t, arguments);
            }
            catch
            {
                // log it maybe
            }

            return obj;
        }
    }
}
