using FactorioSaveDeflater.Services;
using FactorioSaveDeflater.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FactorioSaveDeflater.Deflater
{
    public class DeflaterViewModel : ObservableObject
    {
        private IFileSelectionService _fileSelectionService;

        private string sourceFilePath = "";
        public string SourceFilePath
        {
            get => sourceFilePath;
            set => SetAndNotify(ref sourceFilePath, value);
        }

        private string targetFilePath = "";
        public string TargetFilePath
        {
            get => targetFilePath;
            set => SetAndNotify(ref targetFilePath, value);
        }

        private string? error;
        public string? Error
        {
            get => error;
            set
            {
                SetAndNotify(ref error, value);
                Raise(nameof(ErrorVisibility));
            }
        }

        public Visibility ErrorVisibility => string.IsNullOrWhiteSpace(Error) ? Visibility.Hidden : Visibility.Visible;

        private Visibility isDeflating = Visibility.Hidden;
        public Visibility IsDeflatingVisibility
        {
            get => isDeflating;
            set => SetAndNotify(ref isDeflating, value);
        }

        public ICommand TargetFileCommand { get; }
        public ICommand SourceFileCommand { get; }
        public DelegateCommand DefalteCommand { get; }

        public DeflaterViewModel(IFileSelectionService fileSelectionService)
        {
            _fileSelectionService = fileSelectionService;

            DefalteCommand = new DelegateCommand(DeflateCommandExecute, DeflateCommandCanExecute);
            SourceFileCommand = new DelegateCommand(SourceFileCommandExecute);
            TargetFileCommand = new DelegateCommand(TargetFileCommandExecute);
        }

        private void SourceFileCommandExecute()
        {
            if (_fileSelectionService.OpenFile(out string? path))
            {
                SourceFilePath = path;

                if (string.IsNullOrWhiteSpace(TargetFilePath))
                {
                    string extension = Path.GetExtension(SourceFilePath);
                    TargetFilePath = SourceFilePath.Remove(SourceFilePath.Length - extension.Length) + "-deflated.zip";
                }
            }

            DefalteCommand.RaiseCanExecuteChanged();
        }

        private void TargetFileCommandExecute()
        {
            if (_fileSelectionService.SaveFile(out string? path))
            {
                TargetFilePath = path;
            }
        }

        private bool DeflateCommandCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SourceFilePath))
            {
                return false;
            }

            var file = new FileInfo(SourceFilePath);
            return file.Exists && file.Extension == ".zip";
        }

        private async void DeflateCommandExecute()
        {
            Error = null;
            IsDeflatingVisibility = Visibility.Visible;

            Error = await Task.Run(() =>
            {
                try
                {
                    if (!string.Equals(SourceFilePath, TargetFilePath, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(SourceFilePath, TargetFilePath, true);
                    }

                    var defalter = new SaveDeflater();
                    defalter.Deflate(TargetFilePath);

                    return null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            });

            IsDeflatingVisibility = Visibility.Hidden;
        }
    }
}
