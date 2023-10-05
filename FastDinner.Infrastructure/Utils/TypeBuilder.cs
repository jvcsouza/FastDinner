using System.Reflection;
using System.Reflection.Emit;

namespace FastDinner.Infrastructure.Utils
{
    public static class TypeBuilder
    {
        public static object NewObject(string typeName = "dynamicType",
            PropertyInfo[] propertyInfos = null, Type parent = null)
        {
            var myType = NewType(typeName, propertyInfos ?? new PropertyInfo[] { }, parent);
            var myObject = Activator.CreateInstance(myType);
            return myObject;
        }

        public static Type CopyWithParent<T, TP>()
        {
            var genericType = typeof(T);
            var properties = genericType.GetProperties();

            return NewType(genericType.Name, properties, typeof(TP));
        }

        public static Type NewType(string typeName = "dynamicType",
            PropertyInfo[] propertyInfos = null, Type parent = null)
        {
            var props = new List<PropertyInfo>(propertyInfos!).DistinctBy(x => x.Name);

            var tb = GetTypeBuilder(typeName, parent);
            var constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            foreach (var field in props)
                CreateProperty(tb, field.Name, field.PropertyType);

            var objectType = tb.CreateType();
            return objectType;
        }

        private static System.Reflection.Emit.TypeBuilder GetTypeBuilder(string typeName, Type parent)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assembly.GetName(), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.ManifestModule.Name);
            var tb = moduleBuilder.DefineType(typeName,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    parent);

            return tb;
        }

        private static void CreateProperty(System.Reflection.Emit.TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Public);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
