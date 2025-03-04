using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using WinRT.Interop;
using Windows.Graphics;
using Microsoft.UI.Windowing;
using System.Runtime.CompilerServices;
using System;
using System.Text;
using Windows.Graphics.Imaging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Windows.Media.Protection.PlayReady;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Activation;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace frontend
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private TextBox inputCase;

        private Button butEnter;
        private Button butNew;
        private Button butSave;
        private Button butLoad;
        private Button butEnd;
        //private Button butView;

        private string defaultTxt_inputCase = "Write what ya next action should be.";

        //public TextBox logCase;
        public RichTextBlock logRichCase;
        public ScrollViewer logScrollViewer;

        private TextBlock minimapTextBlock;
        //full app are 1280x720 0.2x menu, 0.5x input, 0.3x visual

        private int logBox_Width = 1280 / 2 - 5;
        private int logBox_Height = 720 - 80;

        private int input_Width = 1280 / 2;
        private int input_Height = 40;

        private int butMenu_Width = 1280 / 5 - 10;
        private int butMenu_Height = 60;

        private int menu_Width = 1280 / 5;
        private int menu_Height = 720;

        private int visual_Width = 1280 * 3 / 10;
        private int visual_Height = 720;

        private int inventory_Square_Width = 50;
        private int inventory_Square_Height = 50;
        private int inventoryGrid_Width = 240;
        private int inventoryGrid_Height = 120;

        private ProgressBar healthBar;
        private ProgressBar manaBar;
        private ProgressBar staminaBar;

        //game
        public int mapSize = 15;
        public Player player;
        private Game game;
        private Grid minimapGrid;

        //server
        private bool Loading = false;

        //iteme path pt inventar
        private string imagePath_banana = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/banana.jpg";
        private string imagePath_staff = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/staff1.jpg";
        private string imagePath_sword = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/sword1.jpg";
        private string imagePath_bow = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/bow1.jpg";
        private string imagePath_manaPot = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/manaPot.jpg";
        private string imagePath_healthPot = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/healthPot.jpg";

        private string imagePath_itemUsed = "file:///F:/Proiect-PIU/piupiu/client/frontend/frontend (Package)/Images/banana.jpg";

        private Microsoft.UI.Xaml.Controls.Image usedItemImage;


        public MainWindow()
        {
            //this.InitializeComponent();

            initializeMWindow();
            centerWindow();
            player = new Player(mapSize);
            game = new Game(mapSize, player);

            game.MinimapUpdated += UpdateMinimapText;

            inputCase = new TextBox
            {
                Width = input_Width - 70,
                Height = input_Height,
                Text = defaultTxt_inputCase,
            };

            StackPanel stackPanel_inputBox = new StackPanel
            {
                Width = input_Width,
                Height = input_Height,
                Orientation = Orientation.Horizontal,
            };

            StackPanel stackPanel_input = new StackPanel
            {
                Width = logBox_Width,
                Height = logBox_Height + input_Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            StackPanel stackPanel_meniu = new StackPanel
            {
                Width = menu_Width,
                Height = menu_Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 20,
            };

            StackPanel stackPanel_visual = new StackPanel
            {

                Width = visual_Width,
                Height = visual_Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            logRichCase = new RichTextBlock
            {
                Width = logBox_Width,

                TextWrapping = TextWrapping.Wrap,

                UseSystemFocusVisuals = false,
                FocusVisualPrimaryBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
                FocusVisualSecondaryBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent),

                Padding = new Thickness(15, 0, 0, 50)
            };

            logScrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = logRichCase,
                Height = logBox_Height,
                Width = logBox_Width,
            };


            TextBlock menuTitle = new TextBlock
            {
                Text = "Legenda Bibanului",
                FontSize = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10, 40, 10, 0),
            };


            butEnter = new Button
            {
                Content = "Enter",
                Width = 70,
                Height = input_Height,
            };

            butNew = new Button
            {
                Content = "New",
                Width = butMenu_Width,
                Height = butMenu_Height,
                Margin = new Thickness(10, 0, 0, 0),
            };

            butLoad = new Button
            {
                Content = "Load",
                Width = butMenu_Width,
                Height = butMenu_Height,
                Margin = new Thickness(10, 0, 0, 0),
            };

            butSave = new Button
            {
                Content = "Save",
                Width = butMenu_Width,
                Height = butMenu_Height,
                Margin = new Thickness(10, 0, 0, 0),
            };


            butEnd = new Button
            {
                Content = "End",
                Width = butMenu_Width,
                Height = butMenu_Height,
                Margin = new Thickness(10, 0, 0, 0),
            };

            TextBlock legenda = new TextBlock
            {
                Text = "P - Player\nG - Goblin\nM - Mountain\nW - Water\n. - Empty Space",
                FontSize = 15,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 170, 10, 0),
            };


            /* minimapTextBlock = new TextBlock()
                        {
                            FontSize = 10,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            Margin = new Thickness(5),
                        };*/

            //___________________Minimap___________________

            minimapGrid = new Grid
            {
                Width = visual_Width,
                Height = visual_Height * 0.4,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            for (int i = 0; i < mapSize; i++)
            {
                minimapGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                minimapGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            string minimapContent = game.GenerateMinimapContent();
            PopulateMinimapGrid(minimapContent);


            Grid visualGrid = new Grid();

            visualGrid.Width = visual_Width;
            visualGrid.Height = visual_Height;

            visualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            visualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });

            visualGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            visualGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            //minimapTextBlock.Text = game.GenerateMinimapContent();

            Grid.SetRow(minimapGrid, 0);
            Grid.SetColumnSpan(minimapGrid, 2);
            visualGrid.Children.Add(minimapGrid);

            //___________________Inventar___________________

            Grid inventoryGrid = new Grid
            {
                Width = inventoryGrid_Width,
                Height = inventoryGrid_Height,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            for (int i = 0; i < 2; ++i)
            {
                inventoryGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            }
            for (int i = 0; i < 4; ++i)
            {
                inventoryGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            }

            for (int i = 0; i < 8; i++)
            {
                var square = new Border
                {
                    BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Green),
                    BorderThickness = new Thickness(2),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Height = inventory_Square_Height,
                    Width = inventory_Square_Width,
                    Margin = new Thickness(5),
                };

                var itemImage_banana = new Microsoft.UI.Xaml.Controls.Image
                {
                    Source = new BitmapImage(new Uri(imagePath_banana, UriKind.Absolute)),
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                var itemImage_Used = new Microsoft.UI.Xaml.Controls.Image
                {
                    Source = new BitmapImage(new Uri(imagePath_itemUsed, UriKind.Absolute)),
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                if (i == 1)
                {
                    usedItemImage = itemImage_Used;
                }

                var itemImage_manaPot = new Microsoft.UI.Xaml.Controls.Image
                {
                    Source = new BitmapImage(new Uri(imagePath_manaPot, UriKind.Absolute)),
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                var itemImage_healthPot = new Microsoft.UI.Xaml.Controls.Image
                {
                    Source = new BitmapImage(new Uri(imagePath_healthPot, UriKind.Absolute)),
                    Stretch = Stretch.Uniform,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                switch (i)
                {
                    case 0: 
                        square.Child = itemImage_banana;
                        break;

                    case 1:
                        square.Child = itemImage_Used;
                        break;

                    case 2:
                        square.Child = itemImage_healthPot;
                        break;

                    case 3:
                        square.Child = itemImage_manaPot;
                        break;

                    case 4:

                        break;

                    case 5:

                        break;

                    case 6:

                        break;

                    case 7:

                        break;

                    default: break;
                }
                

                Grid.SetRow(square, i / 4);
                Grid.SetColumn(square, i % 4);
                inventoryGrid.Children.Add(square);
            }

            Grid.SetRow(inventoryGrid, 1);
            Grid.SetColumnSpan(inventoryGrid, 2);
            visualGrid.Children.Add(inventoryGrid);

            //___________________Statusbars___________________

            healthBar = new ProgressBar
            {
                Width = 200,
                Height = 20,
                Maximum = 150,
                Value = player.Health,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red),
                Margin = new Thickness(5)
            };

            manaBar = new ProgressBar
            {
                Width = 200,
                Height = 20,
                Maximum = 100,
                Value = player.Mana,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Blue),
                Margin = new Thickness(5)
            };

            staminaBar = new ProgressBar
            {
                Width = 200,
                Height = 20,
                Maximum = player.MaxMoves,
                Value = player.RemainingMoves,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green),
                Margin = new Thickness(5)
            };

            TextBlock healthLabel = new TextBlock
            {
                Text = "Health",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0)
            };

            TextBlock manaLabel = new TextBlock
            {
                Text = "Mana",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0)
            };

            TextBlock staminaLabel = new TextBlock
            {
                Text = "Stamina",
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 0)
            };

            var statusBarPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10),
                Spacing = 0,
            };

            statusBarPanel.Children.Add(healthLabel);
            statusBarPanel.Children.Add(healthBar);
            statusBarPanel.Children.Add(manaLabel);
            statusBarPanel.Children.Add(manaBar);
            statusBarPanel.Children.Add(staminaLabel);
            statusBarPanel.Children.Add(staminaBar);

            Grid.SetRow(statusBarPanel, 2);
            Grid.SetColumnSpan(statusBarPanel, 2);
            visualGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            visualGrid.Children.Add(statusBarPanel);


            //________________________________________________

            butEnter.Click += butEnter_Click;
            inputCase.KeyDown += inputTextBox_KeyDown;
            inputCase.GotFocus += inputCase_GotFocus;
            butEnd.Click += butEnd_Click;
            butNew.Click += butNew_Click;
            butSave.Click += butSave_Click;
            butLoad.Click += butLoad_Click;


            stackPanel_inputBox.Children.Add(inputCase);
            stackPanel_inputBox.Children.Add(butEnter);

            stackPanel_input.Children.Add(logScrollViewer);
            stackPanel_input.Children.Add(stackPanel_inputBox);
            //stackPanel_input.Children.Add(inputCase);
            //stackPanel_input.Children.Add(butEnter);

            stackPanel_meniu.Children.Add(menuTitle);
            stackPanel_meniu.Children.Add(butNew);
            stackPanel_meniu.Children.Add(butLoad);
            stackPanel_meniu.Children.Add(butSave);
            /*stackPanel_meniu.Children.Add(butView);*/
            stackPanel_meniu.Children.Add(butEnd);
            stackPanel_meniu.Children.Add(legenda);

            //stackPanel_visual.Children.Add(butFalci);
            stackPanel_visual.Children.Add(visualGrid);

            Grid mainGrid = new Grid();

            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            mainGrid.Children.Add(stackPanel_meniu);
            mainGrid.Children.Add(stackPanel_input);
            mainGrid.Children.Add(stackPanel_visual);

            Grid.SetColumn(stackPanel_meniu, 0);
            Grid.SetColumn(stackPanel_input, 1);
            Grid.SetColumn(stackPanel_visual, 2);

            this.Content = mainGrid;

        }

        public class ResponseWrapper
        {
            [JsonPropertyName("response")]
            public Response Response { get; set; }

            [JsonPropertyName("image")]
            public string? Image { get; set; }
        }

        public class Response
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("data")]
            public Action Data { get; set; }
        }

        [JsonConverter(typeof(ActionConverter))]
        public abstract class Action
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

        public class WalkAction : Action
        {
            [JsonPropertyName("direction")]
            public string Direction { get; set; }
        }

        public class AttackAction : Action
        {
            [JsonPropertyName("itemsUsed")]
            public List<string> ItemsUsed { get; set; }

            [JsonPropertyName("target")]
            public string Target { get; set; }
        }

        public class SleepAction : Action
        { 
        }

        public class SpellAction : Action
        {
            [JsonPropertyName("target")]
            public string Target { get; set; }
        }

        public class ActionConverter : JsonConverter<Action>
        {
            public override Action Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using JsonDocument doc = JsonDocument.ParseValue(ref reader);
                JsonElement root = doc.RootElement;

                string type = root.GetProperty("type").GetString();
                return type switch
                {
                    "walk" => JsonSerializer.Deserialize<WalkAction>(root.GetRawText(), options),
                    "attack" => JsonSerializer.Deserialize<AttackAction>(root.GetRawText(), options),
                    "sleep" => JsonSerializer.Deserialize<SleepAction>(root.GetRawText(), options),
                    "spell" => JsonSerializer.Deserialize<SpellAction>(root.GetRawText(), options),
                    _ => throw new NotSupportedException($"Action type '{type}' is not supported")
                };
            }

            public override void Write(Utf8JsonWriter writer, Action value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }

        private async Task sendToServer()
        {
            Loading = true;
            string url = "http://localhost:8000/prompt";
            string jsonBody = $$"""{"query":"{{inputCase.Text}}", "context":"Position: Up: mountains; Down: water; Left: empty; Right: goblin;"}""";


            Debug.WriteLine(jsonBody);

            using HttpClient client = new();
            try
            {
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                string responseContent = await response.Content.ReadAsStringAsync();
                //Debug.WriteLine($"Response: {response.StatusCode}");
                //Debug.WriteLine(responseContent);

                //AddLogText("Gamemaster(debug): " + responseContent);

                ResponseWrapper wrappedResponse = JsonSerializer.Deserialize<ResponseWrapper>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new ActionConverter() }
                });

                Action action = wrappedResponse.Response.Data;

                if (wrappedResponse.Image != null)
                {
                    AddLogImage(wrappedResponse.Image);
                }

                int hpBeforeMove = player.Health;
                int damage;
                if (action is WalkAction walkAction)
                {
                    switch (walkAction.Direction)
                    {
                        case "left":
                            if (player.Move(-1, 0, mapSize, game) == 1)
                            {
                                damage = hpBeforeMove - player.Health;
                                AddLogText($"Gamemaster: You stepped into a Goblin! It attacked you for {damage} ");
                            }
                            break;
                        case "right":
                            if (player.Move(1, 0, mapSize, game) == 1)
                            {
                                damage = hpBeforeMove - player.Health;
                                AddLogText($"Gamemaster: You stepped into a Goblin! It attacked you for {damage} ");
                            }
                            break;
                        case "up":
                            if (player.Move(0, -1, mapSize, game) == 1)
                            {
                                damage = hpBeforeMove - player.Health;
                                AddLogText($"Gamemaster: You stepped into a Goblin! It attacked you for {damage} ");
                            }
                            break;
                        case "down":
                            if (player.Move(0, 1, mapSize, game) == 1)
                            {
                                damage = hpBeforeMove - player.Health;
                                AddLogText($"Gamemaster: You stepped into a Goblin! It attacked you for {damage} ");
                            }
                            break;
                    }

                    PopulateMinimapGrid(game.GenerateMinimapContent());

                    AddLogText("Gamemaster: Moved to: " + walkAction.Direction);
                }
                else if (action is AttackAction attackAction)
                {
                    //logCase.Text += "Gamemaster: attacked " + attackAction.Target + " with items: " + string.Join(", ", attackAction.ItemsUsed) + Environment.NewLine + Environment.NewLine;

                    int[,] directions = new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };
                    bool attacked = false;


                    //logCase.Text += $"Player position:({player.posX}, {player.posY})" + Environment.NewLine;

                    for (int i = 0; i < directions.GetLength(0); ++i)
                    {
                        int targetX = player.posX + directions[i, 0];
                        int targetY = player.posY + directions[i, 1];

                        //AddLogText($"Checking position ({targetX}, {targetY}) for Goblin...");

                        Monster goblin = game.CheckForMonster(targetY, targetX);
                        if (goblin != null)
                        {
                            goblin.Health -= player.Att;
                            attacked = true;

                            AddLogText($"Gamemaster: You attacked a Goblin at ({targetX}, {targetY}) for {player.Att} damage! ");
                            if (goblin.Health > 0)
                            {
                                AddLogText($"The Goblin has {goblin.Health} HP left.");

                                int goblinDamage = goblin.Attack();
                                player.TakeDamage(goblinDamage);
                                AddLogText($"The Goblin attacked you back for {goblinDamage} damage! Your health is now {player.Health}.");
                            }
                            else
                            {
                                AddLogText("The Goblin has been defeated!");
                                game.Monsters.Remove(goblin);

                                player.IncreaseKillCount();

                                AddLogText($"You have killed {player.killCount} Goblins.");
                                PopulateMinimapGrid(game.GenerateMinimapContent());

                                break;
                            }
                        }
                    }

                    if (!attacked)
                    {
                        AddLogText("Gamemaster: No Goblins in proximity to attack!");
                    }
                }
                else if (action is SleepAction sleepAction)
                {
                    AddLogText($"Gamemaster: You have regained your Stamina ({player.MaxMoves} maximum moves).");
                    player.Sleep();
                }
                else if (action is SpellAction spellAction)
                {
                    //logCase.Text += "Gamemaster: attacked " + attackAction.Target + " with items: " + string.Join(", ", attackAction.ItemsUsed) + Environment.NewLine + Environment.NewLine;

                    int[,] directions = new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 } };
                    bool attacked = false;


                    //logCase.Text += $"Player position:({player.posX}, {player.posY})" + Environment.NewLine;

                    for (int i = 0; i < directions.GetLength(0); ++i)
                    {
                        int targetX = player.posX + directions[i, 0];
                        int targetY = player.posY + directions[i, 1];

                        //AddLogText($"Checking position ({targetX}, {targetY}) for Goblin...");

                        Monster goblin = game.CheckForMonster(targetY, targetX);
                        if (goblin != null)
                        {
                            goblin.Health -= player.spellAtt;
                            attacked = true;

                            AddLogText($"Gamemaster: You bursted a Goblin with a spell at ({targetX}, {targetY}) for {player.spellAtt} damage! ");
                            if (goblin.Health > 0)
                            {
                                AddLogText($"The Goblin has {goblin.Health} HP left.");

                                int goblinDamage = goblin.Attack();
                                player.TakeDamage(goblinDamage);
                                AddLogText($"The Goblin attacked you back for {goblinDamage} damage! Your health is now {player.Health}.");
                            }
                            else
                            {
                                AddLogText("The Goblin has been defeated!");
                                game.Monsters.Remove(goblin);

                                player.IncreaseKillCount();

                                AddLogText($"You have killed {player.killCount} Goblins.");
                                PopulateMinimapGrid(game.GenerateMinimapContent());

                                break;
                            }
                        }
                    }

                    if (!attacked)
                    {
                        AddLogText("Gamemaster: No Goblins in proximity to attack!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                Loading = false;
            }
        }

        private void LogAllGoblinsPositions()
        {
            foreach (var goblin in game.Monsters)
            {
                AddLogText($"Goblin at position ({goblin.posX}, {goblin.posY})");
            }
        }

        private async void butEnter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputCase.Text))
            {
                await ShowAlertAsync("No Empty Prompts!", "Cannot send an empty prompt!");
                return;
            }
            _ = sendToServer();

            updateInputCase();
        }
        private void butEnd_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.UI.Xaml.Application.Current.Exit();
        }
        private async void inputTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (Loading)
                {
                    return;
                }

                try
                {
                    if (string.IsNullOrWhiteSpace(inputCase.Text))
                    {
                        await ShowAlertAsync("No Empty Prompts!", "Cannot send an empty prompt.");
                        return;
                    }

                    sendToServer();
                    updateInputCase();
                }
                finally
                {
                }
            }
        }

        private void updateInputCase()
        {
            AddLogText("Player: " + inputCase.Text);
            inputCase.Text = "";
        }

        private void inputCase_GotFocus(object sender, RoutedEventArgs e)
        {
            inputCase.SelectAll();
        }

        private void initializeMWindow()
        {
            var appWindow = this.AppWindow;
            this.Title = "Legenda Bibanului";
            appWindow.Resize(new SizeInt32(1280, 720));
        }

        private void centerWindow()
        {
            var appWindow = this.AppWindow;

            var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);

            if (displayArea != null)
            {
                var workArea = displayArea.WorkArea;

                int screenWidth = workArea.Width;
                int screenHeight = workArea.Height;

                appWindow.Move(new PointInt32((screenWidth - 1280) / 2, (screenHeight - 720) / 2));
            }
        }

        private void UpdateMinimapText(string newMinimapContent)
        {
            PopulateMinimapGrid(newMinimapContent);
        }

        private void PopulateMinimapGrid(string minimapContent)
        {
            minimapGrid.Children.Clear();

            string[] rows = minimapContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < rows[row].Length; col++)
                {
                    char terrainChar = rows[row][col];


                    var cell = new TextBlock
                    {
                        Text = terrainChar.ToString(),
                        FontSize = 15,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0)
                    };

                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);
                    minimapGrid.Children.Add(cell);
                }
            }

        }

        private async Task ShowAlertAsync(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot,
            };

            await dialog.ShowAsync();
        }

        private async void butNew_Click(object sender, RoutedEventArgs e)
        {
            player = new Player(mapSize);

            player.SetPosX(mapSize/2);
            player.SetPosY(mapSize/2);

            game = new Game(mapSize, player);

            string selectedClass = await ShowClassSelectionDialogAsync();
            switch (selectedClass)
            {
                case "Mage":
                    player.SetHealth(80);
                    player.SetMana(100);
                    player.SetAtt(15);
                    player.SetSpellAtt(35);
                    player.SetClass("Mage");

                    imagePath_itemUsed = imagePath_staff;

                    usedItemImage.Source = new BitmapImage(new Uri(imagePath_staff, UriKind.Absolute));
                    break;

                case "Archer":
                    player.SetHealth(120);
                    player.SetMana(80);
                    player.SetAtt(100);
                    player.SetSpellAtt(20);
                    player.SetClass("Archer");

                    imagePath_itemUsed = imagePath_bow;

                    usedItemImage.Source = new BitmapImage(new Uri(imagePath_bow, UriKind.Absolute));
                    break;

                case "Warrior":
                    player.SetHealth(150);
                    player.SetMana(50);
                    player.SetAtt(40);
                    player.SetSpellAtt(15);
                    player.SetClass("Warrior");

                    imagePath_itemUsed = imagePath_sword;

                    usedItemImage.Source = new BitmapImage(new Uri(imagePath_sword, UriKind.Absolute));
                    break;
            }

            logRichCase.Blocks.Clear();

            AddLogText($"Gamemaster: You are now a(n) {selectedClass}.");

            PopulateMinimapGrid(game.GenerateMinimapContent());
        }

        private async Task<string> ShowClassSelectionDialogAsync()
        {
            var tcs = new TaskCompletionSource<string>();

            var dialog = new ContentDialog
            {
                Title = "Select Your Class",
                CloseButtonText = "Cancel",
                XamlRoot = this.Content.XamlRoot
            };

            var panel = new StackPanel();

            Button mageButton = new Button { Content = "Mage", Margin = new Thickness(5) };
            Button archerButton = new Button { Content = "Archer", Margin = new Thickness(5) };
            Button warriorButton = new Button { Content = "Warrior", Margin = new Thickness(5) };

            mageButton.Click += (_, __) =>
            {
                tcs.TrySetResult("Mage");
                dialog.Hide();
            };

            archerButton.Click += (_, __) =>
            {
                tcs.TrySetResult("Archer");
                dialog.Hide();
            };

            warriorButton.Click += (_, __) =>
            {
                tcs.TrySetResult("Warrior");
                dialog.Hide();
            };

            panel.Children.Add(mageButton);
            panel.Children.Add(archerButton);
            panel.Children.Add(warriorButton);

            dialog.Content = panel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                tcs.TrySetResult("None");
            }

            return await tcs.Task;
        }

        private void butSave_Click(object sender, RoutedEventArgs e)
        {
            var saveData = new SaveData
            {
                Player = player,
                Log = ""
            };

            string saveJson = JsonSerializer.Serialize(saveData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText("savegame.json", saveJson);
            AddLogText("Gamemaster: Game saved successfully.");
        }


        private void butLoad_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("savegame.json"))
            {
                string saveJson = File.ReadAllText("savegame.json");
                var saveData = JsonSerializer.Deserialize<SaveData>(saveJson);

                player = saveData.Player;
                AddLogText(saveData.Log);

                game = new Game(mapSize, player);
                PopulateMinimapGrid(game.GenerateMinimapContent());

                AddLogText("Gamemaster: Game loaded successfully.");
            }
            else
            {
                AddLogText("Gamemaster: No save file found.");
            }
        }
 
        private void AddLogText(string text)
        {
            var p = new Paragraph { Margin = new Thickness(0, 0, 0, 10) };
            p.Inlines.Add(new Run { Text = text });
            logRichCase.Blocks.Add(p);

            logScrollViewer.ChangeView(null, 1000 * logScrollViewer.ScrollableHeight, null);
        }

        private void AddLogImage(string base64Image)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream.AsRandomAccessStream());

                var paragraphWithImage = new Paragraph();
                var inlineUIContainer = new InlineUIContainer();

                var image = new Microsoft.UI.Xaml.Controls.Image
                {
                    Source = bitmapImage,
                    Width = 512,
                    Height = 512,
                    Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 10)
                };

                inlineUIContainer.Child = image;
                paragraphWithImage.Inlines.Add(inlineUIContainer);
                logRichCase.Blocks.Add(paragraphWithImage);
            }

            logScrollViewer.ChangeView(null, 1000 * logScrollViewer.ScrollableHeight, null);
        }

    }
}

//TODO:
/*
 * Schimb nume la app V
 * Pus poza la app X~
 * Schimbat putin dimensiune titlu V
 * Sa pun legenda pe menu V
 * Sa pot sa fac un move din consola V
 * Autoscroll pe textboxu principal
 */