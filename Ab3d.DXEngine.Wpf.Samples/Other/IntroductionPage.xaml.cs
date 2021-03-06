using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using Ab3d.DirectX;
using Microsoft.Win32;

namespace Ab3d.DXEngine.Wpf.Samples.Other
{
    public partial class IntroductionPage : Page
    {
        public IntroductionPage()
        {
            InitializeComponent();

            if (IsRunningInVirtualMachine())
                VirtualMachineWarningTextBlock.Visibility = Visibility.Visible;

            CarEngineImage.Visibility = Visibility.Visible;
        }

        private static bool IsRunningInVirtualMachine()
        {
            // The standard solution to check for virtual machine is to use WMI
            // But WMI can be very slow and can be sometimes corrupted.
            // Therefore it is much better to check in registry:
            try
            {
                RegistryKey regkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\Disk\Enum", false);
                if (regkey == null)
                    return false;

                var names = regkey.GetValueNames();

                foreach (var name in names)
                {
                    RegistryValueKind kind = regkey.GetValueKind(name);
                    if (kind == RegistryValueKind.String)
                    {
                        string value = regkey.GetValue(name).ToString().ToLower();
                        if (value.Contains("wmvare") ||
                            value.Contains("virt") ||
                            value.Contains("vbox") ||
                            value.Contains("xen") ||
                            value.Contains("aws"))
                        {
                            return true;
                        }
                    }
                }

                regkey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System", false);
                if (regkey == null)
                    return false;

                var keyVal = regkey.GetValue("VideoBiosVersion");
                if (keyVal == null)
                    return false;

                var parallelsVal = keyVal as string[];
                if (parallelsVal == null)
                    return false;

                foreach (string val in parallelsVal)
                {
                    if (val.ToLower().Contains("parallels"))
                        return true;
                }

                return false;
            }
            catch
            { }

            return false;
        }
    }
}