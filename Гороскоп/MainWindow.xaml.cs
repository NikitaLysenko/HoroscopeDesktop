using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HtmlAgilityPack;
using System.Xml.Serialization;
using Microsoft.Win32;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace WpfApplication3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
	    public string currentZodiac { get; set; } = "aries";
        private Horoscope.Properties.Settings ps = Horoscope.Properties.Settings.Default;
        public SettingStyle styleWindow { get; set; } = new SettingStyle();

        public MainWindow()
		{
			InitializeComponent();

            Autorun(true);
            ApplySetting(styleWindow);
		    try
		    {
                Class.serverParse();
                writeTextBlock(ps.CurrentZodiac);
            }
		    catch (Exception e)
		    {
		        
		    }
        }

        public void Autorun(bool ck)
        {
            try
            {
                string ExePath = System.Windows.Forms.Application.ExecutablePath;
                RegistryKey reg;
                reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                if (ck)
                {
                    reg.DeleteValue("Гороскоп на рабочий стол");
                    reg.SetValue("Гороскоп на рабочий стол", ExePath);
                    reg.Close();
                }
                else
                {
                    reg.DeleteValue("Гороскоп на рабочий стол");
                }
            }
            catch (Exception)
            {
                presentDay.Text = "Ошибка подключения к серверу";
                tomorrow.Text = "Ошибка подключения к серверу";
                week.Text = "Ошибка подключения к серверу";
            }
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle,
                GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }

        public void ApplySetting(SettingStyle style)
	    {
	        tabControl.Width = ps.Width;
            tabControl.Height = ps.Height;
            border.Background.Opacity = ps.Opacity ? 0 : 100;
            currentZodiac = ps.CurrentZodiac;
	        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ps.Background));
	        gridOpacity.Opacity = ps.WindowOrasity;
	        WindowMain.FontSize = ps.FontSize;
	        WindowMain.FontFamily = (System.Windows.Media.FontFamily)new FontFamilyConverter().ConvertFromString(ps.FontName);
            presentDay.Foreground.Opacity = ps.FontOpacity;
            tomorrow.Foreground.Opacity = ps.FontOpacity;
            week.Foreground.Opacity = ps.FontOpacity;

            var colorText = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ps.Foreground));
	        presentDay.Foreground = colorText;
            tomorrow.Foreground = colorText;
            week.Foreground = colorText;
            
            if (ps.FontStyle == "Normal")
	            WindowMain.FontStyle = FontStyles.Normal;
	        if (ps.FontStyle == "Italic")
	            WindowMain.FontStyle = FontStyles.Italic;

	        if (ps.FontWeight == "Normal")
	            WindowMain.FontWeight = FontWeights.Normal;
	        if (ps.FontWeight == "Bold")
	            WindowMain.FontWeight = FontWeights.Bold;
	    }

        public void WriteSetting(SettingStyle style)
        {
            Horoscope.Properties.Settings ps = Horoscope.Properties.Settings.Default;
            ps.CurrentZodiac = currentZodiac;
            var converter = new BrushConverter();
            ps.Background = converter.ConvertToString(border.Background);
            ps.WindowOrasity = gridOpacity.Opacity;
            ps.FontSize = WindowMain.FontSize;
            ps.FontName = WindowMain.FontFamily.Source;
            ps.FontOpacity= presentDay.Foreground.Opacity;
            ps.Foreground = converter.ConvertToString(presentDay.Foreground);

            if (WindowMain.FontStyle == FontStyles.Normal)
                ps.FontStyle = "Normal";
            if (WindowMain.FontStyle == FontStyles.Italic)
                ps.FontStyle = "Italic";

            if (WindowMain.FontWeight == FontWeights.Normal)
                ps.FontWeight = "Normal";
            if (WindowMain.FontWeight == FontWeights.Bold)
                ps.FontWeight = "Bold";
        }

        internal void writeTextBlock(string zodiac = "aries")
		{
			currentZodiac_image.Source = new BitmapImage( new Uri(Directory.GetCurrentDirectory() + "\\Resources\\images\\" + zodiac+".png"));
			currentZodiac_image.ToolTip = zodiacListRu[zodiac] + " (текущий гороскоп)";
			presentDay.Text = Class._all[0][zodiac];
			tomorrow.Text = Class._all[1][zodiac];
			week.Text = Class._all[2][zodiac];
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
		    try
		    {
		        if (!ps.FormFixed)
		        {
                    this.DragMove();
                }
		    }
		    catch (Exception)
		    {
		    }
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		private void buttonAries_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("aries");
		}

		private void buttonGemini_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("gemini");
		}

		private void buttonTaurus_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("taurus");
		}

	    private void buttonCancer_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("cancer");
		}

		private void buttonLeo_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("leo");
		}

		private void buttonVirgo_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("virgo");
		}

		private void buttonLibra_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("libra");
		}

		private void buttonScorpio_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("scorpio");
		}

		private void buttonSagittarius_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("sagittarius");
		}

		private void buttonCapricorn_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("capricorn");
		}

		private void buttonAquarius_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("aquarius");
		}

		private void buttonPisces_Click(object sender, RoutedEventArgs e)
		{
			writeTextBlock("pisces");
		}

		private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
		{
			var form = new Setting(this);
			form.ShowDialog();
		}

        #region zodiacListRu
        internal Dictionary<string, string> zodiacListRu = new Dictionary<string, string>
        {
            {"aries", "Овен"},
            {"taurus", "Телец"},
            {"gemini", "Близнецы"},
            {"cancer", "Рак"},
            {"leo", "Лев"},
            {"virgo", "Дева"},
            {"libra", "Весы"},
            {"scorpio", "Скорпион"},
            {"sagittarius", "Стрелец"},
            {"capricorn", "Козерог"},
            {"aquarius", "Водолей"},
            {"pisces", "Рыбы"}
        }; 
        #endregion

        #region horoscopesList
        public List<string> horoscopesList = new List<string>
        {
            "aries",
            "taurus",
            "gemini",
            "cancer",
            "leo",
            "virgo",
            "libra",
            "scorpio",
            "sagittarius",
            "capricorn",
            "aquarius",
            "pisces"
        };
        #endregion

        private void WindowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Horoscope.Properties.Settings ps = Horoscope.Properties.Settings.Default;
            ps.Top = this.Top;
            ps.Left = this.Left;
            ps.Save();
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            HideFromAltTab(new WindowInteropHelper(this).Handle);
            Horoscope.Properties.Settings ps = Horoscope.Properties.Settings.Default;
            this.Top = ps.Top;
            this.Left = ps.Left;
        }
    }
    static class Class
	{
		private static string[] _horoscopesList = new string[12];
		public static List<Dictionary<string,string>> _all = new List<Dictionary<string, string>>();

		public static void serverParse()
		{
			_all.Add(pa());
			_all.Add(pa("tomorrow"));
			_all.Add(pa("week"));
		}

		private static Dictionary<string, string> pa(string adress = "")
		{
			var _horoscopesList = new List<string>()
			{"aries","taurus", "gemini","cancer", "leo","virgo","libra", "scorpio", "sagittarius", "capricorn","aquarius",
				"pisces"};
			var horoscopesDictoinary = new Dictionary<string, string> {};
			using (WebClient client = new WebClient())
			{
				client.Encoding = Encoding.UTF8;
				foreach (var v in _horoscopesList)
				{
					string html = client.DownloadString(@"http://m.horoscopes.rambler.ru/" + v + "/"+ adress);
					HtmlDocument doc = new HtmlDocument();
					doc.LoadHtml(html);
					HtmlNodeCollection node = doc.DocumentNode.SelectNodes("//div[@class='horo-body']");
					if (node != null)
					{
						foreach (HtmlNode docs in node)
						{
							horoscopesDictoinary.Add(v, docs.FirstChild.InnerText);
						}
					}
				}
				return horoscopesDictoinary;
			}
		}
	}

    public class Default
    {
        [XmlIgnore]
        internal static Horoscope.Properties.Settings ps = Horoscope.Properties.Settings.Default;
    }

    [Serializable]
    public class SettingStyle:Default
    {
        public WindowStile WindowStile { get; set; } = new WindowStile();
        public FountStile FountStile { get; set; } = new FountStile();
     }
    [Serializable]
    public class FountStile:Default
    {
        public double FontSize { get; set; } = ps.FontSize;
        public string FontName { get; set; } = ps.FontName;
        public string FontWeight { get; set; } = ps.FontWeight;
        public string FontStyle { get; set; } = ps.FontStyle;
        public double Opacity { get; set; } = ps.FontOpacity;
        public string Foreground { get; set; } = ps.Foreground;
    }
    [Serializable]
    public class WindowStile:Default
    {
        public double Orasity { get; set;} = ps.WindowOrasity;
        public string Background { get; set; } = ps.Background;
    }
}
