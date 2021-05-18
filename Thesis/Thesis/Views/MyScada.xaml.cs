using Microcharts;
using Opc.Ua;
using Opc.Ua.Client;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace Thesis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyScada : TabbedPage, INotifyPropertyChanged
    {
        public float Level1, Level2;
        private string color1, color2;
        private List<Entry> Gauge1;
        private List<Entry> Gauge2;

        public event PropertyChangedEventHandler PropertyChanged;

        public Chart Chart1 { get; set; }
        public Chart Chart2 { get; set; }
        public string textstring { get; set; }

        private string item1 = "ns=2;s=Pump_1.Start_Button",
            item2 = "ns=2;s=Pump_1.Stop_Button",
            item3 = "ns=2;s=Pump_1.Status",
            item6 = "ns=2;s=Tank_1.Status_Tank",
            item7 = "ns=2;s=Tank_1.Sensor_Level",
            item8 = "ns=2;s=Tank_1.Full_Level",
            item9 = "ns=2;s=Tank_1.Empty_Level",
            item10 = "ns=2;s=Door_1.Open_Button",
            item11 = "ns=2;s=Door_1.Close_Button",
            item12 = "ns=2;s=Door_1.Stop_Button",
            item13 = "ns=2;s=Door_1.Status",
            item14 = "ns=2;s=Pump_2.Start_Button",
            item15 = "ns=2;s=Pump_2.Stop_Button",
            item16 = "ns=2;s=Pump_2.Status",
            item19 = "ns=2;s=Tank_2.Status",
            item20 = "ns=2;s=Tank_2.Sensor_Level",
            item21 = "ns=2;s=Tank_2.Full_Level",
            item22 = "ns=2;s=Tank_2.Empty_Level",
            item23 = "ns=2;s=Door_2.Open_Button",
            item24 = "ns=2;s=Door_2.Close_Button",
            item25 = "ns=2;s=Door_2.Stop_Button",
            item26 = "ns=2;s=Door_2.Status",
            item27 = "ns=2;s=System_Emergency",
            item28 = "ns=2;s=System_Reset",
            item29 = "ns=2;s=System_Auto_Manual",
            item30 = "ns=2;s=System_Status",
            item31 = "ns=2;s=System_Fault",
            item32 = "ns=2;s=Tank_1.Open_Valve",
            item33 = "ns=2;s=Tank_1.Close_Valve",
            item34 = "ns=2;s=Tank_1.Status_Valve",
            item35 = "ns=2;s=Tank_2.Open_Valve",
            item36 = "ns=2;s=Tank_2.Close_Valve",
            item37 = "ns=2;s=Tank_2.Status_Valve";

        private int max_tank = 1000, min_tank = 0;
        private List<string> arrayvalue;
        private List<String> nodeIdStrings;

        private SampleClient opcClient;

        private List<String> values;
        private MonitoredItem myMonitoredItem;
        private Subscription mySubscription;

        public MyScada()
        {
            InitializeComponent();

            opcClient = MainPage.OpcClient;
            subcribe();
            //Device.StartTimer(TimeSpan.FromMilliseconds(10000), () =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        HomeTabRead();
            //        PumpTabRead();
            //        TankTabRead();
            //        GateTabRead();
            //        GaugeTank1();
            //        GaugeTank2();
            //        this.BindingContext = this;
            //    });
            //    return true;
            //});
        }
        
        private void subcribe()
        {
            try
            {
                if (mySubscription == null)
                {
                    mySubscription = opcClient.Subscribe(1000);
                }
                int i = 0;
                myMonitoredItem = opcClient.AddMonitoredItem(mySubscription, item3, item3, 1);
                //i++;
                myMonitoredItem = opcClient.AddMonitoredItem(mySubscription, item13, item13, 1);
                //i++;
                opcClient.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.ToString());
            }
        }

        private void Notification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }
            else
            {
                ;
                //arrayvalue.Add(notification.Value.WrappedValue.ToString());

            }
        }

        private void Change_Date1_Clicked(object sender, EventArgs e)
        {
            if (DT1.Text != null)
            {
                values = new List<string>();
                nodeIdStrings = new List<string>();
                values.Add(DT1.Text);
                nodeIdStrings.Add("ns=2;s=\"Door_1\".\"Planting_Day\"");
                try
                {
                    opcClient.VariableWrite(values, nodeIdStrings);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
            else
            {
                DisplayAlert("Warning", "Type DateTime to Set Planing Gate1", "OK");
            }
        }

        private void Change_Date2_Clicked(object sender, EventArgs e)
        {
            if (DT2.Text != null)
            {
                values = new List<string>();
                nodeIdStrings = new List<string>();
                values.Add(DT2.Text);
                nodeIdStrings.Add("ns = 2; s =\"Door_2\".\"Planting_Day\"");
                try
                {
                    opcClient.VariableWrite(values, nodeIdStrings);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
            else
            {
                DisplayAlert("Warning", "Type DateTime to Set Planing Gate2", "OK");
            }
        }

        private void Change_Tank1_Clicked(object sender, EventArgs e)
        {
            if (up1.Text != null && up1.Text != "")
            {
                values = new List<string>();
                nodeIdStrings = new List<string>();
                values.Add(up1.Text);
                nodeIdStrings.Add(item8);
                try
                {
                    opcClient.VariableWrite(values, nodeIdStrings);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
            if (low1.Text != null && low1.Text != "")
            {
                values = new List<string>();
                nodeIdStrings = new List<string>();
                values.Add(low1.Text);
                nodeIdStrings.Add(item9);
                try
                {
                    opcClient.VariableWrite(values, nodeIdStrings);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
        }

        private void Change_Tank2_Clicked(object sender, EventArgs e)
        {
            if (up2.Text != null && up2.Text != "")
            {
                values = new List<string>();
                nodeIdStrings = new List<string>();
                values.Add(up2.Text);
                nodeIdStrings.Add(item21);
                try
                {
                    opcClient.VariableWrite(values, nodeIdStrings);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }

            if (low2.Text != null && low2.Text != "")
            {
                values = new List<string>();
                nodeIdStrings = new List<string>();
                values.Add(low2.Text);
                nodeIdStrings.Add(item22);
                try
                {
                    opcClient.VariableWrite(values, nodeIdStrings);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
        }

        private void Close_Gate1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item11);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Close_Gate2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item24);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Emer_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item27);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values = new List<string>();
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void GateTabRead()
        {
            //staga1=Status Gate1
            var staga1 = opcClient.VariableRead(item13);
            if (staga1 == "True")
            {
                statusgate1.Text = "Opened";
            }
            else
            {
                statusgate1.Text = "Closed";
            }

            //staga2=Status Gate2
            var staga2 = opcClient.VariableRead(item26);
            if (staga2 == "True")
            {
                statusgate2.Text = "Opened";
            }
            else
            {
                statusgate2.Text = "Closed";
            }
        }

        private void GaugeTank1()
        {
            var uplimit = opcClient.VariableRead(item8);
            var downlimit = opcClient.VariableRead(item9);
            var level = opcClient.VariableRead(item7);
            if (uplimit != null && downlimit != null && level != null)
            {
                double up = Convert.ToDouble(uplimit);
                double down = Convert.ToDouble(downlimit);
                double le = Convert.ToDouble(level);
                Up1.Text = "Upper Limit:" + uplimit.ToString();
                Le1.Text = "Level Tank :" + level.ToString();
                Do1.Text = "Lower Limit :" + downlimit.ToString();
                if (le >= up || le <= down)
                {
                    color1 = "#c90c0c";
                }
                else if ((le >= (up - 100)) || (le <= (down + 100)))
                {
                    color1 = "#f5e50f";
                }
                else { color1 = "#54d419"; }

                if (level != null | level != "")
                {
                    Level1 = Convert.ToSingle(level);
                    Gauge1 = new List<Entry>
            {
                    new Entry(Level1)
                {
                    Color=SKColor.Parse(color1),
                    Label = "",
                    ValueLabel = "",
                },
                new Entry(max_tank-Level1)
                {
                    Color=SKColor.Parse("#ababab"),
                    Label = "",
                    ValueLabel =""
                }
            };
                    Chart1 = new DonutChart()
                    {
                        AnimationDuration = new TimeSpan(0),
                        IsAnimated = false,
                        Entries = Gauge1,
                        MinValue = min_tank,
                        MaxValue = max_tank,
                        BackgroundColor = SKColor.Empty,
                        LabelTextSize = 30f,
                        HoleRadius = 0,
                    };

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Chart1)));
                }
            }
        }

        private void GaugeTank2()
        {
            var uplimit = opcClient.VariableRead(item21);
            var downlimit = opcClient.VariableRead(item22);
            var level = opcClient.VariableRead(item20);
            if (uplimit != null && downlimit != null && level != null)
            {
                double up = Convert.ToDouble(uplimit);
                double down = Convert.ToDouble(downlimit);
                double le = Convert.ToDouble(level);
                Up2.Text = "Upper Limit:" + uplimit.ToString();
                Le2.Text = "Level Tank :" + level.ToString();
                Do2.Text = "Lower Limit :" + downlimit.ToString();
                if (le >= up || le <= down)
                {
                    color2 = "#c90c0c";
                }
                else if ((le > (up - 100)) || (le < (down + 100)))
                {
                    color2 = "#f5e50f";
                }
                else { color2 = "#54d419"; }

                if (level != null | level != "")
                {
                    Level2 = Convert.ToSingle(level);
                    Gauge2 = new List<Entry>
            {
                    new Entry(Level2)
                {
                    Color=SKColor.Parse(color2),
                    Label = "",
                    ValueLabel = ""
                },
                new Entry(max_tank-Level2)
                {
                    Color=SKColor.Parse("#ababab"),
                    Label = "",
                    ValueLabel =""
                }
            };
                    Chart2 = new DonutChart()
                    {
                        AnimationDuration = new TimeSpan(0),
                        IsAnimated = false,
                        Entries = Gauge2,
                        MinValue = min_tank,
                        MaxValue = max_tank,
                        BackgroundColor = SKColor.Empty,
                        LabelTextSize = 30f,
                        HoleRadius = 0,
                    };

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Chart1)));
                }
            }
        }

        private void HomeTabRead()
        {
            //ma=ManualAuto
            var ma = opcClient.VariableRead(item29);
            if (ma == "True")
            {
                MaAu.Text = "Auto";
            }
            else
            {
                MaAu.Text = "Manual";
            }

            //staled=StatusLed
            var staled = opcClient.VariableRead(item30);
            if (staled == "True")
            {
                Statusled.Source = "light_green_on.png";
            }
            else
            {
                Statusled.Source = "light_green_off.jpg";
            }

            //fauled=FaultLed
            var fauled = opcClient.VariableRead(item31);
            if (fauled == "True")
            {
                Faultled.Source = "light_red_on.jpg";
            }
            else
            {
                Faultled.Source = "light_red_off.png";
            }
            var emerled = opcClient.VariableRead(item27);
            if (emerled == "True")
            {
                Emerled.Source = "light_yellow_on.png";
            }
            else
            {
                Emerled.Source = "light_yellow_off.png";
            }
        }

        private void PumpTabRead()
        {
            //run1=Run Pump1
            var run1 = opcClient.VariableRead(item3);
            if (run1 == "True")
            {
                StatusPump1.Text = "On";
            }
            else
            {
                StatusPump1.Text = "Off";
            }
            //run2=Run Pump2
            var run2 = opcClient.VariableRead(item16);
            if (run2 == "True")
            {
                StatusPump2.Text = "On";
            }
            else
            {
                StatusPump2.Text = "Off";
            }
        }

        private void TankTabRead()
        {
            //lev1=Level Water Tank1
            var lev1 = opcClient.VariableRead(item7);
            if (lev1 != null)
            {
                level1.Text = lev1;
            }
            else { return; }
            //lev2=Level Water Tank2
            var lev2 = opcClient.VariableRead(item20);
            if (lev2 != null)
            {
                level2.Text = lev2;
            }
            //sta1=Status Tank1 0:Empty - 1:Normal - 2:Full
            var sta1 = opcClient.VariableRead(item6);
            if (sta1 == "0")
            {
                status1.Text = "Empty";
            }
            else if (sta1 == "1")
            {
                status1.Text = "Normal";
            }
            else if (sta1 == "2")
            {
                status1.Text = "Full";
            }
            else { return; }
            //sta2=Status Tank2 0:Empty - 1:Normal - 2:Full
            var sta2 = opcClient.VariableRead(item19);
            if (sta2 == "0")
            {
                status2.Text = "Empty";
            }
            else if (sta2 == "1")
            {
                status2.Text = "Normal";
            }
            else if (sta2 == "2")
            {
                status2.Text = "Full";
            }
            //stava1=Status Valve1
            var stava1 = opcClient.VariableRead(item34);
            if (stava1 == "True")
            {
                statusvalve1.Text = "On";
            }
            else
            {
                statusvalve1.Text = "Off";
            }
            //stava2=Status Valve2
            var stava2 = opcClient.VariableRead(item37);
            if (stava2 == "True")
            {
                statusvalve2.Text = "On";
            }
            else
            {
                statusvalve2.Text = "Off";
            }
        }

        private void Manual_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("False");
            nodeIdStrings.Add(item29);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Auto_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item29);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Open_Gate1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            nodeIdStrings.Add(item10);
            values.Add("True");

            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");

            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Open_Gate2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            nodeIdStrings.Add(item23);
            values.Add("True");

            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");

            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item28);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values = new List<string>();
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Start_Pump1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item1);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values = new List<string>();
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Start_Pump2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item14);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values = new List<string>();
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Start_Valve1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item32);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            nodeIdStrings.Add(item32);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Start_Valve2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            nodeIdStrings.Add(item35);
            values.Add("True");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Stop_Gate1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item12);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");

            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Stop_Gate2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item25);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Stop_Pump1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item2);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values = new List<string>();
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Stop_Pump2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item15);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values = new List<string>();
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Stop_Valve1_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item33);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private void Stop_Valve2_Clicked(object sender, EventArgs e)
        {
            values = new List<string>();
            nodeIdStrings = new List<string>();
            values.Add("True");
            nodeIdStrings.Add(item36);
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
            values.Add("False");
            try
            {
                opcClient.VariableWrite(values, nodeIdStrings);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}