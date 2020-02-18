using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace elalAPI.Managers
{
    public interface ILogger {

        void LogError(string txt);
        void LogInfo(string txt);
        void LogStart();
        void LogFinish();
    }
    public class Logger : ILogger
    {
        public void LogError(string txt)
        {
            throw new NotImplementedException();
        }

        public void LogFinish()
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string txt)
        {
            throw new NotImplementedException();
        }

        public void LogStart()
        {
            
            StackFrame _stack = new StackFrame(1, true);
            string funcName = _stack.GetMethod().Name;
            string className =_stack.GetMethod().DeclaringType.Name;
            
        }
        public void LogStart(object obj)
        {
            StackFrame _stack = new StackFrame(1, true);
            string funcName = _stack.GetMethod().Name;
            string className = _stack.GetMethod().DeclaringType.Name;
            string parameter = obj.ToString();
        }
    }
}