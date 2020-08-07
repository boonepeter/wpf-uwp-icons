using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Svg2Xaml
{
    public class MainWindowVM : ObservableObject
    {
        private ObservableCollection<string> _SVGFilenames = new ObservableCollection<string>();
        public ObservableCollection<string> SVGFilenames
        {
            get { return _SVGFilenames; }
            set
            {
                if (value != _SVGFilenames)
                {
                    _SVGFilenames = value;
                    OnPropertyChanged(nameof(SVGFilenames));
                }
            }
        }

        private string _OutputFilepath;
        public string OutputFilepath
        {
            get { return _OutputFilepath; }
            set
            {
                if (value != _OutputFilepath)
                {
                    _OutputFilepath = value;
                    OnPropertyChanged(nameof(OutputFilepath));
                }
            }
        }


        public void SelectSVGs(object sender, RoutedEventArgs args)
        {
            var files = new OpenFileDialog();
            files.Filter = "SVG|*.svg";
            files.Multiselect = true;

            bool? result = files.ShowDialog();
            if (result == true)
            {
                for (int i = 0; i < files.FileNames.Length; i++)
                {
                    SVGFilenames.Add(files.FileNames[i]);
                }
            }
        }

        public void DropFiles(object sender, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])args.Data.GetData(DataFormats.FileDrop);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].ToLower().EndsWith(".svg"))
                    {
                        SVGFilenames.Add(files[i]);

                    }
                }
            }
        }

        public void SelectOutputFilepath(object sender, RoutedEventArgs args)
        {
            var file = new SaveFileDialog();
            file.DefaultExt = ".xaml";

            bool? result = file.ShowDialog();

            if (result == true)
            {
                OutputFilepath = file.FileName;
            }
            else
            {
                OutputFilepath = null;
            }
        }

        public void ClearSVGs(object sender, RoutedEventArgs args)
        {
            SVGFilenames.Clear();
        }

        public void Convert(object sender, RoutedEventArgs args)
        {
            if (SVGFilenames == null || OutputFilepath == null) return;
            string dictionary = Converter.BuildResourceDictionary(SVGFilenames.ToArray());
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilepath));
            File.WriteAllText(OutputFilepath, dictionary);
        }
    }
}
