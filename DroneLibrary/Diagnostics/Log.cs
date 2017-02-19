﻿using System;
using System.Text;

namespace DroneLibrary.Diagnostics
{
    /// <summary>
    /// Stellt verschiedene Funktionen für den Log bereit.
    /// </summary>
    public static class Log
    {
        private static object locker = new object();
        private static StringBuilder buffer = new StringBuilder();

        public static LogStorage Storage { get; set; } = new LogStorage();

        public static bool WriteToConsole { get; set; }
        public static bool WriteToDebug { get; set; }

        private static bool allowBuffer = true;
        /// <summary>
        /// Erlaubt ob die Log-Klasse Einträge buffern darf.
        /// </summary>
        public static bool AllowBuffer
        {
            get { return allowBuffer; }
            set
            {
                lock (locker)
                {
                    if (allowBuffer != value)
                    {
                        allowBuffer = value;
                        if (!allowBuffer)
                            InternalFlushBuffer();
                    }
                }
            }
        }

        /// <summary>
        /// Erlaubt ob der Buffer automatisch geleert wird.
        /// </summary>
        public static bool AutomaticFlush { get; set; }

        private static int bufferCapacity = 5000;
        /// <summary>
        /// Gibt an wie viele Zeichen gebuffert werden sollen bis sie zur Konsole oder Debug-Output geschrieben werden soll.
        /// </summary>
        public static int BufferCapacity
        {
            get { return bufferCapacity; }
            set
            {
                lock (locker)
                {
                    if (value != bufferCapacity)
                    {
                        bufferCapacity = value;
                        if (buffer.Length >= bufferCapacity)
                            InternalFlushBuffer();
                    }
                }
            }
        }

        /// <summary>
        /// Das Level ab dem Nachrichten im Log angezeigt werden.
        /// Standard: LogLevel.Verbose
        /// </summary>
        public static LogLevel LevelMinimum { get; set; } = LogLevel.Verbose;

        public static event EventHandler<LogFlushEventArgs> OnFlushBuffer;

        /// <summary>
        /// Sorgt dafür das der Buffer in die Konsole oder Debug-Output geschrieben wird.
        /// </summary>
        public static void FlushBuffer()
        {
            InternalFlushBuffer(false);
        }

        private static void InternalFlushBuffer()
        {
            InternalFlushBuffer(true);
        }

        private static void InternalFlushBuffer(bool isAutomatic)
        {
            if (!AutomaticFlush && isAutomatic)
                return;

            lock (locker)
            {
                string bufferString = buffer.ToString();
                if (WriteToConsole)
                    Console.Write(bufferString);
                if (WriteToDebug)
                    System.Diagnostics.Debug.Write(bufferString);

                OnFlushBuffer?.Invoke(null, new LogFlushEventArgs(bufferString));
                buffer.Clear();
            }
        }

        /// <summary>
        /// Schreibt eine Nachricht in den Log.
        /// </summary>
        /// <param name="level">Das Level für die Nachricht</param>
        /// <param name="message">Die Nachricht</param>
        public static void Write(LogLevel level, string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (level >= LevelMinimum)// alle Einträge die unter dem Minimum fallen ignoreren
            {
                lock (locker)
                {
                    string[] messageLines = message.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in messageLines)
                    {
                        string formattedMessage = string.Format("[{0}] {1} {2}", DateTime.Now.ToLongTimeString(), ("(" + level.ToString() + ")").PadRight(10), str);
                        buffer.Append(formattedMessage);
                        buffer.AppendLine();

                        Storage.AddLine(formattedMessage);
                    }

                    if (!allowBuffer || buffer.Length >= bufferCapacity)
                        InternalFlushBuffer();
                }
            }
        }

        /// <summary>
        /// Schreibt eine formattierte Nachricht in den Log.
        /// </summary>
        /// <param name="level">Das Level für die Nachricht</param>
        /// <param name="format">Das Format der Nachricht</param>
        /// <param name="args">Die Argumente für das Format</param>
        public static void Write(LogLevel level, string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Write(level, string.Format(format, args));
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Verbose, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Verbose(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Write(LogLevel.Verbose, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Debug, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Debug(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Write(LogLevel.Debug, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Info, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Info(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Write(LogLevel.Info, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Warning, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Warning(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Write(LogLevel.Warning, message);
        }

        /// <summary>
        /// Schreibt eine Exception in den Log.
        /// </summary>
        /// <param name="exception">Die Exception welche geschrieben werden soll.</param>
        public static void Error(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (exception.Message == null)
                Error("Exception {0}", exception.GetType().Name);
            else
                Error(exception.Message);

            if (exception.StackTrace == null)
                Error("No stacktrace");
            else
                Error(exception.StackTrace);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Error, message) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Error(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            Write(LogLevel.Error, message);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Verbose, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Verbose(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Write(LogLevel.Verbose, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Debug, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Debug(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Write(LogLevel.Debug, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Info, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Info(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Write(LogLevel.Info, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Warning, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Warning(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Write(LogLevel.Warning, format, args);
        }

        /// <summary>
        /// Ruft Log.Write(LogLevel.Error, format, args) auf.
        /// </summary>
        /// <param name="message">Die Nachricht</param>
        public static void Error(string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            Write(LogLevel.Error, format, args);
        }


        /// <summary>
        /// Schreibt alle Felder des Objekts obj in den Log.
        /// </summary>
        /// <param name="level">Das LogLevel welches benutzt werden soll.</param>
        /// <param name="obj">Das Objekt mit den Feldern die in den Log geschrieben werden sollen.</param>
        public static void WriteFields(LogLevel level, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var fields = obj.GetType().GetFields();
            Log.Write(level, "All fields for {0} [n: {1}]:", obj.GetType().FullName, fields.Length);
            foreach (var field in fields)
                Log.Write(level, "...{0} = {1}", field.Name, field.GetValue(obj));
        }

        /// <summary>
        /// Schreibt alle Eigenschaft des Objekts obj in den Log.
        /// </summary>
        /// <param name="level">Das LogLevel welches benutzt werden soll.</param>
        /// <param name="obj">Das Objekt mit den Eigenschaften die in den Log geschrieben werden sollen.</param>
        public static void WriteProperties(LogLevel level, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var properties = obj.GetType().GetProperties();
            Log.Write(level, "All properties for {0} [n: {1}]:", obj.GetType().FullName, properties.Length);
            foreach (var property in properties)
                Log.Write(level, "...{0} = {1}", property.Name, property.GetValue(obj));
        }

        /// <summary>
        /// Schreibt Systeminformationen in den Log.
        /// </summary>
        public static void LogSystemInfo()
        {
            Log.Debug("System Information:");
            Log.Debug("OS: {0}", Environment.OSVersion);
            Log.Debug("64bit: os ({0}) process ({1})", Environment.Is64BitOperatingSystem, Environment.Is64BitProcess);
            Log.Debug("CLR: {0}", Environment.Version);
            if (BitConverter.IsLittleEndian)
                Log.Debug("System is little endian");
            else
                Log.Debug("System is big endian");
            Log.Debug("=================");
        }
    }
}