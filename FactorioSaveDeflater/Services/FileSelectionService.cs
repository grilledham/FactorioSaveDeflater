using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FactorioSaveDeflater.Services
{
    public interface IFileSelectionService
    {
        bool OpenFile([MaybeNullWhen(false)] out string path);
        bool SaveFile([MaybeNullWhen(false)] out string path);
    }

    public class FileSelectionService : IFileSelectionService
    {
        public const string Filter = "factorio save files|*.zip";

        public bool OpenFile(out string path)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = Filter;
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                return true;
            }

            path = null!;
            return false;
        }

        public bool SaveFile(out string path)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = Filter;
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                if (Path.GetExtension(path) != ".zip")
                {
                    path += ".zip";
                }

                return true;
            }

            path = null!;
            return false;
        }
    }
}
