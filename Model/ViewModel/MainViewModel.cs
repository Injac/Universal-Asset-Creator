using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace Model.ViewModel
{
    public delegate void ConversionStarted(object sender, EventArgs e);

    public delegate void ConversionEnded(object sender, EventArgs e);

    public class MainViewModel : ViewModelBase
    {
        public event ConversionStarted Started;

        public event ConversionEnded Ended;


        public WriteableBitmap ReferenceImage
        {
            get { return _referenceImage; }
            private set
            {
                _referenceImage = value;
                ChooseOutputFolder.RaiseCanExecuteChanged();
            }
        }

        public double Precentage
        {
            get { return _precentage; }
            set
            {
                if (_precentage != value)
                {
                    RaisePropertyChanged(() => Precentage);
                }
                _precentage = value;
            }
        }


        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set
            {
                if (value != _outputDirectory)
                {
                    RaisePropertyChanged(() => OutputDirectory);
                }
                _outputDirectory = value;
                WriteOutImages.RaiseCanExecuteChanged();
            }
        }

        public string CurrentFileName
        {
            get { return _currentFileName; }
            set
            {
                if (_currentFileName != value)
                {
                    RaisePropertyChanged(() => CurrentFileName);
                }
                _currentFileName = value;
            }
        }

        public RelayCommand LoadReferenceImage
        {
            get { return _loadReferenceImage; }
            set { _loadReferenceImage = value; }
        }

        public RelayCommand WriteOutImages { get; set; }

        public BitmapImage RefImage
        {
            get { return _refImage; }
            set
            {
                _refImage = value;

                RaisePropertyChanged(() => RefImage);
            }
        }

        public RelayCommand ChooseOutputFolder { get; set; }

        public ThemeChosen GenerateForTheme
        {
            get { return _theme; }
        }

        public ThemeChosen ChosenTheme { get; set; }

        public Visibility ToolControlsVisibility
        {
            get { return _toolControlsVisibility; }
            set
            {
                _toolControlsVisibility = value;
                RaisePropertyChanged(() => ToolControlsVisibility);
            }
        }

        public Visibility ChooseThemeVisibility
        {
            get { return _chooseThemeVisibility; }
            set
            {
                _chooseThemeVisibility = value;
                RaisePropertyChanged(() => ChooseThemeVisibility);
            }
        }

        private BitmapImage _refImage;

        private double _precentage;

        private string _outputDirectory;

        private string _currentFileName;

        private string _pathRegex =
            "^(?:[a-zA-Z]\\:(\\|\\/)|file\\:\\/\\/|\\\\|\\.(\\/|\\))([^\\\\/\\:\\*\\?\\<\\>\"\\|]+(\\|\\/){0,1})+$";


        private RelayCommand _loadReferenceImage;

        private ThemeChosen _theme;
        private WriteableBitmap _referenceImage;

        private Visibility _toolControlsVisibility;

        private Visibility _chooseThemeVisibility;

        public MainViewModel()
        {
            LoadReferenceImage = new RelayCommand(LoadLogoReferenceImage);
            WriteOutImages = new RelayCommand(WriteImages, CanWriteImages);
            ChooseOutputFolder = new RelayCommand(SelectOutputDirectory, CanChooseOutputFolder);
            RefImage = new BitmapImage();
            ToolControlsVisibility = Visibility.Collapsed;
            ChooseThemeVisibility = Visibility.Collapsed;
        }

        private bool CanChooseOutputFolder()
        {
            return this.ReferenceImage != null;
        }

        private bool CanWriteImages()
        {
            return !string.IsNullOrEmpty(OutputDirectory) && !string.IsNullOrWhiteSpace(OutputDirectory);
        }

        private async void WriteImages()
        {
            await ResizeReferenceImage();
        }

        private async void LoadLogoReferenceImage()
        {
            await ChooseReferenceImage();
        }

        private async Task ResizeReferenceImage()
        {
            this.Precentage = 0.0D;

            ToolControlsVisibility = Visibility.Visible;

            await DispatcherHelper.RunAsync(async () =>
            {
                try
                {
                    OnStarted();

                    var entries = Enum.GetValues(typeof (VisualAssets));
                    var count = entries.Length;
                    var percentagePerResizeCycle = 100/count;

                    StorageFolder fld = null;

                    if (!string.IsNullOrEmpty(OutputDirectory) || !string.IsNullOrWhiteSpace(OutputDirectory))
                    {
                        fld = await StorageFolder.GetFolderFromPathAsync(OutputDirectory);
                    }

                    if (ReferenceImage != null)
                    {
                        foreach (VisualAssets vasset in entries)
                        {
                            CurrentFileName = OutputDirectory + Enum.GetName(typeof (VisualAssets), vasset) + ".png";

                            //Set the current cycle
                            Precentage += percentagePerResizeCycle;


                            var currentImageData = EnumTools.GetAttribute<ImageDataAttribute>(vasset);

                            WriteableBitmap imgCopy = ReferenceImage.Clone();


                            var resizedImage = imgCopy.Resize(currentImageData.IconicWidth,
                                currentImageData.IconicHeight,
                                WriteableBitmapExtensions.Interpolation.Bilinear);

                            var visualAsset = new WriteableBitmap(currentImageData.Width, currentImageData.Height);


                            var outerWidth = visualAsset.PixelWidth/2;
                            var outerHeight = visualAsset.PixelHeight/2;

                            var innerWidth = resizedImage.PixelWidth/2;
                            var innerHeight = resizedImage.PixelHeight/2;

                            var placmentPointWidht = outerWidth - innerWidth;
                            var placmentPointHeight = outerHeight - innerHeight;

                            visualAsset.Blit(new Point(placmentPointWidht, placmentPointHeight), resizedImage,
                                new Rect(0, 0, resizedImage.PixelWidth, resizedImage.PixelHeight), Colors.White,
                                WriteableBitmapExtensions.BlendMode.Alpha);


                            IRandomAccessStream stream = new InMemoryRandomAccessStream();


                            var bytes = visualAsset.PixelBuffer.ToArray();

                            var assetName = "";

                            if (Enum.GetName(typeof (VisualAssets), vasset).Contains("altform"))
                            {
                                assetName = Enum.GetName(typeof (VisualAssets), vasset)
                                    .Replace("_altform", "_altform-unplated.png").Replace("TargetSize",".targetsize-");
                            }
                            else
                            {
                                assetName = Enum.GetName(typeof (VisualAssets), vasset)
                                    .Replace("Scale", ".scale-")
                                    .Replace("Square", "")
                                    .Replace("Wide", "") +
                                            (ChosenTheme == ThemeChosen.Dark ? "_black.png" : "_white.png");
                            }

                            using (
                                var imageStream =
                                    await
                                        fld.OpenStreamForWriteAsync(assetName
                                            ,
                                            CreationCollisionOption.ReplaceExisting))
                            {
                                BitmapEncoder enc =
                                    await
                                        BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId,
                                            imageStream.AsRandomAccessStream());
                                enc.SetPixelData(BitmapPixelFormat.Bgra8,
                                    BitmapAlphaMode.Straight,
                                    (uint) visualAsset.PixelWidth,
                                    (uint) visualAsset.PixelHeight,
                                    96, 96,
                                    visualAsset.PixelBuffer.ToArray());

                                await enc.FlushAsync();
                            }
                        }

                        Precentage = 100;
                        RaisePropertyChanged(() => Precentage);

                        await Task.Delay(300);

                        ToolControlsVisibility = Visibility.Collapsed;
                        ChooseThemeVisibility = Visibility.Collapsed;
                        
                        OnEnded();

                        var messageDialog = new MessageDialog("Asset Creation successfully finished.");

                        await messageDialog.ShowAsync();

                    }
                }
                catch (Exception)
                {
                    var messageDialog =
                        new MessageDialog(
                            "Sorry, but you have to choose a folder within your pictures library. Thank you.");
                    await messageDialog.ShowAsync();
                }
            });
        }

        private async Task ChooseReferenceImage()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    var img = new BitmapImage();


                    using (Windows.Storage.Streams.IRandomAccessStream fileStream =
                        await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        // Set the image source to the selected bitmap.
                        var refImage =
                            new BitmapImage();
                        refImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;


                        refImage.SetSource(fileStream);
                        RefImage = refImage;

                        ReferenceImage = new WriteableBitmap(refImage.PixelWidth, refImage.PixelHeight);
                        //fileStream.Seek(0);
                        ReferenceImage = await ReferenceImage.FromStream(fileStream);

                        ChooseThemeVisibility = Visibility.Visible;
                    }
                }
                catch (Exception)
                {
                    var messageDialog =
                        new MessageDialog("Please select an image. Any other file-type is not supported.");
                    await messageDialog.ShowAsync();
                }
            }
        }

        private async void SelectOutputDirectory()
        {
            FolderPicker outputFolderPicker = new FolderPicker();
            outputFolderPicker.FileTypeFilter.Add(".jpg");
            outputFolderPicker.FileTypeFilter.Add(".jpeg");
            outputFolderPicker.FileTypeFilter.Add(".png");
            outputFolderPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            var folder = await outputFolderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                OutputDirectory = folder.Path;
            }
        }

        protected virtual void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }
    }
}