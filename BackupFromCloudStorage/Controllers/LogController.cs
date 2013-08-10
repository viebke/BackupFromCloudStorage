using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackupFromCloud.Controllers
{
    public class LogController
    {
        public delegate void AddLogEntryGoogle(string msg);
        public delegate void AddLogEntryDropbox(string msg);
        public static event AddLogEntryGoogle LogGoogle;
        public static event AddLogEntryDropbox LogDropbox;  

        public static void AddEntryGoogle(string msg)
        {
            if (LogGoogle != null)
            {
                LogGoogle(msg);
            }
        }

        public static void AddEntryDropbox(string msg)
        {
            if (LogDropbox != null)
            {
                LogDropbox(msg);
            }
        }
    }
}
