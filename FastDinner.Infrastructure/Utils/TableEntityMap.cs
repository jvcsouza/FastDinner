using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastDinner.Infrastructure.Utils
{
    public class TableEntityMap<T> where T : class, new()
    {
        // 1) Lazy<> garante que compilamos o delegate só 1 vez, em thread-safe
        private static readonly Lazy<Func<TableEntity, T>> MapFunc = new(CreateMapFunc);

        // Exposição do mapper: TableEntityMapper<T>.Map(entity)
        public static T Map(TableEntity entity) => MapFunc.Value(entity);

        private static Func<TableEntity, T> CreateMapFunc()
        {
            var entityParam = Expression.Parameter(typeof(TableEntity), "entity");

            // 2) Converte TableEntity em IDictionary<string,object>
            var dictInterface = typeof(IDictionary<string, object>);
            var asDict = Expression.Convert(entityParam, dictInterface);

            // 3) Cria: new Dictionary<string,object>(entity, StringComparer.OrdinalIgnoreCase)
            var dictCtor = typeof(Dictionary<string, object>)
                .GetConstructor(new[] { dictInterface, typeof(IEqualityComparer<string>) })!;

            var newDict = Expression.New(
                dictCtor,
                asDict,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));

            var dictVar = Expression.Variable(typeof(Dictionary<string, object>), "dict");

            // 4) Cria instância de T: var result = new T();
            var resultVar = Expression.Variable(typeof(T), "result");
            var expressions = new List<Expression>
            {
                Expression.Assign(dictVar, newDict),
                Expression.Assign(resultVar, Expression.New(typeof(T)))
            };

            // 5) Para cada propriedade pública e gravável de T,
            //    gera:
            //      if (dict.TryGetValue("PropName", out var val))
            //          result.PropName = (PropType) Convert.ChangeType(val, PropType);
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite);

            foreach (var prop in props)
            {
                var keyConst = Expression.Constant(prop.Name);
                var valueVar = Expression.Variable(typeof(object), "val");

                // dict.TryGetValue("PropName", out val)
                var tryGet = Expression.Call(
                    dictVar,
                    dictInterface.GetMethod("TryGetValue")!,
                    keyConst,
                    valueVar);

                // Convert.ChangeType(val, PropType)
                var converted = Expression.Convert(
                    Expression.Call(
                        typeof(Convert),
                        nameof(Convert.ChangeType),
                        Type.EmptyTypes,
                        valueVar,
                        Expression.Constant(prop.PropertyType)),
                    prop.PropertyType);

                // result.PropName = (PropType)converted
                var setProp = Expression.Call(
                    resultVar,
                    prop.GetSetMethod()!,
                    converted);

                expressions.Add(
                    Expression.Block(
                        new[] { valueVar },
                        Expression.IfThen(tryGet, setProp)
                    )
                );
            }

            // 6) retorna o objeto
            expressions.Add(resultVar);

            // Monta o bloco e compila
            var body = Expression.Block(new[] { dictVar, resultVar }, expressions);
            var lambda = Expression.Lambda<Func<TableEntity, T>>(body, entityParam);
            return lambda.Compile();  // gera o Func<TableEntity,T>
        }
    }
}
