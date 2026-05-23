// CaesarCipherPlugin.cs
using System;
using System.Text;
using PluginInterface;

namespace CaesarCipherPlugin
{
    public class CaesarCipherProcessor : IDataProcessorPlugin
    {
        private int _shift = 3; // Default shift

        public string ProcessorId => "com.laba5.caesar.v1";
        public string ProcessorName => "Caesar Cipher";
        public string Description => "Simple shift cipher (Base64 safe)";

        public byte[] ProcessBeforeSave(byte[] data)
        {
            // Сначала в Base64 (чтобы не ломать JSON), потом шифруем
            string base64 = Convert.ToBase64String(data);
            char[] chars = base64.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsLetter(chars[i]))
                {
                    char offset = char.IsUpper(chars[i]) ? 'A' : 'a';
                    chars[i] = (char)(((chars[i] - offset + _shift) % 26) + offset);
                }
            }

            return Encoding.UTF8.GetBytes(new string(chars));
        }

        public byte[] ProcessAfterLoad(byte[] data)
        {
            // Сначала расшифровываем Caesar
            string text = Encoding.UTF8.GetString(data);
            char[] chars = text.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsLetter(chars[i]))
                {
                    char offset = char.IsUpper(chars[i]) ? 'A' : 'a';
                    chars[i] = (char)(((chars[i] - offset - _shift + 26) % 26) + offset);
                }
            }

            string base64 = new string(chars);

            try
            {
                return Convert.FromBase64String(base64);
            }
            catch
            {
                // Если что-то пошло не так — возвращаем как есть
                return data;
            }
        }

        public bool Configure()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter shift value (1-25):", "Caesar Cipher Settings", _shift.ToString());

            if (int.TryParse(input, out int newShift) && newShift >= 1 && newShift <= 25)
            {
                _shift = newShift;
                //MessageBox.Show($"Shift set to {_shift}", "Success");
                return true;
            }
            return false;
        }

        public IDataProcessorPlugin Clone()
        {
            return new CaesarCipherProcessor() { _shift = this._shift };
        }
    }

    public class CaesarPluginEntry : IShapePlugin
    {
        public string PluginId => "com.laba5.caesar.entry";
        public string PluginName => "Caesar Cipher Processor";
        public string Version => "1.0.0";

        public bool Initialize() => true;
        public IShapeFactoryPlugin GetShapeFactory() => null;
    }
}