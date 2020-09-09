using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WFC.SelfServeClient.Helper
{
    public delegate void SnapShotEventHandler(object sender, Bitmap snapshot);

    public class CameraCaptureHelper
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoDevice;
        private VideoCapabilities[] videoCapabilities;
        private VideoCapabilities[] snapshotCapabilities;
        private VideoSourcePlayer videoSourcePlayer;

        public List<Size> SnapshotResolutions { get; set; } = new List<Size>();
        public List<Size> VideoResolutions { get; set; } = new List<Size>();
        /// <summary>
        /// 抓拍事件，从子进程调用，如果进行显示的话，需要Invoke到主线程执行
        /// </summary>
        public event SnapShotEventHandler OnSnapShot;

        public CameraCaptureHelper(VideoSourcePlayer videoSourcePlayer)
        {
            this.videoSourcePlayer = videoSourcePlayer;
            // enumerate video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                throw new Exception("没有找到摄像头");
            }

            videoDevice = new VideoCaptureDevice(videoDevices[0].MonikerString);

            videoCapabilities = videoDevice.VideoCapabilities;
            snapshotCapabilities = videoDevice.SnapshotCapabilities;

            foreach (VideoCapabilities capabilty in videoCapabilities)
            {
                VideoResolutions.Add(new Size(capabilty.FrameSize.Width, capabilty.FrameSize.Height));
            }

            foreach (VideoCapabilities capabilty in snapshotCapabilities)
            {
                SnapshotResolutions.Add(new Size(capabilty.FrameSize.Width, capabilty.FrameSize.Height));
            }
        }

        /// <summary>
        /// 连接摄像头
        /// </summary>
        /// <param name="videoResolutionIndex"></param>
        /// <param name="snapshotResolutionIndex"></param>
        public void Connect(int videoResolutionIndex = 0, int snapshotResolutionIndex = 0)
        {
            videoDevice.ProvideSnapshots = true;
            //videoDevice.SnapshotResolution = snapshotCapabilities[snapshotResolutionIndex];
            videoDevice.SnapshotFrame += VideoDevice_SnapshotFrame;

            if (videoSourcePlayer != null)
            {
                videoSourcePlayer.VideoSource = videoDevice;
                videoSourcePlayer.Start();
            }
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void Disconnect()
        {
            if (videoSourcePlayer?.VideoSource != null)
            {
                // stop video device
                videoSourcePlayer.SignalToStop();
                videoSourcePlayer.WaitForStop();
                videoSourcePlayer.VideoSource = null;
            }

            if (videoDevice.ProvideSnapshots)
            {
                videoDevice.SnapshotFrame -= VideoDevice_SnapshotFrame;
            }
        }

        /// <summary>
        /// 抓拍，图形通过OnSnapShot事件回调返回
        /// </summary>
        public void Snapshot()
        {
            if ((videoDevice != null) && (videoDevice.ProvideSnapshots))
            {
                videoDevice.SimulateTrigger();
            }
        }

        private void VideoDevice_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            OnSnapShot?.Invoke(sender, (Bitmap)eventArgs.Frame.Clone());
        }
    }
}
