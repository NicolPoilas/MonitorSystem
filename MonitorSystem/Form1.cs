using System;
using System.Windows.Forms;
using canTransport;
using canDriver;
using SecurityAccess;
using Dongzr.MidiLite;
using System.Text.RegularExpressions;
using System.Threading;

namespace MonitorSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BusParamsInit();
            //MmTime_init();
            Trans_init();
        }

        canDriverKvaser driver = new canDriverKvaser();
        canTrans driverTrans = new canTrans();
        SecurityKey securityDriver = new SecurityKey();
        DateTime time = new DateTime();

        byte[] RxMsgs = new byte[0];//实际有效数据
        int readcount;
        string[] Readdata = { "22D502", "22D503", "22FD0C", "22FD10", "22A002" };//读IO状态
        bool CMDFlag;//循环开启Flag

        int i, j = 0;
        int minutes = 0;
        int mode = 0;//0表示2.1模式,1表示2.3模式
        string[] IOCtrl = { "1003", "2701", "2FFDA003FFFFFFFFFFFFFFFFFF" };//IOCtrl
        string[] Sleep = { "1003", "2709", "3101FDF1" };//Sleep

        int looptime1 = 1;// 小循环次数
        int looptime2 = 1;//大循环次数

        #region IOFlag
        /*定义模拟输入*/
        int HOOD_AJAR_IN, HOOD_AJAR_IN_FLAG = 0;
        int LOW_BEAN_POSTTION_LAMP_IN, LOW_BEAN_POSTTION_LAMP_IN_FLAG = 0;
        int FOG_IN, FOG_IN_FLAG = 0;
        int WIPER_INT_IN, WIPER_INT_IN_FLAG = 0;
        int Turn_Light_IN, Turn_Light_IN_FLAG = 0;
        int FLASH_IN, FLASH_IN_FLAG = 0;
        int CEN_LOCK_IN, CEN_LOCK_IN_FLAG = 0;
        int WIN_FR_IN, WIN_FR_IN_FLAG = 0;
        int WIN_RL_IN, WIN_RL_IN_FLAG = 0;
        int WIN_RR_IN, WIN_RR_IN_FLAG = 0;
        int IP_ILL_SET1_IN, IP_ILL_SET1_IN_FLAG = 0;
        int IP_ILL_SET2_IN, IP_ILL_SET2_IN_FLAG = 0;
        int REAR_WIPER_IN, REAR_WIPER_IN_FLAG = 0;

        /*定义数字输入*/
        int Hazard_SW, Hazard_SW_FLAG = 0;
        int Driver_Door_Ajar_SW, Driver_Door_Ajar_SW_FLAG = 0;
        int Passenger_Door_Ajar_SW, Passenger_Door_Ajar_SW_FLAG = 0;
        int RL_Door_Ajar_SW, RL_Door_Ajar_SW_FLAG = 0;
        int RR_Door_Ajar_SW, RR_Door_Ajar_SW_FLAG = 0;
        int Trunk_Ajar_SW, Trunk_Ajar_SW_FLAG = 0;
        int Trunk_Release_SW, Trunk_Release_SW_FLAG = 0;
        int Driver_Door_Lock_SW, Driver_Door_Lock_SW_FLAG = 0;
        int Passenger_Door_Lock_SW, Passenger_Door_Lock_SW_FLAG = 0;
        int RL_Door_Lock_SW, RL_Door_Lock_SW_FLAG = 0;
        int RR_Door_Lock_SW, RR_Door_Lock_SW_FLAG = 0;
        int Horn_SW, Horn_SW_FLAG = 0;
        int Reverse_Gear_SW, Reverse_Gear_SW_FLAG = 0;
        int Cluch_Pedal_SW, Cluch_Pedal_SW_FLAG = 0;
        int Neutral_shift_SW, Neutral_shift_SW_FLAG = 0;
        int Front_Wiper_SW_1, Front_Wiper_SW_1_FLAG = 0;
        int Front_Wiper_SW_2, Front_Wiper_SW_2_FLAG = 0;
        int WINLCK_SW, WINLCK_SW_FLAG = 0;
        int Mirror_Heater_SW, Mirror_Heater_SW_FLAG = 0;
        int Front_Wiper_Park_SW, Front_Wiper_Park_SW_FLAG = 0;
        int Rear_Wiper_Park_SW, Rear_Wiper_Park_SW_FLAG = 0;
        int Winter_Mode_SW, Winter_Mode_SW_FLAG = 0;
        int FL_Dir_Lamp_FB, FL_Dir_Lamp_FB_FLAG = 0;
        int Rear_defrost_SW, Rear_defrost_SW_FLAG = 0;
        int FR_Dir_Lamp_FB, FR_Dir_Lamp_FB_FLAG = 0;
        int Mirror_Fold_Unfold_SW, Mirror_Fold_Unfold_SW_FLAG = 0;
        int Park_Shift_SW, Park_Shift_SW_FLAG = 0;
        int Brake_Pedal_SW, Brake_Pedal_SW_FLAG = 0;
        int KL15_Ignition_SW, KL15_Ignition_SW_FLAG = 0;
        int KLR_Accessory_SW, KLR_Accessory_SW_FLAG = 0;
        int Key_In_SW, Key_In_SW_FLAG = 0;
        int KL50_Starter_SW, KL50_Starter_SW_FLAG = 0;

        /*定义输出*/
        int High_Beam_assistant_L, High_Beam_assistant_L_FLAG = 0;
        int High_Beam_assistant_R, High_Beam_assistant_R_FLAG = 0;
        int Passenger_Door_Lock, Passenger_Door_Lock_FLAG = 0;
        int All_Doors_Unlock, All_Doors_Unlock_FLAG = 0;
        int Horn, Horn_FLAG = 0;
        int Driver_Door_Fuel_Lock, Driver_Door_Fuel_Lock_FLAG = 0;
        int Front_Washer_Pump, Front_Washer_Pump_FLAG = 0;
        int Rear_Washer_Pump, Rear_Washer_Pump_FLAG = 0;
        int Wiper_Motor_Low, Wiper_Motor_Low_FLAG = 0;
        int Wiper_Motor_High, Wiper_Motor_High_FLAG = 0;
        int Rear_Wiper, Rear_Wiper_FLAG = 0;
        int Mirror_Fold, Mirror_Fold_FLAG = 0;
        int Mirror_Unfold, Mirror_Unfold_FLAG = 0;
        int Low_Beam_L, Low_Beam_L_FLAG = 0;
        int Low_Beam_R, Low_Beam_R_FLAG = 0;
        int Rear_Defrost_Relay, Rear_Defrost_Relay_FLAG = 0;
        int High_Beam_Solenoid, High_Beam_Solenoid_FLAG = 0;
        int Window_Lock_Indicator, Window_Lock_Indicator_FLAG = 0;
        int Ignition_Key_Indicator, Ignition_Key_Indicator_FLAG = 0;
        int Starter_Solenoid_Relay, Starter_Solenoid_Relay_FLAG = 0;
        int Front_Fog_Lamp_L, Front_Fog_Lamp_L_FLAG = 0;
        int Front_Fog_Lamp_R, Front_Fog_Lamp_R_FLAG = 0;
        int Sunroof_Enable_HS, Sunroof_Enable_HS_FLAG = 0;
        int Sunroof_Enable_LS, Sunroof_Enable_LS_FLAG = 0;
        int Passenger_Window_Up, Passenger_Window_Up_FLAG = 0;
        int Passenger_Window_Down, Passenger_Window_Down_FLAG = 0;
        int RL_Window_Up, RL_Window_Up_FLAG = 0;
        int RL_Window_Down, RL_Window_Down_FLAG = 0;
        int RR_Window_Up, RR_Window_Up_FLAG = 0;
        int RR_Window_Down, RR_Window_Down_FLAG = 0;
        int Window_Lift_Enable_1, Window_Lift_Enable_1_FLAG = 0;
        int Window_Lift_Enable_2, Window_Lift_Enable_2_FLAG = 0;
        int Security_Indication, Security_Indication_FLAG = 0;
        int Central_Lock_Indication, Central_Lock_Indication_FLAG = 0;
        int Hazard_SW_Indication, Hazard_SW_Indication_FLAG = 0;
        int Door_Open_Illumination, Door_Open_Illumination_FLAG = 0;
        int Rear_Door_Lamp, Rear_Door_Lamp_FLAG = 0;
        int Atmosphere_Lamp, Atmosphere_Lamp_FLAG = 0;
        int Brake_Lamps, Brake_Lamps_FLAG = 0;
        int Batttery_Saver, Batttery_Saver_FLAG = 0;
        int Turn_Lamp_L, Turn_Lamp_L_FLAG = 0;
        int Turn_Lamp_R, Turn_Lamp_R_FLAG = 0;
        int Reverse_Lamps, Reverse_Lamps_FLAG = 0;
        int Daytime_Running_Lamps, Daytime_Running_Lamps_FLAG = 0;
        int Rear_Fog_Lamps, Rear_Fog_Lamps_FLAG = 0;
        int Key_Locked_Solenoid, Key_Locked_Solenoid_FLAG = 0;
        int Shifter_Lock_Solenoid, Shifter_Lock_Solenoid_FLAG = 0;
        int Position_Lamps_L, Position_Lamps_L_FLAG = 0;
        int Position_Lamps_R, Position_Lamps_R_FLAG = 0;
        int CHMSL, CHMSL_FLAG = 0;
        int License_Plate_Lamps, License_Plate_Lamps_FLAG = 0;
        int Trunk_Unlock, Trunk_Unlock_FLAG = 0;
        int Headlamp_Washer_Relay, Headlamp_Washer_Relay_FLAG = 0;
        int Welcome_Lamps, Welcome_Lamps_FLAG = 0;
        int PRND_IndicatorCtrl, PRND_IndicatorCtrl_FLAG = 0;

        /*电压*/
        int Battery_Up, Battery_Down, Battery_Voltage, Battery_Voltage_FLAG = 0;
        #endregion

        #region BusSetting
        private void BusParamsInit()
        {
            string[] channel = new string[0];
            channel = driver.GetChannel();
            comboBoxChannel.Items.Clear();
            comboBoxChannel.Items.AddRange(channel);//add items for comboBox
            comboBoxChannel.SelectedIndex = 0;//default select the first , physical driver always come first
            comboBoxBaudrate.SelectedIndex = 4;//default select 500K   
            comboBoxSession.SelectedIndex = 2;
            comboBoxAccess.SelectedIndex = 0;
            comboBoxIO.SelectedIndex = 0;
        }

        private void BusOffOn_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus On")
            {
                if (driver.OpenChannel(comboBoxChannel.SelectedIndex, comboBoxBaudrate.Text) == true)
                {
                    BusOffOn.Text = "Bus Off";
                    driverTrans.Start();
                    mmTimer.Start();
                    labelBusLoad.Text = "Bus Load:" + driver.BusLoad().ToString() + "%";
                    comboBoxBaudrate.Enabled = false;
                    comboBoxChannel.Enabled = false;
                }
                else
                {
                    MessageBox.Show("打开" + comboBoxChannel.Text + "通道失败!"); //最好能把原因定位出来 给故障编码写入帮助文件
                }
            }
            else
            {
                BusOffOn.Text = "Bus On";
                driverTrans.Stop();
                mmTimer.Stop();
                driver.CloseChannel();
                labelBusLoad.Text = "Bus Load:0%";
                buttonSession.Enabled = true;//使能所有发送数据的开关
                buttonAccess.Enabled = true;
                CMDFlag = false;
                buttonCMD.Text = "Monitor";
                comboBoxBaudrate.Enabled = true;
                comboBoxChannel.Enabled = true;
                groupBox2.Enabled = true;
                groupBox9.Enabled = true;
                comboBoxIO.Enabled = true;
            }
        }
        #endregion

        private void buttonSessoin_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus Off")
                driverTrans.CanSendString("10" + comboBoxSession.Text);
        }

        private void buttonAccess_Click(object sender, EventArgs e)
        {
            if (BusOffOn.Text == "Bus Off")
                driverTrans.CanSendString("27" + comboBoxAccess.Text);
        }

        private void buttonIO_Click(object sender, EventArgs e)
        {
            if ((BusOffOn.Text == "Bus Off") && (comboBoxIO.SelectedIndex == 0))
            {
                driverTrans.CanSendString("2FFDA003FFFFFFFFFFFFFFFFFF");
                mode = 1;
            }
            else if ((BusOffOn.Text == "Bus Off") && (comboBoxIO.SelectedIndex == 1))
            {
                driverTrans.CanSendString("2FFDA00");
                mode = 0;
            }
        }

        private void buttonCMD_Click(object sender, EventArgs e)
        {
            if ((buttonCMD.Text == "Monitor") && (BusOffOn.Text == "Bus Off"))
            {
                buttonCMD.Text = "Stop";
                CMDFlag = true;
                groupBox2.Enabled = false;
                groupBox9.Enabled = false;
                time = DateTime.Now;
                minutes = 0;
            }
            else
            {
                buttonCMD.Text = "Monitor";
                CMDFlag = false;
                groupBox2.Enabled = true;
                groupBox9.Enabled = true;
            }
        }

        private void Clear1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void Clear2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
        }

        /*将十六进制字符串转换成十六进制数组（不足末尾补0），失败返回空数组*/
        byte[] StringToHex(string strings)
        {
            byte[] hex = new byte[0];
            try
            {
                strings = strings.Replace("0x", "");
                strings = strings.Replace("0X", "");
                strings = strings.Replace(" ", "");
                strings = Regex.Replace(strings, @"(?i)[^a-f\d\s]+", "");//表示不可变正则表达式
                if (strings.Length % 2 != 0)
                {
                    strings += "0";
                }
                hex = new byte[strings.Length / 2];
                for (int i = 0; i < hex.Length; i++)
                {
                    hex[i] = Convert.ToByte(strings.Substring(i * 2, 2), 16);
                }
                return hex;
            }
            catch
            {
                return hex;
            }
        }

        /*将十六进制数组转换成十六进制字符串，并以space隔开*/
        string HexToStrings(byte[] hex)
        {
            string strings = "";
            for (int i = 0; i < hex.Length; i++)//逐字节变为16进制字符，并以space隔开
            {
                strings += hex[i].ToString("X2") + " ";
            }
            return strings;
        }

        /*使用事件委托传参*/
        void Trans_init()
        {
            driverTrans.EventTxFarms += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.FarmsEventArgs args = (canTrans.FarmsEventArgs)e1;
                    EventHandler TextBoxDisplayUpdate = delegate
                    {
                        if (!CMDFlag)
                        {
                            richTextBox1.AppendText("$" + args.ToString() + "\r\n");//发送数据流
                            richTextBox1.ScrollToCaret();
                        }
                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );
            driverTrans.EventRxFarms += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.FarmsEventArgs args = (canTrans.FarmsEventArgs)e1;
                    EventHandler TextBoxDisplayUpdate = delegate
                    {
                        if (!CMDFlag)
                        {
                            richTextBox1.AppendText("$" + args.ToString() + "\r\n");//接收数据流
                            richTextBox1.ScrollToCaret();
                        }
                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );
            driverTrans.EventRxMsgs += new EventHandler(
                (sender1, e1) =>
                {
                    canTrans.RxMsgsEventArgs RxMsgs = (canTrans.RxMsgsEventArgs)e1;
                    AutoResponse(StringToHex(RxMsgs.ToString()));
                    EventHandler TextBoxDisplayUpdate = delegate
                    {
                        if (mode == 1)
                        {
                            Analysis(StringToHex(RxMsgs.ToString()));//只在2.3模式才--+解析收到的有效数据
                        }
                    };
                    try { Invoke(TextBoxDisplayUpdate); } catch { };
                }
                );
            driverTrans.EventError += new EventHandler(
                (sender1, e1) =>
                {
                    if (!CMDFlag)
                    {
                        canTrans.ErrorEventArgs args = (canTrans.ErrorEventArgs)e1;
                        EventHandler TextBoxDisplayUpdate = delegate
                        {
                            //richTextBox2.AppendText("$" + args.ToString() + "\r\n");//错误信号
                            //richTextBox2.ScrollToCaret();
                        };
                        try { Invoke(TextBoxDisplayUpdate); } catch { };
                    }
                }
                );

            driverTrans.tx_id_load = 0x7B0;//发送ID
            driverTrans.rx_id_load = 0x7B8;//接收ID

            driverTrans.CanRead += driver.ReadSpecfic;
            driverTrans.CanWrite += driver.WriteData;
        }

        /*判断安全进入*/
        private void AutoResponse(byte[] data)
        {
            if (data[0] == 0x67)
            {
                uint seed = 0;
                byte level;
                uint key = 0;
                if (data.Length == 4)
                {
                    seed = (uint)data[2] << 8
                        | (uint)data[3];
                }
                else if (data.Length == 6)
                {
                    seed = (uint)data[2] << 24
                        | (uint)data[3] << 16
                        | (uint)data[4] << 8
                        | (uint)data[5];
                }
                level = data[1];
                if (seed != 0 && level % 2 != 0)
                {
                    key = securityDriver.UdsCallback_CalcKey(seed, level);
                    if (data.Length == 4)
                    {
                        key &= 0xFFFF;
                        driverTrans.CanSendString("27" + (level + 1).ToString("x2") + key.ToString("x4"));
                    }
                    else if (data.Length == 6)
                    {
                        driverTrans.CanSendString("27" + (level + 1).ToString("x2") + key.ToString("x8"));
                    }
                }
            }
        }

        private void delay(int delayTime)//延时函数
        {
            Thread t = new Thread(o => Thread.Sleep(delayTime));
            t.Start(this);
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
        }

        /*定时器*/
        #region Timer
        public delegate void Tick_10ms();
        public delegate void Tick_20ms();
        public delegate void Tick_50ms();
        public delegate void Tick_100ms();
        public delegate void Tick_200ms();
        public delegate void Tick_1s();
        public delegate void Tick_2s();
        public delegate void Tick_5s();
        public delegate void Tick_60s();
        public Tick_10ms mmtimer_tick_10ms;
        public Tick_10ms mmtimer_tick_20ms;
        public Tick_10ms mmtimer_tick_50ms;
        public Tick_100ms mmtimer_tick_100ms;
        public Tick_200ms mmtimer_tick_200ms;
        public Tick_1s mmtimer_tick_1s;
        public Tick_2s mmtimer_tick_2s;
        public Tick_5s mmtimer_tick_5s;
        public Tick_60s mmtimer_tick_60s;
        public MmTimer mmTimer;
        const int timer_interval = 10;
        int timer_10ms_counter = 0;
        int timer_20ms_counter = 0;
        int timer_50ms_counter = 0;
        int timer_100ms_counter = 0;
        int timer_200ms_counter = 0;
        int timer_1s_counter = 0;
        int timer_2s_counter = 0;
        int timer_5s_counter = 0;
        int timer_60s_counter = 0;

        private void MmTime_init()
        {
            mmTimer = new MmTimer
            {
                Mode = MmTimerMode.Periodic,
                Interval = timer_interval
            };
            mmTimer.Tick += mmTimer_tick;

            mmtimer_tick_10ms += delegate
            {

            };

            mmtimer_tick_20ms += delegate
            {

            };

            mmtimer_tick_50ms += delegate
            {

            };

            mmtimer_tick_100ms += delegate
            {
                if ((CMDFlag) && (mode == 1))
                {
                    driverTrans.CanSendString(Readdata[readcount]);
                    if (readcount < 4)
                        readcount++;
                    else
                        readcount = 0;
                }
            };

            mmtimer_tick_200ms += delegate
            {

            };

            mmtimer_tick_1s += delegate
            {
                EventHandler BusLoadUpdate = delegate
                {
                    label7.Text = DateTime.Now.ToString();
                    if (CMDFlag)
                    {
                        label6.Text = "Continued:   " + (DateTime.Now - time).ToString(@"hh\:mm\:ss");
                    }
                    labelBusLoad.Text = "Bus Load:" + driver.BusLoad().ToString() + "% ";//更新BusLoad
                    if (driver.BusLoad() > 95) //CAN通讯检测
                    {
                        richTextBox2.AppendText("CAN Bus Timeout," + DateTime.Now.ToString() + "\r\n");
                        richTextBox2.ScrollToCaret();
                    }
                };
                try { Invoke(BusLoadUpdate); } catch { };
            };

            mmtimer_tick_2s += delegate
            {

            };

            mmtimer_tick_5s += delegate
            {

            };

            mmtimer_tick_60s += delegate
            {
                if (CMDFlag)
                {
                    minutes++;//分钟数加1
                    if (looptime2 <= 2)//2个大循环
                    {
                        if (looptime1 <= 8)//8个小循环
                        {
                            if ((minutes < 60) && (minutes >= 55))
                            {
                                if (mode == 0)
                                {
                                    for (j = 0; j < 3; j++)//2.3模式，发IOCtrl
                                    {
                                        driverTrans.CanSendString(IOCtrl[j]);
                                        delay(50);
                                    }
                                    mode = 1;
                                }
                            }
                            else if (minutes >= 60)
                            {
                                if (mode == 1)
                                {
                                    driverTrans.CanSendString("2FFDA000");
                                    delay(1000);
                                    for (i = 0; i < 3; i++)//2.1模式，发Sleep
                                    {
                                        driverTrans.CanSendString(Sleep[i]);
                                        delay(50);
                                    }
                                    mode = 0;
                                    minutes = 0;
                                    looptime1++;
                                }
                            }
                        }
                        else
                        {
                            if (minutes >= 240)
                            {
                                looptime1 = 1;
                                minutes = 0;
                                looptime2++;
                            }
                        }
                    }

                }
            };
        }

        void mmTimer_tick(object sender, EventArgs e)
        {
            timer_10ms_counter += timer_interval;
            if (timer_10ms_counter >= 10)
            {
                timer_10ms_counter = 0;
                mmtimer_tick_10ms?.Invoke();
            }

            timer_20ms_counter += timer_interval;
            if (timer_20ms_counter >= 10)
            {
                timer_20ms_counter = 0;
                mmtimer_tick_20ms?.Invoke();
            }

            timer_50ms_counter += timer_interval;
            if (timer_50ms_counter >= 50)
            {
                timer_50ms_counter = 0;
                mmtimer_tick_50ms?.Invoke();
            }

            timer_100ms_counter += timer_interval;
            if (timer_100ms_counter >= 100)
            {
                timer_100ms_counter = 0;
                mmtimer_tick_100ms?.Invoke();
            }

            timer_200ms_counter += timer_interval;
            if (timer_200ms_counter >= 200)
            {
                timer_200ms_counter = 0;
                mmtimer_tick_200ms?.Invoke();
            }

            timer_1s_counter += timer_interval;
            if (timer_1s_counter >= 1000)
            {
                timer_1s_counter = 0;
                mmtimer_tick_1s?.Invoke();
            }

            timer_2s_counter += timer_interval;
            if (timer_2s_counter >= 2000)
            {
                timer_2s_counter = 0;
                mmtimer_tick_2s?.Invoke();
            }

            timer_5s_counter += timer_interval;
            if (timer_5s_counter >= 5000)
            {
                timer_5s_counter = 0;
                mmtimer_tick_5s?.Invoke();
            }

            timer_60s_counter += timer_interval;
            if (timer_60s_counter >= 60000)
            {
                timer_60s_counter = 0;
                mmtimer_tick_60s?.Invoke();
            }
        }
        #endregion

        /*连续监测到n次Flag报错*/
        private void ErrorAdd(ref int flag, ref int io, string str)
        {
            flag++;
            if (flag == 2)
            {
                richTextBox2.AppendText(str + " Error," + DateTime.Now.ToString() + "\r\n");
                richTextBox2.ScrollToCaret();//滚动到光标处
                io++;
                flag = 0;
            }
        }

        /*清Flag*/
        private void FlagClear(ref int flag)
        {
            flag = 0;
        }

        /*解析收到报文存放到对应数组*/
        private void Analysis(byte[] data)
        {
            switch (data[1] + data[2])
            {
                //case 0x10D:
                //    {
                //        /*模拟输入*/
                //        #region Analog Input 
                //        if ((data[1] == 0xFD) && (data[2] == 0x10))
                //        {
                //            if (!((data[3] > 0x57) && (data[3] < 0xCC)) && checkBox42.Checked)
                //            {
                //                ErrorAdd(ref HOOD_AJAR_IN_FLAG, ref HOOD_AJAR_IN, "HOOD_AJAR_IN");
                //                if (HOOD_AJAR_IN != 0)
                //                {
                //                    textBox42.Text = HOOD_AJAR_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref HOOD_AJAR_IN_FLAG);
                //            }
                //            if (!((data[4] > 0x87) && (data[4] < 0xCC)) && checkBox40.Checked)
                //            {
                //                ErrorAdd(ref LOW_BEAN_POSTTION_LAMP_IN_FLAG, ref LOW_BEAN_POSTTION_LAMP_IN, "LOW_BEAN_POSTTION_LAMP_IN");
                //                if (LOW_BEAN_POSTTION_LAMP_IN != 0)
                //                {
                //                    textBox40.Text = LOW_BEAN_POSTTION_LAMP_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref LOW_BEAN_POSTTION_LAMP_IN_FLAG);
                //            }
                //            if (!((data[5] > 0x87) && (data[5] < 0xCC)) && checkBox41.Checked)
                //            {
                //                ErrorAdd(ref FOG_IN_FLAG, ref FOG_IN, "FOG_IN");
                //                if (FOG_IN != 0)
                //                {
                //                    textBox41.Text = FOG_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref FOG_IN_FLAG);
                //            }
                //            if (!((data[6] > 0x87) && (data[6] < 0xCC)) && checkBox39.Checked)
                //            {
                //                ErrorAdd(ref WIPER_INT_IN_FLAG, ref WIPER_INT_IN, "WIPER_INT_IN");
                //                if (WIPER_INT_IN != 0)
                //                {
                //                    textBox39.Text = WIPER_INT_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref WIPER_INT_IN_FLAG);
                //            }
                //            if (!((data[7] > 0x87) && (data[7] < 0xCC)) && checkBox38.Checked)
                //            {
                //                ErrorAdd(ref Turn_Light_IN_FLAG, ref Turn_Light_IN, "Turn_Light_IN");
                //                if (Turn_Light_IN != 0)
                //                {
                //                    textBox38.Text = Turn_Light_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Turn_Light_IN_FLAG);
                //            }
                //            if (!((data[8] > 0x87) && (data[8] < 0xCC)) && checkBox37.Checked)
                //            {
                //                ErrorAdd(ref FLASH_IN_FLAG, ref FLASH_IN, "FLASH_IN");
                //                if (FLASH_IN != 0)
                //                {
                //                    textBox37.Text = FLASH_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref FLASH_IN_FLAG);
                //            }
                //            if (!((data[9] > 0x87) && (data[9] < 0xCC)) && checkBox36.Checked)
                //            {
                //                ErrorAdd(ref CEN_LOCK_IN_FLAG, ref CEN_LOCK_IN, "CEN_LOCK_IN");
                //                if (CEN_LOCK_IN != 0)
                //                {
                //                    textBox36.Text = CEN_LOCK_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref CEN_LOCK_IN_FLAG);
                //            }
                //            if (!((data[10] > 0x87) && (data[10] < 0xCC)) && checkBox35.Checked)
                //            {
                //                //ErrorAdd(ref WIN_FR_IN_FLAG, ref WIN_FR_IN, "WIN_FR_IN");
                //                //if (WIN_FR_IN != 0)
                //                //{
                //                //    textBox35.Text = WIN_FR_IN.ToString();
                //                //}
                //            }
                //            else
                //            {
                //                FlagClear(ref WIN_FR_IN_FLAG);
                //            }
                //            if (!((data[11] > 0x87) && (data[11] < 0xCC)) && checkBox34.Checked)
                //            {
                //                ErrorAdd(ref WIN_RL_IN_FLAG, ref WIN_RL_IN, "WIN_RL_IN");
                //                if (WIN_RL_IN != 0)
                //                {
                //                    textBox34.Text = WIN_RL_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref WIN_RL_IN_FLAG);
                //            }
                //            if (!((data[12] > 0x87) && (data[12] < 0xCC)) && checkBox33.Checked)
                //            {
                //                ErrorAdd(ref WIN_RR_IN_FLAG, ref WIN_RR_IN, "WIN_RR_IN");
                //                if (WIN_RR_IN != 0)
                //                {
                //                    textBox33.Text = WIN_RR_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref WIN_RR_IN_FLAG);
                //            }
                //            if (!((data[13] > 0x87) && (data[13] < 0xCC)) && checkBox43.Checked)
                //            {
                //                ErrorAdd(ref IP_ILL_SET1_IN_FLAG, ref IP_ILL_SET1_IN, "IP_ILL_SET1_IN");
                //                if (IP_ILL_SET1_IN != 0)
                //                {
                //                    textBox43.Text = IP_ILL_SET1_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref IP_ILL_SET1_IN_FLAG);
                //            }
                //            if (!((data[14] > 0x87) && (data[14] < 0xCC)) && checkBox44.Checked)
                //            {
                //                ErrorAdd(ref IP_ILL_SET2_IN_FLAG, ref IP_ILL_SET2_IN, "IP_ILL_SET2_IN");
                //                if (IP_ILL_SET2_IN != 0)
                //                {
                //                    textBox44.Text = IP_ILL_SET2_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref IP_ILL_SET2_IN_FLAG);
                //            }
                //            if (!((data[15] > 0x87) && (data[15] < 0xCC)) && checkBox45.Checked)
                //            {
                //                ErrorAdd(ref REAR_WIPER_IN_FLAG, ref REAR_WIPER_IN, "REAR_WIPER_IN");
                //                if (REAR_WIPER_IN != 0)
                //                {
                //                    textBox45.Text = REAR_WIPER_IN.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref REAR_WIPER_IN_FLAG);
                //            }
                //        }
                //        #endregion};
                //        break;
                //    }
                //case 0x109:
                //    {
                //        /*数字输入*/
                //        #region Digital Input
                //        if ((data[1] == 0xFD) && (data[2] == 0x0C))
                //        {
                //            /*Byte1*/
                //            if (((data[3] & 0x80) == 0x80) && checkBox1.Checked)
                //            {
                //                ErrorAdd(ref Hazard_SW_FLAG, ref Hazard_SW, "Hazard_SW");
                //                if (Hazard_SW != 0)
                //                {
                //                    textBox1.Text = Hazard_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Hazard_SW_FLAG);
                //            }
                //            if (((data[3] & 0x20) == 0x20) && checkBox3.Checked)
                //            {
                //                ErrorAdd(ref Driver_Door_Ajar_SW_FLAG, ref Driver_Door_Ajar_SW, "Driver_Door_Ajar_SW");
                //                if (Driver_Door_Ajar_SW != 0)
                //                {
                //                    textBox3.Text = Driver_Door_Ajar_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Driver_Door_Ajar_SW);
                //            }
                //            if (((data[3] & 0x10) == 0x10) && checkBox4.Checked)
                //            {
                //                ErrorAdd(ref Passenger_Door_Ajar_SW_FLAG, ref Passenger_Door_Ajar_SW, "Passenger_Door_Ajar_SW");
                //                if (Passenger_Door_Ajar_SW != 0)
                //                {
                //                    textBox4.Text = Passenger_Door_Ajar_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Passenger_Door_Ajar_SW_FLAG);
                //            }
                //            if (((data[3] & 0x08) == 0x08) && checkBox5.Checked)
                //            {
                //                ErrorAdd(ref RL_Door_Ajar_SW_FLAG, ref RL_Door_Ajar_SW, "RL_Door_Ajar_SW");
                //                if (RL_Door_Ajar_SW != 0)
                //                {
                //                    textBox5.Text = RL_Door_Ajar_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref RL_Door_Ajar_SW_FLAG);
                //            }
                //            if (((data[3] & 0x04) == 0x04) && checkBox6.Checked)
                //            {
                //                ErrorAdd(ref RR_Door_Ajar_SW_FLAG, ref RR_Door_Ajar_SW, "RR_Door_Ajar_SW");
                //                if (RR_Door_Ajar_SW != 0)
                //                {
                //                    textBox6.Text = RR_Door_Ajar_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref RR_Door_Ajar_SW_FLAG);
                //            }
                //            if (((data[3] & 0x02) == 0x02) && checkBox7.Checked)
                //            {
                //                ErrorAdd(ref Trunk_Ajar_SW_FLAG, ref Trunk_Ajar_SW, "Trunk_Ajar_SW");
                //                if (Trunk_Ajar_SW != 0)
                //                {
                //                    textBox7.Text = Trunk_Ajar_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Trunk_Ajar_SW_FLAG);
                //            }

                //            /*Byte2*/
                //            if (((data[4] & 0x80) == 0x80) && checkBox8.Checked)
                //            {
                //                ErrorAdd(ref Trunk_Release_SW_FLAG, ref Trunk_Release_SW, "Trunk_Release_SW");
                //                if (Trunk_Release_SW != 0)
                //                {
                //                    textBox8.Text = Trunk_Release_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Trunk_Release_SW_FLAG);
                //            }
                //            if (((data[4] & 0x40) != 0x40) && checkBox9.Checked)
                //            {
                //                ErrorAdd(ref Driver_Door_Lock_SW_FLAG, ref Driver_Door_Lock_SW, "Driver_Door_Lock_SW");
                //                if (Driver_Door_Lock_SW != 0)
                //                {
                //                    textBox9.Text = Driver_Door_Lock_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Driver_Door_Lock_SW_FLAG);
                //            }
                //            if (((data[4] & 0x20) != 0x20) && checkBox10.Checked)
                //            {
                //                ErrorAdd(ref Passenger_Door_Lock_SW_FLAG, ref Passenger_Door_Lock_SW, "Passenger_Door_Lock_SW");
                //                if (Passenger_Door_Lock_SW != 0)
                //                {
                //                    textBox10.Text = Passenger_Door_Lock_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Passenger_Door_Lock_SW_FLAG);
                //            }
                //            if (((data[4] & 0x10) != 0x10) && checkBox11.Checked)
                //            {
                //                ErrorAdd(ref RL_Door_Lock_SW_FLAG, ref RL_Door_Lock_SW, "RL_Door_Lock_SW");
                //                if (RL_Door_Lock_SW != 0)
                //                {
                //                    textBox11.Text = RL_Door_Lock_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref RL_Door_Lock_SW_FLAG);
                //            }
                //            if (((data[4] & 0x08) != 0x08) && checkBox12.Checked)
                //            {
                //                ErrorAdd(ref RR_Door_Lock_SW_FLAG, ref RR_Door_Lock_SW, "RR_Door_Lock_SW");
                //                if (RR_Door_Lock_SW != 0)
                //                {
                //                    textBox12.Text = RR_Door_Lock_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref RR_Door_Lock_SW_FLAG);
                //            }
                //            if (((data[4] & 0x04) == 0x04) && checkBox13.Checked)
                //            {
                //                ErrorAdd(ref Horn_SW_FLAG, ref Horn_SW, "Horn_SW");
                //                if (Horn_SW != 0)
                //                {
                //                    textBox13.Text = Horn_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Horn_SW_FLAG);
                //            }
                //            if (((data[4] & 0x02) == 0x02) && checkBox14.Checked)
                //            {
                //                ErrorAdd(ref Reverse_Gear_SW_FLAG, ref Reverse_Gear_SW, "Reverse_Gear_SW");
                //                if (Reverse_Gear_SW != 0)
                //                {
                //                    textBox14.Text = Reverse_Gear_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Reverse_Gear_SW_FLAG);
                //            }
                //            if (((data[4] & 0x01) == 0x01) && checkBox15.Checked)
                //            {
                //                ErrorAdd(ref Cluch_Pedal_SW_FLAG, ref Cluch_Pedal_SW, "Cluch_Pedal_SW");
                //                if (Cluch_Pedal_SW != 0)
                //                {
                //                    textBox15.Text = Cluch_Pedal_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Cluch_Pedal_SW_FLAG);
                //            }

                //            /*Byte3*/
                //            if (((data[5] & 0x10) == 0x10) && checkBox24.Checked)
                //            {
                //                ErrorAdd(ref Neutral_shift_SW_FLAG, ref Neutral_shift_SW, "Neutral_shift_SW");
                //                if (Neutral_shift_SW != 0)
                //                {
                //                    textBox24.Text = Neutral_shift_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Neutral_shift_SW_FLAG);
                //            }
                //            if (((data[5] & 0x08) == 0x08) && checkBox23.Checked)
                //            {
                //                ErrorAdd(ref Front_Wiper_SW_1_FLAG, ref Front_Wiper_SW_1, "Front_Wiper_SW_1");
                //                if (Front_Wiper_SW_1 != 0)
                //                {
                //                    textBox23.Text = Front_Wiper_SW_1.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Front_Wiper_SW_1_FLAG);
                //            }
                //            if (((data[5] & 0x04) == 0x04) && checkBox22.Checked)
                //            {
                //                ErrorAdd(ref Front_Wiper_SW_2_FLAG, ref Front_Wiper_SW_2, "Front_Wiper_SW_2");
                //                if (Front_Wiper_SW_2 != 0)
                //                {
                //                    textBox22.Text = Front_Wiper_SW_2.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Front_Wiper_SW_2_FLAG);
                //            }
                //            if (((data[5] & 0x02) == 0x02) && checkBox21.Checked)
                //            {
                //                ErrorAdd(ref WINLCK_SW_FLAG, ref WINLCK_SW, "WINLCK_SW");
                //                if (WINLCK_SW != 0)
                //                {
                //                    textBox21.Text = WINLCK_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref WINLCK_SW_FLAG);
                //            }
                //            if (((data[5] & 0x01) == 0x01) && checkBox20.Checked)
                //            {
                //                ErrorAdd(ref Mirror_Heater_SW_FLAG, ref Mirror_Heater_SW, "Mirror_Heater_SW");
                //                if (Mirror_Heater_SW != 0)
                //                {
                //                    textBox20.Text = Mirror_Heater_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Mirror_Heater_SW_FLAG);
                //            }

                //            /*Byte4*/
                //            if (((data[6] & 0x80) != 0x80) && checkBox19.Checked)
                //            {
                //                ErrorAdd(ref Front_Wiper_Park_SW_FLAG, ref Front_Wiper_Park_SW, "Front_Wiper_Park_SW");
                //                if (Front_Wiper_Park_SW != 0)
                //                {
                //                    textBox19.Text = Front_Wiper_Park_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Front_Wiper_Park_SW_FLAG);
                //            }
                //            if (((data[6] & 0x40) != 0x40) && checkBox2.Checked)
                //            {
                //                ErrorAdd(ref Rear_Wiper_Park_SW_FLAG, ref Rear_Wiper_Park_SW, "Rear_Wiper_Park_SW");
                //                if (Rear_Wiper_Park_SW != 0)
                //                {
                //                    textBox2.Text = Rear_Wiper_Park_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Rear_Wiper_Park_SW_FLAG);
                //            }
                //            if (((data[6] & 0x20) == 0x20) && checkBox18.Checked)
                //            {
                //                ErrorAdd(ref Winter_Mode_SW_FLAG, ref Winter_Mode_SW, "Winter_Mode_SW");
                //                if (Winter_Mode_SW != 0)
                //                {
                //                    textBox18.Text = Winter_Mode_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Winter_Mode_SW_FLAG);
                //            }
                //            if (((data[6] & 0x10) == 0x10) && checkBox17.Checked)
                //            {
                //                ErrorAdd(ref FL_Dir_Lamp_FB_FLAG, ref FL_Dir_Lamp_FB, "FL_Dir_Lamp_FB");
                //                if (FL_Dir_Lamp_FB != 0)
                //                {
                //                    textBox17.Text = FL_Dir_Lamp_FB.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref FL_Dir_Lamp_FB_FLAG);
                //            }
                //            if (((data[6] & 0x08) == 0x08) && checkBox16.Checked)
                //            {
                //                ErrorAdd(ref Rear_defrost_SW_FLAG, ref Rear_defrost_SW, "Rear_defrost_SW");
                //                if (Rear_defrost_SW != 0)
                //                {
                //                    textBox16.Text = Rear_defrost_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Rear_defrost_SW_FLAG);
                //            }
                //            if (((data[6] & 0x04) == 0x04) && checkBox25.Checked)
                //            {
                //                ErrorAdd(ref FR_Dir_Lamp_FB_FLAG, ref FR_Dir_Lamp_FB, "FR_Dir_Lamp_FB");
                //                if (FR_Dir_Lamp_FB != 0)
                //                {
                //                    textBox25.Text = FR_Dir_Lamp_FB.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref FR_Dir_Lamp_FB_FLAG);
                //            }
                //            if (((data[6] & 0x02) == 0x02) && checkBox26.Checked)
                //            {
                //                ErrorAdd(ref Mirror_Fold_Unfold_SW_FLAG, ref Mirror_Fold_Unfold_SW, "Mirror_Fold_Unfold_SW");
                //                if (Mirror_Fold_Unfold_SW != 0)
                //                {
                //                    textBox26.Text = Mirror_Fold_Unfold_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Mirror_Fold_Unfold_SW_FLAG);
                //            }
                //            if (((data[6] & 0x01) == 0x01) && checkBox27.Checked)
                //            {
                //                ErrorAdd(ref Park_Shift_SW_FLAG, ref Park_Shift_SW, "Park_Shift_SW");
                //                if (Park_Shift_SW != 0)
                //                {
                //                    textBox27.Text = Park_Shift_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Park_Shift_SW_FLAG);
                //            }

                //            /*Byte7*/
                //            if (((data[9] & 0x80) == 0x80) && checkBox28.Checked)
                //            {
                //                ErrorAdd(ref Brake_Pedal_SW_FLAG, ref Brake_Pedal_SW, "Brake_Pedal_SW");
                //                if (Brake_Pedal_SW != 0)
                //                {
                //                    textBox28.Text = Brake_Pedal_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Brake_Pedal_SW_FLAG);
                //            }
                //            if (((data[9] & 0x08) == 0x08) && checkBox32.Checked)
                //            {
                //                ErrorAdd(ref KL15_Ignition_SW_FLAG, ref KL15_Ignition_SW, "KL15_Ignition_SW");
                //                if (KL15_Ignition_SW != 0)
                //                {
                //                    textBox32.Text = KL15_Ignition_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref KL15_Ignition_SW_FLAG);
                //            }
                //            if (((data[9] & 0x04) == 0x04) && checkBox31.Checked)
                //            {
                //                ErrorAdd(ref KLR_Accessory_SW_FLAG, ref KLR_Accessory_SW, "KLR_Accessory_SW");
                //                if (KLR_Accessory_SW != 0)
                //                {
                //                    textBox31.Text = KLR_Accessory_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref KLR_Accessory_SW_FLAG);
                //            }
                //            if (((data[9] & 0x02) == 0x02) && checkBox30.Checked)
                //            {
                //                ErrorAdd(ref Key_In_SW_FLAG, ref Key_In_SW, "Key_In_SW");
                //                if (Key_In_SW != 0)
                //                {
                //                    textBox30.Text = Key_In_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref Key_In_SW_FLAG);
                //            }
                //            if (((data[9] & 0x01) == 0x01) && checkBox29.Checked)
                //            {
                //                ErrorAdd(ref KL50_Starter_SW_FLAG, ref KL50_Starter_SW, "KL50_Starter_SW");
                //                if (KL50_Starter_SW != 0)
                //                {
                //                    textBox29.Text = KL50_Starter_SW.ToString();
                //                }
                //            }
                //            else
                //            {
                //                FlagClear(ref KL50_Starter_SW_FLAG);
                //            }
                //        }
                //        #endregion
                //    };
                //    break;
                case 0xD7:
                    {
                        /*输出1*/
                        #region Output1
                        if ((data[1] == 0xD5) && (data[2] == 0x02))
                        {
                            /*Byte1*/
                            if (((data[3] & 0x80) != 0x80) && checkBox74.Checked)
                            {
                                ErrorAdd(ref High_Beam_assistant_L_FLAG, ref High_Beam_assistant_L, "High_Beam_assistant_L");
                                if (High_Beam_assistant_L != 0)
                                {
                                    textBox74.Text = High_Beam_assistant_L.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref High_Beam_assistant_L_FLAG);
                            }
                            if (((data[3] & 0x40) != 0x40) && checkBox72.Checked)
                            {
                                ErrorAdd(ref High_Beam_assistant_R_FLAG, ref High_Beam_assistant_R, "High_Beam_assistant_R");
                                if (High_Beam_assistant_R != 0)
                                {
                                    textBox72.Text = High_Beam_assistant_R.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref High_Beam_assistant_R_FLAG);
                            }

                            if (((data[3] & 0x20) != 0x20) && checkBox73.Checked)
                            {
                                ErrorAdd(ref Passenger_Door_Lock_FLAG, ref Passenger_Door_Lock, "Passenger_Door_Lock");
                                if (Passenger_Door_Lock != 0)
                                {
                                    textBox73.Text = Passenger_Door_Lock.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Passenger_Door_Lock_FLAG);
                            }
                            if (((data[3] & 0x10) != 0x10) && checkBox71.Checked)
                            {
                                ErrorAdd(ref All_Doors_Unlock_FLAG, ref All_Doors_Unlock, "All_Doors_Unlock");
                                if (All_Doors_Unlock != 0)
                                {
                                    textBox71.Text = All_Doors_Unlock.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref All_Doors_Unlock_FLAG);
                            }
                            if (((data[3] & 0x08) != 0x08) && checkBox70.Checked)
                            {
                                ErrorAdd(ref Horn_FLAG, ref Horn, "Horn");
                                if (Horn != 0)
                                {
                                    textBox70.Text = Horn.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Horn_FLAG);
                            }
                            if (((data[3] & 0x04) != 0x04) && checkBox69.Checked)
                            {
                                ErrorAdd(ref Driver_Door_Fuel_Lock_FLAG, ref Driver_Door_Fuel_Lock, "Driver_Door_Fuel_Lock");
                                if (Driver_Door_Fuel_Lock != 0)
                                {
                                    textBox69.Text = Driver_Door_Fuel_Lock.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Driver_Door_Fuel_Lock_FLAG);
                            }
                            if (((data[3] & 0x02) != 0x02) && checkBox68.Checked)
                            {
                                ErrorAdd(ref Front_Washer_Pump_FLAG, ref Front_Washer_Pump, "Front_Washer_Pump");
                                if (Front_Washer_Pump != 0)
                                {
                                    textBox68.Text = Front_Washer_Pump.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Front_Washer_Pump_FLAG);
                            }
                            if (((data[3] & 0x01) != 0x01) && checkBox67.Checked)
                            {
                                ErrorAdd(ref Rear_Washer_Pump_FLAG, ref Rear_Washer_Pump, "Rear_Washer_Pump");
                                if (Rear_Washer_Pump != 0)
                                {
                                    textBox67.Text = Rear_Washer_Pump.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Rear_Washer_Pump_FLAG);
                            }

                            /*Byte2*/
                            if (((data[4] & 0x80) != 0x80) && checkBox66.Checked)
                            {
                                ErrorAdd(ref Wiper_Motor_Low_FLAG, ref Wiper_Motor_Low, "Wiper_Motor_Low");
                                if (Wiper_Motor_Low != 0)
                                {
                                    textBox66.Text = Wiper_Motor_Low.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Wiper_Motor_Low_FLAG);
                            }
                            if (((data[4] & 0x40) != 0x40) && checkBox65.Checked)
                            {
                                ErrorAdd(ref Wiper_Motor_High_FLAG, ref Wiper_Motor_High, "Wiper_Motor_High");
                                if (Wiper_Motor_High != 0)
                                {
                                    textBox65.Text = Wiper_Motor_High.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Wiper_Motor_High_FLAG);
                            }

                            if (((data[4] & 0x20) != 0x20) && checkBox64.Checked)
                            {
                                ErrorAdd(ref Rear_Wiper_FLAG, ref Rear_Wiper, "Rear_Wiper");
                                if (Rear_Wiper != 0)
                                {
                                    textBox64.Text = Rear_Wiper.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Rear_Wiper_FLAG);
                            }
                            if (((data[4] & 0x10) != 0x10) && checkBox63.Checked)
                            {
                                ErrorAdd(ref Mirror_Fold_FLAG, ref Mirror_Fold, "Mirror_Fold");
                                if (Mirror_Fold != 0)
                                {
                                    textBox63.Text = Mirror_Fold.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Mirror_Fold_FLAG);
                            }
                            if (((data[4] & 0x08) != 0x08) && checkBox62.Checked)
                            {
                                ErrorAdd(ref Mirror_Unfold_FLAG, ref Mirror_Unfold, "Mirror_Unfold");
                                if (Mirror_Unfold != 0)
                                {
                                    textBox62.Text = Mirror_Unfold.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Mirror_Unfold_FLAG);
                            }
                            if (((data[4] & 0x04) != 0x04) && checkBox61.Checked)
                            {
                                ErrorAdd(ref Low_Beam_L_FLAG, ref Low_Beam_L, "Low_Beam_L");
                                if (Low_Beam_L != 0)
                                {
                                    textBox61.Text = Low_Beam_L.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Low_Beam_L_FLAG);
                            }
                            if (((data[4] & 0x02) != 0x02) && checkBox60.Checked)
                            {
                                ErrorAdd(ref Low_Beam_R_FLAG, ref Low_Beam_R, "Low_Beam_R");
                                if (Low_Beam_R != 0)
                                {
                                    textBox60.Text = Low_Beam_R.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Low_Beam_R_FLAG);
                            }
                            if (((data[4] & 0x01) != 0x01) && checkBox59.Checked)
                            {
                                ErrorAdd(ref Rear_Defrost_Relay_FLAG, ref Rear_Defrost_Relay, "Rear_Defrost_Relay");
                                if (Rear_Defrost_Relay != 0)
                                {
                                    textBox59.Text = Rear_Defrost_Relay.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Rear_Defrost_Relay_FLAG);
                            }

                            /*Byte3*/
                            if (((data[5] & 0x80) != 0x80) && checkBox58.Checked)
                            {
                                ErrorAdd(ref High_Beam_Solenoid_FLAG, ref High_Beam_Solenoid, "High_Beam_Solenoid");
                                if (High_Beam_Solenoid != 0)
                                {
                                    textBox58.Text = High_Beam_Solenoid.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref High_Beam_Solenoid_FLAG);
                            }
                            if (((data[5] & 0x40) != 0x40) && checkBox57.Checked)
                            {
                                ErrorAdd(ref Window_Lock_Indicator_FLAG, ref Window_Lock_Indicator, "Window_Lock_Indicator");
                                if (Window_Lock_Indicator != 0)
                                {
                                    textBox57.Text = Window_Lock_Indicator.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Window_Lock_Indicator_FLAG);
                            }

                            if (((data[5] & 0x20) != 0x20) && checkBox56.Checked)
                            {
                                ErrorAdd(ref Ignition_Key_Indicator_FLAG, ref Ignition_Key_Indicator, "Ignition_Key_Indicator");
                                if (Ignition_Key_Indicator != 0)
                                {
                                    textBox56.Text = Ignition_Key_Indicator.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Ignition_Key_Indicator_FLAG);
                            }
                            if (((data[5] & 0x10) != 0x10) && checkBox55.Checked)
                            {
                                ErrorAdd(ref Starter_Solenoid_Relay_FLAG, ref Starter_Solenoid_Relay, "Starter_Solenoid_Relay");
                                if (Starter_Solenoid_Relay != 0)
                                {
                                    textBox55.Text = Starter_Solenoid_Relay.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Starter_Solenoid_Relay_FLAG);
                            }
                            if (((data[5] & 0x08) != 0x08) && checkBox54.Checked)
                            {
                                ErrorAdd(ref Front_Fog_Lamp_L_FLAG, ref Front_Fog_Lamp_L, "Front_Fog_Lamp_L");
                                if (Front_Fog_Lamp_L != 0)
                                {
                                    textBox54.Text = Front_Fog_Lamp_L.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Front_Fog_Lamp_L_FLAG);
                            }
                            if (((data[5] & 0x04) != 0x04) && checkBox53.Checked)
                            {
                                ErrorAdd(ref Front_Fog_Lamp_R_FLAG, ref Front_Fog_Lamp_R, "Front_Fog_Lamp_R");
                                if (Front_Fog_Lamp_R != 0)
                                {
                                    textBox53.Text = Front_Fog_Lamp_R.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Front_Fog_Lamp_R_FLAG);
                            }
                            if (((data[5] & 0x02) != 0x02) && checkBox52.Checked)
                            {
                                ErrorAdd(ref Sunroof_Enable_HS_FLAG, ref Sunroof_Enable_HS, "Sunroof_Enable_HS");
                                if (Sunroof_Enable_HS != 0)
                                {
                                    textBox52.Text = Sunroof_Enable_HS.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Sunroof_Enable_HS_FLAG);
                            }
                            if (((data[5] & 0x01) != 0x01) && checkBox51.Checked)
                            {
                                ErrorAdd(ref Sunroof_Enable_LS_FLAG, ref Sunroof_Enable_LS, "Sunroof_Enable_LS");
                                if (Sunroof_Enable_LS != 0)
                                {
                                    textBox51.Text = Sunroof_Enable_LS.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Sunroof_Enable_LS_FLAG);
                            }

                            /*Byte4*/
                            if (((data[6] & 0x80) != 0x80) && checkBox50.Checked)
                            {
                                ErrorAdd(ref Passenger_Window_Up_FLAG, ref Passenger_Window_Up, "Passenger_Window_Up");
                                if (Passenger_Window_Up != 0)
                                {
                                    textBox50.Text = Passenger_Window_Up.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Passenger_Window_Up_FLAG);
                            }
                            if (((data[6] & 0x40) != 0x40) && checkBox49.Checked)
                            {
                                ErrorAdd(ref Passenger_Window_Down_FLAG, ref Passenger_Window_Down, "Passenger_Window_Down");
                                if (Passenger_Window_Down != 0)
                                {
                                    textBox49.Text = Passenger_Window_Down.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Passenger_Window_Down_FLAG);
                            }

                            if (((data[6] & 0x20) != 0x20) && checkBox48.Checked)
                            {
                                ErrorAdd(ref RL_Window_Up_FLAG, ref RL_Window_Up, "RL_Window_Up");
                                if (RL_Window_Up != 0)
                                {
                                    textBox48.Text = RL_Window_Up.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref RL_Window_Up_FLAG);
                            }
                            if (((data[6] & 0x10) != 0x10) && checkBox47.Checked)
                            {
                                ErrorAdd(ref RL_Window_Down_FLAG, ref RL_Window_Down, "RL_Window_Down");
                                if (RL_Window_Down != 0)
                                {
                                    textBox47.Text = RL_Window_Down.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref RL_Window_Down_FLAG);
                            }
                            if (((data[6] & 0x08) != 0x08) && checkBox46.Checked)
                            {
                                ErrorAdd(ref RR_Window_Up_FLAG, ref RR_Window_Up, "RR_Window_Up");
                                if (RR_Window_Up != 0)
                                {
                                    textBox46.Text = RR_Window_Up.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref RR_Window_Up_FLAG);
                            }
                            if (((data[6] & 0x04) != 0x04) && checkBox75.Checked)
                            {
                                ErrorAdd(ref RR_Window_Down_FLAG, ref RR_Window_Down, "RR_Window_Down");
                                if (RR_Window_Down != 0)
                                {
                                    textBox75.Text = RR_Window_Down.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref RR_Window_Down_FLAG);
                            }
                            if (((data[6] & 0x02) != 0x02) && checkBox76.Checked)
                            {
                                ErrorAdd(ref Window_Lift_Enable_1_FLAG, ref Window_Lift_Enable_1, "Window_Lift_Enable_1");
                                if (Window_Lift_Enable_1 != 0)
                                {
                                    textBox76.Text = Window_Lift_Enable_1.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Window_Lift_Enable_1_FLAG);
                            }
                            if (((data[6] & 0x01) != 0x01) && checkBox77.Checked)
                            {
                                ErrorAdd(ref Window_Lift_Enable_2_FLAG, ref Window_Lift_Enable_2, "Window_Lift_Enable_2");
                                if (Window_Lift_Enable_2 != 0)
                                {
                                    textBox77.Text = Window_Lift_Enable_2.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Window_Lift_Enable_2_FLAG);
                            }
                        }
                        #endregion
                    };
                    break;
                case 0xD8:
                    {
                        /*输出2*/
                        #region Output2
                        if ((data[1] == 0xD5) && (data[2] == 0x03))
                        {
                            /*Byte1*/
                            if (((data[3] & 0x80) != 0x80) && checkBox78.Checked)
                            {
                                ErrorAdd(ref Security_Indication_FLAG, ref Security_Indication, "Security_Indication");
                                if (Security_Indication != 0)
                                {
                                    textBox78.Text = Security_Indication.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Security_Indication_FLAG);
                            }
                            if (((data[3] & 0x40) != 0x40) && checkBox80.Checked)
                            {
                                ErrorAdd(ref Central_Lock_Indication_FLAG, ref Central_Lock_Indication, "Central_Lock_Indication");
                                if (Central_Lock_Indication != 0)
                                {
                                    textBox80.Text = Central_Lock_Indication.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Central_Lock_Indication_FLAG);
                            }

                            if (((data[3] & 0x20) != 0x20) && checkBox82.Checked)
                            {
                                ErrorAdd(ref Hazard_SW_Indication_FLAG, ref Hazard_SW_Indication, "Hazard_SW_Indication");
                                if (Hazard_SW_Indication != 0)
                                {
                                    textBox82.Text = Hazard_SW_Indication.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Hazard_SW_Indication_FLAG);
                            }
                            if (((data[3] & 0x10) != 0x10) && checkBox81.Checked)
                            {
                                ErrorAdd(ref Door_Open_Illumination_FLAG, ref Door_Open_Illumination, "Door_Open_Illumination");
                                if (Door_Open_Illumination != 0)
                                {
                                    textBox81.Text = Door_Open_Illumination.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Door_Open_Illumination_FLAG);
                            }
                            if (((data[3] & 0x08) != 0x08) && checkBox82.Checked)
                            {
                                ErrorAdd(ref Rear_Door_Lamp_FLAG, ref Rear_Door_Lamp, "Rear_Door_Lamp");
                                if (Rear_Door_Lamp != 0)
                                {
                                    textBox82.Text = Rear_Door_Lamp.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Rear_Door_Lamp_FLAG);
                            }
                            if (((data[3] & 0x04) != 0x04) && checkBox83.Checked)
                            {
                                ErrorAdd(ref Atmosphere_Lamp_FLAG, ref Atmosphere_Lamp, "Atmosphere_Lamp");
                                if (Atmosphere_Lamp != 0)
                                {
                                    textBox85.Text = Atmosphere_Lamp.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Atmosphere_Lamp_FLAG);
                            }
                            if (((data[3] & 0x02) != 0x02) && checkBox84.Checked)
                            {
                                ErrorAdd(ref Brake_Lamps_FLAG, ref Brake_Lamps, "Brake_Lamps");
                                if (Brake_Lamps != 0)
                                {
                                    textBox84.Text = Brake_Lamps.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Brake_Lamps_FLAG);
                            }
                            if (((data[3] & 0x01) != 0x01) && checkBox85.Checked)
                            {
                                ErrorAdd(ref Batttery_Saver_FLAG, ref Batttery_Saver, "Batttery_Saver");
                                if (Batttery_Saver != 0)
                                {
                                    textBox85.Text = Batttery_Saver.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Batttery_Saver_FLAG);
                            }

                            /*Byte2*/
                            if (((data[4] & 0x04) != 0x04) && checkBox86.Checked)
                            {
                                ErrorAdd(ref Turn_Lamp_L_FLAG, ref Turn_Lamp_L, "Turn_Lamp_L");
                                if (Turn_Lamp_L != 0)
                                {
                                    textBox86.Text = Turn_Lamp_L.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Turn_Lamp_L_FLAG);
                            }
                            if (((data[4] & 0x02) != 0x02) && checkBox87.Checked)
                            {
                                ErrorAdd(ref Turn_Lamp_R_FLAG, ref Turn_Lamp_R, "Turn_Lamp_R");
                                if (Turn_Lamp_R != 0)
                                {
                                    textBox87.Text = Turn_Lamp_R.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Turn_Lamp_R_FLAG);
                            }
                            if (((data[4] & 0x01) != 0x01) && checkBox88.Checked)
                            {
                                ErrorAdd(ref Reverse_Lamps_FLAG, ref Reverse_Lamps, "Reverse_Lamps");
                                if (Reverse_Lamps != 0)
                                {
                                    textBox88.Text = Reverse_Lamps.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Reverse_Lamps_FLAG);
                            }

                            /*Byte3*/
                            if (((data[5] & 0x80) != 0x80) && checkBox89.Checked)
                            {
                                ErrorAdd(ref Daytime_Running_Lamps_FLAG, ref Daytime_Running_Lamps, "Daytime_Running_Lamps");
                                if (Daytime_Running_Lamps != 0)
                                {
                                    textBox89.Text = Daytime_Running_Lamps.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Daytime_Running_Lamps_FLAG);
                            }
                            if (((data[5] & 0x40) != 0x40) && checkBox90.Checked)
                            {
                                ErrorAdd(ref Rear_Fog_Lamps_FLAG, ref Rear_Fog_Lamps, "Rear_Fog_Lamps");
                                if (Rear_Fog_Lamps != 0)
                                {
                                    textBox90.Text = Rear_Fog_Lamps.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Rear_Fog_Lamps_FLAG);
                            }

                            if (((data[5] & 0x20) != 0x20) && checkBox91.Checked)
                            {
                                ErrorAdd(ref Key_Locked_Solenoid_FLAG, ref Key_Locked_Solenoid, "Key_Locked_Solenoid");
                                if (Key_Locked_Solenoid != 0)
                                {
                                    textBox91.Text = Key_Locked_Solenoid.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Key_Locked_Solenoid_FLAG);
                            }
                            if (((data[5] & 0x10) != 0x10) && checkBox92.Checked)
                            {
                                ErrorAdd(ref Shifter_Lock_Solenoid_FLAG, ref Shifter_Lock_Solenoid, "Shifter_Lock_Solenoid");
                                if (Shifter_Lock_Solenoid != 0)
                                {
                                    textBox92.Text = Shifter_Lock_Solenoid.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Shifter_Lock_Solenoid_FLAG);
                            }
                            if (((data[5] & 0x08) != 0x08) && checkBox93.Checked)
                            {
                                ErrorAdd(ref Position_Lamps_L_FLAG, ref Position_Lamps_L, "Position_Lamps_L");
                                if (Position_Lamps_L != 0)
                                {
                                    textBox93.Text = Position_Lamps_L.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Position_Lamps_L_FLAG);
                            }
                            if (((data[5] & 0x04) != 0x04) && checkBox94.Checked)
                            {
                                ErrorAdd(ref Position_Lamps_R_FLAG, ref Position_Lamps_R, "Position_Lamps_R");
                                if (Position_Lamps_R != 0)
                                {
                                    textBox94.Text = Position_Lamps_R.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Position_Lamps_R_FLAG);
                            }
                            if (((data[5] & 0x02) != 0x02) && checkBox95.Checked)
                            {
                                ErrorAdd(ref CHMSL_FLAG, ref CHMSL, "CHMSL");
                                if (CHMSL != 0)
                                {
                                    textBox95.Text = CHMSL.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref CHMSL_FLAG);
                            }
                            if (((data[5] & 0x01) != 0x01) && checkBox96.Checked)
                            {
                                ErrorAdd(ref License_Plate_Lamps_FLAG, ref License_Plate_Lamps, "License_Plate_Lamps");
                                if (License_Plate_Lamps != 0)
                                {
                                    textBox96.Text = License_Plate_Lamps.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref License_Plate_Lamps_FLAG);
                            }

                            /*Byte4*/
                            if (((data[6] & 0x80) != 0x80) && checkBox97.Checked)
                            {
                                ErrorAdd(ref Trunk_Unlock_FLAG, ref Trunk_Unlock, "Trunk_Unlock");
                                if (Trunk_Unlock != 0)
                                {
                                    textBox97.Text = Trunk_Unlock.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Trunk_Unlock_FLAG);
                            }
                            if (((data[6] & 0x40) != 0x40) && checkBox98.Checked)
                            {
                                ErrorAdd(ref Headlamp_Washer_Relay_FLAG, ref Headlamp_Washer_Relay, "Headlamp_Washer_Relay");
                                if (Headlamp_Washer_Relay != 0)
                                {
                                    textBox98.Text = Headlamp_Washer_Relay.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Headlamp_Washer_Relay_FLAG);
                            }

                            if (((data[6] & 0x20) != 0x20) && checkBox99.Checked)
                            {
                                ErrorAdd(ref Welcome_Lamps_FLAG, ref Welcome_Lamps, "Welcome_Lamps");
                                if (Welcome_Lamps != 0)
                                {
                                    textBox99.Text = Welcome_Lamps.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref Welcome_Lamps_FLAG);
                            }
                            if (((data[6] & 0x10) != 0x10) && checkBox100.Checked)
                            {
                                ErrorAdd(ref PRND_IndicatorCtrl_FLAG, ref PRND_IndicatorCtrl, "PRND_IndicatorCtrl");
                                if (PRND_IndicatorCtrl != 0)
                                {
                                    textBox100.Text = PRND_IndicatorCtrl.ToString();
                                }
                            }
                            else
                            {
                                FlagClear(ref PRND_IndicatorCtrl_FLAG);
                            }
                        }
                        #endregion
                    };
                    break;
                    //case 0xA2:
                    //    {
                    //        /*读电压*/
                    //        #region Battery_Voltage
                    //        if ((data[1] == 0xA0) && (data[2] == 0x02) && checkBox104.Checked)
                    //        {
                    //            if ((data[3] > Battery_Up) || (data[3] < Battery_Down))
                    //            {
                    //                ErrorAdd(ref Battery_Voltage_FLAG, ref Battery_Voltage, "Battery Voltage");
                    //            }
                    //            else
                    //            {
                    //                FlagClear(ref Battery_Voltage_FLAG);
                    //            }
                    //        }
                    //        #endregion
                    //    };
                    //    break;
            }
        }

        /*选中打开或关闭监控*/
        #region CheckedChange
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkBox109.Checked = true;
            checkBox108.Checked = true;
            checkBox107.Checked = true;
            checkBox106.Checked = true;
            checkBox105.Checked = true;
            checkBox103.Checked = true;
            checkBox102.Checked = true;
            checkBox101.Checked = true;
            checkBox100.Checked = true;
            checkBox99.Checked = true;
            checkBox98.Checked = true;
            checkBox97.Checked = true;
            checkBox96.Checked = true;
            checkBox95.Checked = true;
            checkBox94.Checked = true;
            checkBox93.Checked = true;
            checkBox92.Checked = true;
            checkBox91.Checked = true;
            checkBox90.Checked = true;
            checkBox89.Checked = true;
            checkBox88.Checked = true;
            checkBox87.Checked = true;
            checkBox86.Checked = true;
            checkBox85.Checked = true;
            checkBox84.Checked = true;
            checkBox83.Checked = true;
            checkBox82.Checked = true;
            checkBox81.Checked = true;
            checkBox80.Checked = true;
            checkBox79.Checked = true;
            checkBox78.Checked = true;
            checkBox77.Checked = true;
            checkBox76.Checked = true;
            checkBox75.Checked = true;
            checkBox74.Checked = true;
            checkBox73.Checked = true;
            checkBox72.Checked = true;
            checkBox71.Checked = true;
            checkBox70.Checked = true;
            checkBox69.Checked = true;
            checkBox68.Checked = true;
            checkBox67.Checked = true;
            checkBox66.Checked = true;
            checkBox65.Checked = true;
            checkBox64.Checked = true;
            checkBox63.Checked = true;
            checkBox62.Checked = true;
            checkBox61.Checked = true;
            checkBox60.Checked = true;
            checkBox59.Checked = true;
            checkBox58.Checked = true;
            checkBox57.Checked = true;
            checkBox56.Checked = true;
            checkBox55.Checked = true;
            checkBox54.Checked = true;
            checkBox53.Checked = true;
            checkBox52.Checked = true;
            checkBox51.Checked = true;
            checkBox50.Checked = true;
            checkBox49.Checked = true;
            checkBox48.Checked = true;
            checkBox47.Checked = true;
            checkBox46.Checked = true;
        }

        private void cancelAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkBox109.Checked = false;
            checkBox108.Checked = false;
            checkBox107.Checked = false;
            checkBox106.Checked = false;
            checkBox105.Checked = false;
            checkBox103.Checked = false;
            checkBox102.Checked = false;
            checkBox101.Checked = false;
            checkBox100.Checked = false;
            checkBox99.Checked = false;
            checkBox98.Checked = false;
            checkBox97.Checked = false;
            checkBox96.Checked = false;
            checkBox95.Checked = false;
            checkBox94.Checked = false;
            checkBox93.Checked = false;
            checkBox92.Checked = false;
            checkBox91.Checked = false;
            checkBox90.Checked = false;
            checkBox89.Checked = false;
            checkBox88.Checked = false;
            checkBox87.Checked = false;
            checkBox86.Checked = false;
            checkBox85.Checked = false;
            checkBox84.Checked = false;
            checkBox83.Checked = false;
            checkBox82.Checked = false;
            checkBox81.Checked = false;
            checkBox80.Checked = false;
            checkBox79.Checked = false;
            checkBox78.Checked = false;
            checkBox77.Checked = false;
            checkBox76.Checked = false;
            checkBox75.Checked = false;
            checkBox74.Checked = false;
            checkBox73.Checked = false;
            checkBox72.Checked = false;
            checkBox71.Checked = false;
            checkBox70.Checked = false;
            checkBox69.Checked = false;
            checkBox68.Checked = false;
            checkBox67.Checked = false;
            checkBox66.Checked = false;
            checkBox65.Checked = false;
            checkBox64.Checked = false;
            checkBox63.Checked = false;
            checkBox62.Checked = false;
            checkBox61.Checked = false;
            checkBox60.Checked = false;
            checkBox59.Checked = false;
            checkBox58.Checked = false;
            checkBox57.Checked = false;
            checkBox56.Checked = false;
            checkBox55.Checked = false;
            checkBox54.Checked = false;
            checkBox53.Checked = false;
            checkBox52.Checked = false;
            checkBox51.Checked = false;
            checkBox50.Checked = false;
            checkBox49.Checked = false;
            checkBox48.Checked = false;
            checkBox47.Checked = false;
            checkBox46.Checked = false;
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            checkBox32.Checked = true;
            checkBox31.Checked = true;
            checkBox30.Checked = true;
            checkBox29.Checked = true;
            checkBox28.Checked = true;
            checkBox27.Checked = true;
            checkBox26.Checked = true;
            checkBox25.Checked = true;
            checkBox24.Checked = true;
            checkBox23.Checked = true;
            checkBox22.Checked = true;
            checkBox21.Checked = true;
            checkBox20.Checked = true;
            checkBox19.Checked = true;
            checkBox18.Checked = true;
            checkBox17.Checked = true;
            checkBox16.Checked = true;
            checkBox15.Checked = true;
            checkBox14.Checked = true;
            checkBox13.Checked = true;
            checkBox12.Checked = true;
            checkBox11.Checked = true;
            checkBox10.Checked = true;
            checkBox9.Checked = true;
            checkBox8.Checked = true;
            checkBox7.Checked = true;
            checkBox6.Checked = true;
            checkBox5.Checked = true;
            checkBox4.Checked = true;
            checkBox3.Checked = true;
            checkBox2.Checked = true;
            checkBox1.Checked = true;
        }

        private void cancelAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            checkBox32.Checked = false;
            checkBox31.Checked = false;
            checkBox30.Checked = false;
            checkBox29.Checked = false;
            checkBox28.Checked = false;
            checkBox27.Checked = false;
            checkBox26.Checked = false;
            checkBox25.Checked = false;
            checkBox24.Checked = false;
            checkBox23.Checked = false;
            checkBox22.Checked = false;
            checkBox21.Checked = false;
            checkBox20.Checked = false;
            checkBox19.Checked = false;
            checkBox18.Checked = false;
            checkBox17.Checked = false;
            checkBox16.Checked = false;
            checkBox15.Checked = false;
            checkBox14.Checked = false;
            checkBox13.Checked = false;
            checkBox12.Checked = false;
            checkBox11.Checked = false;
            checkBox10.Checked = false;
            checkBox9.Checked = false;
            checkBox8.Checked = false;
            checkBox7.Checked = false;
            checkBox6.Checked = false;
            checkBox5.Checked = false;
            checkBox4.Checked = false;
            checkBox3.Checked = false;
            checkBox2.Checked = false;
            checkBox1.Checked = false;
        }

        private void selectAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            checkBox115.Checked = true;
            checkBox114.Checked = true;
            checkBox113.Checked = true;
            checkBox112.Checked = true;
            checkBox111.Checked = true;
            checkBox110.Checked = true;
            checkBox45.Checked = true;
            checkBox44.Checked = true;
            checkBox43.Checked = true;
            checkBox42.Checked = true;
            checkBox41.Checked = true;
            checkBox40.Checked = true;
            checkBox39.Checked = true;
            checkBox38.Checked = true;
            checkBox37.Checked = true;
            checkBox36.Checked = true;
            checkBox35.Checked = true;
            checkBox34.Checked = true;
            checkBox33.Checked = true;
        }

        private void cancelAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            checkBox115.Checked = false;
            checkBox114.Checked = false;
            checkBox113.Checked = false;
            checkBox112.Checked = false;
            checkBox111.Checked = false;
            checkBox110.Checked = false;
            checkBox45.Checked = false;
            checkBox44.Checked = false;
            checkBox43.Checked = false;
            checkBox42.Checked = false;
            checkBox41.Checked = false;
            checkBox40.Checked = false;
            checkBox39.Checked = false;
            checkBox38.Checked = false;
            checkBox37.Checked = false;
            checkBox36.Checked = false;
            checkBox35.Checked = false;
            checkBox34.Checked = false;
            checkBox33.Checked = false;
        }

        private void checkBox104_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox104.Checked)
            {
                case true:
                    if (!Regex.IsMatch(textBox104.Text, @"\d+"))//输入必须是数字才有效
                    {
                        checkBox104.Checked = false;
                    }
                    else
                    {
                        textBox104.ReadOnly = true;
                        Battery_Up = Convert.ToInt32((float.Parse(textBox104.Text) * 11)) - 3;
                        Battery_Down = Convert.ToInt32((float.Parse(textBox104.Text) * 9)) - 3;
                    }
                    break;
                case false:
                    textBox104.ReadOnly = false;
                    break;
            }

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox1.Checked)
            {
                case true:
                    textBox1.ReadOnly = true;
                    break;
                case false:
                    textBox1.ReadOnly = false;
                    break;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox3.Checked)
            {
                case true:
                    textBox3.ReadOnly = true;
                    break;
                case false:
                    textBox3.ReadOnly = false;
                    break;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox7.Checked)
            {
                case true:
                    textBox7.ReadOnly = true;
                    break;
                case false:
                    textBox7.ReadOnly = false;
                    break;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox10.Checked)
            {
                case true:
                    textBox10.ReadOnly = true;
                    break;
                case false:
                    textBox10.ReadOnly = false;
                    break;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox9.Checked)
            {
                case true:
                    textBox9.ReadOnly = true;
                    break;
                case false:
                    textBox9.ReadOnly = false;
                    break;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox8.Checked)
            {
                case true:
                    textBox8.ReadOnly = true;
                    break;
                case false:
                    textBox8.ReadOnly = false;
                    break;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox11.Checked)
            {
                case true:
                    textBox11.ReadOnly = true;
                    break;
                case false:
                    textBox11.ReadOnly = false;
                    break;
            }
        }

        private void checkBox107_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox107.Checked)
            {
                case true:
                    textBox107.ReadOnly = true;
                    break;
                case false:
                    textBox107.ReadOnly = false;
                    break;
            }
        }

        private void checkBox108_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox108.Checked)
            {
                case true:
                    textBox108.ReadOnly = true;
                    break;
                case false:
                    textBox108.ReadOnly = false;
                    break;
            }
        }

        private void checkBox109_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox109.Checked)
            {
                case true:
                    textBox109.ReadOnly = true;
                    break;
                case false:
                    textBox109.ReadOnly = false;
                    break;
            }
        }

        private void checkBox101_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox101.Checked)
            {
                case true:
                    textBox101.ReadOnly = true;
                    break;
                case false:
                    textBox101.ReadOnly = false;
                    break;
            }
        }

        private void checkBox103_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox103.Checked)
            {
                case true:
                    textBox103.ReadOnly = true;
                    break;
                case false:
                    textBox103.ReadOnly = false;
                    break;
            }
        }

        private void checkBox102_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox102.Checked)
            {
                case true:
                    textBox102.ReadOnly = true;
                    break;
                case false:
                    textBox102.ReadOnly = false;
                    break;
            }
        }

        private void checkBox105_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox105.Checked)
            {
                case true:
                    textBox105.ReadOnly = true;
                    break;
                case false:
                    textBox105.ReadOnly = false;
                    break;
            }
        }

        private void checkBox106_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox106.Checked)
            {
                case true:
                    textBox106.ReadOnly = true;
                    break;
                case false:
                    textBox106.ReadOnly = false;
                    break;
            }
        }

        private void checkBox110_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox110.Checked)
            {
                case true:
                    textBox110.ReadOnly = true;
                    break;
                case false:
                    textBox110.ReadOnly = false;
                    break;
            }
        }

        private void checkBox112_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox112.Checked)
            {
                case true:
                    textBox112.ReadOnly = true;
                    break;
                case false:
                    textBox112.ReadOnly = false;
                    break;
            }
        }

        private void checkBox111_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox111.Checked)
            {
                case true:
                    textBox111.ReadOnly = true;
                    break;
                case false:
                    textBox111.ReadOnly = false;
                    break;
            }
        }

        private void checkBox113_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox113.Checked)
            {
                case true:
                    textBox113.ReadOnly = true;
                    break;
                case false:
                    textBox113.ReadOnly = false;
                    break;
            }
        }

        private void checkBox114_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox114.Checked)
            {
                case true:
                    textBox114.ReadOnly = true;
                    break;
                case false:
                    textBox114.ReadOnly = false;
                    break;
            }
        }

        private void checkBox115_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox115.Checked)
            {
                case true:
                    textBox115.ReadOnly = true;
                    break;
                case false:
                    textBox115.ReadOnly = false;
                    break;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox6.Checked)
            {
                case true:
                    textBox6.ReadOnly = true;
                    break;
                case false:
                    textBox6.ReadOnly = false;
                    break;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox5.Checked)
            {
                case true:
                    textBox5.ReadOnly = true;
                    break;
                case false:
                    textBox5.ReadOnly = false;
                    break;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox2.Checked)
            {
                case true:
                    textBox2.ReadOnly = true;
                    break;
                case false:
                    textBox2.ReadOnly = false;
                    break;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox4.Checked)
            {
                case true:
                    textBox4.ReadOnly = true;
                    break;
                case false:
                    textBox4.ReadOnly = false;
                    break;
            }
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox30.Checked)
            {
                case true:
                    textBox30.ReadOnly = true;
                    break;
                case false:
                    textBox30.ReadOnly = false;
                    break;
            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox31.Checked)
            {
                case true:
                    textBox31.ReadOnly = true;
                    break;
                case false:
                    textBox31.ReadOnly = false;
                    break;
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox32.Checked)
            {
                case true:
                    textBox32.ReadOnly = true;
                    break;
                case false:
                    textBox32.ReadOnly = false;
                    break;
            }
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox28.Checked)
            {
                case true:
                    textBox28.ReadOnly = true;
                    break;
                case false:
                    textBox28.ReadOnly = false;
                    break;
            }
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox27.Checked)
            {
                case true:
                    textBox27.ReadOnly = true;
                    break;
                case false:
                    textBox27.ReadOnly = false;
                    break;
            }
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox26.Checked)
            {
                case true:
                    textBox26.ReadOnly = true;
                    break;
                case false:
                    textBox26.ReadOnly = false;
                    break;
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox25.Checked)
            {
                case true:
                    textBox25.ReadOnly = true;
                    break;
                case false:
                    textBox25.ReadOnly = false;
                    break;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox16.Checked)
            {
                case true:
                    textBox16.ReadOnly = true;
                    break;
                case false:
                    textBox16.ReadOnly = false;
                    break;
            }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox17.Checked)
            {
                case true:
                    textBox17.ReadOnly = true;
                    break;
                case false:
                    textBox17.ReadOnly = false;
                    break;
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox18.Checked)
            {
                case true:
                    textBox18.ReadOnly = true;
                    break;
                case false:
                    textBox18.ReadOnly = false;
                    break;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox19.Checked)
            {
                case true:
                    textBox19.ReadOnly = true;
                    break;
                case false:
                    textBox19.ReadOnly = false;
                    break;
            }
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox20.Checked)
            {
                case true:
                    textBox20.ReadOnly = true;
                    break;
                case false:
                    textBox20.ReadOnly = false;
                    break;
            }
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox21.Checked)
            {
                case true:
                    textBox21.ReadOnly = true;
                    break;
                case false:
                    textBox21.ReadOnly = false;
                    break;
            }
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox22.Checked)
            {
                case true:
                    textBox22.ReadOnly = true;
                    break;
                case false:
                    textBox22.ReadOnly = false;
                    break;
            }
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox23.Checked)
            {
                case true:
                    textBox23.ReadOnly = true;
                    break;
                case false:
                    textBox23.ReadOnly = false;
                    break;
            }
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox24.Checked)
            {
                case true:
                    textBox24.ReadOnly = true;
                    break;
                case false:
                    textBox24.ReadOnly = false;
                    break;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox15.Checked)
            {
                case true:
                    textBox15.ReadOnly = true;
                    break;
                case false:
                    textBox15.ReadOnly = false;
                    break;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox14.Checked)
            {
                case true:
                    textBox14.ReadOnly = true;
                    break;
                case false:
                    textBox14.ReadOnly = false;
                    break;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox13.Checked)
            {
                case true:
                    textBox13.ReadOnly = true;
                    break;
                case false:
                    textBox13.ReadOnly = false;
                    break;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox12.Checked)
            {
                case true:
                    textBox12.ReadOnly = true;
                    break;
                case false:
                    textBox12.ReadOnly = false;
                    break;
            }
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox29.Checked)
            {
                case true:
                    textBox29.ReadOnly = true;
                    break;
                case false:
                    textBox29.ReadOnly = false;
                    break;
            }
        }

        private void checkBox45_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox45.Checked)
            {
                case true:
                    textBox45.ReadOnly = true;
                    break;
                case false:
                    textBox45.ReadOnly = false;
                    break;
            }
        }

        private void checkBox44_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox44.Checked)
            {
                case true:
                    textBox44.ReadOnly = true;
                    break;
                case false:
                    textBox44.ReadOnly = false;
                    break;
            }
        }

        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox43.Checked)
            {
                case true:
                    textBox43.ReadOnly = true;
                    break;
                case false:
                    textBox43.ReadOnly = false;
                    break;
            }
        }

        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox42.Checked)
            {
                case true:
                    textBox42.ReadOnly = true;
                    break;
                case false:
                    textBox42.ReadOnly = false;
                    break;
            }
        }

        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox41.Checked)
            {
                case true:
                    textBox41.ReadOnly = true;
                    break;
                case false:
                    textBox41.ReadOnly = false;
                    break;
            }
        }

        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox40.Checked)
            {
                case true:
                    textBox40.ReadOnly = true;
                    break;
                case false:
                    textBox40.ReadOnly = false;
                    break;
            }
        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox39.Checked)
            {
                case true:
                    textBox39.ReadOnly = true;
                    break;
                case false:
                    textBox39.ReadOnly = false;
                    break;
            }
        }

        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox38.Checked)
            {
                case true:
                    textBox38.ReadOnly = true;
                    break;
                case false:
                    textBox38.ReadOnly = false;
                    break;
            }
        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox37.Checked)
            {
                case true:
                    textBox37.ReadOnly = true;
                    break;
                case false:
                    textBox37.ReadOnly = false;
                    break;
            }
        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox36.Checked)
            {
                case true:
                    textBox36.ReadOnly = true;
                    break;
                case false:
                    textBox36.ReadOnly = false;
                    break;
            }
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox35.Checked)
            {
                case true:
                    textBox35.ReadOnly = true;
                    break;
                case false:
                    textBox35.ReadOnly = false;
                    break;
            }
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox34.Checked)
            {
                case true:
                    textBox34.ReadOnly = true;
                    break;
                case false:
                    textBox34.ReadOnly = false;
                    break;
            }
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox33.Checked)
            {
                case true:
                    textBox33.ReadOnly = true;
                    break;
                case false:
                    textBox33.ReadOnly = false;
                    break;
            }
        }

        private void checkBox73_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox73.Checked)
            {
                case true:
                    textBox73.ReadOnly = true;
                    break;
                case false:
                    textBox73.ReadOnly = false;
                    break;
            }
        }

        private void checkBox76_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox76.Checked)
            {
                case true:
                    textBox76.ReadOnly = true;
                    break;
                case false:
                    textBox76.ReadOnly = false;
                    break;
            }
        }

        private void checkBox77_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox77.Checked)
            {
                case true:
                    textBox77.ReadOnly = true;
                    break;
                case false:
                    textBox77.ReadOnly = false;
                    break;
            }
        }

        private void checkBox75_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox75.Checked)
            {
                case true:
                    textBox75.ReadOnly = true;
                    break;
                case false:
                    textBox75.ReadOnly = false;
                    break;
            }
        }

        private void checkBox71_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox71.Checked)
            {
                case true:
                    textBox71.ReadOnly = true;
                    break;
                case false:
                    textBox71.ReadOnly = false;
                    break;
            }
        }

        private void checkBox70_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox70.Checked)
            {
                case true:
                    textBox70.ReadOnly = true;
                    break;
                case false:
                    textBox70.ReadOnly = false;
                    break;
            }
        }

        private void checkBox69_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox69.Checked)
            {
                case true:
                    textBox69.ReadOnly = true;
                    break;
                case false:
                    textBox69.ReadOnly = false;
                    break;
            }
        }

        private void checkBox68_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox68.Checked)
            {
                case true:
                    textBox68.ReadOnly = true;
                    break;
                case false:
                    textBox68.ReadOnly = false;
                    break;
            }
        }

        private void checkBox67_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox67.Checked)
            {
                case true:
                    textBox67.ReadOnly = true;
                    break;
                case false:
                    textBox67.ReadOnly = false;
                    break;
            }
        }

        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox66.Checked)
            {
                case true:
                    textBox66.ReadOnly = true;
                    break;
                case false:
                    textBox66.ReadOnly = false;
                    break;
            }
        }

        private void checkBox65_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox65.Checked)
            {
                case true:
                    textBox65.ReadOnly = true;
                    break;
                case false:
                    textBox65.ReadOnly = false;
                    break;
            }
        }

        private void checkBox64_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox64.Checked)
            {
                case true:
                    textBox64.ReadOnly = true;
                    break;
                case false:
                    textBox64.ReadOnly = false;
                    break;
            }
        }

        private void checkBox63_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox63.Checked)
            {
                case true:
                    textBox63.ReadOnly = true;
                    break;
                case false:
                    textBox63.ReadOnly = false;
                    break;
            }
        }

        private void checkBox62_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox62.Checked)
            {
                case true:
                    textBox62.ReadOnly = true;
                    break;
                case false:
                    textBox62.ReadOnly = false;
                    break;
            }
        }

        private void checkBox61_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox61.Checked)
            {
                case true:
                    textBox61.ReadOnly = true;
                    break;
                case false:
                    textBox61.ReadOnly = false;
                    break;
            }
        }

        private void checkBox60_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox60.Checked)
            {
                case true:
                    textBox60.ReadOnly = true;
                    break;
                case false:
                    textBox60.ReadOnly = false;
                    break;
            }
        }

        private void checkBox59_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox59.Checked)
            {
                case true:
                    textBox59.ReadOnly = true;
                    break;
                case false:
                    textBox59.ReadOnly = false;
                    break;
            }
        }

        private void checkBox58_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox58.Checked)
            {
                case true:
                    textBox58.ReadOnly = true;
                    break;
                case false:
                    textBox58.ReadOnly = false;
                    break;
            }
        }

        private void checkBox57_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox57.Checked)
            {
                case true:
                    textBox57.ReadOnly = true;
                    break;
                case false:
                    textBox57.ReadOnly = false;
                    break;
            }
        }

        private void checkBox56_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox56.Checked)
            {
                case true:
                    textBox56.ReadOnly = true;
                    break;
                case false:
                    textBox56.ReadOnly = false;
                    break;
            }
        }

        private void checkBox55_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox55.Checked)
            {
                case true:
                    textBox55.ReadOnly = true;
                    break;
                case false:
                    textBox55.ReadOnly = false;
                    break;
            }
        }

        private void checkBox54_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox54.Checked)
            {
                case true:
                    textBox54.ReadOnly = true;
                    break;
                case false:
                    textBox54.ReadOnly = false;
                    break;
            }
        }

        private void checkBox53_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox53.Checked)
            {
                case true:
                    textBox53.ReadOnly = true;
                    break;
                case false:
                    textBox53.ReadOnly = false;
                    break;
            }
        }

        private void checkBox52_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox52.Checked)
            {
                case true:
                    textBox52.ReadOnly = true;
                    break;
                case false:
                    textBox52.ReadOnly = false;
                    break;
            }
        }

        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox51.Checked)
            {
                case true:
                    textBox51.ReadOnly = true;
                    break;
                case false:
                    textBox51.ReadOnly = false;
                    break;
            }
        }

        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox50.Checked)
            {
                case true:
                    textBox50.ReadOnly = true;
                    break;
                case false:
                    textBox50.ReadOnly = false;
                    break;
            }
        }

        private void checkBox49_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox49.Checked)
            {
                case true:
                    textBox49.ReadOnly = true;
                    break;
                case false:
                    textBox49.ReadOnly = false;
                    break;
            }
        }

        private void checkBox48_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox48.Checked)
            {
                case true:
                    textBox48.ReadOnly = true;
                    break;
                case false:
                    textBox48.ReadOnly = false;
                    break;
            }
        }

        private void checkBox47_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox47.Checked)
            {
                case true:
                    textBox47.ReadOnly = true;
                    break;
                case false:
                    textBox47.ReadOnly = false;
                    break;
            }
        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox46.Checked)
            {
                case true:
                    textBox46.ReadOnly = true;
                    break;
                case false:
                    textBox46.ReadOnly = false;
                    break;
            }
        }

        private void checkBox74_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox74.Checked)
            {
                case true:
                    textBox74.ReadOnly = true;
                    break;
                case false:
                    textBox74.ReadOnly = false;
                    break;
            }
        }

        private void checkBox72_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox72.Checked)
            {
                case true:
                    textBox72.ReadOnly = true;
                    break;
                case false:
                    textBox72.ReadOnly = false;
                    break;
            }
        }

        private void checkBox96_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox96.Checked)
            {
                case true:
                    textBox96.ReadOnly = true;
                    break;
                case false:
                    textBox96.ReadOnly = false;
                    break;
            }
        }

        private void checkBox97_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox97.Checked)
            {
                case true:
                    textBox97.ReadOnly = true;
                    break;
                case false:
                    textBox97.ReadOnly = false;
                    break;
            }
        }

        private void checkBox98_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox98.Checked)
            {
                case true:
                    textBox98.ReadOnly = true;
                    break;
                case false:
                    textBox98.ReadOnly = false;
                    break;
            }
        }

        private void checkBox99_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox99.Checked)
            {
                case true:
                    textBox99.ReadOnly = true;
                    break;
                case false:
                    textBox99.ReadOnly = false;
                    break;
            }
        }

        private void checkBox100_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox100.Checked)
            {
                case true:
                    textBox100.ReadOnly = true;
                    break;
                case false:
                    textBox100.ReadOnly = false;
                    break;
            }
        }

        private void checkBox87_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox87.Checked)
            {
                case true:
                    textBox87.ReadOnly = true;
                    break;
                case false:
                    textBox87.ReadOnly = false;
                    break;
            }
        }

        private void checkBox88_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox88.Checked)
            {
                case true:
                    textBox88.ReadOnly = true;
                    break;
                case false:
                    textBox88.ReadOnly = false;
                    break;
            }
        }

        private void checkBox89_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox89.Checked)
            {
                case true:
                    textBox89.ReadOnly = true;
                    break;
                case false:
                    textBox89.ReadOnly = false;
                    break;
            }
        }

        private void checkBox90_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox90.Checked)
            {
                case true:
                    textBox90.ReadOnly = true;
                    break;
                case false:
                    textBox90.ReadOnly = false;
                    break;
            }
        }

        private void checkBox91_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox91.Checked)
            {
                case true:
                    textBox91.ReadOnly = true;
                    break;
                case false:
                    textBox91.ReadOnly = false;
                    break;
            }
        }

        private void checkBox92_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox92.Checked)
            {
                case true:
                    textBox92.ReadOnly = true;
                    break;
                case false:
                    textBox92.ReadOnly = false;
                    break;
            }
        }

        private void checkBox93_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox93.Checked)
            {
                case true:
                    textBox93.ReadOnly = true;
                    break;
                case false:
                    textBox93.ReadOnly = false;
                    break;
            }
        }

        private void checkBox94_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox94.Checked)
            {
                case true:
                    textBox94.ReadOnly = true;
                    break;
                case false:
                    textBox94.ReadOnly = false;
                    break;
            }
        }

        private void checkBox95_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox95.Checked)
            {
                case true:
                    textBox95.ReadOnly = true;
                    break;
                case false:
                    textBox95.ReadOnly = false;
                    break;
            }
        }

        private void checkBox78_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox78.Checked)
            {
                case true:
                    textBox78.ReadOnly = true;
                    break;
                case false:
                    textBox78.ReadOnly = false;
                    break;
            }
        }

        private void checkBox79_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox79.Checked)
            {
                case true:
                    textBox79.ReadOnly = true;
                    break;
                case false:
                    textBox79.ReadOnly = false;
                    break;
            }
        }

        private void checkBox80_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox80.Checked)
            {
                case true:
                    textBox80.ReadOnly = true;
                    break;
                case false:
                    textBox80.ReadOnly = false;
                    break;
            }
        }

        private void checkBox81_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox81.Checked)
            {
                case true:
                    textBox81.ReadOnly = true;
                    break;
                case false:
                    textBox81.ReadOnly = false;
                    break;
            }
        }

        private void checkBox82_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox82.Checked)
            {
                case true:
                    textBox82.ReadOnly = true;
                    break;
                case false:
                    textBox82.ReadOnly = false;
                    break;
            }
        }

        private void checkBox83_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox83.Checked)
            {
                case true:
                    textBox83.ReadOnly = true;
                    break;
                case false:
                    textBox83.ReadOnly = false;
                    break;
            }
        }

        private void checkBox84_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox84.Checked)
            {
                case true:
                    textBox84.ReadOnly = true;
                    break;
                case false:
                    textBox84.ReadOnly = false;
                    break;
            }
        }

        private void checkBox85_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox85.Checked)
            {
                case true:
                    textBox85.ReadOnly = true;
                    break;
                case false:
                    textBox85.ReadOnly = false;
                    break;
            }
        }

        private void checkBox86_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBox86.Checked)
            {
                case true:
                    textBox86.ReadOnly = true;
                    break;
                case false:
                    textBox86.ReadOnly = false;
                    break;
            }
        }

        private void checkBox16_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox16.Checked)
            {
                case true:
                    textBox16.ReadOnly = true;
                    break;
                case false:
                    textBox16.ReadOnly = false;
                    break;
            }
        }

        private void checkBox17_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox17.Checked)
            {
                case true:
                    textBox17.ReadOnly = true;
                    break;
                case false:
                    textBox17.ReadOnly = false;
                    break;
            }
        }

        private void checkBox31_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox31.Checked)
            {
                case true:
                    textBox31.ReadOnly = true;
                    break;
                case false:
                    textBox31.ReadOnly = false;
                    break;
            }
        }

        private void checkBox30_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox30.Checked)
            {
                case true:
                    textBox30.ReadOnly = true;
                    break;
                case false:
                    textBox30.ReadOnly = false;
                    break;
            }
        }

        private void checkBox29_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox29.Checked)
            {
                case true:
                    textBox29.ReadOnly = true;
                    break;
                case false:
                    textBox29.ReadOnly = false;
                    break;
            }
        }

        private void checkBox32_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox32.Checked)
            {
                case true:
                    textBox32.ReadOnly = true;
                    break;
                case false:
                    textBox32.ReadOnly = false;
                    break;
            }
        }

        private void checkBox28_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox28.Checked)
            {
                case true:
                    textBox28.ReadOnly = true;
                    break;
                case false:
                    textBox28.ReadOnly = false;
                    break;
            }
        }

        private void checkBox27_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox27.Checked)
            {
                case true:
                    textBox27.ReadOnly = true;
                    break;
                case false:
                    textBox27.ReadOnly = false;
                    break;
            }
        }

        private void checkBox26_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox26.Checked)
            {
                case true:
                    textBox26.ReadOnly = true;
                    break;
                case false:
                    textBox26.ReadOnly = false;
                    break;
            }
        }

        private void checkBox25_CheckedChanged_1(object sender, EventArgs e)
        {
            switch (checkBox25.Checked)
            {
                case true:
                    textBox25.ReadOnly = true;
                    break;
                case false:
                    textBox25.ReadOnly = false;
                    break;
            }
        }
        #endregion
    }
}