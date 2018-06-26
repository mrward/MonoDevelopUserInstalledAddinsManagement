//
// UserAddinManagerDialog.UI.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2018 Microsoft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using MonoDevelop.Core;
using Xwt;
using Mono.Addins;
using Xwt.Drawing;

namespace MonoDevelop.UserInstalledAddinsManagement
{
	partial class UserAddinManagerDialog : Dialog
	{
		ListView addinsListView;
		ListStore addinsListStore;
		DataField<Image> addinImageField;
		DataField<string> addinNameField;
		DataField<Addin> addinField;
		Button enableButton;
		Button disableButton;
		Button enableAllButton;
		Button disableAllButton;
		Button restartButton;

		void Build ()
		{
			Title = GettextCatalog.GetString ("User Extensions");
			Height = 480;
			Width = 640;

			var mainHBox = new HBox ();

			addinImageField = new DataField<Image> ();
			addinNameField = new DataField<string> ();
			addinField = new DataField<Addin> ();
			addinsListStore = new ListStore (addinImageField, addinNameField, addinField);

			addinsListView = new ListView ();
			addinsListView.HeadersVisible = false;
			addinsListView.SelectionMode = SelectionMode.Multiple;
			addinsListView.DataSource = addinsListStore;
			mainHBox.PackStart (addinsListView, true, true);

			var column = new ListViewColumn ();

			var imageCellView = new ImageCellView ();
			imageCellView.ImageField = addinImageField;
			column.Views.Add (imageCellView);

			var cellView = new TextCellView ();
			cellView.MarkupField = addinNameField;
			column.Views.Add (cellView);
			addinsListView.Columns.Add (column);

			var buttonVBox = new VBox ();
			enableButton = new Button ();
			enableButton.Label = GettextCatalog.GetString ("Enable");
			buttonVBox.PackStart (enableButton);

			disableButton = new Button ();
			disableButton.Label = GettextCatalog.GetString ("Disable");
			buttonVBox.PackStart (disableButton);

			var separator = new HSeparator ();
			buttonVBox.PackStart (separator);

			enableAllButton = new Button ();
			enableAllButton.Label = GettextCatalog.GetString ("Enable All");
			buttonVBox.PackStart (enableAllButton);

			disableAllButton = new Button ();
			disableAllButton.Label = GettextCatalog.GetString ("Disable All");
			buttonVBox.PackStart (disableAllButton);

			separator = new HSeparator ();
			buttonVBox.PackStart (separator);

			restartButton = new Button ();
			restartButton.Label = GettextCatalog.GetString ("Restart {0}", BrandingService.ApplicationName);
			buttonVBox.PackStart (restartButton);

			mainHBox.PackStart (buttonVBox, false, false);

			Content = mainHBox;

			var closeButton = new DialogButton (Command.Close);
			closeButton.Clicked += CloseButtonClicked;
			Buttons.Add (closeButton);
		}

		void CloseButtonClicked (object sender, System.EventArgs e)
		{
			Close ();
		}
	}
}
