using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using InterfaceOfSnapshotsWithAlgorithmsAndVisualizer;
using InterfaceOfCanvasWrappersWithVisualizer;

namespace DrawableWrapperRegistryImplementation
{
    public class DrawableWrapperRegistry
    {
        public bool AddFromAssembly(Assembly asm, Type interfaceType)
        {
            bool success = false;
            try
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (typeof(IDrawableOnCanvas).IsAssignableFrom(t))
                    {
                        foreach (ConstructorInfo ctorInfo in t.GetConstructors())
                        {
                            ParameterInfo[] parameters =
                                ctorInfo.GetParameters();
                            if (parameters.Count() == 2 &&
                                parameters[1].ParameterType.IsAssignableFrom(typeof(IVisualHints)))
                            {
                                Type type = parameters[0].ParameterType;
                                _registry[type] = ctorInfo;
                                success = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exception) { }
            return success;
        }

        //public void RegisterWrapper(Type T, Type TWrapper)
        //{
        //    _registry.Add(T, TWrapper);
        //}


        public bool RegisterWrapper<T>(Type WrapperClass)
        {
          bool success = false;
          if (WrapperClass is DrawableOnCanvas<T>)
          {
            _registry.Add(typeof(T), WrapperClass);
            success = true;
          }
          return success;
        }

        public IDrawableOnCanvas GetDrawableWrapper(
            object obj, IVisualHints visualHints)
        {
            IDrawableOnCanvas instance = null;
            Type TypeToDraw = obj.GetType();

            ConstructorInfo CtorInfo;
            if (_registry.TryGetValue(TypeToDraw, out CtorInfo))
            {
                instance = GetIntance(CtorInfo, obj, visualHints);
            }
            else
            {
                foreach (ConstructorInfo TargetTypeConstructor in GetTypesAssignableFrom(TypeToDraw))
                {
                    instance = GetIntance(TargetTypeConstructor, obj, visualHints);
                    if (instance != null)
                    {
                        _registry.Add(TypeToDraw, TargetTypeConstructor);
                        break;
                    }
                }   
            }
            return instance;
        }

        private IDrawableOnCanvas GetIntance(ConstructorInfo TargetTypeConstructor, object obj, IVisualHints visualHints)
        {
            IDrawableOnCanvas instance = null;
            object[] parameters = { obj, visualHints };
            try
            {
                instance = TargetTypeConstructor.Invoke(parameters) as IDrawableOnCanvas;
            }
            catch
            {
            }
            return instance;
        }

        private IEnumerable<ConstructorInfo> GetTypesAssignableFrom(Type TypeToDraw)
        {
            foreach (KeyValuePair<Type, ConstructorInfo> constructor in _registry)
            {
                if (constructor.Key.IsAssignableFrom(TypeToDraw))
                    yield return constructor.Value;
            }
        }

        private Dictionary<Type, ConstructorInfo> _registry = 
            new Dictionary<Type,ConstructorInfo>();
    }
}
