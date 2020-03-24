﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_Project.Model;
using WPF_Project.Server;

namespace WPF_Project
{
    class AppModel : IAppModel
    {
        private double positionLongitudeDeg;
        private double positionLatitudeDeg;
        private IJoystickModel joystickModel;
        private double aileron;
        private double throttle;
        private double indicatedHeadingDeg;
        private double gpsIndicatedVerticalSpeed;
        private double gpsIndicatedGroundSpeedKt;
        private double airspeedIndicatorIndicatedSpeedKt;
        private double gpsIndicatedAltitudeFt;
        private double attitudeIndicatorInternalRollDeg;
        private double attitudeIndicatorInternalPitchDeg;
        private double altimeterIndicatedAltitudeFt;

        private string connectionButton;

        IServer server;
        volatile Boolean stop;
        public void stopModel()
        {
            this.stop = true;
            ConnectionButton = "Connect";
        }

        public void startModel()
        {
            this.stop = false;
            ConnectionButton = "Disconnect";
        }
        public bool getStop()
        {
            return this.stop;
        }

        public string ConnectionButton
        {
            get
            {
                return connectionButton;
            }
            set
            {
                connectionButton = value;
                NotifyPropertyChanged("ConnectionButton");
            }
        }


        public double PositionLongitudeDeg
        {
            get { return positionLongitudeDeg; }
            set
            {
                positionLongitudeDeg = value;
                NotifyPropertyChanged("PositionLongitudeDeg");
                // "/position/longitude-deg"
            }
        }
        public double PositionLatitudeDeg
        {
            get { return positionLatitudeDeg; }
            set
            {
                positionLatitudeDeg = value;
                NotifyPropertyChanged("PositionLatitudeDeg");
                // "/position/latitude-deg"
            }
        }
        public IJoystickModel JoystickModel
        {
            get
            {
                return joystickModel;
            }
            set
            {
                joystickModel.controlJoystick(value.Rudder, value.Elevator);
            }
        }
        public double Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
                // "/controls/flight/aileron"
            }
        }
        public double Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
                // "/controls/engines/current-engine/throttle"
            }
        }
        public double IndicatedHeadingDeg
        {
            get { return indicatedHeadingDeg; }
            set
            {
                indicatedHeadingDeg = value;
                NotifyPropertyChanged("IndicatedHeadingDeg");
                // "/instrumentation/heading-indicator/indicated-heading-deg"
            }
        }
        public double GpsIndicatedVerticalSpeed
        {
            get { return gpsIndicatedVerticalSpeed; }
            set
            {
                gpsIndicatedVerticalSpeed = value;
                NotifyPropertyChanged("GpsIndicatedVerticalSpeed");
                // "/instrumentation/gps/indicated-vertical-speed"
            }
        }
        public double GpsIndicatedGroundSpeedKt
        {
            get { return gpsIndicatedGroundSpeedKt; }
            set
            {
                gpsIndicatedGroundSpeedKt = value;
                NotifyPropertyChanged("GpsIndicatedGroundSpeedKt");
                // "/instrumentation/gps/indicated-ground-speed-kt"
            }
        }
        public double AirspeedIndicatorIndicatedSpeedKt
        {
            get { return airspeedIndicatorIndicatedSpeedKt; }
            set
            {
                airspeedIndicatorIndicatedSpeedKt = value;
                NotifyPropertyChanged("AirspeedIndicatorIndicatedSpeedKt");
                // "/instrumentation/airspeed-indicator/indicated-speed-kt"
            }
        }
        public double GpsIndicatedAltitudeFt
        {
            get { return gpsIndicatedAltitudeFt; }
            set
            {
                gpsIndicatedAltitudeFt = value;
                NotifyPropertyChanged("GpsIndicatedAltitudeFt");
                // "/instrumentation/gps/indicated-altitude-ft"
            }
        }
        public double AttitudeIndicatorInternalRollDeg
        {
            get { return attitudeIndicatorInternalRollDeg; }
            set
            {
                attitudeIndicatorInternalRollDeg = value;
                NotifyPropertyChanged("AttitudeIndicatorInternalRollDeg");
                // "/instrumentation/attitude-indicator/internal-roll-deg"
            }
        }
        public double AttitudeIndicatorInternalPitchDeg
        {
            get { return attitudeIndicatorInternalPitchDeg; }
            set
            {
                attitudeIndicatorInternalPitchDeg = value;
                NotifyPropertyChanged("AttitudeIndicatorInternalPitchDeg");
                // "/instrumentation/attitude-indicator/internal-pitch-deg"
            }
        }
        public double AltimeterIndicatedAltitudeFt
        {
            get { return altimeterIndicatedAltitudeFt; }
            set
            {
                altimeterIndicatedAltitudeFt = value;
                NotifyPropertyChanged("AltimeterIndicatedAltitudeFt");
                // "/instrumentation/altimeter/indicated-altitude-ft"
            }
        }

        //dealing with property changing
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public AppModel(IServer server)
        {
            this.server = server;
            this.joystickModel = new JoystickModel(this);
            stopModel();           
        }

        public void connect(string ip, int port)
        {
            server.Connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            server.disconnect();
        }

        public void start()
        {
            new Thread(delegate ()
            {                
                try
                {
                    while (!stop)
                    {
                        //Dashboard:

                        server.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                        IndicatedHeadingDeg = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/gps/indicated-vertical-speed\n");
                        GpsIndicatedVerticalSpeed = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                        GpsIndicatedGroundSpeedKt = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                        AirspeedIndicatorIndicatedSpeedKt = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/gps/indicated-altitude-ft\n");
                        GpsIndicatedAltitudeFt = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                        AttitudeIndicatorInternalRollDeg = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                        AttitudeIndicatorInternalPitchDeg = Math.Round(Double.Parse(server.read()), 2);

                        server.write("get /instrumentation/gps/indicated-altitude-ft\n");
                        AltimeterIndicatedAltitudeFt = Math.Round(Double.Parse(server.read()), 2);

                        //Controllers:

                        server.write("set /controls/flight/aileron " + Aileron + "\n");
                        Aileron = Math.Round(Double.Parse(server.read()), 2);

                        server.write("set /controls/engines/current-engine/throttle " + Throttle + "\n");
                        Throttle = Math.Round(Double.Parse(server.read()), 2);

                        server.write("set /controls/flight/rudder " + JoystickModel.Rudder + "\n");
                        JoystickModel.Rudder = Math.Round(Double.Parse(server.read()), 2);

                        server.write("set /controls/flight/elevator " + JoystickModel.Elevator + "\n");
                        JoystickModel.Elevator = Math.Round(Double.Parse(server.read()), 2);

                        Thread.Sleep(25);
                    }
                }
                catch (Exception)
                {
                    server.disconnect();
                    stopModel();
                }                
            }).Start();
        }

        public void controlJoystick(double r, double e)
        {
            joystickModel.controlJoystick(r, e);
        }

        public void controlAileron(double a)
        {
            if (a > 1)
            {
                Aileron = 1;
            }
            else if (a < -1)
            {
                Aileron = -1;
            }
            else
            {
                Aileron = a;
            }
        }

        public void controlThrottle(double t)
        {
            if (t > 1)
            {
                Throttle = 1;
            }
            else if (t < 0)
            {
                Throttle = 0;
            }
            else
            {
                Throttle = t;
            }
        }
    }
}
