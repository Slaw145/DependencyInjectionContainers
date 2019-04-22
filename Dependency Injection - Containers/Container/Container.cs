using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection___Containers
{
    public class Container
    {
        readonly Dictionary<Type, Type> services = new Dictionary<Type, Type>();

        readonly Dictionary<Type, Func<object>> singletonServices = new Dictionary<Type, Func<object>>();

        public void Register<TRegister, TImplement>()
        {
            services.Add(typeof(TRegister), typeof(TImplement));
        }

        public void RegisterType<T>()
        {
            services.Add(typeof(T), typeof(T));
        }

        public void RegisterAsSingleton<T>(T obj)
        {
            singletonServices.Add(typeof(T), ()=>obj);
        }

        public object ResolveSingleton(Type type)
        {
            if (singletonServices.ContainsKey(type))
            {
                return singletonServices[type]();
            }

            return Activator.CreateInstance(type);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (singletonServices.ContainsKey(type))
            {
                return ResolveSingleton(type);
            }
            else
            {
                Type implementationType;
                bool implementationFound = services.TryGetValue(type, out implementationType);
                if (implementationFound == false)
                {
                    throw new TypeNotRegisteredException(type);
                }

                ConstructorInfo[] ctors = implementationType.GetConstructors();

                object[] ctorParams = ctors[0].GetParameters()
                    .Select(x => Resolve(x.ParameterType))
                    .ToArray();

                return Activator.CreateInstance(implementationType, ctorParams);
            } 
        }
    }

    public abstract class ResolveException : Exception
    {
        public readonly Type Type;

        public ResolveException(Type type, string message, Exception innerException = null)
            : base(message, innerException)
        {
            Type = type;
        }
    }

    public class TypeNotRegisteredException : ResolveException
    {
        public TypeNotRegisteredException(Type type)
            : base(type, string.Format("Type {0} not registered in the container", type.Name))
        {}
    }
}
