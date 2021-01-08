using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;

namespace Galatee.Sms.Tools
{
    public class ModemSettings
    {
        public class Settings
        {
            /// <summary> Port settings. </summary>
            public class Port
            {
                public static string PortName = "COM1";
                public static int BaudRate = 115200;
                public static int DataBits = 8;
                public static int ReadTimeout = 300;
                public static int WriteTimeout = 300;

                public static System.IO.Ports.Parity Parity = System.IO.Ports.Parity.None;
                public static System.IO.Ports.StopBits StopBits = System.IO.Ports.StopBits.One;
                public static System.IO.Ports.Handshake Handshake = System.IO.Ports.Handshake.None;
            }

            /// <summary> Option settings. </summary>
            public class Option
            {
                //D�finir ici les param�tres de communication avec le port
                public static String mServeurSQL = "";
                public static String mBaseDonnees = "";
                public static String mWebServiceUrl = "";
                public static int iNbreTraitementSms = 0;
            }

            /// <summary>
            ///   Read the settings from disk. </summary>
            public static void Read()
            {
                String sAppliPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                IniFile ini = new IniFile(sAppliPath + @"\ConfigigurationModem.ini");
                //IniFile ini = new IniFile(@"C:\ConfigModem.ini");
                Port.PortName = ini.ReadValue("Port", "PortName", Port.PortName);
                Port.BaudRate = ini.ReadValue("Port", "BaudRate", Port.BaudRate);
                Port.DataBits = ini.ReadValue("Port", "DataBits", Port.DataBits);
                Port.Parity = (Parity)Enum.Parse(typeof(Parity), ini.ReadValue("Port", "Parity", Port.Parity.ToString()), true);
                Port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), ini.ReadValue("Port", "StopBits", Port.StopBits.ToString()), true);
                Port.Handshake = (Handshake)Enum.Parse(typeof(Handshake), ini.ReadValue("Port", "Handshake", Port.Handshake.ToString()), true);

                Port.ReadTimeout = int.Parse(ini.ReadValue("Port", "ReadTimeout", Port.ReadTimeout.ToString()));
                Port.WriteTimeout = int.Parse(ini.ReadValue("Port", "WriteTimeout", Port.WriteTimeout.ToString()));

                Option.mServeurSQL = ini.ReadValue("Option", "ServeurSQL", Option.mServeurSQL);
                Option.mBaseDonnees = ini.ReadValue("Option", "BaseDonnees", Option.mBaseDonnees);
                Option.mWebServiceUrl = ini.ReadValue("Option", "mWebServiceUrl", Option.mWebServiceUrl);
                Option.iNbreTraitementSms = (byte)ini.ReadValue("Option", "iNbreTraitementSms", Option.iNbreTraitementSms);

            }

            /// <summary>
            ///   Write the settings to disk. </summary>
            public static void Write()
            {
                String sAppliPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //IniFile ini = new IniFile(sAppliPath + @"\ConfigPort.ini");
                IniFile ini = new IniFile(@"C:\ConfigigurationModem.ini");
                ini.WriteValue("Port", "PortName", Port.PortName);
                ini.WriteValue("Port", "BaudRate", Port.BaudRate);
                ini.WriteValue("Port", "DataBits", Port.DataBits);
                ini.WriteValue("Port", "Parity", Port.Parity.ToString());
                ini.WriteValue("Port", "StopBits", Port.StopBits.ToString());
                ini.WriteValue("Port", "Handshake", Port.Handshake.ToString());
                ini.WriteValue("Port", "ReadTimeout", Port.ReadTimeout.ToString());
                ini.WriteValue("Port", "WriteTimeout", Port.WriteTimeout.ToString());

                ini.WriteValue("Option", "ServeurSQL", Option.mServeurSQL);
                ini.WriteValue("Option", "BaseDonnees", Option.mBaseDonnees);
                ini.WriteValue("Option", "mWebServiceUrl", Option.mWebServiceUrl);

            }

            public static void IniSetting()
            {
                Port.PortName = "COM3";
                Port.BaudRate = 300;
                Port.DataBits = 7;
                Port.Parity = Parity.Mark;
                Port.StopBits = StopBits.One;
                Port.Handshake = Handshake.None;

                Option.mServeurSQL ="";
                Option.mBaseDonnees = "";
                Option.mWebServiceUrl = "";

            }

        }

        public class IniFile
        {
            public string path;

            [DllImport("kernel32.dll")]
            private static extern long WritePrivateProfileString(string section,
                string key, string val, string filePath);

            [DllImport("kernel32.dll")]
            private static extern int GetPrivateProfileString(string section,
                string key, string def, StringBuilder retVal, int size, string filePath);

            public IniFile(string INIPath)
            {
                path = INIPath;
            }

            public void WriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }

            public string ReadValue(string Section, string Key, string Default)
            {
                StringBuilder buffer = new StringBuilder(255);
                GetPrivateProfileString(Section, Key, Default, buffer, 255, this.path);

                return buffer.ToString();
            }

            public void WriteValue(string Section, string Key, int Value)
            {
                WritePrivateProfileString(Section, Key, Value.ToString(), this.path);
            }

            public int ReadValue(string Section, string Key, int Default)
            {
                StringBuilder buffer = new StringBuilder(255);
                GetPrivateProfileString(Section, Key, Default.ToString(), buffer, 255, this.path);

                return int.Parse(buffer.ToString());
            }
        }
    }
}
