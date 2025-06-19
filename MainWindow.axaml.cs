using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
 using System.Collections.ObjectModel;
using Lightning_Bolt_Kit_Creator.Models;
using System.ComponentModel;
using System.Threading.Tasks;



namespace Lightning_Bolt_Kit_Creator;

public partial class MainWindow : Window
{
    private int TabNum = -1;
    private String romfsPath;
    private String outputPath;
    private int SelectedWeaponClass = 0;
    private int SelectedMainWeapon = 0;
    //For handling data writing
    FileProcessing fileProcessing;
    //Main Window Initialization
    public MainWindow()
    {

        InitializeComponent();
        fileProcessing = new FileProcessing();
        DataContext = this;
        ChangeTab(0);
    }
    public async Task WaitOtherLoaded()
    {
        while (!Debug.GetLoadState())
        {
            await Task.Delay(25);
        }
        return;
    }

    private void LoadComboBoxes()
    {
        WeaponTypeComboBox.ItemsSource = new ObservableCollection<WeaponItem>(WeaponItem.GetAllMainClasses());
        WeaponTypeComboBox.SelectedIndex = 1;
        SubWeaponComboBox.ItemsSource = new ObservableCollection<WeaponItem>(WeaponItem.GetAllSubs());
        SubWeaponComboBox.SelectedIndex = 1;
        SpecialWeaponComboBox.ItemsSource = new ObservableCollection<WeaponItem>(WeaponItem.GetAllSpecials());
        SpecialWeaponComboBox.SelectedIndex = 1;


    }
    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // Your existing debug code...
        TryLoadSavedPaths();

        // Create the collection and debug it
        var weaponItems = WeaponItem.GetAllMainClasses();
        if (Debug.IsDebug()) { WaitOtherLoaded(); }
        Debug.Write($"Created {weaponItems.Count} weapon items");

        foreach (var item in weaponItems)
        {
            Debug.Write($"Weapon: {item.Name}, Icon: {item.Icon}");
        }


        Debug.Write($"Set WeaponTypeComboBox ItemsSource with {weaponItems.Count} items");

