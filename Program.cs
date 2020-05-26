using NeuInvent.NMS.Tests.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NeuInvent.NMS.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region 打开设备(第一步)

                string logFile = "测试.log";

                var suc = LotusAPI.OpenDevices(logFile);
                if (suc == (int)StatusEnum.Success)
                {
                    Console.WriteLine("打开设备成功");
                }
                else
                {
                    Console.WriteLine($"打开设备失败");
                }

                #endregion

                #region 关闭设备

                var closeSuc = LotusAPI.CloseDevices();
                if (closeSuc)
                {
                    Console.WriteLine("关闭设备成功");
                }
                else
                {
                    Console.WriteLine($"关闭设备失败");
                }

                #endregion

                #region 查找采集硬件(#无法得到想要的值 manufacturer_id_buffer和sn_buffer)

                int size_buffer = 10;//# 传多少？

                string manufacturer_id_buffer = string.Empty;
                string sn_buffer = string.Empty;




                int fhwSuc = LotusAPI.FindAcquisitionHardware(ref manufacturer_id_buffer, ref sn_buffer, size_buffer);

                if (fhwSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.FindAcquisitionHardware)}调用成功！");
                }
                //manufacturer_id_buffer = Marshal.AllocHGlobal(size_buffer);

                //sn_buffer = Marshal.AllocHGlobal(size_buffer);

                //if (manufacturer_id_buffer != IntPtr.Zero)
                //{
                //    s_manufacturer_id_buffer = Marshal.PtrToStringUni(manufacturer_id_buffer);
                //    Marshal.FreeHGlobal(manufacturer_id_buffer);//释放内存

                //    s_sn_buffer = Marshal.PtrToStringUni(sn_buffer);
                //    Marshal.FreeHGlobal(sn_buffer);//释放内存
                //    //uint len = 0;
                //    //while (Marshal.ReadByte(manufacturer_id_buffer, (int)len) != 0)
                //    //{
                //    //    len++;
                //    //}

                //    //byte[] buffer = new byte[len];
                //    //Marshal.Copy(manufacturer_id_buffer, buffer, 0, buffer.Length);
                //    //string s = Encoding.Default.GetString(buffer);
                //}




                //Console.WriteLine($"manufacturer_id_buffer:{s_manufacturer_id_buffer}");
                //Console.WriteLine($"sn_buffer:{s_sn_buffer}");

                if (fhwSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine("查找采集硬件完成");
                }
                else
                {
                    string errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(fhwSuc));
                    Console.WriteLine($"查找采集硬件失败,错误码:{fhwSuc},错误信息:{errMsg}");
                }


                #endregion

                #region 采集模块自检

                int device_info_size = 10;//# 传多少？

                byte[] device_info = new byte[device_info_size];

                int acqSelfCheck = LotusAPI.ReportAcquisitionSelfCheck(device_info);

                if (acqSelfCheck == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.ReportAcquisitionSelfCheck)}调用成功！");
                    Console.WriteLine($"{Encoding.UTF8.GetString(device_info)}");
                }
                else
                {
                    Console.WriteLine($"{nameof(LotusAPI.ReportAcquisitionSelfCheck)}调用失败,错误码：{acqSelfCheck}");
                }

                #endregion

                #region 查找控制硬件(#同采集硬件)

                /*
                string manufacturer_id_buffer1 = string.Empty;
                string sn_buffer1 = string.Empty;
                int fhwSuc1 = LotusAPI.FindControlHardware(ref manufacturer_id_buffer1, ref sn_buffer1);

                if (fhwSuc1 == (int)StatusEnum.Success)
                {
                    Console.WriteLine("查找控制硬件完成");
                }
                else
                {
                    string errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(fhwSuc1));
                    Console.WriteLine($"查找控制硬件失败,错误码:{fhwSuc1},错误信息:{errMsg}");
                }

                //*/


                #endregion

                #region 查找刺激硬件(#同采集硬件)
                /*
                string manufacturer_id_buffer2 = string.Empty;
                string sn_buffer2 = string.Empty;
                int fhwSuc2 = LotusAPI.FindStimulationHardware(ref manufacturer_id_buffer2, ref sn_buffer2);

                if (fhwSuc2 == (int)StatusEnum.Success)
                {
                    Console.WriteLine("查找刺激硬件完成");
                }
                else
                {
                    var errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(fhwSuc2));
                    Console.WriteLine($"查找刺激硬件失败,错误码:{fhwSuc2},错误信息:{errMsg}");
                }

                //*/


                #endregion

                #region 获取刺激通道信息（#数组大小）


                int array_size = 10;//#如何确定大小
                ulong[] ch_id_vector = new ulong[array_size];

                int stimChannelInfoSuc = LotusAPI.GetStimChannelInfo(ch_id_vector, array_size);

                if (stimChannelInfoSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine("获取刺激通道信息成功");
                }
                else
                {
                    var errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(stimChannelInfoSuc));
                    Console.WriteLine($"查找刺激硬件失败,错误码:{stimChannelInfoSuc},错误信息:{errMsg}");
                }

                //if (ptr_channelInfo != null)
                //{
                //    Byte[] recordHandles = new Byte[100 * sizeof(UInt64)];
                //    Marshal.Copy(ptr_channelInfo, recordHandles, 0, recordHandles.Length);

                //    Int64[] int64Array = new Int64[100];
                //    Buffer.BlockCopy(recordHandles, 0, int64Array, 0, recordHandles.Length);
                //}

                #endregion

                #region 设置刺激参数

                uint s_stim_ch = 0;
                int s_size = Marshal.SizeOf(typeof(CStimulationTrain));

                CStimulationTrain train = new CStimulationTrain();
                train.m_delay = 0f;
                train.m_num_pulses = 0f;
                train.m_pulse_width = 0f;
                train.m_pulse_off_time = 0f; // m_pulse_off_time + m_pulse_width = pulse Period 
                train.m_num_trains = 0f;  // set to 0 for continous trains
                train.m_intertrain_duration = 0f;
                train.m_pulse_amplitude = 0f;
                train.m_stim_mode = Stim_mode.CC20;
                train.m_start_with_recording = true;
                train.m_bipolar = true;

                var by_buffer = LotusAPI.ConverStructureToBytes(train);

                LotusAPI.SetStimulationParameters(s_stim_ch, by_buffer);


                #endregion

                #region 获取或设置控制盒参数

                int stim_index = 0;

                CControlStimulationParameters cControlStimulationParameters = new CControlStimulationParameters();
                cControlStimulationParameters.m_alarm = true;
                cControlStimulationParameters.m_stim_intensity = 1f;
                cControlStimulationParameters.m_stim_mode = Stim_mode.CC20;

                var by_buffer_control = LotusAPI.ConverStructureToBytes(cControlStimulationParameters);

                LotusAPI.SetControlStimulatorParameters(stim_index, by_buffer_control);

                /************************************/

                int new_by_buffer_control_size = Marshal.SizeOf(typeof(CControlStimulationParameters));

                var new_by_buffer_control = new byte[new_by_buffer_control_size];

                LotusAPI.GetControlStimulatorParameters(stim_index, new_by_buffer_control);

                var newCControlStimulationParameters_by = LotusAPI.ConverBytesToStructure<CControlStimulationParameters>(new_by_buffer_control);

                #endregion

                #region 获取采集通道数据

                int num_samples_acquired_per_channel = 0;
                //IntPtr intPtr_Data = IntPtr.Zero;
                int data_size = 16000;//#数组大小16000报错

                float[] data = new float[data_size];

                var readDataSuc = LotusAPI.ReadDataFromAcquisitionDevice(ref num_samples_acquired_per_channel, data, data_size);
                if (readDataSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.ReadDataFromAcquisitionDevice)}调用成功！");
                }
                //intPtr_Data = Marshal.AllocHGlobal(data_size);

                //if (intPtr_Data != IntPtr.Zero)
                //{

                //    float[] data = new float[data_size];
                //    Marshal.Copy(intPtr_Data, data, 0, data_size);

                //    //uint len = 0;
                //    //while (Marshal.ReadByte(manufacturer_id_buffer, (int)len) != 0)
                //    //{
                //    //    len++;
                //    //}

                //    //byte[] buffer = new byte[len];
                //    //Marshal.Copy(manufacturer_id_buffer, buffer, 0, buffer.Length);
                //    //string s = Encoding.Default.GetString(buffer);
                //}


                #endregion

                #region 获取刺激通道数据

                int t_num_samples_acquired_per_channel = 0;
                //IntPtr intPtr_Data = IntPtr.Zero;
                int t_data_size = 16000;//#数组大小16000报错

                float[] t_data = new float[data_size];

                var t_readDataSuc = LotusAPI.ReadDataFromStimulatorDevice(ref t_num_samples_acquired_per_channel, t_data, t_data_size);
                if (t_readDataSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.ReadDataFromStimulatorDevice)}调用成功！");
                }
                #endregion

                #region 陷波滤波器

                int arrLength = 1000;
                float[] input_array = new float[arrLength];

                for (int i = 0; i < input_array.Length; i++)
                {
                    Random random = new Random(i);
                    input_array[i] = Convert.ToSingle(random.NextDouble());
                }

                float[] out_array = new float[arrLength];
                var notchFilterSuc = LotusAPI.NotchFilterData(input_array, out_array, arrLength, 4000, 50, true, true);
                if (notchFilterSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.NotchFilterData)}调用成功！");
                }

                #endregion

                #region 带通滤波器

                int d_arrLength = 1000;
                float lowcutoff = 1f;
                float highcutoff = 2f;
                int order = 1;
                float[] d_input_array = new float[arrLength];

                for (int i = 0; i < d_input_array.Length; i++)
                {
                    Random random = new Random(i);
                    d_input_array[i] = Convert.ToSingle(random.NextDouble());
                }

                float[] d_out_array = new float[arrLength];
                var filterSuc = LotusAPI.FilterData(d_input_array, d_out_array, d_arrLength, 4000, lowcutoff, highcutoff, order);
                if (filterSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.FilterData)}调用成功！");
                }


                #endregion

                #region 刺激

                uint ch_index = 0;
                var fireSuc = LotusAPI.FireStimulator(ch_index);
                if (fireSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.FireStimulator)}调用成功！");
                }
                else
                {
                    var errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(fireSuc));
                    Console.WriteLine($"{nameof(LotusAPI.FireStimulator)}查找刺激硬件失败,错误码:{fireSuc},错误信息:{errMsg}");
                }

                #endregion

                #region 刺激阻抗

                int stim_ch_index = 0;
                var stimChannelImpedanceSuc = LotusAPI.GetStimChannelImpedance(stim_ch_index);
                if (stimChannelImpedanceSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.GetStimChannelImpedance)}调用成功！");
                }
                else
                {
                    var errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(stimChannelImpedanceSuc));
                    Console.WriteLine($"{nameof(LotusAPI.GetStimChannelImpedance)}查找刺激硬件失败,错误码:{stimChannelImpedanceSuc},错误信息:{errMsg}");
                }

                #endregion

                #region 采集通道信息

                int acq_ch_index = 0;
                var acquisitionChannelInfoSuc = LotusAPI.GetAcquisitionChannelInfo(acq_ch_index);
                if (acquisitionChannelInfoSuc == (int)StatusEnum.Success)
                {
                    Console.WriteLine($"{nameof(LotusAPI.GetAcquisitionChannelInfo)}调用成功！");
                }
                else
                {
                    var errMsg = Marshal.PtrToStringUni(LotusAPI.GetErrorMessage(acquisitionChannelInfoSuc));
                    Console.WriteLine($"{nameof(LotusAPI.GetAcquisitionChannelInfo)}查找刺激硬件失败,错误码:{acquisitionChannelInfoSuc},错误信息:{errMsg}");
                }

                #endregion

                #region 音频音量设置



                #endregion



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }
    }
}
