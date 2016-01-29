using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yaTinyIoCContainer.Core;

namespace iocTest
{
    interface ITest1
    {
        void Print();
    }

    class ClassTest1 : ITest1
    {
        public void Print()
        {
            Console.WriteLine("ClassName: {0}, HashCode: {1}", this.GetType().Name, this.GetHashCode());
        }
    }

    interface ITest2
    {
        void Print();
    }

    class ClassTest2 : ITest2
    {
        public void Print()
        {
            Console.WriteLine("ClassName: {0}, HashCode: {1}", this.GetType().Name, this.GetHashCode());
        }
    }


    interface One
    {
        void FunctionOne();
    }

    interface Two
    {
        void FunctionTwo();
    }

    class ClassOne : One
    {
        ITest1 m_Itest1 = null;
        public ClassOne(ITest1 test1)
        {
            m_Itest1 = test1;
        }

        public void FunctionOne()
        {
            Console.WriteLine("ClassName: {0}, HashCode: {1}", this.GetType().Name, this.GetHashCode());

            m_Itest1.Print();
        }
    }

    class ClassTwo : Two
    {
        One m_One = null;
        ITest1 m_Itest1 = null;
        
        public ClassTwo(ITest1 test1, One one)
        {
            m_Itest1 = test1;
            m_One = one;
        }

        public void FunctionTwo()
        {
            Console.WriteLine("ClassName: {0}, HashCode: {1}", this.GetType().Name, this.GetHashCode());

            m_Itest1.Print();
            m_One.FunctionOne();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = new yaTinyIoCContainer.Container();

            // testing instance type resigtration for class
            Console.WriteLine("testing instance type resigtration for class");
            container.RegisterInstanceType<ITest1, ClassTest1>();

            ITest1 obj1 = container.Resolve<ITest1>();
            ITest1 obj2 = container.Resolve<ITest1>();
            ITest1 obj3 = container.Resolve<ITest1>();
            ITest1 obj4 = container.Resolve<ITest1>();

            obj1.Print();
            obj2.Print();
            obj3.Print();
            obj4.Print();

            // testing singleton registration for class
            Console.WriteLine();
            Console.WriteLine("testing singleton registration for class");
            container.RegisterSingletonType<ITest2, ClassTest2>();

            ITest2 obj5 = container.Resolve<ITest2>();
            ITest2 obj6 = container.Resolve<ITest2>();
            ITest2 obj7 = container.Resolve<ITest2>();
            ITest2 obj8 = container.Resolve<ITest2>();

            obj5.Print();
            obj6.Print();
            obj7.Print();
            obj8.Print();

            // testing nested dependency for 2 levels
            Console.WriteLine();
            Console.WriteLine("testing nested dependency for 2 levels");
            container.RegisterInstanceType<One, ClassOne>();
            One obj9 = container.Resolve<One>();

            obj9.FunctionOne();

            // testing nested dependency for 2 levels with 2 arguments
            Console.WriteLine();
            Console.WriteLine("testing nested dependency for 2 levels with 2 arguments");
            container.RegisterInstanceType<Two, ClassTwo>();
            Two obj10 = container.Resolve<Two>();

            obj10.FunctionTwo();

            Console.ReadLine();
        }
    }
}
