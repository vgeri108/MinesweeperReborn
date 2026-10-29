/*
 * Copyright (C) 2026 vgeri108
 *
 * This file is part of MinesweeperReborn.
 *
 * MinesweeperReborn is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3 only.
 *
 * MinesweeperReborn is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with MinesweeperReborn. If not, see <https://www.gnu.org/licenses/>.
 */

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MinesweeperCore;
using System.Threading.Tasks;

namespace MinesweeperReborn
{
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            BoardShowcase.Source = AssetPaths.LoadBitmap(ThemeManager.CellStatusImage[CellStatus.Covered]);
        }

        private async void JatekBeallitasModositas_Click(object sender, RoutedEventArgs e)
        {
            Inditas.IsExpanded = false;
            Mentes.IsExpanded = false;
            JatekBeallitasok.IsExpanded = true;
            Megjelenes.IsExpanded = false;

            JatekBeallitasok.BringIntoView();

            await HighlightExpander(JatekBeallitasok);
        }

        private async Task HighlightExpander(Expander expander)
        {
            IBrush? eredetiSzin = expander.Foreground;
            FontWeight eredetiBetu = expander.FontWeight;

            for (int i = 0; i < 3; i++)
            {
                expander.Foreground = Brushes.OrangeRed;
                expander.FontWeight = FontWeight.ExtraBold;

                await Task.Delay(200);

                expander.Foreground = eredetiSzin;
                expander.FontWeight = eredetiBetu;

                await Task.Delay(200);
            }
        }

        private async void EgyediTema_Click(object sender, RoutedEventArgs e)
        {
            Window dialog = new()
            {
                Title = "Súgó",
                Width = 360,
                Height = 150,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            Button closeButton = new()
            {
                Content = "OK",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
            };
            closeButton.Click += (_, _) => dialog.Close();
            dialog.Content = new StackPanel
            {
                Margin = new Thickness(16),
                Spacing = 16,
                Children =
                {
                    new TextBlock
                    {
                        Text = "Ez a súgótéma még nem érhető el.",
                        TextWrapping = TextWrapping.Wrap
                    },
                    closeButton
                }
            };
            Window owner = Owner is Window ownerWindow ? ownerWindow : this;
            await dialog.ShowDialog(owner);
        }
    }
}