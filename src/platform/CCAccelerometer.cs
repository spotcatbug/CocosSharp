﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Input;
using System.Reflection;

#if NETFX_CORE
using Windows.Devices.Sensors;
#elif IOS
using CoreMotion;
using Foundation;
#elif !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL && !WINDOWSDX
using Microsoft.Devices.Sensors;
#endif

namespace CocosSharp
{
    public class CCAcceleration
    {
        public double X;
        public double Y;
        public double Z;
        public double TimeStamp;
    }

    public class CCAccelerometer
    {

#if IOS
        static CoreMotion.CMMotionManager accelerometer = null;

#elif !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL && !WINDOWSDX
        // the accelerometer sensor on the device
        static Accelerometer accelerometer = null;
#endif
#if NETFX_CORE
        // the accelerometer sensor on the device
        static Accelerometer accelerometer = null;
#endif

        const float TG3_GRAVITY_EARTH = 9.80665f;
        readonly CCAcceleration accelerationValue = new CCAcceleration();


        CCEventAccelerate accelerateEvent = new CCEventAccelerate();
        bool enabled = false;


        #region Properties

        public CCWindow Window { get; set; }

        bool Active { get; set; }
        bool Emulating { get; set; }

        public bool Enabled 
        {
            get { return enabled; }

            set 
            {
                enabled = value;
                if (enabled && !Active)
                {
#if IOS
                    try
                    {
                        if (accelerometer.AccelerometerAvailable)
                        {
                            accelerometer.StartAccelerometerUpdates();
                            Active = true;

                        }
                        else
                        {
                            Active = false;
                        }
                    }
                    catch (Exception)
                    {
                        Active = false;
                    }
#elif !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL && !WINDOWSDX
                    try
                    {
                        if(Accelerometer.IsSupported)
                        {
                            accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
                            accelerometer.Start();
                            Active = true;
                        }
                        else
                        {
                            Active = false;
                        }
                    }
                    catch (Microsoft.Devices.Sensors.AccelerometerFailedException)
                    {
                        Active = false;
                    }

#elif NETFX_CORE
                    try
                    {
                        if(accelerometer != null)
                        {
                            accelerometer.ReadingChanged += accelerometer_ReadingChanged;
                            Active = true;
                        }
                        else
                        {
                            Active = false;
                        }
                    }
                    catch (Exception)
                    {
                        Active = false;
                    }
#endif

                    if (!Active)
                    {
                        Active = true;
                        Emulating = true;
                    }
                    else
                    {
                        Emulating = false;
                    }
                }
                else
                {
                    if (Active && !Emulating)
                    {
#if IOS
                    if (accelerometer != null)
                    {
                        accelerometer.StopAccelerometerUpdates();
                    }
#elif !WINDOWS && !OUYA &&!NETFX_CORE && !MACOS && !WINDOWSGL && !WINDOWSDX
                        if (accelerometer != null)
                        {
                            accelerometer.CurrentValueChanged -= accelerometer_CurrentValueChanged;
                            accelerometer.Stop();
                        }
#endif
#if NETFX_CORE
                        if (accelerometer != null)
                        {
                            accelerometer.ReadingChanged -= accelerometer_ReadingChanged;
                        }
#endif
                    }

                    ResetAccelerometer();

                    Active = false;
                    Emulating = false;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCAccelerometer(CCWindow window)
        {
            Window = window;

#if IOS
            accelerometer = new CoreMotion.CMMotionManager();
            //accelerometer.AccelerometerUpdateInterval = 60f;
#elif !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL && !WINDOWSDX
            try
            {
                accelerometer = new Accelerometer();
            }
            catch (Exception ex)
            {
                CCLog.Log(ex.ToString());
                CCLog.Log("No accelerometer on platform. CCAccelerometer will default to emulation code.");
            }

#elif NETFX_CORE
            accelerometer = Accelerometer.GetDefault();
#endif
        }

        #endregion Constructors


        void ResetAccelerometer()
        {
            accelerationValue.X = 0;
            accelerationValue.Y = 0;
            accelerationValue.Z = 0;
        }

#if NETFX_CORE
        void accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            if (args == null)
                return;

            var reading = args.Reading;
            accelerationValue.Y = reading.AccelerationY;
            accelerationValue.X = reading.AccelerationX;
            accelerationValue.Z = reading.AccelerationZ;

            accelerationValue.TimeStamp = reading.Timestamp.Ticks;
        }

#endif

#if !WINDOWS && !OUYA && !NETFX_CORE && !MACOS && !WINDOWSGL && !WINDOWSDX && !IOS
        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<Microsoft.Devices.Sensors.AccelerometerReading> e)
        {

#if ANDROID || IOS
            var sensorReading = e.SensorReading.Acceleration;
#elif WINDOWS_PHONE8
            // We have to use reflection to get the Vector3 value out of Acceleration
            // What happens is that the Sensor used XNA Vector3 and what we have done is replaced
            // the XNA with MonoGame which our Sensor does not compile against.
            //
            // Result is this ugly hack.
            object val = e.SensorReading.GetType()
            .GetProperty("Acceleration",
            BindingFlags.FlattenHierarchy |
            BindingFlags.Instance |
            BindingFlags.Public)
            .GetValue(e.SensorReading, null);

            var sensorReading = ParseVector3(val.ToString());


#endif
            accelerationValue.X = sensorReading.X;
            accelerationValue.Y = sensorReading.Y;
            accelerationValue.Z = sensorReading.Z;

            accelerationValue.TimeStamp = e.SensorReading.Timestamp.Ticks;
        }

        static Vector3 ParseVector3(string acceleration)
        {
            string[] temp = acceleration.Substring(1, acceleration.Length - 2).Split(':');
            float x = float.Parse(temp[1].Substring(0, temp[1].Length - 1));
            float y = float.Parse(temp[2].Substring(0, temp[2].Length - 1));
            float z = float.Parse(temp[3]);
            Vector3 rValue = new Vector3(x, y, z);
            return rValue;
        }

        #endif

        public void Update()
        {
            var dispatcher = Window.EventDispatcher;
            if (dispatcher.IsEventListenersFor(CCEventListenerAccelerometer.LISTENER_ID))
            {
                if (Emulating)
                {
                    // if we're in the emulator, we'll generate a fake acceleration value using the arrow keys
                    // press the pause/break key to toggle keyboard input for the emulator
                    KeyboardState keyboardState = Keyboard.GetState();

                    var stateValue = new Vector3();

                    stateValue.Z = -1;

                    if (keyboardState.IsKeyDown(Keys.Left))
                        stateValue.X = -.1f;
                    if (keyboardState.IsKeyDown(Keys.Right))
                        stateValue.X = .1f;
                    if (keyboardState.IsKeyDown(Keys.Up))
                        stateValue.Y = .1f;
                    if (keyboardState.IsKeyDown(Keys.Down))
                        stateValue.Y = -.1f;

                    stateValue.Normalize();

                    accelerationValue.X = stateValue.X;
                    accelerationValue.Y = stateValue.Y;
                    accelerationValue.Z = stateValue.Z;
                    accelerationValue.TimeStamp = DateTime.UtcNow.Ticks;
                }

#if IOS
            if (accelerometer.AccelerometerAvailable)
            {
                var accelerometerData = accelerometer.AccelerometerData;
            
                // The accelerometer data is optional and may be null at times even though AccelerometerAvailable is true. 
                if (accelerometerData != null)
                {
                    var acceleration = accelerometerData.Acceleration;
                    accelerationValue.X = acceleration.X;
                    accelerationValue.Y = acceleration.Y;
                    accelerationValue.Z = acceleration.Z;
                }
            }

#endif
                accelerateEvent.Acceleration = accelerationValue;
                dispatcher.DispatchEvent (accelerateEvent);
            }
        }
    }
}
