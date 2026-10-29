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
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MinesweeperCore;
using System;
using System.Collections.Generic;

namespace MinesweeperReborn
{
    internal class Draw
    {
        public Game game;
        public UniformGrid grid;
        private readonly List<Image> cellImages = new();
        private readonly List<Border> cellHighlights = new();
        private readonly List<bool> hoveredCells = new();
        private readonly ButtonClicks buttonClicks;
        private readonly TextBlock mineCounter;
        private IBrush buttonBackground = Brushes.Transparent;
        private IBrush cellHoverBrush = Brushes.Transparent;
        private double buttonCorner;

        private Dictionary<object, Bitmap> ImageCache = new Dictionary<object, Bitmap>();

        public Draw(Game game, UniformGrid grid, TextBlock mineCounter, Action gameOver)
        {
            this.game = game;
            this.grid = grid;
            buttonClicks = new ButtonClicks(game, DrawBoard, gameOver);
            this.mineCounter = mineCounter;

            CacheImages();
            InitBoard();
            DrawBoard();
        }

        private void CacheImages()
        {
            foreach (var item in Enum.GetValues<Cell>())
            {
                if (!ImageCache.ContainsKey(item)) ImageCache.Add(item, AssetPaths.LoadBitmap(ThemeManager.CellImage[item]));
            }
            foreach (var item in Enum.GetValues<CellStatus>())
            {
                if (item != CellStatus.Uncovered)
                {
                    if (!ImageCache.ContainsKey(item)) ImageCache.Add(item, AssetPaths.LoadBitmap(ThemeManager.CellStatusImage[item]));
                }
            }
            foreach (var image in ThemeManager.GameOverImages.Values)
            {
                if (!ImageCache.ContainsKey(image)) ImageCache.Add(image, AssetPaths.LoadBitmap(image));
            }
        }

        private void InitBoard()
        {
            grid.Children.Clear();
            cellImages.Clear();
            cellHighlights.Clear();
            hoveredCells.Clear();
            grid.Rows = game.Height;
            grid.Columns = game.Width;
            buttonBackground = new SolidColorBrush(ThemeManager.GameBoardButtonColor);
            cellHoverBrush = new SolidColorBrush(ThemeManager.GameBoardHoverColor);
            double boardScale = Math.Min(1d, 9d / Math.Max(game.Width, game.Height));
            double buttonMargin = ThemeManager.GameBoardMargin * boardScale;
            buttonCorner = ThemeManager.GameBoardCorner * boardScale;

            for (int x = 0; x < game.Height; x++)
            {
                for (int y = 0; y < game.Width; y++)
                {
                    Button btn = new Button
                    {
                        Tag = new Point(x, y),
                        Margin = new Thickness(buttonMargin),
                        Padding = new Thickness(0),
                        BorderThickness = new Thickness(0),
                        BorderBrush = Brushes.Transparent,
                        CornerRadius = new CornerRadius(buttonCorner),
                        Background = Brushes.Transparent,
                        ClipToBounds = true,
                    };

                    int index = x * game.Width + y;
                    hoveredCells.Add(false);
                    btn.PointerEntered += (_, _) => SetCellHovered(index, true);
                    btn.PointerExited += (_, _) => SetCellHovered(index, false);
                    btn.AddHandler(InputElement.PointerPressedEvent, buttonClicks.Button_LeftClick, RoutingStrategies.Bubble, handledEventsToo: true);
                    btn.AddHandler(InputElement.PointerPressedEvent, buttonClicks.Button_RightClick, RoutingStrategies.Bubble, handledEventsToo: true);
                    btn.AddHandler(InputElement.DoubleTappedEvent, buttonClicks.Button_DoubleClick, RoutingStrategies.Bubble, handledEventsToo: true);

                    btn.Content = CreateCellContent(CellStatus.Covered);

                    grid.Children.Add(btn);
                }
            }
        }

        private void DrawBoard()
        {
            mineCounter.Text = game.RemainingFlags.ToString();

            for (int x = 0; x < game.Height; x++)
            {
                for (int y = 0; y < game.Width; y++)
                {
                    int index = x * game.Width + y;

                    object sourceCell = Cell.Empty;

                    switch (game.Status[x, y])
                    {
                        case CellStatus.Uncovered:
                            sourceCell = game.GetCell(x, y);
                            break;
                        case CellStatus.Covered:
                            sourceCell = game.Status[x, y];
                            break;
                        case CellStatus.Flagged:
                            sourceCell = game.Status[x, y];
                            break;
                        case CellStatus.Questioned:
                            sourceCell = game.Status[x, y];
                            break;
                    }

                    string? gameOverImage = null;
                    if (game.Result == Game.GameStatus.Lose)
                    {
                        if (game.GetCell(x, y) == Cell.Mine)
                        {
                            gameOverImage = game.Status[x, y] switch
                            {
                                CellStatus.Uncovered => "MineClicked",
                                CellStatus.Covered => "MineNotClicked",
                                CellStatus.Flagged => "MineFlagged",
                                CellStatus.Questioned => "MineQuestion",
                                _ => null
                            };
                        }
                        else if (game.Status[x, y] == CellStatus.Flagged)
                        {
                            gameOverImage = "NotMine";
                        }
                    }
                    else if (game.Result == Game.GameStatus.Win && game.GetCell(x, y) == Cell.Mine)
                    {
                        gameOverImage = game.Status[x, y] switch
                        {
                            CellStatus.Questioned => "MineWinQuestion",
                            CellStatus.Flagged => null,
                            _ => "MineWinNotClicked"
                        };
                    }

                    if (gameOverImage != null && ThemeManager.GameOverImages.TryGetValue(gameOverImage, out string? imagePath))
                        sourceCell = imagePath;

                    cellImages[index].Source = ImageCache[sourceCell];
                    UpdateCellHighlight(index);
                }
            }
        }

        private void SetCellHovered(int index, bool isHovered)
        {
            hoveredCells[index] = isHovered;
            UpdateCellHighlight(index);
        }

        private void UpdateCellHighlight(int index)
        {
            int x = index / game.Width;
            int y = index % game.Width;
            cellHighlights[index].Background = hoveredCells[index] && game.Status[x, y] != CellStatus.Uncovered
                ? cellHoverBrush
                : Brushes.Transparent;
        }

        private Border CreateCellContent(object sourceCell)
        {
            Image image = new()
            {
                Source = ImageCache[sourceCell],
                Stretch = Avalonia.Media.Stretch.UniformToFill,
                Margin = new Thickness(0),
            };
            Border highlight = new();
            cellImages.Add(image);
            cellHighlights.Add(highlight);

            return new Border
            {
                CornerRadius = new CornerRadius(buttonCorner),
                ClipToBounds = true,
                Background = buttonBackground,
                Child = new Grid
                {
                    Children =
                    {
                        image,
                        highlight
                    }
                }
            };
        }
    }
}
