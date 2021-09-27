using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSU.CS.Task6.Decoder
{
    public class Decoder
    {
        private readonly List<char> _alphabet;

        public Decoder()
        {
            string alphabet = "ЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮёйцукенгшщзхъфывапролджэячсмитьбю.,?!':;<>\\/ ";
            _alphabet = alphabet.ToList();
        }

        public void Decode(string encodedFile, string keyword)
        {
            var encodedText = File.ReadAllText(encodedFile);
            ICollection<char> key = keyword.ToList();

            var resultText = GetText(encodedText, key);

            var fileInfos = resultText.ToString().Split(new string[] { "--dir" }, StringSplitOptions.RemoveEmptyEntries);
            SaveFiles(fileInfos);

            File.Delete(encodedFile);
        }

        private string GetText(string encodedText, IEnumerable<char> key)
        {
            var keyEnumerator = key.GetEnumerator();
            var resultText = new StringBuilder();

            for (int i = 0; i < encodedText.Length; i++)
            {
                if (_alphabet.Contains(encodedText[i]))
                {
                    var charIndex = _alphabet.IndexOf(encodedText[i]);
                    if (!keyEnumerator.MoveNext())
                    {
                        keyEnumerator.Reset();
                        keyEnumerator.MoveNext();
                    }
                    var keyIndex = _alphabet.IndexOf(keyEnumerator.Current);
                    var newIndex = charIndex - keyIndex;
                    newIndex += newIndex < 0 ? _alphabet.Count : 0;
                    newIndex %= _alphabet.Count;
                    resultText.Append(_alphabet[newIndex]);
                }
                else
                {
                    resultText.Append(encodedText[i]);
                }
            }

            return resultText.ToString();
        }

        private void SaveFiles(params string[] fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                var newLineIndex = fileInfo.IndexOf(Environment.NewLine);
                var path = fileInfo.Substring(0, newLineIndex);
                var text = fileInfo.Substring(newLineIndex + 2);
                var file = new FileInfo(path);
                file.Directory.Create();
                File.WriteAllText(file.FullName, text);
            }
        }
    }
}
