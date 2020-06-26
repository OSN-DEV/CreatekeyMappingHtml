using CreateKeymappingHtml.AppUtil;
using CreateKeymappingHtml.Data;
using System.Windows;
using OsnCsLib.File;
using System.Text;

namespace CreateKeymappingHtml {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        #region Constrcutor
        public MainWindow() {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Event
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true)) {
                e.Effects = System.Windows.DragDropEffects.Copy;
            } else {
                e.Effects = System.Windows.DragDropEffects.None;
            }
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Drop(object sender, DragEventArgs e) {
            var files = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (null == files || !System.IO.File.Exists(files[0])) {
                return;
            }

            this.cFile.Text = files[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Click(object sender, RoutedEventArgs e) {
            var data = new StringBuilder();
            var converter = new RomanTableConverter();
            string parent;
            using (var src = new FileOperator(this.cFile.Text)) {
                if (!src.Exists()) {
                    MessageBox.Show("invalid input file");
                    return;
                }
                parent = src.FileDir;
                src.OpenForRead();
                while(!src.Eof) {
                    data.AppendLine(converter.Convert(src.ReadLine()));
                }
            }

            using (var dest = new FileOperator(parent + @"\mapping.html")) {
                var template = new FileOperator(OsnCsLib.Common.Util.GetAppPath() + @"Res\index.html").ReadAll();
                dest.Delete();
                dest.OpenForWrite();
                dest.Write(template.Replace("<!--container-->", data.ToString()));
            }

            var settings = AppRepository.GetInstance();
            settings.ConvertFile = this.cFile.Text;
            settings.Save();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize window
        /// </summary>
        private void Initialize() {
            var settings = AppRepository.Init(Constants.SettingFile);
            this.cFile.Text =  settings.ConvertFile;

        }
        #endregion
    }
}
