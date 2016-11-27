﻿using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SIL.PlatformUtilities;
using uw_edit.USFM;
using uw_edit.Views;

namespace uw_edit
{
	public static class Program
	{
	    private static string _appDirectory;
		private static Font _textFont;
		private static Font _tagFont;
		public static ParatextStylesheet StyleSheet { get; private set; }
		public static MainView MainView { get; private set; }

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(true);

		    var viewModel = new MainViewModel();
			StyleSheet = new ParatextStylesheet();

		    var args = Environment.GetCommandLineArgs();
		    if (args.Length > 1)
		        viewModel.FileToOpen = args[1];

			MainView = new MainView(viewModel);
			Application.Run(MainView);
		}

		public static string GetAppDirectory()
		{
			if (string.IsNullOrEmpty(_appDirectory))
			{
				var asm = Assembly.GetExecutingAssembly();
				var file = asm.CodeBase.Replace("file://", string.Empty);
				if (Platform.IsWindows)
					file = file.TrimStart('/');
				_appDirectory = Path.GetDirectoryName(file);
			}

			return _appDirectory;
		}

		public static string GetResourcesDirectory()
		{
			return Path.Combine(GetAppDirectory(), "Resources");
		}

		public static string GetTextResource(string resourceFileName)
		{
			return File.ReadAllText(Path.Combine(GetResourcesDirectory(), resourceFileName), Encoding.UTF8);
		}

		public static Image GetImageResource(string imageFileName)
		{
			return Image.FromFile(Path.Combine(GetResourcesDirectory(), imageFileName));
		}

		public static Font GetTextFont()
		{
			if (_textFont != null)
				return _textFont;
			
			// search for fonts in this order
			var preferredFonts = new[] { "Nirmala UI", "FreeSerif" };

			foreach (var fontName in preferredFonts)
			{
				foreach (FontFamily family in FontFamily.Families)
				{
					if (family.Name == fontName)
					{
						_textFont = new Font(family, 11, FontStyle.Regular);
						return _textFont;
					}
				}
			}

			// fall through
			_textFont = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Regular);
			return _textFont;
		}

		public static Font GetTagFont()
		{
			if (_tagFont != null)
				return _tagFont;
			
			// search for fonts in this order
			var preferredFonts = new[] { "DejaVu Sans Mono", "Consolas", "Droid Sans Mono", "Liberation Mono", "Terminus", "Courier New" };

			foreach (var fontName in preferredFonts)
			{
				foreach (FontFamily family in FontFamily.Families)
				{
					if (family.Name == fontName)
					{
						_tagFont = new Font(family, 11, FontStyle.Regular);
						return _textFont;
					}
				}
			}

			_tagFont = new Font(FontFamily.GenericMonospace, 11, FontStyle.Regular);
			return _tagFont;
		}
	}
}
