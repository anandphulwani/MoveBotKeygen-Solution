using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MoveBotKey : Form
    {
        public string CODE_BIT;
        public byte[] Car_Amm;
        public static byte[] CODELED = new byte[11];

        public MoveBotKey()
        {
            this.CODE_BIT = "00000000000";
            this.Car_Amm = new byte[0x40];
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int index = 0;
            while (true)
            {
                this.Car_Amm[index] = (byte)(0x41 + index);
                index++;
                if (index > 0x19)
                {
                    int num2 = 0x1a;
                    while (true)
                    {
                        this.Car_Amm[num2] = (byte)((0x30 + num2) - 0x1a);
                        num2++;
                        if (num2 > 30)
                        {
                            int num3 = 0x1f;
                            while (true)
                            {
                                this.Car_Amm[num3] = (byte)((0x61 + num3) - 0x1f);
                                num3++;
                                if (num3 > 0x38)
                                {
                                    int num4 = 0x39;
                                    while (true)
                                    {
                                        this.Car_Amm[num4] = (byte)((0x35 + num4) - 0x39);
                                        num4++;
                                        if (num4 > 0x3d)
                                        {
                                            this.Car_Amm[0x3e] = 0x2b;
                                            this.Car_Amm[0x3f] = 0x2d;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string ATTIVA_LED(byte indice = 1)
        {
            CODELED = new byte[11];
            this.CODE_BIT = Strings.Format(Conversion.Val(IntToBin((int)Oct2Dec("3777"))), "00000000000");
            int index = 0;
            do
            {
                CODELED[index] = (byte)Conversion.Val(this.CODE_BIT.ToCharArray()[index]);
                index++;
            }
            while (index <= 10);    
            return this.CODE_BIT;
        }
        
        public string CODEX_CONTROL()
        {
            this.ATTIVA_LED(1);
            char[] chArray = this.CODEX_CONVERSION_UIC(textBoxUIC.Text).ToCharArray();
            char[] chArray2 = this.CODEX_CONVERSION_EMAIL(textBoxEmail.Text).ToCharArray();
            char[] chArray3 = this.CODEX_CONVERSION_ACTIVATION(Strings.Left(this.CODE_BIT, 11)).ToCharArray();
            string str = "";
            int index = 7;
            while (true)
            {
                char ch = chArray[index];
                char ch2 = chArray2[7 - index];
                char ch3 = chArray3[index];
                str = str + Strings.Format(((1 + Strings.Asc(ch)) * (1 + Strings.Asc(ch2))) * (1 + Strings.Asc(ch3)), "000000");
                index += -1;
                if (index < 0)
                {
                    if ((str.Length % 2) != 0)
                    {
                        str = str + "0";
                    }
                    string str2 = "";
                    int length = str.Length;
                    for (int i = 1; i <= length; i += 2)
                    {
                        string inputStr = Strings.Mid(str, i, 2);
                        str2 = (Conversion.Val(inputStr) > 0x3f) ? (str2 + inputStr) : (str2 + Conversions.ToString(Strings.Chr(this.Car_Amm[Conversions.ToInteger(inputStr)])));
                    }
                    string str3 = "";
                    int num4 = 0;
                    while (true)
                    {
                        str3 = str3 + CODELED[num4].ToString();
                        num4++;
                        if (num4 > 10)
                        {
                            str2 = str2 + ":" + Strings.Format(Conversion.Val(Dec2Oct((long)_BIT_to_BYTE(str3, 11))), "0000");
                            double num5 = Conversion.Val(Dec2Oct((long)_BIT_to_BYTE(str3, 11)));
                            // this.Label7.Text = (num5 != 0x179) ? ((num5 != 0x309) ? ((num5 != 0x561) ? ((num5 != 0x949) ? "max 1" : "max 10") : "max 5") : "max 1") : "max 1";
                            return str2;
                        }
                    }
                }
            }
        }

        public string CODEX_CONVERSION_ACTIVATION(string ACTIVATION)
        {
            long expression = 0L;
            int num2 = 0;
            foreach (char ch in ACTIVATION.ToCharArray())
            {
                num2 += 0x400;
                expression += Strings.Asc(ch) * num2;
            }
            return Strings.Format(expression, "00000000");
        }

        public string CODEX_CONVERSION_EMAIL(string EMAIL)
        {
            if (EMAIL.Length > 0x20)
            {
                EMAIL = EMAIL.Substring(1, 0x20);
            }
            long expression = 0L;
            int num2 = 0;
            foreach (char ch in EMAIL.ToCharArray())
            {
                num2 += 0x100;
                expression += Strings.Asc(ch) * num2;
            }
            return Strings.Format(expression, "00000000");
        }

        public string CODEX_CONVERSION_UIC(string UIC)
        {
            long expression = 0L;
            UIC = UIC.Replace("-", "");
            int num2 = 0;
            foreach (char ch in UIC.ToCharArray())
            {
                num2 += 0x100;
                expression += Strings.Asc(ch) * num2;
            }
            return Strings.Format(expression, "00000000");
        }

        public static ulong _BIT_to_BYTE(string _BitSeries, byte Num_Bit)
        {
            ulong num1 = 0;
            if (_BitSeries.Length > (int)Num_Bit)
                _BitSeries = Microsoft.VisualBasic.Strings.Left(_BitSeries, (int)Num_Bit);
            int num2 = (int)Num_Bit - 1;
            for (int y = 0; y <= num2; ++y)
            {
                if ((byte)Math.Round(Conversion.Val(Microsoft.VisualBasic.Strings.Mid(_BitSeries, y + 1, 1))) != (byte)0)
                    num1 = (ulong)Math.Round((double)num1 + Math.Pow(2.0, (double)y));
            }
            return num1;
        }

        public static string Dec2Oct(long Decimal_Number)
        {
            StackTrace stackTrace = new StackTrace();
            string Name_Function = "{" + Microsoft.VisualBasic.Strings.Replace(stackTrace.GetFrame(0).GetMethod().ReflectedType.ToString(), ".", "->") + "->" + stackTrace.GetFrame(0).GetMethod().Name.ToString() + "}";
            string str = "";
            try
            {
                Decimal_Number = Math.Abs(Decimal_Number);
                do
                {
                    str = Conversions.ToString(Decimal_Number % 8L) + str;
                    Decimal_Number /= 8L;
                    Application.DoEvents();
                }
                while (Decimal_Number > 0L);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            return str;
        }

        public static long Oct2Dec(string Octal_Number)
        {
            StackTrace stackTrace = new StackTrace();
            string Name_Function = "{" + Microsoft.VisualBasic.Strings.Replace(stackTrace.GetFrame(0).GetMethod().ReflectedType.ToString(), ".", "->") + "->" + stackTrace.GetFrame(0).GetMethod().Name.ToString() + "}";
            long num = 0;
            try
            {
                for (int Start = Microsoft.VisualBasic.Strings.Len(Octal_Number); Start >= 1; Start += -1)
                    num = (long)Math.Round((double)Conversions.ToInteger(Microsoft.VisualBasic.Strings.Mid(Octal_Number, Start, 1)) * Math.Pow(8.0, (double)(Octal_Number.Length - Start)) + (double)num);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            return num;
        }

        public static string IntToBin(int filevalue)
        {
            BitArray bitArray = new BitArray(new int[1]
            {
        filevalue
            });
            string str = "";
            int num = bitArray.Count - 1;
            for (int index = 0; index <= num; ++index)
                str = bitArray[index] ? str + "1" : str + "0";
            return Microsoft.VisualBasic.Strings.Left(str, 11);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxActivationCode.Text = this.CODEX_CONTROL();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxUIC.Text = "";
            textBoxEmail.Text = "";
            textBoxActivationCode.Text = "";
        }
    }
}
