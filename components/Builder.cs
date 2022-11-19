namespace builder.components
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using Microsoft.CSharp;

    public static class Builder
    {
        public static void Editor()
        {
            if (Directory.Exists("Stub")) // проверимяем папку где хранятся наши классы
            {
                string SourceCode = ""; // создём пустую переменную  SourceCode
                try
                {
                    foreach (string search in Directory.EnumerateFiles("Stub", "*.cs", SearchOption.TopDirectoryOnly)) // собираем файлы в цикле
                    {
                        SourceCode += File.ReadAllText(search); // обязательно += это позволяет соединять классы.
                    }




                    SourceCode = SourceCode.Replace("[NAME]", "Новое значение");



                    CompilerResults results = Compiler(new[] { SourceCode, }, $"builds/EncodeStealer.exe", $"/target:exe"); // Компилируем
                    if (results.Errors.Count == 0) // Проверка на ошибки
                    {
                        MessageBox.Show("Билд создан", "Builder Stub'a", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Если есть ошибки пройдёмся циклом и запишем их все.
                        foreach (CompilerError compilerError in results.Errors)
                        {
                            File.WriteAllText("Error_Compiler.txt", $"Ошибка: {compilerError.ToString()} \r\nСтрока: {compilerError.Line}\r\n");
                        }
                    }
                }
                catch { }
            }
            else
            {
                File.WriteAllText("DirError.txt", $"Ошибка: Папки classes не существует");
            }
        }

        private static CompilerResults Compiler(string[] sources, string output, string target, params string[] references)
        {
            var parameters = new CompilerParameters(references, output, false)
            {
                GenerateExecutable = true,
                CompilerOptions = target,
                WarningLevel = 3,
                TreatWarningsAsErrors = false
            };
            var providerOptions = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
            using (var provider = new CSharpCodeProvider(providerOptions))
            {
                return provider.CompileAssemblyFromSource(parameters, sources);
            }
        }
    }
}