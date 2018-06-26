//
// UserAddinManagerDialog.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;
using MonoDevelop.Ide;
using Xwt.Drawing;

namespace MonoDevelop.UserInstalledAddinsManagement
{
	partial class UserAddinManagerDialog
	{
		List<Addin> userAddins;
		Image enabledAddinImage = ImageService.GetIcon ("gtk-plugin");
		Image disabledAddinImage = ImageService.GetIcon ("gtk-plugin").WithAlpha (0.6);

		public UserAddinManagerDialog ()
		{
			Build ();
			GetUserAddins ();
			DisplayUserAddins ();

			restartButton.Clicked += RestartButtonClicked;
			disableButton.Clicked += DisableButtonClicked;
			disableAllButton.Clicked += DisableAllButtonClicked;
			enableButton.Clicked += EnableButtonClicked;
			enableAllButton.Clicked += EnableAllButtonClicked;

			addinsListView.SelectionChanged += AddinsListViewSelectionChanged;
		}

		void GetUserAddins ()
		{
			userAddins = AddinManager.Registry.GetModules (AddinSearchFlags.IncludeAddins | AddinSearchFlags.LatestVersionsOnly)
				.Where (addin => addin.IsUserAddin)
				.ToList ();

			userAddins.Sort (CompareAddin);
		}

		static int CompareAddin (Addin x, Addin y)
		{
			return string.Compare (x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
		}

		void DisplayUserAddins ()
		{
			foreach (Addin addin in userAddins) {
				int row = addinsListStore.AddRow ();
				UpdateAddinRow (row, addin);
			}
		}

		void UpdateAddinRow (int row, Addin addin)
		{
			addinsListStore.SetValues (
				row,
				addinImageField,
				GetImage (addin),
				addinNameField,
				GetAddinNameMarkup (addin),
				addinField,
				addin);
		}

		Image GetImage (Addin addin)
		{
			if (addin.Enabled)
				return enabledAddinImage;

			return disabledAddinImage;
		}

		static string GetAddinNameMarkup (Addin addin)
		{
			if (addin.Enabled)
				return "<b>" + addin.Name + "</b>";

			return "<span weight='ultralight'>" + addin.Name + "</span>";
		}

		void RestartButtonClicked (object sender, EventArgs e)
		{
			IdeApp.Restart (true).Ignore ();
		}

		void AddinsListViewSelectionChanged (object sender, EventArgs e)
		{
			UpdateButtonsState ();
		}

		void UpdateButtonsState ()
		{
			int[] rows = addinsListView.SelectedRows;
			if (rows.Length == 0) {
				enableButton.Sensitive = false;
				disableButton.Sensitive = false;
			} else if (rows.Length == 1) {
				Addin addin = addinsListStore.GetValue (rows [0], addinField);
				enableButton.Sensitive = !addin.Enabled;
				disableButton.Sensitive = addin.Enabled;
			} else {
				enableButton.Sensitive = true;
				disableButton.Sensitive = true;
			}
		}

		void DisableButtonClicked (object sender, EventArgs e)
		{
			int[] rows = addinsListView.SelectedRows;
			try {
				foreach (int row in rows) {
					DisableAddin (row);
				}
			} catch (Exception ex) {
				MessageService.ShowError (ex.Message);
			}

			UpdateButtonsState ();
		}

		void EnableButtonClicked (object sender, EventArgs e)
		{
			int[] rows = addinsListView.SelectedRows;
			try {
				foreach (int row in rows) {
					EnableAddin (row);
				}
			} catch (Exception ex) {
				MessageService.ShowError (ex.Message);
			}

			UpdateButtonsState ();
		}

		void EnableAddin (int row)
		{
			ConfigureAddin (row, enabled: true);
		}

		void ConfigureAddin (int row, bool enabled)
		{
			Addin addin = addinsListStore.GetValue (row, addinField);
			if (addin.Enabled != enabled) {
				addin.Enabled = enabled;
				UpdateAddinRow (row, addin);
			}
		}

		void DisableAddin (int row)
		{
			ConfigureAddin (row, enabled: false);
		}

		void DisableAllButtonClicked (object sender, EventArgs e)
		{
			try {
				for (int row = 0; row < addinsListStore.RowCount; ++row) {
					DisableAddin (row);
				}
			} catch (Exception ex) {
				MessageService.ShowError (ex.Message);
			}

			UpdateButtonsState ();
		}

		void EnableAllButtonClicked (object sender, EventArgs e)
		{
			try {
				for (int row = 0; row < addinsListStore.RowCount; ++row) {
					EnableAddin (row);
				}
			} catch (Exception ex) {
				MessageService.ShowError (ex.Message);
			}

			UpdateButtonsState ();
		}
	}
}
