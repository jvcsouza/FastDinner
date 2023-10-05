using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FastDinner.Infrastructure.Persistence
{
    internal class CommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            Console.WriteLine(command.CommandText);
            return result;
        }

        public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        {
            Console.WriteLine(result.CommandText);
            return base.CommandCreated(eventData, result);
        }
    }
}
