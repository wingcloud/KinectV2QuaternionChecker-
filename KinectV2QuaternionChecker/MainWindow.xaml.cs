using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace KinectV2QuaternionChecker
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor kinectSensor = null;

        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Reader for body frames
        /// </summary>
        private BodyFrameReader reader = null;

        /// <summary>
        /// Array for the bodies
        /// </summary>
        private Body[] bodies = null;

        private JointType LookingJoint = JointType.Head;//The Orientation of Head is always zero <=> initial state

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.kinectSensor = KinectSensor.GetDefault();
                this.kinectSensor.Open();
                this.kinectSensor.IsAvailableChanged += kinectSensor_IsAvailableChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        void kinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            if (e.IsAvailable)
            {
                if (this.kinectSensor != null)
                {
                    this.coordinateMapper = this.kinectSensor.CoordinateMapper;
                    FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;
                    this.bodies = new Body[this.kinectSensor.BodyFrameSource.BodyCount];
                    this.reader = this.kinectSensor.BodyFrameSource.OpenReader();
                    this.reader.FrameArrived += reader_FrameArrived;
                }
            }
            else
            {
            }
        }

        void reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            BodyFrameReference frameReference = e.FrameReference;
            BodyFrame frame = frameReference.AcquireFrame();

            if (frame != null)
            {
                using (frame)
                {
                    frame.GetAndRefreshBodyData(this.bodies);
                    foreach (Body body in this.bodies)
                    {
                        if (body.IsTracked)
                        {
                            RenewLookingJoint();
                            Vector4 kinectQ = body.JointOrientations[LookingJoint].Orientation;
                            QuaternionRotation3D QRotate = new QuaternionRotation3D();
                            Quaternion Q = new Quaternion();
                            Q.W = kinectQ.W;
                            Q.X = -kinectQ.X;
                            Q.Y = -kinectQ.Y;
                            Q.Z = -kinectQ.Z;
                            QRotate.Quaternion = Q;
                            CuboidRotation.CenterY = -2.5;
                            CuboidRotation.Rotation = QRotate;
                            FrontRotation.CenterY = -3.189610004425049;
                            FrontRotation.CenterZ = -0.5;
                            FrontRotation.Rotation = QRotate;
                            LeftRotation.CenterX = -0.5099999904632568;
                            LeftRotation.CenterY = -3.189610004425049;
                            LeftRotation.Rotation = QRotate;
                            RightRotation.CenterX = 0.5099999904632568;
                            RightRotation.CenterY = -3.189610004425049;
                            RightRotation.Rotation = QRotate;
                            BackRotation.CenterY = -3.189610004425049;
                            BackRotation.CenterZ = 0.5099999904632568;
                            BackRotation.Rotation = QRotate;
                        }
                    }
                }
            }
        }
        private void RenewLookingJoint()
        {
            if (Neck.IsChecked == true)
            {
                LookingJoint = JointType.Neck;
            }
            else if (SpineShoulder.IsChecked == true)
            {
                LookingJoint = JointType.SpineShoulder;
            }
            else if (SpineMid.IsChecked == true)
            {
                LookingJoint = JointType.SpineMid;
            }
            else if (SpineBase.IsChecked == true)
            {
                LookingJoint = JointType.SpineBase;
            }
            else if (ShoulderRight.IsChecked == true)
            {
                LookingJoint = JointType.ShoulderRight;
            }
            else if (ShoulderLeft.IsChecked == true)
            {
                LookingJoint = JointType.ShoulderLeft;
            }
            else if (HipRight.IsChecked == true)
            {
                LookingJoint = JointType.HipRight;
            }
            else if (HipLeft.IsChecked == true)
            {
                LookingJoint = JointType.HipLeft;
            }
            else if (ElbowRight.IsChecked == true)
            {
                LookingJoint = JointType.ElbowRight;
            }
            else if (WristRight.IsChecked == true)
            {
                LookingJoint = JointType.WristRight;
            }
            else if (HandRight.IsChecked == true)
            {
                LookingJoint = JointType.HandRight;
            }
            else if (ElbowLeft.IsChecked == true)
            {
                LookingJoint = JointType.ElbowLeft;
            }
            else if (WristLeft.IsChecked == true)
            {
                LookingJoint = JointType.WristLeft;
            }
            else if (HandLeft.IsChecked == true)
            {
                LookingJoint = JointType.HandLeft;
            }
            else if (KneeRight.IsChecked == true)
            {
                LookingJoint = JointType.KneeRight;
            }
            else if (AnkleRight.IsChecked == true)
            {
                LookingJoint = JointType.AnkleRight;
            }
            else if (KneeLeft.IsChecked == true)
            {
                LookingJoint = JointType.KneeLeft;
            }
            else if (AnkleLeft.IsChecked == true)
            {
                LookingJoint = JointType.AnkleLeft;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.reader != null)
            {
                this.reader.Dispose();
                this.reader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }


    }


}
