using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Data_Library
{
    public class DBFactory
    {

        private static AchievementsEntities _context;

        public static AchievementsEntities CreateConnection()
        {
            if (_context == null)
            {
                _context = new ExtendedAchievementsEntities();
            }

            return _context;
        }

        public static void BeginWorkUnit()
        {
            _context = new ExtendedAchievementsEntities();
        }

        public static void EndWorkUnit()
        {
            _context.Dispose();
            _context = null;
        }
    }

    public partial class ExtendedAchievementsEntities : AchievementsEntities
    {
        private TextWriter logOutput;

        public ExtendedAchievementsEntities()
            : this("name=AchievementsEntities" )
        {
        }

        public ExtendedAchievementsEntities(string connectionString)
            : base(EntityConnectionWrapperUtils.CreateEntityConnectionWithWrappers(
                    connectionString,
                    "EFTracingProvider"
            ))
        {
        }

        #region Tracing Extensions

        private EFTracingConnection TracingConnection
        {
            get { return this.UnwrapConnection<EFTracingConnection>(); }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandExecuting
        {
            add { this.TracingConnection.CommandExecuting += value; }
            remove { this.TracingConnection.CommandExecuting -= value; }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandFinished
        {
            add { this.TracingConnection.CommandFinished += value; }
            remove { this.TracingConnection.CommandFinished -= value; }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandFailed
        {
            add { this.TracingConnection.CommandFailed += value; }
            remove { this.TracingConnection.CommandFailed -= value; }
        }

        private void AppendToLog(object sender, CommandExecutionEventArgs e)
        {
            if (this.logOutput != null)
            {
                this.logOutput.WriteLine(e.ToTraceString().TrimEnd());
                this.logOutput.WriteLine();
            }
        }

        public TextWriter Log
        {
            get { return this.logOutput; }
            set
            {
                if ((this.logOutput != null) != (value != null))
                {
                    if (value == null)
                    {
                        CommandExecuting -= AppendToLog;
                    }
                    else
                    {
                        CommandExecuting += AppendToLog;
                    }
                }

                this.logOutput = value;
            }
        }


        #endregion

      
    }
}