        LoadComboBoxes();
        TestResourceLoading();
        if (Debug.IsDebug())
        {
            WeaponTypeComboBox.SelectedIndex = 7;
            MainWeaponComboBox.SelectedIndex = 0;
            WeaponNameTextBox.Text = "Tester";
            WeaponSuffixTextBox.Text = "Test";
            SpecialPointsTextBox.Text = "200";
            WeaponIdTextBox.Text = "69";
        }
    }

    private void TestResourceLoading()
    {
        try
        {
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var uri = new Uri($"avares://{assemblyName}/Icon/Weapon/ClassIcons/Shooter.jpg"); // Fixed case

            Debug.Write($"Trying URI: {uri}");

            using var stream = Avalonia.Platform.AssetLoader.Open(uri);
            Debug.Write("✅ Resource loaded successfully!");
        }
        catch (Exception ex)
        {
            Debug.Write($"❌ Failed to load resource: {ex.Message}");
        }
    }
    private int TryLoadSavedPaths()
    {
        String WorkingDirectory = Directory.GetCurrentDirectory();
        try
        {

            if (!File.Exists(WorkingDirectory + "\\Data\\ConfigDat.txt"))
            {
                if (!Directory.Exists(WorkingDirectory + "\\Data"))
                {
                    Directory.CreateDirectory(WorkingDirectory + "\\Data");
                }
                throw new Exception("Config file does not exist yet");
            }
            String[] lines = File.ReadAllLines(WorkingDirectory + "\\Data\\ConfigDat.txt");
            Debug.Write("Read : " + lines[0]);
            romfsPath = lines[0].Substring(lines[0].IndexOf(":") + 1);
            Debug.Write("Romfs : " + romfsPath);
            RomfsPathTextBox.Text = romfsPath;
            Debug.Write("Read : " + lines[1]);
            outputPath = lines[1].Substring(lines[1].IndexOf(":") + 1);
            Debug.Write("Output : " + outputPath);
            OutputPathTextBox.Text = outputPath;

            return 0;


        }
        catch (Exception ex) { Debug.Write("Config File Read Error: " + ex.ToString()); return 1; }
    }

    //Changes your tab
    public void ChangeTab(int newTab)
    {
        Debug.Write($"ChangeTab called: current={TabNum}, new={newTab}");
        if (TabNum == newTab) { return; }
        TabNum = newTab;
        DirectoryTab.IsVisible = false;
        WeaponInfoTab.IsVisible = false;

        switch (TabNum)
        {
            case 0:
                DirectoryTab.IsVisible = true;
                return;
            case 1:
                WeaponInfoTab.IsVisible = true;
                return;
        }
        WeaponTypeComboBox.SelectedIndex = SelectedWeaponClass;
        MainWeaponComboBox.SelectedIndex = SelectedMainWeapon;
    }
    public void ConfirmPaths(object sender, RoutedEventArgs e) { ConfirmPaths(); }
    public void ConfirmPaths()
    {
        String WorkingDirectory = Directory.GetCurrentDirectory();
        try
        {
            if (!Directory.Exists(WorkingDirectory + "\\Data")) { Directory.CreateDirectory(WorkingDirectory + "\\Data"); }
            if (!File.Exists(WorkingDirectory + "\\Data\\ConfigDat.txt"))
            {
                File.Delete(WorkingDirectory + "\\Data\\ConfigDat.txt");
            }
            if (OutputPathTextBox.Text == null || OutputPathTextBox.Text.Equals(""))
            {
                throw new Exception("Invalid Path: " + OutputPathTextBox.Text);
            }
            if (RomfsPathTextBox.Text == null || RomfsPathTextBox.Text.Equals(""))
            {
                throw new Exception("Invalid Path: " + OutputPathTextBox.Text);
            }
            romfsPath = RomfsPathTextBox.Text;
            if (!romfsPath[romfsPath.Length - 1].Equals('\\')) { romfsPath += "\\"; }
            outputPath = OutputPathTextBox.Text;
            if (!outputPath[outputPath.Length - 1].Equals('\\')) { outputPath += "\\"; }
            File.WriteAllText(WorkingDirectory + "\\Data\\ConfigDat.txt", "Romfs Path :" + romfsPath + "\nOutput Path:" + outputPath);
        }
        catch (Exception ex)
        {
            Debug.Write("Config File Write Error: " + ex.ToString());

        }
        fileProcessing.RomfsPath = romfsPath;
        fileProcessing.WeaponFolderRoot = outputPath;
    }
    //Do when tab buttons are clicked
    public void OnTabClick(object sender, RoutedEventArgs e)
    {
        //Console.WriteLine("=== Button clicked ===");
        //WeaponTypeComboBox.ItemsSource = new ObservableCollection<WeaponItem>(WeaponItem.GetAllMainClasses());
        if (sender is Button button)
        {
            //Console.WriteLine($"Button name: {button.Name}");

            switch (button.Name)
            {
                case "DirectoryTabSelector":
                    //Console.WriteLine("Calling ChangeTab(0)");
                    ChangeTab(0);
                    break;
                case "WeaponInfoTabSelector":
                    //Console.WriteLine("Calling ChangeTab(1)");
                    ChangeTab(1);
                    break;
                case "WeaponIconTabSelector":
                    break;
                case "AddWeapon":
                    ConfirmPaths();
                    fileProcessing.WriteWeaponInfo();
                    break;
                default:
                    Console.WriteLine($"Unknown button: {button.Name}");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Sender is not a Button!");
        }
    }
    //Close debug if it exists
    private void MainWindow_Closing(object sender, WindowClosingEventArgs e)
    {
        fileProcessing.onClose();
        Debug.CloseDebug();
    }

    public WeaponItem? GetSelectedWeaponClass()
    {
        if (WeaponTypeComboBox != null && WeaponTypeComboBox.SelectedItem != null)
        {
            return WeaponTypeComboBox.SelectedItem as WeaponItem;
        }
        return null;
    }
    public WeaponItem? GetSelectedMainWeapon()
    {
        if (MainWeaponComboBox != null && MainWeaponComboBox.SelectedItem != null)
        {
            return MainWeaponComboBox.SelectedItem as WeaponItem;
        }
        return null;
    }
    public String GetSelectedSubName()
    {
        if (SubWeaponComboBox != null && SubWeaponComboBox.SelectedItem != null)
        {
            return (SubWeaponComboBox.SelectedItem as WeaponItem).CodeName;
        }
        return null;
    }

    public String GetSelectedSpecialName()
    {
        if (SpecialWeaponComboBox != null && SpecialWeaponComboBox.SelectedItem != null)
        {
            return (SpecialWeaponComboBox.SelectedItem as WeaponItem).CodeName;
        }
        return null;
    }

    public void ChangeSelectedWeaponClass(object sender, SelectionChangedEventArgs e)
    {
        if (GetSelectedWeaponClass() != null )
        {
            MainWeaponComboBox.ItemsSource = WeaponItem.GetMainWeaponsOfClass(GetSelectedWeaponClass().CodeName);
            SelectedWeaponClass = WeaponTypeComboBox.SelectedIndex;
            SelectedMainWeapon = 0;
            MainWeaponComboBox.SelectedIndex = 0;
        }
    }
    public void ChangeSelectedMainWeapon(object sender, SelectionChangedEventArgs e)
    {
        if (GetSelectedMainWeapon() != null)
        SelectedMainWeapon = MainWeaponComboBox.SelectedIndex;
    }
    public void UseWeaponInfo(object sender, RoutedEventArgs e)
    {
        UseWeaponInfo();
    }
    /*OriginalCodename, Sub, Special;*/
    public void UseWeaponInfo()
    {
        if ( WeaponNameTextBox.Text != null &&
            WeaponSuffixTextBox.Text != null &&
            SpecialPointsTextBox.Text != null &&
            WeaponIdTextBox.Text != null &&
            GetSelectedMainWeapon() != null &&
            GetSelectedSubName() != null &&
            GetSelectedSpecialName() != null &&
            GetSelectedMainWeapon() != null )
        {
            fileProcessing.WeaponName = WeaponNameTextBox.Text;
            fileProcessing.WeaponSuffix = WeaponSuffixTextBox.Text;
            fileProcessing.SpecialPoints = SpecialPointsTextBox.Text;
            fileProcessing.ID = WeaponIdTextBox.Text;
            WeaponItem weapon = GetSelectedMainWeapon();
            fileProcessing.Sub = GetSelectedSubName();
            fileProcessing.Special = GetSelectedSpecialName();
            fileProcessing.OriginalCodename = weapon.Class + "_" + weapon.CodeName;

        }
    }
}