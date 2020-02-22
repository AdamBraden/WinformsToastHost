using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinformsToastHost
{
    public partial class Form1 : Form
    {
        //private fields holding runtime identity details
        private string _myPackageId = "";
        private string _myPackagePath = "";
        public Form1()
        {
            InitializeComponent();

            //call Windows packaging apis to get identity information
            _myPackageId = Windows.ApplicationModel.Package.Current.Id.FullName;
            _myPackagePath = Windows.ApplicationModel.Package.Current.InstalledPath;

            //display info to the user
            textblockPackageId.Text = _myPackageId;
            textblockPackagePath.Text = _myPackagePath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create toast content, showing packageId
            string toastXml = $@"<toast>
                            <visual>
                                <binding template='ToastGeneric'>
                                    <text>From ToastHost!</text>
                                    <text>My App Identity is: {_myPackageId}</text>
                                </binding>
                            </visual>
                        </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            //create and show the toast
            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Assembly a = Assembly.Load(Path.Combine(_myPackagePath, "MyExtension.dll"));
            // Get the type to use.
            Type myType = a.GetType("Message");
            // Get the method to call.
            MethodInfo myMethod = myType.GetMethod("SayHello");
            // Create an instance.
            object obj = Activator.CreateInstance(myType);
            // Execute the method.
            Message.Text = myMethod.Invoke(obj, null).ToString();
        }
    }
}
