using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace OdiumBotManager
{
    public partial class CustomDialogs : Window
    {
        public enum DialogType
        {
            Confirm,
            Input,
            Warning,
            Error,
            Success,
            Information
        }

        public enum DialogResult
        {
            None,
            OK,
            Cancel,
            Yes,
            No
        }

        public DialogResult Result { get; private set; } = DialogResult.None;
        public string InputText { get; private set; } = string.Empty;

        private CustomDialogs()
        {
            InitializeComponent();
        }

        #region Static Show Methods

        /// <summary>
        /// Shows a confirmation dialog with Yes/No buttons
        /// </summary>
        public static DialogResult ShowConfirm(string title, string message, Window owner = null)
        {
            var dialog = new CustomDialogs();
            dialog.Owner = owner ?? Application.Current.MainWindow;
            dialog.Title = title;
            dialog.BuildConfirmDialog(title, message);
            dialog.ShowDialog();
            return dialog.Result;
        }

        /// <summary>
        /// Shows an input dialog with OK/Cancel buttons
        /// </summary>
        public static string ShowInput(string title, string message, string defaultValue = "", Window owner = null)
        {
            var dialog = new CustomDialogs();
            dialog.Owner = owner ?? Application.Current.MainWindow;
            dialog.Title = title;
            dialog.BuildInputDialog(title, message, defaultValue);
            dialog.ShowDialog();
            return dialog.Result == DialogResult.OK ? dialog.InputText : null;
        }

        /// <summary>
        /// Shows a warning dialog with OK button
        /// </summary>
        public static void ShowWarning(string title, string message, Window owner = null)
        {
            var dialog = new CustomDialogs();
            dialog.Owner = owner ?? Application.Current.MainWindow;
            dialog.Title = title;
            dialog.BuildMessageDialog(title, message, DialogType.Warning);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Shows an error dialog with OK button
        /// </summary>
        public static void ShowError(string title, string message, Window owner = null)
        {
            var dialog = new CustomDialogs();
            dialog.Owner = owner ?? Application.Current.MainWindow;
            dialog.Title = title;
            dialog.BuildMessageDialog(title, message, DialogType.Error);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Shows a success dialog with OK button
        /// </summary>
        public static void ShowSuccess(string title, string message, Window owner = null)
        {
            var dialog = new CustomDialogs();
            dialog.Owner = owner ?? Application.Current.MainWindow;
            dialog.Title = title;
            dialog.BuildMessageDialog(title, message, DialogType.Success);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Shows an information dialog with OK button
        /// </summary>
        public static void ShowInformation(string title, string message, Window owner = null)
        {
            var dialog = new CustomDialogs();
            dialog.Owner = owner ?? Application.Current.MainWindow;
            dialog.Title = title;
            dialog.BuildMessageDialog(title, message, DialogType.Information);
            dialog.ShowDialog();
        }

        #endregion

        #region Dialog Builders

        private void BuildConfirmDialog(string title, string message)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)FindResource("TextPrimary"),
                Margin = new Thickness(0, 0, 0, 16)
            };
            Grid.SetRow(titleBlock, 0);
            grid.Children.Add(titleBlock);

            var messageBlock = new TextBlock
            {
                Text = message,
                FontSize = 14,
                Foreground = (Brush)FindResource("TextSecondary"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20),
                LineHeight = 22
            };
            Grid.SetRow(messageBlock, 1);
            grid.Children.Add(messageBlock);

            var buttonPanel = new UniformGrid
            {
                Columns = 2,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var noButton = new Button
            {
                Content = "No",
                Style = (Style)FindResource("ModernButton"),
                Margin = new Thickness(0, 0, 8, 0)
            };
            noButton.Click += (s, e) => { Result = DialogResult.No; Close(); };

            var yesButton = new Button
            {
                Content = "Yes",
                Style = (Style)FindResource("PrimaryButton"),
                Margin = new Thickness(8, 0, 0, 0)
            };
            yesButton.Click += (s, e) => { Result = DialogResult.Yes; Close(); };

            buttonPanel.Children.Add(noButton);
            buttonPanel.Children.Add(yesButton);

            Grid.SetRow(buttonPanel, 2);
            grid.Children.Add(buttonPanel);

            MainContent.Children.Add(grid);

            yesButton.Focus();
        }

        private void BuildInputDialog(string title, string message, string defaultValue)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)FindResource("TextPrimary"),
                Margin = new Thickness(0, 0, 0, 16)
            };
            Grid.SetRow(titleBlock, 0);
            grid.Children.Add(titleBlock);

            var messageBlock = new TextBlock
            {
                Text = message,
                FontSize = 14,
                Foreground = (Brush)FindResource("TextSecondary"),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 16),
                LineHeight = 22
            };
            Grid.SetRow(messageBlock, 1);
            grid.Children.Add(messageBlock);

            var inputBox = new TextBox
            {
                Text = defaultValue,
                Style = (Style)FindResource("ModernTextBox"),
                Margin = new Thickness(0, 0, 0, 20)
            };
            inputBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    InputText = inputBox.Text;
                    Result = DialogResult.OK;
                    Close();
                }
                else if (e.Key == Key.Escape)
                {
                    Result = DialogResult.Cancel;
                    Close();
                }
            };
            Grid.SetRow(inputBox, 2);
            grid.Children.Add(inputBox);

            var buttonPanel = new UniformGrid
            {
                Columns = 2,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var cancelButton = new Button
            {
                Content = "Cancel",
                Style = (Style)FindResource("ModernButton"),
                Margin = new Thickness(0, 0, 8, 0)
            };
            cancelButton.Click += (s, e) => { Result = DialogResult.Cancel; Close(); };

            var okButton = new Button
            {
                Content = "OK",
                Style = (Style)FindResource("PrimaryButton"),
                Margin = new Thickness(8, 0, 0, 0)
            };
            okButton.Click += (s, e) =>
            {
                InputText = inputBox.Text;
                Result = DialogResult.OK;
                Close();
            };

            buttonPanel.Children.Add(cancelButton);
            buttonPanel.Children.Add(okButton);

            Grid.SetRow(buttonPanel, 3);
            grid.Children.Add(buttonPanel);

            MainContent.Children.Add(grid);

            inputBox.Focus();
            inputBox.SelectAll();
        }

        private void BuildMessageDialog(string title, string message, DialogType type)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var iconText = "ℹ";
            var iconColor = (Brush)FindResource("Primary");

            switch (type)
            {
                case DialogType.Warning:
                    iconText = "⚠";
                    iconColor = (Brush)FindResource("Warning");
                    break;
                case DialogType.Error:
                    iconText = "✕";
                    iconColor = (Brush)FindResource("Danger");
                    break;
                case DialogType.Success:
                    iconText = "✓";
                    iconColor = (Brush)FindResource("Success");
                    break;
            }

            var iconBlock = new TextBlock
            {
                Text = iconText,
                FontSize = 48,
                FontWeight = FontWeights.Bold,
                Foreground = iconColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 16)
            };
            Grid.SetRow(iconBlock, 0);
            grid.Children.Add(iconBlock);

            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = (Brush)FindResource("TextPrimary"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 12)
            };
            Grid.SetRow(titleBlock, 1);
            grid.Children.Add(titleBlock);

            var messageBlock = new TextBlock
            {
                Text = message,
                FontSize = 14,
                Foreground = (Brush)FindResource("TextSecondary"),
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20),
                LineHeight = 22
            };
            Grid.SetRow(messageBlock, 2);
            grid.Children.Add(messageBlock);

            var okButton = new Button
            {
                Content = "OK",
                Style = type == DialogType.Error ? (Style)FindResource("DangerButton") :
                        type == DialogType.Success ? (Style)FindResource("SuccessButton") :
                        (Style)FindResource("PrimaryButton"),
                MinWidth = 120,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            okButton.Click += (s, e) => { Result = DialogResult.OK; Close(); };

            Grid.SetRow(okButton, 3);
            grid.Children.Add(okButton);

            MainContent.Children.Add(grid);

            okButton.Focus();
        }

        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape)
            {
                Result = DialogResult.Cancel;
                Close();
            }
        }
    }
}