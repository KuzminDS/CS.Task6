using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSU.CS.Task6.Encoder
{
    public class Encoder
    {
        private readonly List<char> _alphabet;

        public Encoder()
        {
            string alphabet = "ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮёйцукенгшщзхъфывапролджэячсмитьбю.,?!':;<>\\/ ";
            _alphabet = alphabet.ToList();
        }

        public void Encode(string directoryPath, string keyword)
        {
            var files = Directory.EnumerateFiles(directoryPath, "*.txt", SearchOption.AllDirectories);
            var key = keyword.ToList();
            var source = new StringBuilder();

            foreach (var file in files)
            {
                var fileInfo = GetFileInfo(file);
                source.Append(fileInfo);
            }

            var code = GetCode(source.ToString(), key);

            using (var sw = new StreamWriter("encoded.txt", append: false, Encoding.Unicode))
            {
                sw.Write(code.ToString());
            }

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, recursive: true);
            }
        }

        private string GetFileInfo(string file)
        {
            var text = File.ReadAllText(file);
            var source = new StringBuilder();
            source.Append($"--dir{file}{Environment.NewLine}");
            source.Append(text);

            return source.ToString();
        }

        private string GetCode(string source, IEnumerable<char> key)
        {
            var code = new StringBuilder();
            var keyEnumerator = key.GetEnumerator();
            for (int i = 0; i < source.Length; i++)
            {
                if (_alphabet.Contains(source[i]))
                {
                    var charIndex = _alphabet.IndexOf(source[i]);
                    if (!keyEnumerator.MoveNext())
                    {
                        keyEnumerator.Reset();
                        keyEnumerator.MoveNext();
                    }
                    var keyIndex = _alphabet.IndexOf(keyEnumerator.Current);
                    var newIndex = (charIndex + keyIndex) % _alphabet.Count;
                    code.Append(_alphabet[newIndex]);
                }
                else
                {
                    code.Append(source[i]);
                }
            }

            return code.ToString();
        }
    }
}
