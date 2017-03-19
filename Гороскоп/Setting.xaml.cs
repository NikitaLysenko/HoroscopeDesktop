using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.Win32;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;

namespace WpfApplication3
{
    public partial class Setting : Window
    {
        private MainWindow _showMainWindow;
        private SettingStyle cyrrentStyle;
        private Horoscope.Properties.Settings ps = Horoscope.Properties.Settings.Default;

        public Setting(MainWindow k)
        {
            InitializeComponent();

            _showMainWindow = k;
            cyrrentStyle = _showMainWindow.styleWindow;

            autoStart_CheckBox.IsChecked = ps.Autorun;
            checkBox_Copy.IsChecked = ps.FormFixed;
            comboBox.ItemsSource = _showMainWindow.zodiacListRu.Values;
            comboBox.SelectedValue = _showMainWindow.zodiacListRu[ps.CurrentZodiac];
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            _showMainWindow.ApplySetting(cyrrentStyle);
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            zodiakSave();
            _showMainWindow.WriteSetting(_showMainWindow.styleWindow);
            Close();
        }

        private void zodiakSave()
        {
            var keys = from entry in _showMainWindow.zodiacListRu
                where entry.Value == comboBox.SelectedItem.ToString()
                select entry.Key;
            foreach (var value in keys)
            {
                _showMainWindow.currentZodiac = value;
                _showMainWindow.writeTextBlock(value);
            }
        }

        private void colorCanvasMainWindows(SolidColorBrush color)
        {
            _showMainWindow.border.Background = color;
        }

        private void colorCanvasFount(SolidColorBrush color)
        {
            _showMainWindow.presentDay.Foreground = color;
            _showMainWindow.tomorrow.Foreground = color;
            _showMainWindow.week.Foreground = color;
        }

        private void _colorCanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var wpfCollor = Color.FromArgb(_colorCanvas.A, _colorCanvas.R, _colorCanvas.G, _colorCanvas.B);
            colorCanvasMainWindows(new SolidColorBrush(wpfCollor));
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_showMainWindow != null)
            {
                _showMainWindow.presentDay.Foreground.Opacity = Math.Round(e.NewValue / 100, 2);
                _showMainWindow.tomorrow.Foreground.Opacity = Math.Round(e.NewValue / 100, 2);
                _showMainWindow.week.Foreground.Opacity = Math.Round(e.NewValue / 100, 2);
            }

        }

        private void _colorCanvas2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var wpfCollor = Color.FromArgb(_colorCanvas2.A, _colorCanvas2.R, _colorCanvas2.G, _colorCanvas2.B);
            colorCanvasFount(new SolidColorBrush(wpfCollor));
        }

        private void heightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_showMainWindow != null)
            {
                _showMainWindow.tabControl.Height = Math.Round(e.NewValue);
            }
        }

        private void widthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_showMainWindow != null)
            {
                _showMainWindow.tabControl.Width = Math.Round(e.NewValue);
            }
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            if (_showMainWindow != null)
            {

                if (checkBox.IsChecked == true)
                {
                    ps.Opacity = true;
                    _showMainWindow.border.Background.Opacity = 0;
                }
                else
                {
                    ps.Opacity = false;
                    _showMainWindow.border.Background.Opacity = 100;
                }
            }
        }

        private void transparentColorsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_showMainWindow != null)
            {
                _showMainWindow.gridOpacity.Opacity = Math.Round(e.NewValue / 100, 2);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FountSetting(fontDialog.Font);
            }
        }

        private void FountSetting(Font font)
        {
            FontFamilyConverter ffc = new FontFamilyConverter();

            _showMainWindow.FontSize = font.Size;
            _showMainWindow.FontFamily = (FontFamily) ffc.ConvertFromString(font.Name);

            if (font.Bold)
                _showMainWindow.FontWeight = FontWeights.Bold;
            else
                _showMainWindow.FontWeight = FontWeights.Normal;

            if (font.Italic)
                _showMainWindow.FontStyle = FontStyles.Italic;
            else
                _showMainWindow.FontStyle = FontStyles.Normal;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _showMainWindow.ApplySetting(cyrrentStyle);
        }

        private void checkBox_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (_showMainWindow != null)
            {
                ps.FormFixed = checkBox_Copy.IsChecked == true;
            }
        }

        private void autoStart_CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if ((bool) checkBox.IsChecked)
            {
                _showMainWindow.Autorun(true);
                ps.Autorun = true;

            }
            else
            {
                _showMainWindow.Autorun(false);
                ps.Opacity = false;
            }
        }

    }
}
