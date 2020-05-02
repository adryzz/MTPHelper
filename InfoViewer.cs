using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaDevices;

namespace MTPHelper
{
    public partial class InfoViewer : Form
    {
        MediaDevice Device;

        public InfoViewer(MediaDevice d)
        {
            InitializeComponent();
            Device = d;
        }

        private void InfoViewer_Load(object sender, EventArgs e)
        {
            label1.Text = string.Format("Friendly name: {0}\nDescription: {1}\nDate and time of the device: {2}\nDeviceID: {3}\nDevice type: {4}\n" +
                "Firmware version: {5}\nManufacturer: {6}\nModel: {7}\nNetworkID: {8}\nTransport: {9}\nPower source: {10}\nPower level: {11}\n" +
                "Protocol: {12}\nPnPID: {13}\n\n", Device.FriendlyName, Device.Description, Device.DateTime.ToString(), Device.DeviceId,
                Device.DeviceType.ToString(), Device.FirmwareVersion, Device.Manufacturer, Device.Model, Device.NetworkIdentifier,
                Device.Transport.ToString(), Device.PowerSource.ToString(), Device.PowerLevel, Device.Protocol, Device.PnPDeviceID);
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = string.Format("Friendly name: {0}\nDescription: {1}\nDate and time of the device: {2}\nDeviceID: {3}\nDevice type: {4}\n" +
    "Firmware version: {5}\nManufacturer: {6}\nModel: {7}\nNetworkID: {8}\nTransport: {9}\nPower source: {10}\nPower level: {11}\n" +
    "Protocol: {12}\nPnPID: {13}\n\n", Device.FriendlyName, Device.Description, Device.DateTime.ToString(), Device.DeviceId,
    Device.DeviceType.ToString(), Device.FirmwareVersion, Device.Manufacturer, Device.Model, Device.NetworkIdentifier,
    Device.Transport.ToString(), Device.PowerSource.ToString(), Device.PowerLevel, Device.Protocol, Device.PnPDeviceID);
        }
    }
}
