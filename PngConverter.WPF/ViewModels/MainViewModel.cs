using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF.Core.Commands;
using WPF.Core.ViewModels;

namespace PngConverter.WPF.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            SelectImageCommand = new DelegateCommandAsync(OnSelectImageCommandExecute, CanSelectImageCommandExecute);
            ConvertImageCommand = new DelegateCommandAsync(OnConvertImageCommandExecute, CanConvertImageCommandExecute);
            SaveConvertedImageCommand = new DelegateCommandAsync(OnSaveConvertedImageCommandExecute, CanSaveConvertedImageCommandExecute);
        }

        #region Properties

        private string _convertingImageTime;

        public string ConvertingImageTime
        {
            get => _convertingImageTime;
            set => Set(ref _convertingImageTime, value);
        }

        private string _loadingImageTime;

        public string LoadingImageTime
        {
            get => _loadingImageTime;
            set => Set(ref _loadingImageTime, value);
        }

        private int _convertingProgress;

        public int ConvertingProgress
        {
            get => _convertingProgress;
            set => Set(ref _convertingProgress, value);
        }

        private string _imageSize;

        public string SelectedImageSize
        {
            get => _imageSize;
            set => Set(ref _imageSize, value);
        }

        private BitmapSource _selectedImage;

        public BitmapSource SelectedImage
        {
            get => _selectedImage;
            set 
            {
                Set(ref _selectedImage, value);
            }
        }

        private BitmapSource _convertedImage;

        public BitmapSource ConvertedImage
        {
            get => _convertedImage;
            set => Set(ref _convertedImage, value);
        }

        private bool _isImageLoading;

        public bool IsImageLoading
        {
            get => _isImageLoading;
            set => Set(ref _isImageLoading, value);
        }

        private bool _isImageConverting;

        public bool IsImageConverting
        {
            get => _isImageConverting;
            set => Set(ref _isImageConverting, value);
        }
        #endregion Properties

        #region Load Image Command

        private PngBitmapDecoder _decoder;

        public ICommand SelectImageCommand { get; set; }

        private async Task OnSelectImageCommandExecute(object obj)
        {
            if (SelectedImage != null)
            {
                var result = MessageBox.Show("Текущий результат пропадет. Продолжить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result == MessageBoxResult.No)
                {
                    return;
                }

                SelectedImage = null;
                ConvertedImage = null;
                GC.Collect();
            }

            var fileDialog = new OpenFileDialog()
            {
                Filter = "Images | *.png",
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == true)
            {
                IsImageLoading = true;
                var timer = Stopwatch.StartNew();

                await Task.Run(() =>
                {
                    _decoder = CreateDecoderPNG(fileDialog.FileName);
                    SelectedImage = _decoder.Frames[0];
                }).ConfigureAwait(false);

                timer.Stop();
                IsImageLoading = false;
                LoadingImageTime = timer.Elapsed.ToString("ss\\:ffff");
                SelectedImageSize = GetFileSizeInMb(fileDialog.FileName);
            }
        }

        private PngBitmapDecoder CreateDecoderPNG(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
            {
                return new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
        }

        private bool CanSelectImageCommandExecute(object arg)
        {
            return !IsImageLoading && !IsImageConverting;
        }

        #endregion Load Image Command

        #region Convert Image Command

        public ICommand ConvertImageCommand { get; set; }

        private async Task OnConvertImageCommandExecute(object obj)
        {
            IsImageConverting = true;
            var timer = Stopwatch.StartNew();

            ConvertedImage = await App.Current.Dispatcher.InvokeAsync(() => ConvertImageToGrayscalePNG(SelectedImage));

            timer.Stop();
            IsImageConverting = false;
            ConvertingImageTime = timer.Elapsed.ToString("ss\\:ffff");
        }

        private BitmapSource ConvertImageToGrayscalePNG(BitmapSource source)
        {
            var formatConverted = new FormatConvertedBitmap();
            formatConverted.BeginInit();
            formatConverted.Source = source;
            formatConverted.DestinationFormat = PixelFormats.Gray32Float;
            formatConverted.EndInit();
            formatConverted.Freeze();
            return formatConverted;
        }

        private bool CanConvertImageCommandExecute(object arg)
        {
            return SelectedImage != null && !IsImageLoading && ConvertedImage == null;
        }

        #endregion Convert Image Command

        #region Save Image Command

        public ICommand SaveConvertedImageCommand { get; set; }

        private async Task OnSaveConvertedImageCommandExecute(object obj)
        {
            var saveDialog = new SaveFileDialog()
            {
                Filter = "Images | *.png"
            };

            if(saveDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    using (var stream = new FileStream(saveDialog.FileName, FileMode.Create))
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(ConvertedImage));
                        encoder.Save(stream);
                    }
                });
            }
        }

        private bool CanSaveConvertedImageCommandExecute(object arg)
        {
            return ConvertedImage != null && !IsImageLoading;
        }

        #endregion Save Image Command

        private string GetFileSizeInMb(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            return $"{fileInfo.Length / (1024 * 1024)} Мб";
        }
    }
}
