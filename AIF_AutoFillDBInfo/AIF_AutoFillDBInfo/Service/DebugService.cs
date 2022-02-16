using System;
using System.Diagnostics;


namespace AIFAutoFillDB.Service
{
    public class TcDebug
    {
        public static void WriteLine(string line = null)
        {
#if DEBUG || RELEASEWITHMESSAGES
            var st = new System.Diagnostics.StackTrace();
            Trace.WriteLine("AutoFillFromDB::" + st.GetFrame(1).GetMethod().Name + "::" + line);
#endif
        }

        public static void Write(string line = null)
        {
#if DEBUG || RELEASEWITHMESSAGES
            var st = new System.Diagnostics.StackTrace();
            Trace.Write("AutoFillFromDB::" + st.GetFrame(1).GetMethod().Name + "::" + line);
#endif
        }

        public static void Start(string line = null)
        {
#if DEBUG || RELEASEWITHMESSAGES
            var st = new System.Diagnostics.StackTrace();
            Trace.WriteLine("AutoFillFromDB::" + st.GetFrame(1).GetMethod().Name + " => " + (line == null ? "()" : line));
#endif
        }

        public static void StartWithFullInfo(string line = null)
        {
#if DEBUG || RELEASEWITHMESSAGES
            var st = new System.Diagnostics.StackTrace();
            Trace.WriteLine("AutoFillFromDB::" + st.GetFrame(1).GetMethod().Name + " => " + (line == null ? "()" : line));

            StackFrame[] frames = st.GetFrames();
            foreach (StackFrame f in frames)
            {
                Trace.WriteLine("called by " + f.GetMethod().Name);
            }
#endif
        }

        public static void End(string line = null)
        {
#if DEBUG || RELEASEWITHMESSAGES
            var st = new System.Diagnostics.StackTrace();
            Trace.WriteLine("AutoFillFromDB::" + st.GetFrame(1).GetMethod().Name + " <=" + (line == null ? "()" : line));
#endif
        }

        public static void End(int ret)
        {
#if DEBUG || RELEASEWITHMESSAGES
            var st = new System.Diagnostics.StackTrace();
            Trace.WriteLine("AutoFillFromDB::" + st.GetFrame(1).GetMethod().Name + " <= (" + ret.ToString() + ")");
#endif
        }
    }
}