using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SmartHomeWin
{
    public partial class Home : Form
    {
        private readonly TuyaApi.TuyaApi tuya;

        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Keys vKey);
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public List<HotKeyBinding> hotkeyBinding;

        public Home()
        {
            InitializeComponent();

            // RegisterHotKey(this.Handle, 0, (int)KeyModifier.Control + (int)KeyModifier.Alt, (int)Keys.NumPad1);
            // RegisterHotKey(this.Handle, 1, (int)KeyModifier.Control + (int)KeyModifier.Alt, (int)Keys.NumPad2);
            // RegisterHotKey(this.Handle, 2, (int)KeyModifier.Control + (int)KeyModifier.Alt, (int)Keys.NumPad3);

            tuya = new TuyaApi.TuyaApi();

            hotkeyBinding = new List<HotKeyBinding>
            {
                new HotKeyBinding
                {
                    DevId = "38022865c82b96e4ff6e_1",
                    Hotkey = new Hotkey
                    {
                        Kmod1 = KeyModifier.ControlKey,
                        Kmod2 = KeyModifier.Menu,
                        Key = 99,
                    }
                },
                new HotKeyBinding
                {
                    DevId = "38022865c82b96e4ff6e_2",
                    Hotkey = new Hotkey
                    {
                        Kmod1 = KeyModifier.ControlKey,
                        Kmod2 = KeyModifier.Menu,
                        Key = 102
                    }
                },
                new HotKeyBinding
                {
                    DevId = "38022865c82b96e4ff6e_3",
                    Hotkey = new Hotkey
                    {
                        Kmod1 = KeyModifier.ControlKey,
                        Kmod2 = KeyModifier.Menu,
                        Key = 105,
                    }
                },
                new HotKeyBinding
                {
                    DevId = "38022865c82b96e50098_1",
                    Hotkey = new Hotkey
                    {
                        Kmod1 = KeyModifier.ControlKey,
                        Kmod2 = KeyModifier.Menu,
                        Key = 98,
                    }
                },
                new HotKeyBinding
                {
                    DevId = "38022865c82b96e50098_2",
                    Hotkey = new Hotkey
                    {
                        Kmod1 = KeyModifier.ControlKey,
                        //Kmod2 = KeyModifier.Menu,
                        Key = 97,
                    }
                },
                new HotKeyBinding
                {
                    DevId = "38022865c82b96e50098_3",
                    Hotkey = new Hotkey
                    {
                        Kmod1 = KeyModifier.ControlKey,
                        Kmod2 = KeyModifier.Menu,
                        Key = 101,
                    }
                },
            };

            for (int i = 0; i < hotkeyBinding.Count; i++)
            {
                RegisterHotKey(this.Handle, i, (int)hotkeyBinding[i].Hotkey.Kmod1 + (int)hotkeyBinding[i].Hotkey.Kmod2, Convert.ToInt32(hotkeyBinding[i].Hotkey.Key));
            }
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

                Debug.WriteLine($"id : {id}");
                Debug.WriteLine($"modifier : {modifier}");
                Debug.WriteLine($"key : {key}");


                var dev = tuya.DevicesList.Where(dev => dev.Id == hotkeyBinding[id].DevId).FirstOrDefault();
                int state = Convert.ToInt32(dev.Data.State);
                tuya.ControlDevicesAsync(hotkeyBinding[id].DevId, state ^ 1);

                dev.Data.State = Convert.ToBoolean(state ^ 1);
                RefreshDevicesList();
                tuya.SaveDeviceData();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDevicesList(true);
        }

        private async void RefreshDevicesList(bool hardRefresh = false)
        {
            if (hardRefresh)
            {
                await tuya.DiscoverDevices();
            }
            deviceListView.Items.Clear();

            foreach (var dev in tuya.DevicesList)
            {
                if (dev.Dev_type == "switch")
                {
                    ListViewItem device = new ListViewItem(dev.Name, 0);

                    device.SubItems.Add(dev.Id);
                    device.SubItems.Add(dev.Dev_type);
                    device.SubItems.Add(Convert.ToInt32(dev.Data.State).ToString());
                    device.SubItems.Add(Convert.ToInt32(dev.Data.Online).ToString());

                    deviceListView.Items.AddRange(new ListViewItem[] { device });
                }
            }
        }

        private void DeviceListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceListView.SelectedItems.Count > 0)
            {
                if (deviceListView.SelectedItems[0].SubItems[3].Text == "True")
                {
                    btnOnOff.Text = "OFF";
                }
                else
                {
                    btnOnOff.Text = "ON";
                }
                btnOnOff.Enabled = true;
                btnBindHotKey.Enabled = true;
            }
            else
            {
                btnOnOff.Text = "ON/OFF";
                btnOnOff.Enabled = false;
            }
        }

        private void BtnOnOff_Click(object sender, EventArgs e)
        {
            btnOnOff.Text = "ON/OFF";
            btnOnOff.Enabled = false;

            string devId = deviceListView.SelectedItems[0].SubItems[1].Text;
            int state = Convert.ToInt32(deviceListView.SelectedItems[0].SubItems[3].Text);

            tuya.ControlDevicesAsync(devId, state ^ 1);
            var dev = tuya.DevicesList.Where(dev => dev.Id == devId).FirstOrDefault();
            dev.Data.State = Convert.ToBoolean(state ^ 1);
            RefreshDevicesList();

            tuya.SaveDeviceData();
        }

        public Hotkey hotkey = new Hotkey();
        public bool listening = false;
        private void BtnBindHotKey_Click(object sender, EventArgs e)
        {
            hotkey = new Hotkey();
            listening = true;
            label1.Text = "listening...";
        }

        private void BtnBindHotKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (listening)
            {
                if (hotkey.Kmod1.ToString() == "None")
                {
                    hotkey.Kmod1 = (KeyModifier)Enum.Parse(typeof(KeyModifier), e.KeyCode.ToString());
                    Debug.WriteLine("kmod1");
                }
                else if (hotkey.Kmod1.ToString() != "None" && hotkey.Kmod2.ToString() == "None" && hotkey.Kmod1.ToString() != e.KeyCode.ToString())
                {
                    try
                    {
                        hotkey.Kmod2 = (KeyModifier)Enum.Parse(typeof(KeyModifier), e.KeyCode.ToString());
                        Debug.WriteLine("kmod2");
                    }
                    catch (Exception)
                    {
                        hotkey.Key = e.KeyValue;
                        Debug.WriteLine("key");
                    }
                }
                else if (hotkey.Kmod1.ToString() != e.KeyCode.ToString() && hotkey.Kmod2.ToString() != e.KeyCode.ToString())
                {
                    hotkey.Key = e.KeyValue;
                    Debug.WriteLine("key");
                }
            }
        }

        private void BtnBindHotKey_KeyUp(object sender, KeyEventArgs e)
        {
            DoneBinding();
        }

        private void DoneBinding()
        {
            btnBindHotKey.Text = $"{hotkey.Kmod1} + {hotkey.Kmod2} + {Enum.GetName(typeof(Keys), hotkey.Key)}";
            listening = false;
            label1.Text = "Edit Key Bind";
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
    }

    public class HotKeyBinding
    {
        public string DevId { get; set; }
        public Hotkey Hotkey { get; set; }
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
