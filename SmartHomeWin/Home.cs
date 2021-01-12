using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartHomeWin
{
    public partial class Home : Form
    {
        public static string hotkeyPath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + @"\tuya\hotkeys.json";
        public static string startupPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Startup) + @"\SmartHome.lnk";

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Keys vKey);
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly List<HotKeyBinding> hotkeyBinding;

        private Hotkey hotkey;
        private bool listening = false;

        public Home()
        {
            InitializeComponent();

            Program.tuya = new TuyaApi.TuyaApi();

            menuLogin.Text = Program.tuya.IsAuthenticated() ? "Logout" : "Login";
            menuAutoStart.Checked = File.Exists(startupPath);

            try
            {
                hotkeyBinding = JsonConvert.DeserializeObject<List<HotKeyBinding>>(File.ReadAllText(hotkeyPath));
            }
            catch (Exception)
            {
                hotkeyBinding = new List<HotKeyBinding>();
            }

            RegisterHotKey();
        }

        private void RegisterHotKey()
        {
            for (int i = 0; i < hotkeyBinding.Count; i++)
            {
                UnregisterHotKey(this.Handle, i);
                RegisterHotKey(this.Handle, i, (int)hotkeyBinding[i].Hotkey.Kmod1 + (int)hotkeyBinding[i].Hotkey.Kmod2, Convert.ToInt32(hotkeyBinding[i].Hotkey.Key));
            }

            if (!Directory.Exists(Path.GetDirectoryName(hotkeyPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(hotkeyPath));

            File.WriteAllText(hotkeyPath, JsonConvert.SerializeObject(hotkeyBinding));
        }

        private void Home_Load(object sender, EventArgs e)
        {
            RefreshDevicesList();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                int id = m.WParam.ToInt32();

                Debug.WriteLine($"id: {id}");
                // Debug.WriteLine($"modifier : {modifier}");
                // Debug.WriteLine($"key : {key}");


                var dev = Program.tuya.DevicesList.Where(dev => dev.Id == hotkeyBinding[id].DevId).FirstOrDefault();

                int state = dev.Dev_type == "scene" ? 0 : Convert.ToInt32(dev.Data.State);
                Program.tuya.ControlDevicesAsync(hotkeyBinding[id].DevId, state ^ 1);

                dev.Data.State = Convert.ToBoolean(state ^ 1);
                RefreshDevicesList();
                Program.tuya.SaveDeviceData();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDevicesList(true);
        }

        private async void RefreshDevicesList(bool hardRefresh = false)
        {
            btnOnOff.Text = "ON/OFF";
            btnOnOff.Enabled = false;
            btnBindHotKey.Enabled = false;
            btnUnbindHotkey.Enabled = false;

            if (hardRefresh)
                await Program.tuya.DiscoverDevices();

            deviceListView.Items.Clear();

            foreach (var dev in Program.tuya.DevicesList)
            {
                if (dev.Dev_type == "switch" || true)
                {
                    ListViewItem device = new ListViewItem(dev.Name, 0);

                    device.SubItems.Add(dev.Id);
                    device.SubItems.Add(dev.Dev_type);
                    device.SubItems.Add(Convert.ToInt32(dev.Data.State).ToString());
                    device.SubItems.Add(Convert.ToInt32(dev.Data.Online).ToString());

                    var selectDeviceHK = hotkeyBinding.Where(hk => hk.DevId == dev.Id).FirstOrDefault();

                    if (selectDeviceHK != null)
                        device.SubItems.Add($"{selectDeviceHK.Hotkey.Kmod1} + {selectDeviceHK.Hotkey.Kmod2} + {Enum.GetName(typeof(Keys), selectDeviceHK.Hotkey.Key)}");
                    else
                        device.SubItems.Add("");

                    deviceListView.Items.AddRange(new ListViewItem[] { device });
                }
            }

            if (hotkeyBinding.Count > 0)
            {
                btnUnbindKeys.Enabled = true;
            }
        }

        private void DeviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceListView.SelectedItems.Count > 0)
            {
                if (deviceListView.SelectedItems[0].SubItems[3].Text == "1")
                    btnOnOff.Text = deviceListView.SelectedItems[0].SubItems[2].Text == "scene" ? "TRIGGER" : "OFF";
                else
                    btnOnOff.Text = deviceListView.SelectedItems[0].SubItems[2].Text == "scene" ? "TRIGGER" : "ON";
                btnOnOff.Enabled = true;
                btnBindHotKey.Enabled = true;
                var selectDeviceHK = hotkeyBinding.Where(hk => hk.DevId == deviceListView.SelectedItems[0].SubItems[1].Text).FirstOrDefault();
                btnUnbindHotkey.Enabled = selectDeviceHK != null;
            }
            else
            {
                btnOnOff.Text = "ON/OFF";
                btnOnOff.Enabled = false;
                btnBindHotKey.Enabled = false;
                btnUnbindHotkey.Enabled = false;
            }
            if (listening)
                DoneBinding();
        }

        private void BtnOnOff_Click(object sender, EventArgs e)
        {
            btnOnOff.Text = "ON/OFF";
            btnOnOff.Enabled = false;

            string devId = deviceListView.SelectedItems[0].SubItems[1].Text;
            int state = deviceListView.SelectedItems[0].SubItems[2].Text == "scene" ? 0 : Convert.ToInt32(deviceListView.SelectedItems[0].SubItems[3].Text);

            Program.tuya.ControlDevicesAsync(devId, state ^ 1);
            var dev = Program.tuya.DevicesList.Where(dev => dev.Id == devId).FirstOrDefault();
            dev.Data.State = Convert.ToBoolean(state ^ 1);
            RefreshDevicesList();

            Program.tuya.SaveDeviceData();
        }

        private void BtnBindHotKey_Click(object sender, EventArgs e)
        {
            hotkey = new Hotkey();
            listening = true;
            btnBindHotKey.Text = "Enter hotkey";
        }

        private void BtnBindHotKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (listening)
            {
                if (hotkey.Kmod1.ToString() == "None")
                {
                    if (Enum.IsDefined(typeof(KeyModifier), e.KeyCode.ToString()))
                    {
                        hotkey.Kmod1 = (KeyModifier)Enum.Parse(typeof(KeyModifier), e.KeyCode.ToString());
                        deviceListView.SelectedItems[0].SubItems[5].Text = e.KeyCode.ToString() + " + ";
                        Debug.WriteLine("kmod1");
                    }
                }
                else if (hotkey.Kmod1.ToString() != "None" && hotkey.Kmod2.ToString() == "None" && hotkey.Kmod1.ToString() != e.KeyCode.ToString())
                {
                    if (Enum.IsDefined(typeof(KeyModifier), e.KeyCode.ToString()))
                    {
                        hotkey.Kmod2 = (KeyModifier)Enum.Parse(typeof(KeyModifier), e.KeyCode.ToString());
                        deviceListView.SelectedItems[0].SubItems[5].Text += e.KeyCode.ToString() + " + ";
                        Debug.WriteLine("kmod2");
                    }
                    else if (hotkey.Key == 0)
                    {
                        hotkey.Key = e.KeyValue;
                        deviceListView.SelectedItems[0].SubItems[5].Text += e.KeyCode.ToString();
                        Debug.WriteLine("2nd key");
                        DoneBinding();
                    }
                }
                else if (hotkey.Key == 0 && hotkey.Kmod1.ToString() != e.KeyCode.ToString() && hotkey.Kmod2.ToString() != e.KeyCode.ToString())
                {
                    hotkey.Key = e.KeyValue;
                    deviceListView.SelectedItems[0].SubItems[5].Text += e.KeyCode.ToString();
                    Debug.WriteLine("3rd key");
                    DoneBinding();
                }
            }
        }

        private void DoneBinding()
        {
            if (hotkey.Key != 0)
            {
                Debug.Write("key binding for");
                Debug.WriteLine(deviceListView.SelectedItems[0].SubItems[1].Text);

                var selectDeviceHK = hotkeyBinding.Where(hk => hk.DevId == deviceListView.SelectedItems[0].SubItems[1].Text).FirstOrDefault();
                if (selectDeviceHK != null)
                {
                    selectDeviceHK.Hotkey = hotkey;
                }
                else
                {
                    hotkeyBinding.Add(
                       new HotKeyBinding
                       {
                           DevId = deviceListView.SelectedItems[0].SubItems[1].Text,
                           Hotkey = hotkey
                       });
                }
                RegisterHotKey();
            }
            else
            {
                Debug.WriteLine("key binding rejected");
            }
            listening = false;
            btnBindHotKey.Text = "Bind Hotkey";
        }

        private void Home_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void MenuLogin_Click(object sender, EventArgs e)
        {
            if (Program.tuya.IsAuthenticated())
            {
                Program.tuya.Logout();
                deviceListView.Items.Clear();
                menuLogin.Text = "Login";
                UnregisterAllHotkeys();
            }
            else
            {
                var result = new LoginForm().ShowDialog();
                if (result == DialogResult.OK)
                {
                    RefreshDevicesList(true);
                    menuLogin.Text = "Logout";
                }
            }
        }
        private void UnregisterAllHotkeys()
        {
            for (int i = 0; i < hotkeyBinding.Count; i++)
            {
                UnregisterHotKey(this.Handle, i);
            }
            hotkeyBinding.Clear();
            File.Delete(hotkeyPath);
            RefreshDevicesList();
            btnUnbindKeys.Enabled = false;
        }

        private void BtnUnbindHotkey_Click(object sender, EventArgs e)
        {
            var selectDeviceHK = hotkeyBinding.Where(hk => hk.DevId == deviceListView.SelectedItems[0].SubItems[1].Text).FirstOrDefault();
            if(selectDeviceHK != null)
            {
                int index = hotkeyBinding.IndexOf(selectDeviceHK);

                Debug.WriteLine("index: " + index);

                for (int i = 0; i < hotkeyBinding.Count; i++)
                {
                    UnregisterHotKey(this.Handle, i);
                }
                hotkeyBinding.Remove(selectDeviceHK);
                RegisterHotKey();
                RefreshDevicesList();
            }
        }

        private void BtnUnbindKeys_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to remove all hotkeys?", "Unbinding Hotkeys", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                UnregisterAllHotkeys();
            }
        }

        private bool minimize = true;

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (minimize)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            minimize = false;
            Close();
        }

        private void CreateShortcut(string shortcutPath)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            string appExePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".exe");
            shortcut.TargetPath = appExePath;
            shortcut.Save();

        }

        private void MenuAutoStart_Click(object sender, EventArgs e)
        {
            if (menuAutoStart.Checked == false)
            {
                CreateShortcut(startupPath);
                menuAutoStart.Checked = true;
            }
            else
            {
                File.Delete(startupPath);
                menuAutoStart.Checked = false;
            }
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tuya smart home app for PC by mgkha.");
        }
            
    }

    public class HotKeyBinding
    {
        public string DevId { get; set; }
        public Hotkey Hotkey { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }

    public class Hotkey
    {
        public KeyModifier Kmod1 { get; set; }
        public KeyModifier Kmod2 { get; set; }
        public int Key { get; set; }
    }

    public enum KeyModifier
    {
        None = 0,
        Menu = 1,
        ControlKey = 2,
        ShiftKey = 4,
        LWin = 8
    }
}
